using UnityEngine;
using System.Collections;

namespace Pashmak.Action
{
    public class LoopNode : ActionNode
    {
        // variable________________________________________________________________
        [SerializeField]
        private int m_startFrom = 0;
        [SerializeField]
        private int m_endTo = 3;
        [SerializeField]
        private int m_incrementBy = 1;
        [SerializeField]
        public bool m_infinit;
        private int m_loopIndex = 0;


        // property________________________________________________________________
        public int StartFrom { get => m_startFrom; private set => m_startFrom = value; }
        public int EndTo { get => m_endTo; private set => m_endTo = value; }
        public int IncrementBy { get => m_incrementBy; private set => m_incrementBy = value; }


        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;

            m_loopIndex = m_startFrom;
            if (CheckForEnd()) EndLoop();
            else ExecuteNextChildActionNode();
        }
        protected override void ExecuteNextChildActionNode()
        {
            if (base.m_subActionNodes.Count == 0)
            {
                m_loopIndex += m_incrementBy;
                if (CheckForEnd())
                {
                    EndLoop();
                }
                else
                {
                    base.Initialize();
                    base.m_isDetected = true;
                    base.m_subActionNodes.Dequeue().Execute();
                }
            }
            else
                base.m_subActionNodes.Dequeue().Execute();
        }
        public bool CheckForEnd()
        {
            if (m_infinit) return false;
            return m_loopIndex > m_endTo;
        }
        public void EndLoop()
        {
            base.ActionIsDone();
        }
    }
}