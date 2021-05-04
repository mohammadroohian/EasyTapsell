using TapsellSDK;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTapsell
{
    public class EasyTapsellVideoAdCaller : MonoBehaviour
    {
        public enum TapsellVideoAdType
        {
            Interstitial = 0,
            Rewarded = 1
        }


        // variable____________________________________________________________________
        [SerializeField] private TapsellVideoAdType m_adType = TapsellVideoAdType.Interstitial;
        private TapsellAd m_ad = null;
        private bool m_available = false;
        [SerializeField] private string m_zoneId = "YOUR ZONE ID";
        [SerializeField] private bool m_showLoadingDialog = true;
        [SerializeField] private bool m_autoAddRequestToButton = false;
        [SerializeField] private bool m_autoShowAd = true;// If be true the ad will be shown after request automaticly.


        // property________________________________________________________________
        public TapsellVideoAdType AdType { get => m_adType; private set => m_adType = value; }
        public TapsellAd Ad { get => m_ad; private set => m_ad = value; }
        public bool Available { get => m_available; set => m_available = value; }
        public string ZoneId { get => m_zoneId; set => m_zoneId = value; }
        public bool ShowLoadingDialog { get => m_showLoadingDialog; set => m_showLoadingDialog = value; }
        public bool AutoAddRequestToButton { get => m_autoAddRequestToButton; private set => m_autoAddRequestToButton = value; }
        public bool AutoShowAd { get => m_autoShowAd; private set => m_autoShowAd = value; }


        // monoBehaviour___________________________________________________________
        private void Awake()
        {
            // Add request video ad for help automatically.
            if (AutoAddRequestToButton)
                GetComponent<Button>().onClick.AddListener(RequestAd);
        }
        private void Start()
        {
            // Check show ad automatically or not after when video is availble.
            if (AutoShowAd)
                EasyTapsellManager.Instance.OnVideoAdAvailable.AddListener(ShowAd);
        }


        // function________________________________________________________________
        private void RequestAd(string zone, bool cached)
        {
            Tapsell.RequestAd(zone, cached,
                (TapsellAd result) =>
                {
                    // onAdAvailable
                    Debug.Log("On Ad Available");
                    this.Available = true;
                    this.Ad = result;

                    // Invoke TapsellManager events.
                    EasyTapsellManager.Instance.OnVideoAdAvailable.Invoke();
                },

                (string zoneId) =>
                {
                    // onNoAdAvailable
                    Debug.Log("No Ad Available");

                    // Invoke TapsellManager events.
                    EasyTapsellManager.Instance.OnNoVideoAdAvailable.Invoke();
                },

                (TapsellError error) =>
                {
                    // onError
                    Debug.Log(error.message);

                    // Invoke TapsellManager events.
                    EasyTapsellManager.Instance.OnVideoError.Invoke();
                },

                (string zoneId) =>
                {
                    // onNoNetwork
                    Debug.Log("No Network: " + zoneId);

                    // Invoke TapsellManager events.
                    EasyTapsellManager.Instance.OnVideoNoNetwork.Invoke();
                },

                (TapsellAd result) =>
                {
                    //onExpiring
                    Debug.Log("Expiring");
                    this.Available = false;
                    this.Ad = null;

                    // Invoke TapsellManager events.
                    EasyTapsellManager.Instance.OnVideoExpiring.Invoke();
                    RequestAd(result.zoneId, false);
                },

                (TapsellAd result) =>
                {
                    // onOpen
                    Debug.Log("open");

                    // Invoke TapsellManager events.
                    EasyTapsellManager.Instance.OnVideoOpen.Invoke();
                },

                (TapsellAd result) =>
                {
                    // onClose
                    Debug.Log("close");

                    // Invoke TapsellManager events.
                    EasyTapsellManager.Instance.OnVideoClose.Invoke();
                }
            );
        }
        public void RequestAd()
        {
            // loading dialog
            if (ShowLoadingDialog)
                EasyTapsellManagerUI.Instance.ShowAdLoadingDialog();

#if UNITY_ANDROID && !UNITY_EDITOR
            // Set reward function.
            Tapsell.SetRewardListener(Tapsell_OnGetReward);

            // Get ad from tapsell.
            RequestAd(m_zoneId, false);
#elif UNITY_ANDROID && UNITY_EDITOR
            // Invoke OnAdAvailable events.
            if (AutoShowAd)
            {
                // Invoke TapsellManager events.
                EasyTapsellManager.Instance.OnVideoAdAvailable.Invoke();
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
            EasyTapsellVideoAdFakeShow.ShowFakeAd(this);
            return;
#elif !UNITY_ANDROID
            Debug.LogError("TapsellVideoCaller just work on android");
#endif
        }
        private void Tapsell_OnGetReward(TapsellAdFinishedResult result)
        {
            // You can validate suggestion from you server by sending a request from your game server to tapsell, passing adId to validate it
            if ((result.completed && result.rewarded) || AdType == TapsellVideoAdType.Interstitial)
            {
                // Invoke TapsellManager events.
                EasyTapsellManager.Instance.OnVideoAdCompeleted.Invoke();
            }
            else
            {
                // Invoke TapsellManager events.
                EasyTapsellManager.Instance.OnVideoAdCanceled.Invoke();
            }
        }
    }
}