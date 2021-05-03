using TapsellSDK;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTapsell
{
    public class EasyTapsellBannerAdCaller : MonoBehaviour
    {
        public enum TapsellBannerAdType
        {
            Standard = 0,
            //Native = 1
        }

        public enum TapsellBannerGravity
        {
            Top = 1,
            Bottom = 2,
            Left = 3,
            Right = 4,
            Center = 5
        }

        public enum TapsellBannerType
        {
            BANNER_320x50 = 1,
            BANNER_320x100 = 2,
            BANNER_250x250 = 3,
            BANNER_300x250 = 4
        }


        // variable____________________________________________________________________
        [SerializeField] private TapsellBannerAdType m_adType = TapsellBannerAdType.Standard;
        private TapsellAd m_ad = null;
        private bool m_available = false;
        [SerializeField] private string m_zoneId = "YOUR ZONE ID";
        [SerializeField] private bool m_showLoadingDialog = true;
        [SerializeField] private bool m_autoAddRequestToButton = false;
        [SerializeField] private bool m_autoShowAd = true;// If be true the ad will be shown after request automaticly.
        [SerializeField] private TapsellBannerType m_bannerSize = TapsellBannerType.BANNER_250x250;
        [SerializeField] private TapsellBannerGravity m_horizontalGravity = TapsellBannerGravity.Bottom;
        [SerializeField] private TapsellBannerGravity m_verticalGravity = TapsellBannerGravity.Center;


        // property________________________________________________________________
        public TapsellBannerAdType AdType { get => m_adType; private set => m_adType = value; }
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
        private void RequestAd(string zone)
        {
            Tapsell.RequestBannerAd(zone, (int)m_bannerSize, (int)m_horizontalGravity, (int)m_verticalGravity,
                (string zoneId) =>
                {
                    // onAdAvailable
                    Debug.Log("On Ad Available");
                    this.Available = true;

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

                (string zoneId) =>
                {
                    Debug.Log("Hide Banner");
                }
            );
        }
        public void RequestAd()
        {
            if (ShowLoadingDialog)
                EasyTapsellManagerUI.Instance.ShowAdLoadingDialog();
#if UNITY_ANDROID && !UNITY_EDITOR
            // Get ad from tapsell.
            RequestAd(m_zoneId);
#elif UNITY_ANDROID && UNITY_EDITOR
            // Invoke OnAdAvailable events.
            if (AutoShowAd)
            {
                // Invoke TapsellManager events.
                EasyTapsellManager.Instance.OnBannerAdAvailable.Invoke();
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

            Tapsell.ShowBannerAd(ZoneId);
#elif UNITY_ANDROID && UNITY_EDITOR
            // show test panel in editor 
            //EasyTapsellAdFakeShow.ShowFakeVideoAdd(this);
            return;
#elif !UNITY_ANDROID
            Debug.LogError("TapsellVideoCaller just work on android");
#endif
        }
        public void Hide() => Tapsell.HideBannerAd(ZoneId);
    }
}