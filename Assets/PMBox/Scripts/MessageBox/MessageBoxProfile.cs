using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Pashmak.MessageBox
{
    [CreateAssetMenu(fileName = "MessageBoxProfile", menuName = "Pashmak/MessageBox/MessageBoxProfile", order = 0)]
    public class MessageBoxProfile : ScriptableObject
    {
        // variable________________________________________________________________
        [SerializeField] private string m_key = "";
        [SerializeField] private MessageBoxSkin m_skin = null;
        [SerializeField] private Alignment m_alignment = Alignment.MiddleCenter;
        [SerializeField] private bool m_autoHide = true;
        [SerializeField] private float m_autoHideTime = 3;
        [SerializeField] private AudioClip m_showSound = null;
        [SerializeField] private AudioClip m_hideSound = null;

        public UnityEvent m_onShow;
        public UnityEvent m_onHide;

        [ReorderableList]
        [SerializeField] private List<TextContent> m_textContents = new List<TextContent>();
        [ReorderableList]
        [SerializeField] private List<ImageContent> m_imageContents = new List<ImageContent>();


        // property________________________________________________________________
        public string Key { get => m_key; }
        public MessageBoxSkin Skin { get => m_skin; }
        public Alignment Alignment { get => m_alignment; }
        public bool AutoHide { get => m_autoHide; }
        public float StayTimeInAutoHide { get => m_autoHideTime; }
        public AudioClip ShowSound { get => m_showSound; }
        public AudioClip HideSound { get => m_hideSound; }
        public List<TextContent> TextContents { get => m_textContents; private set => m_textContents = value; }
        public List<ImageContent> ImageContents { get => m_imageContents; private set => m_imageContents = value; }


    }
    // class___________________________________________________________________
    [System.Serializable]
    public class TextContent
    {
        // variable________________________________________________________________
        [SerializeField]
        private string m_key;
        [TextArea]
        [SerializeField]
        private string m_value;


        // property________________________________________________________________
        public string Key { get => m_key; private set => m_key = value; }
        public string Value { get => m_value; private set => this.m_value = value; }

    }
    [System.Serializable]
    public class ImageContent
    {
        // variable________________________________________________________________
        [SerializeField]
        private string m_key;
        [ShowAssetPreview]
        [SerializeField]
        private Sprite m_value;

        // property________________________________________________________________
        public string Key { get => m_key; private set => m_key = value; }
        public Sprite Value { get => m_value; private set => this.m_value = value; }
    }
}