using UnityEngine;
using TMPro;

namespace Pashmak.MessageBox
{
    public class MessageBoxSkin_TMP : MessageBoxSkin
    {
        // override________________________________________________________________
        public override void SetUIText(GameObject uiElement, string value)
        {
            uiElement.GetComponent<TextMeshProUGUI>().text = value;
        }
    }
}