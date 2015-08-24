using UnityEngine;
using System.Collections;
using System;

namespace Game5095
{

    public class SlotMachine : Rodger.SlotmachineBase
    {
        
        private string[] spriteNames;

        private UIPanel m_Panel;

        SlotLine[] SlotLines;

        Rodger.VOIDCB onSlotStop;

        bool m_waitSlotStop;
        bool m_waitLineTweenCB;
        bool m_waitLineStopCB;
        
        void Awake()
        {
            m_Panel = GetComponent<UIPanel>();

            string[] spritenames = {
                "symbol_001",
                "symbol_002",
                "symbol_003",
                "symbol_004",
                "symbol_005",
                "symbol_006",
                "symbol_007",
                "symbol_008",
                "symbol_009",
                "symbol_010"
            };

            spriteNames = spritenames;

            m_slotLines = new SlotLine[5];
            SlotLines = new SlotLine[5];

            for (int i = 0; i < 5; i++)
            {
                SlotLines[i] = GameObject.Find("SLOTLINE_" + (i + 1)).GetComponent<SlotLine>();
                
                SlotLines[i].onTweenFinishCB = Listener_Tween;

                SlotLines[i].onStopCB = Listener_SlotStop;
                SlotLines[i].Init(spritenames);
            }
            m_slotLines = SlotLines;
        }
        void OnGUI()
        {
            if (GUILayout.Button("StartRun",GUILayout.Width(100),GUILayout.Height(50)))
            {
                StartRun();
            }
            if (GUILayout.Button("StartStop", GUILayout.Width(100), GUILayout.Height(50)))
            {
                StartStop();
            }
            if (GUILayout.Button("StartStop", GUILayout.Width(100), GUILayout.Height(50)))
            {
                StartFastStop();
            }
        }

        // Update is called once per frame
        void Update()
        {
            float distance = -1800 * Time.deltaTime;

            m_slotLines[0].Update_Sync(distance);
            m_slotLines[1].Update_Sync(distance);
            m_slotLines[2].Update_Sync(distance);
            m_slotLines[3].Update_Sync(distance);
            m_slotLines[4].Update_Sync(distance);
        }
        void Listener_Tween()
        {
            m_waitLineTweenCB = false;
            if(m_waitSlotStop)
            {
                m_waitSlotStop = false;

                if (onSlotStop != null)
                    onSlotStop();
            }
        }
        void Listener_SlotStop()
        {
            m_waitLineStopCB = false;
        }
        protected override IEnumerator DoStartRun()
        {
            m_Panel.clipSoftness = new Vector2(0,20);

            for (int i = 0; i < 5; i++)
            {
                m_slotLines[i].StartMoveUp();

                m_waitLineTweenCB = true;
                while (m_waitLineTweenCB)
                    yield return new WaitForEndOfFrame();

                m_slotLines[i].StartRun();
            }

        }
        protected override IEnumerator DoStartStop()
        {
            string str_spriteData = "";

            for (int i = 0; i < 5; i++)
            {
                int[] specifiedSymbols = new int[3] {UnityEngine.Random.Range(0, 6), UnityEngine.Random.Range(0, 6), UnityEngine.Random.Range(0, 6) };

                foreach (int num in specifiedSymbols)
                    str_spriteData += spriteNames[num];

                str_spriteData += "\n";

                m_slotLines[i].SpecifiedSpriteData(specifiedSymbols);
                m_slotLines[i].StartStop();

                m_waitLineStopCB = true;
                while (m_waitLineStopCB)
                    yield return new WaitForEndOfFrame();

                m_slotLines[i].StartMoveDown();
            }
            print(str_spriteData);

            m_waitSlotStop = true;

            m_Panel.clipSoftness = new Vector2(0, 0);
        }
        protected override IEnumerator DoStartFastStop()
        {

            string str_spriteData = "";
            for (int i = 0; i < 5; i++)
            {
                int[] specifiedSymbols = new int[3] { UnityEngine.Random.Range(0, 6), UnityEngine.Random.Range(0, 6), UnityEngine.Random.Range(0, 6) };

                foreach (int num in specifiedSymbols)
                    str_spriteData += spriteNames[num];

                str_spriteData += "\n";

                SlotLines[i].SpecifiedSpriteData(specifiedSymbols);
                SlotLines[i].StartStopAndMoveDown();                
            }

            int cnt = 0;
            while (cnt < 4)
            {
                cnt++;
                m_waitLineStopCB = true;
                while (m_waitLineStopCB)
                    yield return new WaitForEndOfFrame();
            }            

            print(str_spriteData);

            m_Panel.clipSoftness = new Vector2(0, 0);
            m_waitSlotStop = true;
        }
    }
}
