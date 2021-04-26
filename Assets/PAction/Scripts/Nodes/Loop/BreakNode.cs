using UnityEngine;
using System.Collections;

namespace Pashmak.Action
{
    public class BreakNode : ActionNode
    {
        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;

            LoopNode tmp = GetComponentInParent<LoopNode>();
            tmp.EndLoop();
        }
    }
}