using NaughtyAttributes;
using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class EasyTapsellVideoEventTrigger : MonoBehaviour
    {
        public enum TapsellVideoEventType
        {
            None = 0,
            AdCompeleted = 1 << 0, // 1
            AdCanceled = 1 << 1, // 2
            AdAvailable = 1 << 2, // 4
            NoAdAvailable = 1 << 3, // 8
            Error = 1 << 4, // 16
            NoNetwork = 1 << 5, // 32
            Expiring = 1 << 6, // 64
            Open = 1 << 7, // 128
            Close = 1 << 8, // 256
            All = ~0
        }


        // variable____________________________________________________________________
        [SerializeField] private bool m_isActive = true;

        [EnumFlags]
        [OnValueChanged("OnValueChangedMethod_TapsellEvents")]
        [SerializeField] private TapsellVideoEventType m_tapsellEvents = TapsellVideoEventType.None;

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

        [HideInInspector]
        [SerializeField] private bool m_onOpenIsActive = false;
        [ShowIf("m_onOpenIsActive")]
        [SerializeField] private UnityEvent m_onOpen = new UnityEvent();

        [HideInInspector]
        [SerializeField] private bool m_onCloseIsActive = false;
        [ShowIf("m_onCloseIsActive")]
        [SerializeField] private UnityEvent m_onClose = new UnityEvent();


        // property________________________________________________________________
        public bool IsActive { get => m_isActive; set => m_isActive = value; }
        public UnityEvent OnAdCompeleted { get => m_onAdCompeleted; private set => m_onAdCompeleted = value; }
        public UnityEvent OnAdCanceled { get => m_onAdCanceled; private set => m_onAdCanceled = value; }
        public UnityEvent OnAdAvailable { get => m_onAdAvailable; private set => m_onAdAvailable = value; }
        public UnityEvent OnNoAdAvailable { get => m_onNoAdAvailable; set => m_onNoAdAvailable = value; }
        public UnityEvent OnError { get => m_onError; private set => m_onError = value; }
        public UnityEvent OnNoNetwork { get => m_onNoNetwork; private set => m_onNoNetwork = value; }
        public UnityEvent OnExpiring { get => m_onExpiring; private set => m_onExpiring = value; }
        public UnityEvent OnOpen { get => m_onOpen; private set => m_onOpen = value; }
        public UnityEvent OnClose { get => m_onClose; private set => m_onClose = value; }


        // monoBehaviour___________________________________________________________
        void Start()
        {
            // add this trigger event to manager
            EasyTapsellManager.Instance.OnVideoAdCompeleted.AddListener(() =>
            {
                if (IsActive && m_onAdCompeletedIsActive)
                    OnAdCompeleted.Invoke();
            });
            EasyTapsellManager.Instance.OnVideoAdCanceled.AddListener(() =>
            {
                if (IsActive && m_onAdCanceledIsActive)
                    OnAdCanceled.Invoke();
            });
            EasyTapsellManager.Instance.OnVideoAdAvailable.AddListener(() =>
            {
                if (IsActive && m_onAdAvailableIsActive)
                    OnAdAvailable.Invoke();
            });
            EasyTapsellManager.Instance.OnNoVideoAdAvailable.AddListener(() =>
            {
                if (IsActive && m_onNoAdAvailableIsActive)
                    OnNoAdAvailable.Invoke();
            });
            EasyTapsellManager.Instance.OnVideoError.AddListener(() =>
            {
                if (IsActive && m_onErrorIsActive)
                    OnError.Invoke();
            });
            EasyTapsellManager.Instance.OnVideoNoNetwork.AddListener(() =>
            {
                if (IsActive && m_onNoNetworkIsActive)
                    OnNoNetwork.Invoke();
            });
            EasyTapsellManager.Instance.OnVideoExpiring.AddListener(() =>
            {
                if (IsActive && m_onExpiringIsActive)
                    OnExpiring.Invoke();
            });
            EasyTapsellManager.Instance.OnVideoOpen.AddListener(() =>
            {
                if (IsActive && m_onOpenIsActive)
                    OnOpen.Invoke();
            });
            EasyTapsellManager.Instance.OnVideoClose.AddListener(() =>
            {
                if (IsActive && m_onCloseIsActive)
                    OnClose.Invoke();
            });
        }


        // function________________________________________________________________
        private void OnValueChangedMethod_TapsellEvents()
        {
            m_onAdCompeletedIsActive = ((int)m_tapsellEvents & 1) != 0;
            m_onAdCanceledIsActive = ((int)m_tapsellEvents & 2) != 0;
            m_onAdAvailableIsActive = ((int)m_tapsellEvents & 4) != 0;
            m_onNoAdAvailableIsActive = ((int)m_tapsellEvents & 8) != 0;
            m_onErrorIsActive = ((int)m_tapsellEvents & 16) != 0;
            m_onNoNetworkIsActive = ((int)m_tapsellEvents & 32) != 0;
            m_onExpiringIsActive = ((int)m_tapsellEvents & 64) != 0;
            m_onOpenIsActive = ((int)m_tapsellEvents & 128) != 0;
            m_onCloseIsActive = ((int)m_tapsellEvents & 256) != 0;
        }
    }

}
