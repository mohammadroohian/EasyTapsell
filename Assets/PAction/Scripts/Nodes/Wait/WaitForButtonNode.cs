using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Pashmak.Core.CU._UnityEngine._Input;

namespace Pashmak.Action
{
    [RequireComponent(typeof(CU_Input_GetButton))]
    public class WaitForButtonNode : ActionNode
    {
        public enum ButtonType { GetButton, GetButtonDown, GetButtonUp }
        // variable________________________________________________________________
        private CU_Input_GetButton m_buttonInfo = null;
        [SerializeField]
        private UnityEvent m_onDetect = new UnityEvent();


        // property________________________________________________________________
        public UnityEvent OnDetect { get => m_onDetect; private set => m_onDetect = value; }
        public string ButtonName
        {
            get => GetComponent<CU_Input_GetButton>().ButtonName;
            set => GetComponent<CU_Input_GetButton>().ButtonName = value;
        }


        // monoBehaviour___________________________________________________________
        protected override void Awake()
        {
            base.Awake();

            m_buttonInfo = GetComponent<CU_Input_GetButton>();
            if (m_buttonInfo == null)
                Debug.LogError("Debug: No CU_Input_GetButton Funded!", this);
            else
                m_buttonInfo.OnDetectButton.AddListener(OnEventDetection);

        }


        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;
        }
        private void OnEventDetection()
        {
            if (!base.IsDetected) return;
            if (base.IsDone) return;

            OnDetect.Invoke();
            base.ExecuteNextChildActionNode();
        }
    }
}