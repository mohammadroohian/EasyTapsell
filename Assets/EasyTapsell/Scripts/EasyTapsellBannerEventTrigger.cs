using NaughtyAttributes;
using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class EasyTapsellBannerEventTrigger : MonoBehaviour
    {
        public enum TapsellBannerEventType
        {
            None = 0,
            AdAvailable = 1 << 0, // 1
            NoAdAvailable = 1 << 1, // 2
            Error = 1 << 2, // 4
            NoNetwork = 1 << 3, // 8
            Hide = 1 << 4, // 16
            All = ~0
        }


        // variable____________________________________________________________________
        [SerializeField] private bool m_isActive = true;

        [EnumFlags]
        [OnValueChanged("OnValueChangedMethod_TapsellEvents")]
        [SerializeField] private TapsellBannerEventType m_tapsellEvents = TapsellBannerEventType.None;

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
        [SerializeField] private bool m_onHideIsActive = false;
        [ShowIf("m_onHideIsActive")]
        [SerializeField] private UnityEvent m_onHide = new UnityEvent();


        // property________________________________________________________________
        public bool IsActive { get => m_isActive; set => m_isActive = value; }
        public UnityEvent OnAdAvailable { get => m_onAdAvailable; private set => m_onAdAvailable = value; }
        public UnityEvent OnNoAdAvailable { get => m_onNoAdAvailable; set => m_onNoAdAvailable = value; }
        public UnityEvent OnError { get => m_onError; private set => m_onError = value; }
        public UnityEvent OnNoNetwork { get => m_onNoNetwork; private set => m_onNoNetwork = value; }
        public UnityEvent OnHide { get => m_onHide; private set => m_onHide = value; }


        // monoBehaviour___________________________________________________________
        void Start()
        {
            // add this trigger event to manager
            EasyTapsellManager.Instance.OnBannerAdAvailable.AddListener(() =>
            {
                if (IsActive && m_onAdAvailableIsActive)
                    OnAdAvailable.Invoke();
            });
            EasyTapsellManager.Instance.OnNoBannerAdAvailable.AddListener(() =>
            {
                if (IsActive && m_onNoAdAvailableIsActive)
                    OnNoAdAvailable.Invoke();
            });
            EasyTapsellManager.Instance.OnBannerError.AddListener(() =>
            {
                if (IsActive && m_onErrorIsActive)
                    OnError.Invoke();
            });
            EasyTapsellManager.Instance.OnBannerNoNetwork.AddListener(() =>
            {
                if (IsActive && m_onNoNetworkIsActive)
                    OnNoNetwork.Invoke();
            });
            EasyTapsellManager.Instance.OnBannerHide.AddListener(() =>
            {
                if (IsActive && m_onHideIsActive)
                    OnHide.Invoke();
            });
        }


        // function________________________________________________________________
        private void OnValueChangedMethod_TapsellEvents()
        {
            m_onAdAvailableIsActive = ((int)m_tapsellEvents & 1) != 0;
            m_onNoAdAvailableIsActive = ((int)m_tapsellEvents & 2) != 0;
            m_onErrorIsActive = ((int)m_tapsellEvents & 4) != 0;
            m_onNoNetworkIsActive = ((int)m_tapsellEvents & 8) != 0;
            m_onHideIsActive = ((int)m_tapsellEvents & 16) != 0;
        }
    }

}
