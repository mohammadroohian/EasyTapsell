using UnityEngine;
using UnityEngine.Events;

namespace Pashmak.Action
{
    public class ExecuteNode : ActionNode
    {
        // variable________________________________________________________________
        [SerializeField] private UnityEvent m_onExecute = new UnityEvent();


        // property________________________________________________________________
        public UnityEvent OnExecute { get => m_onExecute; }


        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;

            m_onExecute.Invoke();
            base.ExecuteNextChildActionNode();
        }
    }
}