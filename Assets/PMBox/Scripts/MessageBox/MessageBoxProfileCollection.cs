using System.Collections.Generic;
using UnityEngine;

namespace Pashmak.MessageBox
{
    [CreateAssetMenu(fileName = "MessageBoxProfileCollection", menuName = "Pashmak/MessageBox/MessageBoxProfileCollection", order = 0)]
    public class MessageBoxProfileCollection : ScriptableObject
    {
        // variable________________________________________________________________
        [SerializeField] private List<MessageBoxProfile> m_MessageBoxProfiles = new List<MessageBoxProfile>();


        // property________________________________________________________________
        public List<MessageBoxProfile> MessageBoxProfiles { get => m_MessageBoxProfiles; }
    }
}