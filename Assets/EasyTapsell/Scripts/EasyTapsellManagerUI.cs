using TapsellSDK;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTapsell
{
    public class EasyTapsellManagerUI : MonoBehaviour
    {
        // variable____________________________________________________________________
        [SerializeField] private RectTransform m_adLoadingDialog = null;


        // Property________________________________________________
        public static EasyTapsellManagerUI Instance { get; private set; }
        public RectTransform AdLoadingDialog { get => m_adLoadingDialog; private set => m_adLoadingDialog = value; }


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

            // Deactivate Ad loading dialog.
            HideAdLoadingDialog();
        }
        void Start()
        {
            // add Hide dialog after request video.
            EasyTapsellManager.Instance.OnVideoAdCompeleted.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnVideoAdCanceled.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnVideoAdAvailable.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnNoVideoAdAvailable.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnVideoError.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnVideoNoNetwork.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnVideoExpiring.AddListener(HideAdLoadingDialog);

            // add fake dialog buttons to events
            EasyTapsellManager.Instance.OnVideoAdCompeleted.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnAdCompeleted.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnVideoAdCanceled.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnCanceled.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnVideoAdAvailable.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnAdAvailable.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnNoVideoAdAvailable.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnNoAdAvailable.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnVideoError.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnError.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnVideoNoNetwork.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnNoNetwork.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnVideoExpiring.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnExpiring.gameObject.SetActive(true));
        }


        // function________________________________________________________________
        public void ShowAdLoadingDialog() => AdLoadingDialog.gameObject.SetActive(true);
        public void HideAdLoadingDialog() => AdLoadingDialog.gameObject.SetActive(false);
    }
}