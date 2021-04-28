using NaughtyAttributes;
using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class TapsellEventTrigger : MonoBehaviour
    {
        public enum TapsellEventType
        {
            None = 0,
            OnAdCompeleted = 1 << 0, // 1
            OnAdCanceled = 1 << 1, // 2
            OnAdAvailable = 1 << 2, // 4
            OnNoAdAvailable = 1 << 3, // 8
            OnError = 1 << 4, // 16
            OnNoNetwork = 1 << 5, // 32
            OnExpiring = 1 << 6, // 64
            All = ~0
        }


        // variable____________________________________________________________________
        [SerializeField] private bool m_isActive = true;

        [EnumFlags]
        [OnValueChanged("OnValueChangedMethod_TapsellEvents")]
        [SerializeField] private TapsellEventType m_tapsellEvents = TapsellEventType.None;

        [HideInInspector]
        [SerializeField] private bool m_onAdCompeletedIsActive = false;
        [ShowIf("m_onAdCompeletedIsActive")]
        [SerializeField] private UnityEvent m_onAdCompeleted = new UnityEvent();

        [HideInInspector]
        [SerializeField] private bool m_onAdCanceledIsActive = false;
        [ShowIf("m_onAdCanceledIsActive")]
        [SerializeField] private UnityEvent m_onAdCanceled = new UnityEvent();

        [HideInInspector]
        [SerializeField] private bool m_onAdAvailableIsActive = false;
        [ShowIf("m_onAdAvailableIsActive")]
        [SerializeField] private UnityEvent m_onAdAvailable = new UnityEvent();

        [HideInInspector]
        [SerializeField] private bool m_onNoAdAvailableIsActive = false;
        [ShowIf("m_onNoAdAvailableIsActive")]
        [SerializeField] private UnityEvent m_onNoAdAvailable = new UnityEvent();

        [HideInInspector]
        [SerializeField] private bool m_onErrorIsActive = false;
        [ShowIf("m_onErrorIsActive")]
        [SerializeField] private UnityEvent m_onError = new UnityEvent();

        [HideInInspector]
        [SerializeField] private bool m_onNoNetworkIsActive = false;
        [ShowIf("m_onNoNetworkIsActive")]
        [SerializeField] private UnityEvent m_onNoNetwork = new UnityEvent();

        [HideInInspector]
        [SerializeField] private bool m_onExpiringIsActive = false;
        [ShowIf("m_onExpiringIsActive")]
        [SerializeField] private UnityEvent m_onExpiring = new UnityEvent();


        // property________________________________________________________________
        public bool IsActive { get => m_isActive; set => m_isActive = value; }
        public UnityEvent OnAdCompeleted { get => m_onAdCompeleted; private set => m_onAdCompeleted = value; }
        public UnityEvent OnAdCanceled { get => m_onAdCanceled; private set => m_onAdCanceled = value; }
        public UnityEvent OnAdAvailable { get => m_onAdAvailable; private set => m_onAdAvailable = value; }
        public UnityEvent OnNoAdAvailable { get => m_onNoAdAvailable; set => m_onNoAdAvailable = value; }
        public UnityEvent OnError { get => m_onError; private set => m_onError = value; }
        public UnityEvent OnNoNetwork { get => m_onNoNetwork; private set => m_onNoNetwork = value; }
        public UnityEvent OnExpiring { get => m_onExpiring; private set => m_onExpiring = value; }


        // monoBehaviour___________________________________________________________
        void Start()
        {
            // add this trigger event to manager
            TapsellManager.Instance.OnAdCompeleted.AddListener(() =>
            {
                if (IsActive && m_onAdCompeletedIsActive)
                    OnAdCompeleted.Invoke();
            });
            TapsellManager.Instance.OnAdCanceled.AddListener(() =>
            {
                if (IsActive && m_onAdCanceledIsActive)
                    OnAdCanceled.Invoke();
            });
            TapsellManager.Instance.OnAdAvailable.AddListener(() =>
            {
                if (IsActive && m_onAdAvailableIsActive)
                    OnAdAvailable.Invoke();
            });
            TapsellManager.Instance.OnNoAdAvailable.AddListener(() =>
            {
                if (IsActive && m_onNoAdAvailableIsActive)
                    OnNoAdAvailable.Invoke();
            });
            TapsellManager.Instance.OnError.AddListener(() =>
            {
                if (IsActive && m_onErrorIsActive)
                    OnError.Invoke();
            });
            TapsellManager.Instance.OnNoNetwork.AddListener(() =>
            {
                if (IsActive && m_onNoNetworkIsActive)
                    OnNoNetwork.Invoke();
            });
            TapsellManager.Instance.OnExpiring.AddListener(() =>
            {
                if (IsActive && m_onExpiringIsActive)
                    OnExpiring.Invoke();
            });
        }


        private void OnValueChangedMethod_TapsellEvents()
        {
            m_onAdCompeletedIsActive = ((int)m_tapsellEvents & 1) != 0;
            m_onAdCanceledIsActive = ((int)m_tapsellEvents & 2) != 0;
            m_onAdAvailableIsActive = ((int)m_tapsellEvents & 4) != 0;
            m_onNoAdAvailableIsActive = ((int)m_tapsellEvents & 8) != 0;
            m_onErrorIsActive = ((int)m_tapsellEvents & 16) != 0;
            m_onNoNetworkIsActive = ((int)m_tapsellEvents & 32) != 0;
            m_onExpiringIsActive = ((int)m_tapsellEvents & 64) != 0;
        }
    }

}
