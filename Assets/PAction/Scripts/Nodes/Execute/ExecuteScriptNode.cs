using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pashmak.Action
{
    public class ExecuteScriptNode : ActionNode
    {
        // variable________________________________________________________________
        private List<IExecuteNode> m_executeScripts = new List<IExecuteNode>();


        // monoBehaviour___________________________________________________________
        protected override void Awake()
        {
            base.Awake();

            //Set executeScripts and propNodes.
            Component[] tmpCo = GetComponents<Component>();
            for (int i = 0; i < tmpCo.Length; i++)
            {
                if (tmpCo[i] is IExecuteNode)
                {
                    m_executeScripts.Add((IExecuteNode)tmpCo[i]);
                }
            }
        }


        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;

            //Execute executeScripts.
            for (int i = 0; i < m_executeScripts.Count; i++)
            {
                m_executeScripts[i].ExcuteScript(this);
            }
            base.ExecuteNextChildActionNode();
        }
    }
    public interface IExecuteNode
    {
        // function________________________________________________________________
        void ExcuteScript(ActionNode sender);
    }
}