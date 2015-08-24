using UnityEngine;
using System.Collections;

namespace Rodger
{

    public abstract class SlotmachineBase : MonoBehaviour
    {

        protected SlotLineBase[] m_slotLines;

        bool m_Moving;
        bool m_Breaking;        
        
        protected void StartRun()
        {
            StartCoroutine(DoStartRun());
        }
        protected abstract IEnumerator DoStartRun();
        
        protected void StartStop()
        {
            StartCoroutine(DoStartStop());
        }
        protected abstract IEnumerator DoStartStop();
        protected void StartFastStop()
        {
            StartCoroutine(DoStartFastStop());            
        }
        protected abstract IEnumerator DoStartFastStop();
    }
}