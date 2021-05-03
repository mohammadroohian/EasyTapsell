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
            EasyTapsellManager.Instance.OnAdCompeleted.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnAdCanceled.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnAdAvailable.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnNoAdAvailable.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnError.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnNoNetwork.AddListener(HideAdLoadingDialog);
            EasyTapsellManager.Instance.OnExpiring.AddListener(HideAdLoadingDialog);

            // add fake dialog buttons to events
            EasyTapsellManager.Instance.OnAdCompeleted.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnAdCompeleted.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnAdCanceled.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnCanceled.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnAdAvailable.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnAdAvailable.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnNoAdAvailable.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnNoAdAvailable.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnError.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnError.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnNoNetwork.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnNoNetwork.gameObject.SetActive(true));
            EasyTapsellManager.Instance.OnExpiring.AddListener(() => EasyTapsellAdFakeShow.Instance.Btn_OnExpiring.gameObject.SetActive(true));
        }


        // function________________________________________________________________
        public void ShowAdLoadingDialog() => AdLoadingDialog.gameObject.SetActive(true);
        public void HideAdLoadingDialog() => AdLoadingDialog.gameObject.SetActive(false);
    }
}