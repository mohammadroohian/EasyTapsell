using UnityEngine;

namespace Pashmak.Action
{
    public class FunctionNode : ActionNode
    {
        // variable________________________________________________________________
        [SerializeField]
        private string functionName = "func";
        [SerializeField] bool executeIfActive = true;

        [SerializeField]
        private bool runAtStart = false;
        [SerializeField]
        private bool debugMode = false;
        [SerializeField]
        private bool hideComments = false;
        [SerializeField]
        private bool showCompleteNodedetails = false;


        // property________________________________________________________________
        public bool DebugMode { get => debugMode; set => debugMode = value; }
        public string FunctionName { get => functionName; set => functionName = value; }
        public bool HideComments { get => hideComments; private set => hideComments = value; }
        public bool ShowCompleteNodedetails { get => showCompleteNodedetails; private set => showCompleteNodedetails = value; }


        // monoBehaviour___________________________________________________________
        void Start()
        {
            if (runAtStart) Execute();
        }


        // function________________________________________________________________
        public void Call()
        {
            Execute();
        }
        public override void Execute()
        {
            if (executeIfActive && !this.gameObject.activeInHierarchy)
                return;

            base.m_isDetected = true;
            base.Execute();
            if (!EnabledNode)
                return;

            base.ExecuteNextChildActionNode();
        }
        protected override void ExecuteNextActionNodeInParent()
        {
            EndFunction();
        }

        public void EndFunction()
        {
            foreach (var item in GetComponentsInChildren<ActionNode>())
            {
                item.Initialize();
            }
            if (debugMode) print("Function is done!");
        }
    }
}