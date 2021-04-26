using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EasyTapsell
{
    public class TapsellAdCaller : MonoBehaviour
    {
        public enum TapsellAdType
        {
            Video_Interstitial = 0,
            Video_Rewarded = 1,
            Banner_Interstitial = 2
        }


        // variable____________________________________________________________________
        [SerializeField] private TapsellAdType m_adType = TapsellAdType.Video_Interstitial;
        private TapsellAd m_ad = null;
        private bool m_available = false;
        [SerializeField] private string m_zoneId = "YOUR ZONE ID";
        private UnityEvent m_onAdCompeleted = new UnityEvent();
        private UnityEvent m_onAdCanceled = new UnityEvent();
        private UnityEvent m_onAdAvailable = new UnityEvent();
        public UnityEvent m_onNoAdAvailable = new UnityEvent();
        private UnityEvent m_onError = new UnityEvent();
        private UnityEvent m_onNoNetwork = new UnityEvent();
        private UnityEvent m_onExpiring = new UnityEvent();
        [SerializeField] bool m_showLodingDialog = false;
        [SerializeField] private bool m_autoAddRequestToButton = false;
        [SerializeField] bool m_autoShowAd = true;// If be true the ad will be shown after request automaticly.
        [SerializeField] bool m_invokeConfigEvents = false;


        // property________________________________________________________________
        public TapsellAdType AdType { get => m_adType; private set => m_adType = value; }
        public TapsellAd Ad { get => m_ad; set => m_ad = value; }
        public bool Available { get => m_available; set => m_available = value; }
        public string ZoneId { get => m_zoneId; set => m_zoneId = value; }
        public UnityEvent OnAdCompeleted { get => m_onAdCompeleted; private set => m_onAdCompeleted = value; }
        public UnityEvent OnAdCanceled { get => m_onAdCanceled; private set => m_onAdCanceled = value; }
        public UnityEvent OnAdAvailable { get => m_onAdAvailable; private set => m_onAdAvailable = value; }
        public UnityEvent OnError { get => m_onError; private set => m_onError = value; }
        public UnityEvent OnNoNetwork { get => m_onNoNetwork; private set => m_onNoNetwork = value; }
        public UnityEvent OnExpiring { get => m_onExpiring; private set => m_onExpiring = value; }
        public bool ShowLodingDialog { get => m_showLodingDialog; set => m_showLodingDialog = value; }
        public bool AutoAddRequestToButton { get => m_autoAddRequestToButton; private set => m_autoAddRequestToButton = value; }
        public bool AutoShowAd { get => m_autoShowAd; private set => m_autoShowAd = value; }
        public bool InvokeConfigEvents { get => m_invokeConfigEvents; private set => m_invokeConfigEvents = value; }


        // monoBehaviour___________________________________________________________
        private void Awake()
        {
            // Add request video ad for help automaticly.
            if (AutoAddRequestToButton)
            {
                GetComponent<Button>().onClick.AddListener(RequestAd);
            }

            // Check show ad automaticly or not after when video is avalible.
            if (AutoShowAd)
                OnAdAvailable.AddListener(ShowAd);

            // Add Hide dilog after request video.
            OnAdCompeleted.AddListener(TapsellManager.Instance.HideVideoloadingDialog);
            OnAdCanceled.AddListener(TapsellManager.Instance.HideVideoloadingDialog);
            m_onNoAdAvailable.AddListener(TapsellManager.Instance.HideVideoloadingDialog);
            OnError.AddListener(TapsellManager.Instance.HideVideoloadingDialog);
            OnNoNetwork.AddListener(TapsellManager.Instance.HideVideoloadingDialog);
            OnExpiring.AddListener(TapsellManager.Instance.HideVideoloadingDialog);
        }


        // function________________________________________________________________
        private void RequestAd(string zone, bool cached)
        {
            TapsellSDK.Tapsell.RequestAd(zone, cached,
                (TapsellAd result) =>
                {
                    // onAdAvailable
                    Debug.Log("Action: onAdAvailable");
                    this.Available = true;
                    this.Ad = result;

                    // Invoke event.
                    OnAdAvailable.Invoke();

                    // Invoke TapsellConfig events.
                    if (InvokeConfigEvents)
                        TapsellManager.Instance.OnAdAvailable.Invoke();
                },

                (string zoneId) =>
                {
                    // onNoAdAvailable
                    Debug.Log("No Ad Available");

                    // Invoke event.
                    m_onNoAdAvailable.Invoke();

                    // Invoke TapsellConfig events.
                    if (InvokeConfigEvents)
                        TapsellManager.Instance.OnNoAdAvailable.Invoke();
                },

                (TapsellError error) =>
                {
                    // onError
                    Debug.Log(error.message);

                    // Invoke event.
                    OnError.Invoke();

                    // Invoke TapsellConfig events.
                    if (InvokeConfigEvents)
                        TapsellManager.Instance.OnError.Invoke();
                },

                (string zoneId) =>
                {
                    // onNoNetwork
                    Debug.Log("No Network: " + zoneId);

                    // Invoke event.
                    OnNoNetwork.Invoke();

                    // Invoke TapsellConfig events.
                    if (InvokeConfigEvents)
                        TapsellManager.Instance.OnNoNetwork.Invoke();
                },

                (TapsellAd result) =>
                {
                    //onExpiring
                    Debug.Log("Expiring");
                    this.Available = false;
                    this.Ad = null;

                    // Invoke event.
                    OnExpiring.Invoke();

                    // Invoke TapsellConfig events.
                    if (InvokeConfigEvents)
                        TapsellManager.Instance.OnExpiring.Invoke();
                    RequestAd(result.zoneId, false);
                }
            );
        }
        public void RequestAd()
        {
            if (ShowLodingDialog)
                TapsellManager.Instance.ShowVideoloadingDialog();
#if UNITY_ANDROID && !UNITY_EDITOR
            // Set reward function.
            Tapsell.SetRewardListener(Tappsell_OnGetRiward);

            // Get ad from tapsell.
            RequestAd(m_zoneId, false);
#elif UNITY_ANDROID && UNITY_EDITOR
            // Invoke OnAdAvailable events.
            if (AutoShowAd)
            {
                // Invoke event.
                if (OnAdAvailable != null)
                    OnAdAvailable.Invoke();

                // Invoke TapsellConfig events.
                if (InvokeConfigEvents)
                    TapsellManager.Instance.OnAdAvailable.Invoke();
            }
#elif !UNITY_ANDROID
            Debug.LogError("TapsellVideoCaller just work on android");
#endif
        }
        public void ShowAd()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!m_available)
                return;

            // Set options to show ad.
            TapsellShowOptions options = new TapsellShowOptions();
            options.backDisabled = false;
            options.immersiveMode = false;
            options.rotationMode = TapsellShowOptions.ROTATION_LOCKED_LANDSCAPE;
            if (AdType == TapsellAdType.Video_Rewarded)
                options.showDialog = true;
            else
                options.showDialog = false;
            // Show ad.
            Tapsell.ShowAd(m_ad, options);
#elif UNITY_ANDROID && UNITY_EDITOR
            TapsellAdFakeShow.ShowFakeVideoAdd(this);
            return;
#elif !UNITY_ANDROID
            Debug.LogError("TapsellVideoCaller just work on android");
#endif
        }
        private void Tappsell_OnGetRiward(TapsellAdFinishedResult result)
        {
            // You can validate suggestion from you server by sending a request from your game server to tapsell, passing adId to validate it
            if ((result.completed && result.rewarded) || AdType == TapsellAdType.Banner_Interstitial || AdType == TapsellAdType.Video_Interstitial)
            {
                // Invoke event.
                OnAdCompeleted.Invoke();

                // Invoke TapsellConfig events.
                if (InvokeConfigEvents)
                    TapsellManager.Instance.m_onAdCompeleted.Invoke();
            }
            else
            {
                // Invoke event.
                OnAdCanceled.Invoke();

                // Invoke TapsellConfig events.
                if (InvokeConfigEvents)
                    TapsellManager.Instance.OnAdCanceled.Invoke();
            }
        }
    }
}