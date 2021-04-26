using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using Pashmak.Core.CU.MonoEvent;

namespace Pashmak.Action
{
    [RequireComponent(typeof(CU_MonoEvent_Collider))]
    public class WaitForCollisionNode : ActionNode
    {
        // variable________________________________________________________________
        private bool m_collisionCheck = false;
        private CU_MonoEvent_Collider m_colliderInfo = null;
        [SerializeField] private UnityEvent m_onDetect = new UnityEvent();


        // property________________________________________________________________
        public string CollisionTag
        {
            get => GetComponent<CU_MonoEvent_Collider>().ObjectTag;
            set => GetComponent<CU_MonoEvent_Collider>().ObjectTag = value;
        }

        public UnityEvent OnDetect { get => m_onDetect; private set => m_onDetect = value; }


        // monoBehaviour___________________________________________________________
        protected override void Awake()
        {
            base.Awake();

            m_colliderInfo = GetComponent<CU_MonoEvent_Collider>();
            if (m_colliderInfo == null)
                Debug.LogError("Debug: No CU_MonoEvent_Collider Funded!", this);
            else
                m_colliderInfo.OnDetect.AddListener(OnEventDetection);

        }



        // function________________________________________________________________
        public override void Execute()
        {
            base.Execute();
            if (!EnabledNode) return;
        }
        public override void Initialize()
        {
            base.Initialize();
            m_collisionCheck = false;
        }
        private void OnEventDetection()
        {
            if (!base.IsDetected) return;
            if (base.IsDone) return;
            if (m_collisionCheck) return;

            OnDetect.Invoke();
            base.ExecuteNextChildActionNode();
            m_collisionCheck = true;
        }
    }
}