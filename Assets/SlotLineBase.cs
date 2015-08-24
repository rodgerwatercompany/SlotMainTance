using UnityEngine;
using System.Collections;

namespace Rodger
{
    public abstract class SlotLineBase : MiniBehaviour
    {

        public Transform[] m_trans_symbols;

        Transform[] m_trans_stopOrder;

        bool m_moving;
        bool m_breaking;

        string[] Array_SpriteNames;
        string[] m_SpriteName_Specified;

        float m_framespeed;

        public VOIDCB onStopCB;

        protected virtual void Awake()
        {
            m_SpriteName_Specified = new string[3];

            m_trans_stopOrder = new Transform[8];
        }
        public void Init(string[] array_spritename)
        {
            Array_SpriteNames = array_spritename;
        }
        public void StartRun()
        {
            m_moving = true;
            m_breaking = false;
        }
        public void StartStop()
        {
            m_trans_stopOrder[0] = m_trans_symbols[0];
            m_trans_stopOrder[1] = m_trans_symbols[1];
            m_trans_stopOrder[2] = m_trans_symbols[2];
            m_trans_stopOrder[3] = m_trans_symbols[3];
            m_trans_stopOrder[4] = m_trans_symbols[4];
            m_trans_stopOrder[5] = m_trans_symbols[5];
            m_trans_stopOrder[6] = m_trans_symbols[6];
            m_trans_stopOrder[7] = m_trans_symbols[7];

            m_breaking = true;
        }
        public void Update_Sync(float framespeed)
        {
            if(m_moving)
            {
                m_framespeed = framespeed;
                if (m_breaking)
                    BreakingMode();
                else
                    NormalMode();
            }
        }
        public void SpecifiedSpriteData(int[] array_idx)
        {
            m_SpriteName_Specified[0] = Array_SpriteNames[array_idx[0]];
            m_SpriteName_Specified[1] = Array_SpriteNames[array_idx[1]];
            m_SpriteName_Specified[2] = Array_SpriteNames[array_idx[2]];
        }
        #region Move and Break
        void NormalMode()
        {

            for (int i = 0; i < m_trans_symbols.Length; i++)
                m_trans_symbols[i].localPosition = new Vector3(0, m_trans_symbols[i].localPosition.y + m_framespeed, 0);

            if (m_trans_symbols[0].localPosition.y < -200)
            {
                if (m_breaking)
                {
                    // Change to Specified Sprite.
                    if (m_trans_symbols[0] == m_trans_stopOrder[0])
                    {
                        m_trans_symbols[0].GetComponent<UISprite>().spriteName = m_SpriteName_Specified[2];
                    }
                    else if (m_trans_symbols[0] == m_trans_stopOrder[1])
                    {
                        m_trans_symbols[0].GetComponent<UISprite>().spriteName = m_SpriteName_Specified[1];
                    }
                    else if (m_trans_symbols[0] == m_trans_stopOrder[2])
                    {
                        m_trans_symbols[0].GetComponent<UISprite>().spriteName = m_SpriteName_Specified[0];
                    }
                    else
                        RandomChageSprite();
                }
                else
                    RandomChageSprite();

                TurnAround();
                float y = m_trans_symbols[m_trans_symbols.Length - 2].localPosition.y + 190;
                m_trans_symbols[m_trans_symbols.Length - 1].localPosition = new Vector3(0, y, 0);
            }
        }

        void BreakingMode()
        {
            float temp = m_trans_stopOrder[0].localPosition.y + m_framespeed;

            if (temp < 0 && m_trans_stopOrder[0].localPosition.y > 0)
                DoStop();
            else
                NormalMode();
        }

        protected virtual void DoStop()
        {
            m_moving = false;                
            m_breaking = false;

            m_trans_stopOrder[0].localPosition = Vector3.zero;
            m_trans_stopOrder[1].localPosition = new Vector3(0, 190, 0);
            m_trans_stopOrder[2].localPosition = new Vector3(0, 380, 0);
            m_trans_stopOrder[3].localPosition = new Vector3(0, 570, 0);
            m_trans_stopOrder[4].localPosition = new Vector3(0, 760, 0);
            m_trans_stopOrder[5].localPosition = new Vector3(0, 950, 0);
            m_trans_stopOrder[6].localPosition = new Vector3(0, 1140, 0);
            m_trans_stopOrder[7].localPosition = new Vector3(0, 1330, 0);

            TurnAround();
                
            if (onStopCB != null)                    
                onStopCB();
        }

        // 換掉第一個
        void TurnAround()
        {
            for (int i = 1; i < m_trans_symbols.Length; i++)
            {
                Transform temp = m_trans_symbols[i - 1];
                m_trans_symbols[i - 1] = m_trans_symbols[i];
                m_trans_symbols[i] = temp;
            }
        }

        // 換掉第一張牌
        void RandomChageSprite()
        {
            int idx_rand = Random.Range(0,Array_SpriteNames.Length);
            m_trans_symbols[0].GetComponent<UISprite>().spriteName = Array_SpriteNames[idx_rand];
        }
        #endregion

        #region Up and Down
        public abstract void StartMoveUp();
        public abstract void StartMoveDown();        
        #endregion
    }
}