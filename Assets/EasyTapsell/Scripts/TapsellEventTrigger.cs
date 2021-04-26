using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class TapsellEventTrigger : MonoBehaviour
    {
        // variable____________________________________________________________________
        [SerializeField] private UnityEvent m_onAdCompeleted = new UnityEvent();
        [SerializeField] private UnityEvent m_onAdCanceled = new UnityEvent();
        [SerializeField] private UnityEvent m_onAdAvailable = new UnityEvent();
        [SerializeField] private UnityEvent m_onNoAdAvailable = new UnityEvent();
        [SerializeField] private UnityEvent m_onError = new UnityEvent();
        [SerializeField] private UnityEvent m_onNoNetwork = new UnityEvent();
        [SerializeField] private UnityEvent m_onExpiring = new UnityEvent();


        // property________________________________________________________________
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
            TapsellManager.Instance.OnAdCompeleted.AddListener(() => OnAdCompeleted.Invoke());
            TapsellManager.Instance.OnAdCanceled.AddListener(() => OnAdCanceled.Invoke());
            TapsellManager.Instance.OnAdAvailable.AddListener(() => OnAdAvailable.Invoke());
            TapsellManager.Instance.OnNoAdAvailable.AddListener(() => OnNoAdAvailable.Invoke());
            TapsellManager.Instance.OnError.AddListener(() => OnError.Invoke());
            TapsellManager.Instance.OnNoNetwork.AddListener(() => OnNoNetwork.Invoke());
            TapsellManager.Instance.OnExpiring.AddListener(() => OnExpiring.Invoke());
        }
    }
}