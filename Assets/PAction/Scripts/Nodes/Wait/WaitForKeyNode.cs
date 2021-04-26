using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Pashmak.Core.CU._UnityEngine._Input;

namespace Pashmak.Action
{
    [RequireComponent(typeof(CU_Input_GetKey))]
    public class WaitForKeyNode : ActionNode
    {
        public enum KeyType { GetKey, GetKeyDown, GetKeyUp }
        // variable________________________________________________________________
        private CU_Input_GetKey m_keyInfo = null;
        [SerializeField]
        private UnityEvent m_onDetect = new UnityEvent();


        // property________________________________________________________________
        public UnityEvent OnDetect { get => m_onDetect; set => m_onDetect = value; }
        public KeyCode Key
        {
            get => GetComponent<CU_Input_GetKey>().Key;
            set => GetComponent<CU_Input_GetKey>().Key = value;
        }

        // monoBehaviour___________________________________________________________
        protected override void Awake()
        {
            base.Awake();

            m_keyInfo = GetComponent<CU_Input_GetKey>();
            if (m_keyInfo == null)
                Debug.LogError("Debug: No CU_Input_GetKey Funded!", this);
            else
                m_keyInfo.OnDetectKey.AddListener(OnEventDetection);

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