using UnityEngine;

namespace Pashmak.Action
{
    public class WaitForCallback : ActionNode
    {
        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;
        }

        public void Callback()
        {
            if (!EnabledNode) return; // check node itself is active or not
            if (!m_isDetected) return; // if it was false that means node is reinitialized and not continuing any more
            base.ExecuteNextChildActionNode();
        }
    }
}