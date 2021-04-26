using UnityEngine;
using System.Collections;

namespace Pashmak.Action
{
    public class ExitFunctionNode : ActionNode
    {
        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;

            base.m_parentFunction.ActionIsDone();
        }
    }
}