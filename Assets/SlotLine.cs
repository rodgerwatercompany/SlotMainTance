using UnityEngine;

using Rodger;

namespace Game5095
{
    public class SlotLine : Rodger.SlotLineBase
    {

        TweenPosition m_tweener;

        public VOIDCB onTweenFinishCB;

        bool m_waitStop;
        protected override void Awake()
        {
            base.Awake();

            m_tweener = _GetComponent<TweenPosition>();
            m_tweener.onFinished.Clear();
            m_tweener.AddOnFinished(OnTweenFinish);
            
        }
        void Start()
        {
            onStopCB += onStop;
        }
        public override void StartMoveUp()
        {
            Vector3 localPos = transform.localPosition;
            m_tweener.from = new Vector3(localPos.x, 0, 0);
            m_tweener.to = new Vector3(localPos.x, 50, 0);
            m_tweener.duration = 0.4f;

            m_tweener.ResetToBeginning();
            m_tweener.PlayForward();
        }

        public void OnTweenFinish()
        {
            if (onTweenFinishCB != null)
                onTweenFinishCB();
        }
        public override void StartMoveDown()
        {
            Vector3 localPos = transform.localPosition;

            m_tweener.from = new Vector3(localPos.x, 0, 0);
            m_tweener.to = new Vector3(localPos.x, -80, 0);
            m_tweener.duration = 0.4f;

            m_tweener.ResetToBeginning();
            m_tweener.PlayForward();
        }
        public void StartStopAndMoveDown()
        {            
            StartStop();
            m_waitStop = true;
        }
        public void onStop()
        {
            if (m_waitStop)
            {
                m_waitStop = false;
                StartMoveDown();
            }
        }
    }
}