using UnityEngine;
using UnityEngine.UI;

namespace EasyTapsell
{
    public class EasyTapsellBannerAdFakeShow : MonoBehaviour
    {
        // variable____________________________________________________________________
        [SerializeField] private GameObject m_fakePanel = null;
        [SerializeField] private Button m_btn_OnAdAvailable = null;
        [SerializeField] private Button m_btn_OnNoAdAvailable = null;
        [SerializeField] private Button m_btn_OnError = null;
        [SerializeField] private Button m_btn_OnNoNetwork = null;
        [SerializeField] private Button m_btn_OnHide = null;


        // property________________________________________________________________
        public static EasyTapsellBannerAdFakeShow Instance { get; private set; }
        public Button Btn_OnAdAvailable { get => m_btn_OnAdAvailable; private set => m_btn_OnAdAvailable = value; }
        public Button Btn_OnNoAdAvailable { get => m_btn_OnNoAdAvailable; private set => m_btn_OnNoAdAvailable = value; }
        public Button Btn_OnError { get => m_btn_OnError; private set => m_btn_OnError = value; }
        public Button Btn_OnNoNetwork { get => m_btn_OnNoNetwork; private set => m_btn_OnNoNetwork = value; }
        public Button Btn_OnHide { get => m_btn_OnHide; private set => m_btn_OnHide = value; }


        // monoBehaviour___________________________________________________________
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null); // if object has parent DontDestroyOnLoad not working.
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }


        // function________________________________________________________________
#if UNITY_ANDROID && UNITY_EDITOR
        public static void ShowFakeAd(EasyTapsellBannerAdCaller caller)
        {
            // Show fake ad dialog.
            Instance.ShowFakeAdDialog();

            // Add buttons click lesteners.

            // OnAdAvailable.
            Instance.Btn_OnAdAvailable.onClick.RemoveAllListeners();
            Instance.Btn_OnAdAvailable.onClick.AddListener(() =>
            {
                Instance.OnAdAvailable_Click(caller);
            });
            // OnNoAdAvailable.
            Instance.Btn_OnNoAdAvailable.onClick.RemoveAllListeners();
            Instance.Btn_OnNoAdAvailable.onClick.AddListener(() =>
            {
                Instance.OnNoAdAvailable_Click(caller);
            });
            // OnError.
            Instance.Btn_OnError.onClick.RemoveAllListeners();
            Instance.Btn_OnError.onClick.AddListener(() =>
            {
                Instance.OnError_Click(caller);
            });
            // OnNoNetwork.
            Instance.Btn_OnNoNetwork.onClick.RemoveAllListeners();
            Instance.Btn_OnNoNetwork.onClick.AddListener(() =>
            {
                Instance.OnNoNetwork_Click(caller);
            });
            // OnHide.
            Instance.Btn_OnHide.onClick.RemoveAllListeners();
            Instance.Btn_OnHide.onClick.AddListener(() =>
            {
                Instance.OnHide_Click(caller);
            });
        }
        private void ShowFakeAdDialog() => m_fakePanel.SetActive(true);
        private void CloseFakeAdDialog() => m_fakePanel.SetActive(false);

        private void OnAdAvailable_Click(EasyTapsellBannerAdCaller caller)
        {
            // Invoke TapsellManager events.
            EasyTapsellManager.Instance.OnBannerAdAvailable.Invoke();

            // Close fake ad dialog.
            CloseFakeAdDialog();
        }
        private void OnNoAdAvailable_Click(EasyTapsellBannerAdCaller caller)
        {
            // Invoke TapsellManager events.
            EasyTapsellManager.Instance.OnNoBannerAdAvailable.Invoke();

            // Close fake ad dialog.
            CloseFakeAdDialog();
        }
        private void OnError_Click(EasyTapsellBannerAdCaller caller)
        {
            // Invoke TapsellManager events.
            EasyTapsellManager.Instance.OnBannerError.Invoke();

            // Close fake ad dialog.
            CloseFakeAdDialog();
        }
        private void OnNoNetwork_Click(EasyTapsellBannerAdCaller caller)
        {
            // Invoke TapsellManager events.
            EasyTapsellManager.Instance.OnBannerNoNetwork.Invoke();

            // Close fake ad dialog.
            CloseFakeAdDialog();
        }
        private void OnHide_Click(EasyTapsellBannerAdCaller caller)
        {
            // Invoke TapsellManager events.
            EasyTapsellManager.Instance.OnBannerHide.Invoke();

            // Close fake ad dialog.
            CloseFakeAdDialog();
        }
#endif
    }
}