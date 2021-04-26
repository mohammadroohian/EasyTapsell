using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace Pashmak.Server.Ads._Tapsell
{
    public class TapsellManager : MonoBehaviour
    {
        // variable____________________________________________________________________
        [SerializeField] private bool m_initializeAtStart = true;
        [SerializeField] private string m_tapsellKey = "YOUR TAPSELL KEY";
        [SerializeField] private RectTransform m_adLoadingDialog = null;
        [SerializeField] public UnityEvent m_onAdCompeleted = null;
        [SerializeField] private UnityEvent m_onAdCanceled = null;
        [SerializeField] private UnityEvent m_onAdAvailable = null;
        [SerializeField] private UnityEvent m_onNoAdAvailable = null;
        [SerializeField] private UnityEvent m_onError = null;
        [SerializeField] private UnityEvent m_onNoNetwork = null;
        [SerializeField] private UnityEvent m_onExpiring = null;


        // Property________________________________________________
        public static TapsellManager Instance
        {
            get;
            private set;
        }
        public static bool IsInitialized
        {
            get;
            private set;
        }
        public string TapsellKey { get => m_tapsellKey; private set => m_tapsellKey = value; }
        public RectTransform AdLoadingDialog { get => m_adLoadingDialog; private set => m_adLoadingDialog = value; }
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
                Instance = this;
            else Destroy(gameObject);

            // Deactive videoLoading dialog.
            HideVideoloadingDialog();
        }
        void Start()
        {
            // Initialize Tappsell.
            if (m_initializeAtStart)
                InitializeTapsell();

            // add fake dialog buttons to events
            m_onAdCompeleted.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnAdCompeleted.gameObject.SetActive(true));
            OnAdCanceled.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnCanceled.gameObject.SetActive(true));
            OnAdAvailable.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnAdAvailable.gameObject.SetActive(true));
            OnNoAdAvailable.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnNoAdAvailable.gameObject.SetActive(true));
            OnError.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnError.gameObject.SetActive(true));
            OnNoNetwork.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnNoNetwork.gameObject.SetActive(true));
            OnExpiring.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnExpiring.gameObject.SetActive(true));
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
        public void ShowVideoloadingDialog() => AdLoadingDialog.gameObject.SetActive(true);
        public void HideVideoloadingDialog() => AdLoadingDialog.gameObject.SetActive(false);
    }
}