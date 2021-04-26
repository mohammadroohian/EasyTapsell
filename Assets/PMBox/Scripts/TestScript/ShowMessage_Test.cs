using Pashmak.MessageBox;
using UnityEngine;


public class ShowMessage_Test : MonoBehaviour
{
    // variable________________________________________________________________
    public string m_title = "this test is title!";
    public string m_description = "this is a test description!";
    public string m_messageBoxID = "default";


    // monoBehaviour___________________________________________________________
    public void ShowTestMessage()
    {
        MessageBoxManager.ShowMessage(m_messageBoxID, m_title, m_description, null);
    }
}
