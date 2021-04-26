using TapsellSDK;
using UnityEngine;
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
        [SerializeField] private TapsellEventTrigger m_tplEventTrigger = null;
        [SerializeField] bool m_showLoadingDialog = false;
        [SerializeField] private bool m_autoAddRequestToButton = false;
        [SerializeField] bool m_autoShowAd = true;// If be true the ad will be shown after request automaticly.


        // property________________________________________________________________
        public TapsellAdType AdType { get => m_adType; private set => m_adType = value; }
        public TapsellAd Ad { get => m_ad; set => m_ad = value; }
        public bool Available { get => m_available; set => m_available = value; }
        public string ZoneId { get => m_zoneId; set => m_zoneId = value; }
        public TapsellEventTrigger TplEventTrigger { get => m_tplEventTrigger; private set => m_tplEventTrigger = value; }
        public bool ShowLoadingDialog { get => m_showLoadingDialog; set => m_showLoadingDialog = value; }
        public bool AutoAddRequestToButton { get => m_autoAddRequestToButton; private set => m_autoAddRequestToButton = value; }
        public bool AutoShowAd { get => m_autoShowAd; private set => m_autoShowAd = value; }


        // monoBehaviour___________________________________________________________
        private void Awake()
        {
            // Add request video ad for help automatically.
            if (AutoAddRequestToButton)
            {
                GetComponent<Button>().onClick.AddListener(RequestAd);
            }

            // check TplEventTrigger not null
            if (TplEventTrigger == null)
                TplEventTrigger = GetComponent<TapsellEventTrigger>();

            // Check show ad automatically or not after when video is availble.
            if (AutoShowAd)
                TplEventTrigger.OnAdAvailable.AddListener(ShowAd);
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
                    TplEventTrigger.OnAdAvailable.Invoke();

                    // Invoke TapsellManager events.
                    TapsellManager.Instance.OnAdAvailable.Invoke();
                },

                (string zoneId) =>
                {
                    // onNoAdAvailable
                    Debug.Log("No Ad Available");

                    // Invoke event.
                    TplEventTrigger.OnNoAdAvailable.Invoke();

                    // Invoke TapsellManager events.
                    TapsellManager.Instance.OnNoAdAvailable.Invoke();
                },

                (TapsellError error) =>
                {
                    // onError
                    Debug.Log(error.message);

                    // Invoke event.
                    TplEventTrigger.OnError.Invoke();

                    // Invoke TapsellManager events.
                    TapsellManager.Instance.OnError.Invoke();
                },

                (string zoneId) =>
                {
                    // onNoNetwork
                    Debug.Log("No Network: " + zoneId);

                    // Invoke event.
                    TplEventTrigger.OnNoNetwork.Invoke();

                    // Invoke TapsellManager events.
                    TapsellManager.Instance.OnNoNetwork.Invoke();
                },

                (TapsellAd result) =>
                {
                    //onExpiring
                    Debug.Log("Expiring");
                    this.Available = false;
                    this.Ad = null;

                    // Invoke event.
                    TplEventTrigger.OnExpiring.Invoke();

                    // Invoke TapsellManager events.
                    TapsellManager.Instance.OnExpiring.Invoke();
                    RequestAd(result.zoneId, false);
                }
            );
        }
        public void RequestAd()
        {
            if (ShowLoadingDialog)
                TapsellManager.Instance.ShowAdLoadingDialog();
#if UNITY_ANDROID && !UNITY_EDITOR
            // Set reward function.
            Tapsell.SetRewardListener(Tapsell_OnGetReward);

            // Get ad from tapsell.
            RequestAd(m_zoneId, false);
#elif UNITY_ANDROID && UNITY_EDITOR
            // Invoke OnAdAvailable events.
            if (AutoShowAd)
            {
                // Invoke event.
                TplEventTrigger.OnAdAvailable.Invoke();

                // Invoke TapsellManager events.
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
        private void Tapsell_OnGetReward(TapsellAdFinishedResult result)
        {
            // You can validate suggestion from you server by sending a request from your game server to tapsell, passing adId to validate it
            if ((result.completed && result.rewarded) || AdType == TapsellAdType.Banner_Interstitial || AdType == TapsellAdType.Video_Interstitial)
            {
                // Invoke event.
                if (TplEventTrigger != null)
                    TplEventTrigger.OnAdCompeleted.Invoke();

                // Invoke TapsellManager events.
                TapsellManager.Instance.OnAdCompeleted.Invoke();
            }
            else
            {
                // Invoke event.
                if (TplEventTrigger != null)
                    TplEventTrigger.OnAdCanceled.Invoke();

                // Invoke TapsellManager events.
                TapsellManager.Instance.OnAdCanceled.Invoke();
            }
        }
    }
}