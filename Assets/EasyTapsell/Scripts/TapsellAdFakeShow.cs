using UnityEngine;
using UnityEngine.UI;

namespace EasyTapsell
{
    public class TapsellAdFakeShow : MonoBehaviour
    {
        // variable____________________________________________________________________
        private static TapsellAdFakeShow m_instance;
        [SerializeField] private Button m_btn_OnAdCompeleted = null;
        [SerializeField] private Button m_btn_OnCanceled = null;
        [SerializeField] private Button m_btn_OnAdAvailable = null;
        [SerializeField] private Button m_btn_OnNoAdAvailable = null;
        [SerializeField] private Button m_btn_OnError = null;
        [SerializeField] private Button m_btn_OnNoNetwork = null;
        [SerializeField] private Button m_btn_OnExpiring = null;


        // property________________________________________________________________
        public static TapsellAdFakeShow Instance { get => m_instance; set => m_instance = value; }
        public Button Btn_OnAdCompeleted { get => m_btn_OnAdCompeleted; private set => m_btn_OnAdCompeleted = value; }
        public Button Btn_OnCanceled { get => m_btn_OnCanceled; private set => m_btn_OnCanceled = value; }
        public Button Btn_OnAdAvailable { get => m_btn_OnAdAvailable; private set => m_btn_OnAdAvailable = value; }
        public Button Btn_OnNoAdAvailable { get => m_btn_OnNoAdAvailable; private set => m_btn_OnNoAdAvailable = value; }
        public Button Btn_OnError { get => m_btn_OnError; private set => m_btn_OnError = value; }
        public Button Btn_OnNoNetwork { get => m_btn_OnNoNetwork; private set => m_btn_OnNoNetwork = value; }
        public Button Btn_OnExpiring { get => m_btn_OnExpiring; private set => m_btn_OnExpiring = value; }


        // monoBehaviour___________________________________________________________
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                gameObject.transform.SetParent(null);// if object has parent DontDestroyOnLoad not working.
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }


        // function________________________________________________________________
#if UNITY_ANDROID && UNITY_EDITOR
        public static void ShowFakeVideoAdd(TapsellAdCaller caller)
        {
            // Show fake ad dialog.
            Instance.ShowFakeVideoAddDialog();

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
            // OnExpiring.
            Instance.Btn_OnExpiring.onClick.RemoveAllListeners();
            Instance.Btn_OnExpiring.onClick.AddListener(() =>
            {
                Instance.OnExpiring_Click(caller);
            });

            // OnAdCompeleted.
            Instance.Btn_OnAdCompeleted.onClick.RemoveAllListeners();
            Instance.Btn_OnAdCompeleted.onClick.AddListener(() =>
            {
                Instance.OnAdCompeleted_Click(caller);
            });
            // OnCancel.
            Instance.Btn_OnCanceled.onClick.RemoveAllListeners();
            Instance.Btn_OnCanceled.onClick.AddListener(() =>
            {
                Instance.OnCancel_Click(caller);
            });
        }
        private void ShowFakeVideoAddDialog()
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        private void CloseFakeVideoAddDialog()
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
        private void OnAdCompeleted_Click(TapsellAdCaller caller)
        {
            // Invoke TapsellConfig events.
            if (caller.InvokeConfigEvents)
                TapsellManager.Instance.m_onAdCompeleted.Invoke();

            // Invoke TapsellVideoCaller events
            if (caller.OnAdCompeleted != null)
                caller.OnAdCompeleted.Invoke();

            // Close fake add dialog.
            CloseFakeVideoAddDialog();
        }
        private void OnCancel_Click(TapsellAdCaller caller)
        {
            // Invoke TapsellConfig events.
            if (caller.InvokeConfigEvents)
                TapsellManager.Instance.OnAdCanceled.Invoke();

            // Invoke TapsellVideoCaller events
            if (caller.OnAdCanceled != null)
                caller.OnAdCanceled.Invoke();

            // Close fake ad dialog.
            CloseFakeVideoAddDialog();
        }
        private void OnAdAvailable_Click(TapsellAdCaller caller)
        {
            // Invoke TapsellConfig events.
            if (caller.InvokeConfigEvents)
                TapsellManager.Instance.OnAdAvailable.Invoke();

            // Invoke TapsellVideoCaller events
            if (caller.OnAdAvailable != null)
                caller.OnAdAvailable.Invoke();

            // Close fake ad dialog.
            CloseFakeVideoAddDialog();
        }
        private void OnNoAdAvailable_Click(TapsellAdCaller caller)
        {
            // Invoke TapsellConfig events.
            if (caller.InvokeConfigEvents)
                TapsellManager.Instance.OnNoAdAvailable.Invoke();

            // Invoke TapsellVideoCaller events
            if (caller.m_onNoAdAvailable != null)
                caller.m_onNoAdAvailable.Invoke();

            // Close fake ad dialog.
            CloseFakeVideoAddDialog();
        }
        private void OnError_Click(TapsellAdCaller caller)
        {
            // Invoke TapsellConfig events.
            if (caller.InvokeConfigEvents)
                TapsellManager.Instance.OnError.Invoke();

            // Invoke TapsellVideoCaller events
            if (caller.OnError != null)
                caller.OnError.Invoke();

            // Close fake ad dialog.
            CloseFakeVideoAddDialog();
        }
        private void OnNoNetwork_Click(TapsellAdCaller caller)
        {
            // Invoke TapsellConfig events.
            if (caller.InvokeConfigEvents)
                TapsellManager.Instance.OnNoNetwork.Invoke();

            // Invoke TapsellVideoCaller events
            if (caller.OnNoNetwork != null)
                caller.OnNoNetwork.Invoke();

            // Close fake ad dialog.
            CloseFakeVideoAddDialog();
        }
        private void OnExpiring_Click(TapsellAdCaller caller)
        {
            // Invoke TapsellConfig events.
            if (caller.InvokeConfigEvents)
                TapsellManager.Instance.OnExpiring.Invoke();

            // Invoke TapsellVideoCaller events
            if (caller.OnExpiring != null)
                caller.OnExpiring.Invoke();

            // Close fake ad dialog.
            CloseFakeVideoAddDialog();
        }
#endif
    }
}