using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class EasyTapsellManager : MonoBehaviour
    {
        // variable____________________________________________________________________
        [SerializeField] private bool m_initializeAtStart = true;
        [SerializeField] private string m_tapsellKey = "YOUR TAPSELL KEY";

        // video ad events
        private UnityEvent m_onVideoAdCompeleted = new UnityEvent();
        private UnityEvent m_onVideoAdCanceled = new UnityEvent();
        private UnityEvent m_onVideoAdAvailable = new UnityEvent();
        private UnityEvent m_onNoVideoAdAvailable = new UnityEvent();
        private UnityEvent m_onVideoError = new UnityEvent();
        private UnityEvent m_onVideoNoNetwork = new UnityEvent();
        private UnityEvent m_onVideoExpiring = new UnityEvent();
        private UnityEvent m_onVideoOpen = new UnityEvent();
        private UnityEvent m_onVideoClose = new UnityEvent();

        // standard banner events
        private UnityEvent m_onBannerAdAvailable = new UnityEvent();
        private UnityEvent m_onNoBannerAdAvailable = new UnityEvent();
        private UnityEvent m_onBannerError = new UnityEvent();
        private UnityEvent m_onBannerNoNetwork = new UnityEvent();
        private UnityEvent m_onBannerHide = new UnityEvent();


        // Property________________________________________________
        public static EasyTapsellManager Instance { get; private set; }
        public static bool IsInitialized
        {
            get;
            private set;
        }
        public string TapsellKey { get => m_tapsellKey; private set => m_tapsellKey = value; }
        public UnityEvent OnVideoAdCompeleted { get => m_onVideoAdCompeleted; private set => m_onVideoAdCompeleted = value; }
        public UnityEvent OnVideoAdCanceled { get => m_onVideoAdCanceled; private set => m_onVideoAdCanceled = value; }
        public UnityEvent OnVideoAdAvailable { get => m_onVideoAdAvailable; private set => m_onVideoAdAvailable = value; }
        public UnityEvent OnNoVideoAdAvailable { get => m_onNoVideoAdAvailable; private set => m_onNoVideoAdAvailable = value; }
        public UnityEvent OnVideoError { get => m_onVideoError; private set => m_onVideoError = value; }
        public UnityEvent OnVideoNoNetwork { get => m_onVideoNoNetwork; private set => m_onVideoNoNetwork = value; }
        public UnityEvent OnVideoExpiring { get => m_onVideoExpiring; private set => m_onVideoExpiring = value; }
        public UnityEvent OnVideoOpen { get => m_onVideoOpen; private set => m_onVideoOpen = value; }
        public UnityEvent OnVideoClose { get => m_onVideoClose; private set => m_onVideoClose = value; }

        public UnityEvent OnBannerAdAvailable { get => m_onBannerAdAvailable; private set => m_onBannerAdAvailable = value; }
        public UnityEvent OnNoBannerAdAvailable { get => m_onNoBannerAdAvailable; private set => m_onNoBannerAdAvailable = value; }
        public UnityEvent OnBannerError { get => m_onBannerError; private set => m_onBannerError = value; }
        public UnityEvent OnBannerNoNetwork { get => m_onBannerNoNetwork; private set => m_onBannerNoNetwork = value; }
        public UnityEvent OnBannerHide { get => m_onBannerHide; private set => m_onBannerHide = value; }


        // monoBehaviour___________________________________________________________
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null); // if object has parent DontDestroyOnLoad not working.
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
        private void InitializeTapsell(string tapsellkey)
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