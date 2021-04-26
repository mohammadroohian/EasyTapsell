using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Pashmak.Action
{
#if UNITY_EDITOR
    public class ActionMenu : Editor
    {
        [MenuItem("GameObject/UI/U_Action/Button_With_Function", false, 21)]
        static void SetValue()
        {
            // Add button.
            GameObject tmpButtonGO = new GameObject("Button");
            tmpButtonGO.AddComponent<Image>();
            tmpButtonGO.AddComponent<Button>();
            tmpButtonGO.transform.SetParent(Selection.activeTransform);
            tmpButtonGO.transform.localPosition = new Vector3(0, 0, 0);
            tmpButtonGO.transform.localScale = new Vector3(1, 1, 1);

            // Add function.
            GameObject tmpFunctionGO = new GameObject("loading...");
            tmpFunctionGO.AddComponent<FunctionNode>();
            tmpFunctionGO.transform.SetParent(tmpButtonGO.transform);
            tmpFunctionGO.GetComponent<FunctionNode>().FunctionName = "OnClick";

            // Add execute nodes.
            GameObject tmpEN_1 = new GameObject("loading...");
            tmpEN_1.transform.SetParent(tmpFunctionGO.transform);
            tmpEN_1.AddComponent<ExecuteNode>();

            GameObject tmpEN_2 = new GameObject("loading...");
            tmpEN_2.transform.SetParent(tmpFunctionGO.transform);
            tmpEN_2.AddComponent<ExecuteNode>();

            GameObject tmpEN_3 = new GameObject("loading...");
            tmpEN_3.transform.SetParent(tmpFunctionGO.transform);
            tmpEN_3.AddComponent<ExecuteNode>();

            // Add click event.
            UnityEditor.Events.UnityEventTools.AddPersistentListener(tmpButtonGO.GetComponent<Button>().onClick, tmpFunctionGO.GetComponent<FunctionNode>().Call);
        }
    }
#endif
}