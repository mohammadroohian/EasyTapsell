using Pashmak.Core;

namespace Pashmak.Action
{
    public class ConditionNode : ActionNode
    {
        // variable________________________________________________________________
        private ICondition m_condition;


        // property________________________________________________________________
        public ICondition Condition
        {
            get
            {
                if (m_condition == null)
                    m_condition = GetComponent<ICondition>();
                return m_condition;
            }
        }


        // monoBehaviour___________________________________________________________
        protected override void Awake()
        {
            base.Awake();
            m_condition = GetComponent<ICondition>();
        }


        // override________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;

            // check condition
            // don't execute child ActionNode's if condition is false
            if (CheckCondition())
                base.ExecuteNextChildActionNode();
            else
                ActionIsDone();
        }


        // function________________________________________________________________  
        private bool CheckCondition()
        {
            return m_condition.CheckCondition();
        }
    }
}