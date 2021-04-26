using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using Pashmak.Core;

namespace Pashmak.MessageBox
{

    public enum Alignment { UpperLeft, UpperCenter, UpperRight, MiddleLeft, MiddleCenter, MiddleRight, LowerLeft, LowerCenter, LowerRight };

    public class MessageBoxSkin : MonoBehaviour
    {
        // variable________________________________________________________________
        [SerializeField] private CU_ObjectDictionary m_objDict = null;
        [SerializeField] private UnityEvent m_onShow = new UnityEvent();
        [SerializeField] private UnityEvent m_onHide = new UnityEvent();
        private MessageBoxProfile m_messageBoxProfile;
        public const string KEY_TITLE = "default_title";
        public const string KEY_DESCRIPTION = "default_description";
        public const string KEY_PICTURE = "default_picture";


        // property________________________________________________________________
        public CU_ObjectDictionary ObjDict { get => m_objDict; private set => m_objDict = value; }
        public UnityEvent OnShow { get => m_onShow; set => m_onShow = value; }
        public UnityEvent OnHide { get => m_onHide; set => m_onHide = value; }
        public MessageBoxProfile Profile { get => m_messageBoxProfile; private set => m_messageBoxProfile = value; }


        // monoBehaviour___________________________________________________________
        private void Awake()
        {
            if (m_objDict == null)
            {
                m_objDict = GetComponent<CU_ObjectDictionary>();
            }
        }


        // function________________________________________________________________
        public void Show()
        {
            //OnShow UnityEvents.
            m_messageBoxProfile.m_onShow.Invoke();
            m_onShow.Invoke();

            //ShowSound.
            if (m_messageBoxProfile.ShowSound != null)
            {
                MessageBoxManager.AudioSource.PlayOneShot(m_messageBoxProfile.ShowSound);
            }

            //Check auto hide.
            if (m_messageBoxProfile.AutoHide)
            {
                StartCoroutine(Hide(m_messageBoxProfile.StayTimeInAutoHide));
            }
        }
        public IEnumerator Hide(float stayTime)
        {
            yield return new WaitForSeconds(stayTime);

            //OnHide UnityEvents.
            m_messageBoxProfile.m_onHide.Invoke();
            m_onHide.Invoke();
            MessageBoxManager.Instance.m_onHideMessage.Invoke();

            //ShowSound.
            if (m_messageBoxProfile.HideSound != null)
            {
                MessageBoxManager.AudioSource.PlayOneShot(m_messageBoxProfile.HideSound);
            }

            Destroy(gameObject);
        }
        public void SetContent(Dictionary<string, string> textDict, Dictionary<string, Sprite> imageDict)
        {
            // text
            if (textDict != null)
            {
                foreach (var textItem in textDict)
                {
                    if (m_objDict.ContainsKey(textItem.Key))
                        SetUIText(m_objDict[textItem.Key], textItem.Value);
                }
            }

            // image
            if (imageDict != null)
            {
                foreach (var imageItem in imageDict)
                {
                    if (m_objDict.ContainsKey(imageItem.Key))
                        m_objDict[imageItem.Key].GetComponent<Image>().sprite = imageItem.Value;
                }
            }
        }
        public void SetProfileContent()
        {
            // text
            if (m_messageBoxProfile.TextContents != null)
            {
                foreach (var textItem in m_messageBoxProfile.TextContents)
                {
                    if (m_objDict.ContainsKey(textItem.Key))
                        SetUIText(m_objDict[textItem.Key], textItem.Value);
                }
            }

            // image
            if (m_messageBoxProfile.ImageContents != null)
            {
                foreach (var imageItem in m_messageBoxProfile.ImageContents)
                {
                    if (m_objDict.ContainsKey(imageItem.Key))
                        m_objDict[imageItem.Key].GetComponent<Image>().sprite = imageItem.Value;
                }
            }
        }
        public void SetProfile(MessageBoxProfile profile)
        {
            m_messageBoxProfile = profile;
            SetProfileContent();
        }
        public virtual void SetUIText(GameObject uiElement, string value)
        {
            uiElement.GetComponent<Text>().text = value;
        }
    }
}