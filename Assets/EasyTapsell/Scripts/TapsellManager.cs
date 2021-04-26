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
        [SerializeField] private RectTransform m_adLoadingDialog = null;
        [SerializeField] private UnityEvent m_onAdCompeleted = new UnityEvent();
        [SerializeField] private UnityEvent m_onAdCanceled = new UnityEvent();
        [SerializeField] private UnityEvent m_onAdAvailable = new UnityEvent();
        [SerializeField] private UnityEvent m_onNoAdAvailable = new UnityEvent();
        [SerializeField] private UnityEvent m_onError = new UnityEvent();
        [SerializeField] private UnityEvent m_onNoNetwork = new UnityEvent();
        [SerializeField] private UnityEvent m_onExpiring = new UnityEvent();


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
                Instance = this;
            else Destroy(gameObject);

            // Deactivate Ad loading dialog.
            HideAdLoadingDialog();
        }
        void Start()
        {
            // Initialize Tapsell.
            if (m_initializeAtStart)
                InitializeTapsell();

            // add fake dialog buttons to events
            OnAdCompeleted.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnAdCompeleted.gameObject.SetActive(true));
            OnAdCanceled.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnCanceled.gameObject.SetActive(true));
            OnAdAvailable.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnAdAvailable.gameObject.SetActive(true));
            OnNoAdAvailable.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnNoAdAvailable.gameObject.SetActive(true));
            OnError.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnError.gameObject.SetActive(true));
            OnNoNetwork.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnNoNetwork.gameObject.SetActive(true));
            OnExpiring.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnExpiring.gameObject.SetActive(true));

            // add Hide dialog after request video.
            OnAdCompeleted.AddListener(TapsellManager.Instance.HideAdLoadingDialog);
            OnAdCanceled.AddListener(TapsellManager.Instance.HideAdLoadingDialog);
            OnAdAvailable.AddListener(TapsellManager.Instance.HideAdLoadingDialog);
            OnNoAdAvailable.AddListener(TapsellManager.Instance.HideAdLoadingDialog);
            OnError.AddListener(TapsellManager.Instance.HideAdLoadingDialog);
            OnNoNetwork.AddListener(TapsellManager.Instance.HideAdLoadingDialog);
            OnExpiring.AddListener(TapsellManager.Instance.HideAdLoadingDialog);
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
        public void ShowAdLoadingDialog() => AdLoadingDialog.gameObject.SetActive(true);
        public void HideAdLoadingDialog() => AdLoadingDialog.gameObject.SetActive(false);
    }
}