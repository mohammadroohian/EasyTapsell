using UnityEngine;

namespace Pashmak.Action
{
    public class WaitForSecondsNode : ActionNode
    {
        // variable________________________________________________________________
        [SerializeField]
        private float m_waitTime = 1f;


        // property________________________________________________________________
        public float WaitTime { get => m_waitTime; }


        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;

            Invoke("waitFunc", WaitTime);
        }

        private void waitFunc()
        {
            if (!EnabledNode) return; // check node itself is active or not
            if (!m_isDetected) return; // if it was false that means node is reinitialized and not continuing any more
            base.ExecuteNextChildActionNode();
        }
    }
}