using NaughtyAttributes;
using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class TapsellEventTrigger : MonoBehaviour
    {
        // variable____________________________________________________________________
        [SerializeField] private bool m_isActive = true;

        [Label("OnAdCompeleted")]
        [SerializeField] private bool m_onAdCompeletedIsActive = false;
        [ShowIf("m_onAdCompeletedIsActive")]
        [SerializeField] private UnityEvent m_onAdCompeleted = new UnityEvent();

        [Label("OnAdCanceled")]
        [SerializeField] private bool m_onAdCanceledIsActive = false;
        [ShowIf("m_onAdCanceledIsActive")]
        [SerializeField] private UnityEvent m_onAdCanceled = new UnityEvent();

        [Label("OnAdAvailable")]
        [SerializeField] private bool m_onAdAvailableIsActive = false;
        [ShowIf("m_onAdAvailableIsActive")]
        [SerializeField] private UnityEvent m_onAdAvailable = new UnityEvent();

        [Label("OnNoAdAvailable")]
        [SerializeField] private bool m_onNoAdAvailableIsActive = false;
        [ShowIf("m_onNoAdAvailableIsActive")]
        [SerializeField] private UnityEvent m_onNoAdAvailable = new UnityEvent();

        [Label("OnError")]
        [SerializeField] private bool m_onErrorIsActive = false;
        [ShowIf("m_onErrorIsActive")]
        [SerializeField] private UnityEvent m_onError = new UnityEvent();

        [Label("OnNoNetwork")]
        [SerializeField] private bool m_onNoNetworkIsActive = false;
        [ShowIf("m_onNoNetworkIsActive")]
        [SerializeField] private UnityEvent m_onNoNetwork = new UnityEvent();

        [Label("OnExpiring")]
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
        public bool OnAdCompeletedIsActive { get => m_onAdCompeletedIsActive; set => m_onAdCompeletedIsActive = value; }
        public bool OnAdCanceledIsActive { get => m_onAdCanceledIsActive; set => m_onAdCanceledIsActive = value; }
        public bool OnAdAvailableIsActive { get => m_onAdAvailableIsActive; set => m_onAdAvailableIsActive = value; }
        public bool OnNoAdAvailableIsActive { get => m_onNoAdAvailableIsActive; set => m_onNoAdAvailableIsActive = value; }
        public bool OnErrorIsActive { get => m_onErrorIsActive; set => m_onErrorIsActive = value; }
        public bool OnNoNetworkIsActive { get => m_onNoNetworkIsActive; set => m_onNoNetworkIsActive = value; }
        public bool OnExpiringIsActive { get => m_onExpiringIsActive; set => m_onExpiringIsActive = value; }


        // monoBehaviour___________________________________________________________
        void Start()
        {
            // add this trigger event to manager
            TapsellManager.Instance.OnAdCompeleted.AddListener(() =>
            {
                if (IsActive && OnAdCompeletedIsActive)
                    OnAdCompeleted.Invoke();
            });
            TapsellManager.Instance.OnAdCanceled.AddListener(() =>
            {
                if (IsActive && OnAdCanceledIsActive)
                    OnAdCanceled.Invoke();
            });
            TapsellManager.Instance.OnAdAvailable.AddListener(() =>
            {
                if (IsActive && OnAdAvailableIsActive)
                    OnAdAvailable.Invoke();
            });
            TapsellManager.Instance.OnNoAdAvailable.AddListener(() =>
            {
                if (IsActive && OnNoAdAvailableIsActive)
                    OnNoAdAvailable.Invoke();
            });
            TapsellManager.Instance.OnError.AddListener(() =>
            {
                if (IsActive && OnErrorIsActive)
                    OnError.Invoke();
            });
            TapsellManager.Instance.OnNoNetwork.AddListener(() =>
            {
                if (IsActive && OnNoNetworkIsActive)
                    OnNoNetwork.Invoke();
            });
            TapsellManager.Instance.OnExpiring.AddListener(() =>
            {
                if (IsActive && OnExpiringIsActive)
                    OnExpiring.Invoke();
            });
        }
    }
}