using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class TapsellManager : MonoBehaviour
    {
        // variable____________________________________________________________________
        [SerializeField] private bool m_initializeAtStart = true;
        [SerializeField] private string m_tapsellKey = "YOUR TAPSELL KEY";
        [SerializeField] private UnityEvent m_onAdCompeleted = new UnityEvent();
        [SerializeField] private UnityEvent m_onAdCanceled = new UnityEvent();
        [SerializeField] private UnityEvent m_onAdAvailable = new UnityEvent();
        [SerializeField] private UnityEvent m_onNoAdAvailable = new UnityEvent();
        [SerializeField] private UnityEvent m_onError = new UnityEvent();
        [SerializeField] private UnityEvent m_onNoNetwork = new UnityEvent();
        [SerializeField] private UnityEvent m_onExpiring = new UnityEvent();


        // Property________________________________________________
        public static TapsellManager Instance { get; private set; }
        public static bool IsInitialized
        {
            get;
            private set;
        }
        public string TapsellKey { get => m_tapsellKey; private set => m_tapsellKey = value; }
        public UnityEvent OnAdCompeleted { get => m_onAdCompeleted; set => m_onAdCompeleted = value; }
        public UnityEvent OnAdCanceled { get => m_onAdCanceled; set => m_onAdCanceled = value; }
        public UnityEvent OnAdAvailable { get => m_onAdAvailable; set => m_onAdAvailable = value; }
        public UnityEvent OnNoAdAvailable { get => m_onNoAdAvailable; set => m_onNoAdAvailable = value; }
        public UnityEvent OnError { get => m_onError; set => m_onError = value; }
        public UnityEvent OnNoNetwork { get => m_onNoNetwork; set => m_onNoNetwork = value; }
        public UnityEvent OnExpiring { get => m_onExpiring; set => m_onExpiring = value; }


        // monoBehaviour___________________________________________________________
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

        }
        void Start()
        {
            // Initialize Tapsell.
            if (m_initializeAtStart)
                InitializeTapsell();
        }


        // function________________________________________________________________
        public void InitializeTapsell() => InitializeTapsell(TapsellKey);
        public void InitializeTapsell(string tapsellkey)
        {
            // Check Tapsell is initialized or not.
            if (IsInitialized)
                return;

            // Use tapsell key for initialization.
            TapsellSDK.Tapsell.Initialize(tapsellkey);
            IsInitialized = true;
        }
    }
}