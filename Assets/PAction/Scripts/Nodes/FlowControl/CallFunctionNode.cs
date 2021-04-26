using UnityEngine;
using System.Collections;

namespace Pashmak.Action
{
    public class CallFunctionNode : ActionNode
    {
        // variable________________________________________________________________
        [SerializeField]
        private FunctionNode m_functionToRun = null;


        // property________________________________________________________________
        public FunctionNode FunctionToRun { get => m_functionToRun; }


        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;

            m_functionToRun.Execute();
            base.ExecuteNextChildActionNode();
        }
    }
}