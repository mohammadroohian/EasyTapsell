using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class TapsellManagerUI : MonoBehaviour
    {
        // variable____________________________________________________________________
        [SerializeField] private RectTransform m_adLoadingDialog = null;


        // Property________________________________________________
        public static TapsellManagerUI Instance { get; private set; }
        public RectTransform AdLoadingDialog { get => m_adLoadingDialog; private set => m_adLoadingDialog = value; }


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

            // Deactivate Ad loading dialog.
            HideAdLoadingDialog();
        }
        void Start()
        {
            // add Hide dialog after request video.
            TapsellManager.Instance.OnAdCompeleted.AddListener(HideAdLoadingDialog);
            TapsellManager.Instance.OnAdCanceled.AddListener(HideAdLoadingDialog);
            TapsellManager.Instance.OnAdAvailable.AddListener(HideAdLoadingDialog);
            TapsellManager.Instance.OnNoAdAvailable.AddListener(HideAdLoadingDialog);
            TapsellManager.Instance.OnError.AddListener(HideAdLoadingDialog);
            TapsellManager.Instance.OnNoNetwork.AddListener(HideAdLoadingDialog);
            TapsellManager.Instance.OnExpiring.AddListener(HideAdLoadingDialog);

            // add fake dialog buttons to events
            TapsellManager.Instance.OnAdCompeleted.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnAdCompeleted.gameObject.SetActive(true));
            TapsellManager.Instance.OnAdCanceled.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnCanceled.gameObject.SetActive(true));
            TapsellManager.Instance.OnAdAvailable.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnAdAvailable.gameObject.SetActive(true));
            TapsellManager.Instance.OnNoAdAvailable.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnNoAdAvailable.gameObject.SetActive(true));
            TapsellManager.Instance.OnError.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnError.gameObject.SetActive(true));
            TapsellManager.Instance.OnNoNetwork.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnNoNetwork.gameObject.SetActive(true));
            TapsellManager.Instance.OnExpiring.AddListener(() => TapsellAdFakeShow.Instance.Btn_OnExpiring.gameObject.SetActive(true));
        }


        // function________________________________________________________________
        public void ShowAdLoadingDialog() => AdLoadingDialog.gameObject.SetActive(true);
        public void HideAdLoadingDialog() => AdLoadingDialog.gameObject.SetActive(false);
    }
}