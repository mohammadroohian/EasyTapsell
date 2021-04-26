using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Events;
using Pashmak.Core;

namespace Pashmak.Action
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    partial class ActionHierarchy
    {
        static Dictionary<int, ActionNode> markedObjects = new Dictionary<int, ActionNode>();

        static ActionHierarchy()
        {
            EditorApplication.update += UpdateCB;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }

        static void UpdateCB()
        {
            markedObjects.Clear();
            ActionNode[] go = Object.FindObjectsOfType(typeof(ActionNode)) as ActionNode[];
            foreach (ActionNode g in go)
                markedObjects.Add(g.gameObject.GetInstanceID(), g);
        }

        static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            if (!markedObjects.ContainsKey(instanceID)) return;

            // place the icon to the right of the list:
            Rect newRect = new Rect(selectionRect);
            newRect.x = newRect.xMin - 5;
            newRect.width = 9;
            newRect.x = newRect.xMin - 16;

            ActionNode actionNode;
            markedObjects.TryGetValue(instanceID, out actionNode);
            if (actionNode == null) return;

            // show this node is active in hierarchy or not. not just itself. if have any deactive parent this must be false
            bool isActive = actionNode.EnabledNode;
            if (isActive) isActive = CheckAllParentIsActive(actionNode);

            #region warning
            // check warning for all child ExecuteNode nodes
            bool haveWarning = CheckWarning_ExecuteNode(actionNode, isActive);
            #endregion

            #region error
            // check error for all child ExecuteNode nodes
            bool haveError = CheckError_ExecuteNode(actionNode, isActive);
            #endregion

            if (actionNode is LoopNode)
                Compute_LoopNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is BreakNode)
                Compute_ExitLoopNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is ExecuteNode)
                Compute_ExecuteNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is ConditionNode)
                Compute_ConditionNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is ExecuteScriptNode)
                Compute_ExecuteScriptNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is WaitForSecondsNode)
                Compute_WaitTimeNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is ExitFunctionNode)
                Compute_ExitFunctionNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is FunctionNode)
                Compute_FunctionNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is CallFunctionNode)
                Compute_RunFunctionNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is WaitForCollisionNode)
                Compute_WaitForCollisionNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is WaitForKeyNode)
                Compute_WaitForKeyNode(actionNode, newRect, isActive, haveWarning, haveError);
            else if (actionNode is WaitForButtonNode)
                Compute_WaitForButtonNode(actionNode, newRect, isActive, haveWarning, haveError);
        }

        static void Compute_LoopNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            LoopNode loopNode = (LoopNode)actionNode;
            string typeName = "For";
            int tmpCommentIndex = 90;
            string detailsText = "";

            #region task information
            float fTmp = (float)(loopNode.EndTo - loopNode.StartFrom) / (float)loopNode.IncrementBy;
            int iTmp = (int)fTmp;
            if (fTmp - iTmp >= 0)
                iTmp++;
            if (loopNode.m_infinit)
            {
                typeName = "While";
                detailsText = " true:";
            }
            else
            {
                if (loopNode.IncrementBy > 1)
                    detailsText = string.Format(" i in range({0}, {1}, {2}):\t# total:{3}", loopNode.StartFrom, loopNode.EndTo, loopNode.IncrementBy, iTmp.ToString());
                else
                    detailsText = string.Format(" i in range({0}, {1}):\t# total:{2}", loopNode.StartFrom, loopNode.EndTo, iTmp.ToString());
            }
            #endregion

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void Compute_ExitLoopNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            BreakNode loopNode = (BreakNode)actionNode;
            string typeName = "Break";
            int tmpCommentIndex = 60;
            string detailsText = "";

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void Compute_ExecuteNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            ExecuteNode executeNode = (ExecuteNode)actionNode;
            string typeName = "> ";
            int tmpCommentIndex = 90;
            string detailsText = "";

            #region task information
            // check there is any target
            if (executeNode.OnExecute == null)
                return;
            if (executeNode.OnExecute.GetPersistentEventCount() > 0)
            {
                var eventTarget = executeNode.OnExecute.GetPersistentTarget(0);

                // Get target component name.
                string componentName = GetComponentNameFromUnityEventTarget(actionNode, eventTarget);

                // Get target gameObject name.
                string gameObjectName = GetGameObjectNameFromUnityEventTarget(actionNode, eventTarget);

                // Get target methud name.
                string methodName = executeNode.OnExecute.GetPersistentMethodName(0);
                string methodParams = "";

                // Set detailsText.
                if (eventTarget is IDetails)
                    detailsText = ((IDetails)eventTarget).GetDetails(gameObjectName, componentName, methodName, methodParams);
                else
                    detailsText = string.Format(" [ {0} ] . {1} . {2}({3})", gameObjectName, componentName, methodName, methodParams);
            }
            else if (executeNode.GetComponent<IDefaultExcute>() != null)
            {
                UnityEventTools.AddPersistentListener(executeNode.OnExecute, executeNode.GetComponent<IDefaultExcute>().DefaultExecute);
            }
            else
            {
                // Set detailsText.
                detailsText = string.Format(" [ --- ]");
            }
            #endregion

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_NODE);
            #endregion
        }
        static void Compute_ConditionNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            ConditionNode conditionNode = (ConditionNode)actionNode;
            string typeName = "if ";
            int tmpCommentIndex = 100;
            string detailsText = conditionNode.Condition.GetDetails();

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void Compute_ExecuteScriptNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            ExecuteScriptNode executeScriptNode = (ExecuteScriptNode)actionNode;
            string typeName = "Execute Scripts";
            int tmpCommentIndex = 100;
            string detailsText = "";

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void Compute_WaitTimeNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            WaitForSecondsNode waitTimeNode = (WaitForSecondsNode)actionNode;
            string typeName = "WaitForSeconds";
            int tmpCommentIndex = 130;
            string detailsText = string.Format("({0})", waitTimeNode.WaitTime);

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void Compute_ExitFunctionNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            ExitFunctionNode exitFunctionNode = (ExitFunctionNode)actionNode;
            string typeName = "return";
            int tmpCommentIndex = 70;
            string detailsText = "";

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void Compute_FunctionNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            FunctionNode FunctionNode = (FunctionNode)actionNode;
            string typeName = "def ";
            int tmpCommentIndex = 120;
            string detailsText =
                        FunctionNode.FunctionName != ""
                        ?
                            (FunctionNode.FunctionName.Length > 30
                            ?
                            FunctionNode.FunctionName.Substring(0, 30) + "..."
                            :
                            FunctionNode.FunctionName
                            +
                            "():")
                        :
                        "null";
            placeRect.x = placeRect.xMin - 3;

            #region show stuff
            // set Name
            SetNodeName(actionNode, typeName, detailsText, isActive);

            // show head and highlight
            ShowHeaders(actionNode, isActive, haveWarning, haveError, placeRect, COLOR_FunctionNode, "ƒ", true, 15);
            ShowHighlights(actionNode, isActive, haveWarning, haveError, placeRect, ChangeAlphaColor(COLOR_FunctionNode));

            // show comment
            if (!actionNode.GetComponentInParent<FunctionNode>().HideComments && !string.IsNullOrEmpty(actionNode.Comment))
            {
                ShowComment(actionNode, detailsText.ToCharArray().Length, isActive, placeRect, tmpCommentIndex);
            }
            #endregion
        }
        static void Compute_WaitForCollisionNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            WaitForCollisionNode runFunctionNode = (WaitForCollisionNode)actionNode;
            string typeName = "WaitForCollision";
            int tmpCommentIndex = 140;
            string detailsText = string.Format("(\"{0}\")", runFunctionNode.CollisionTag);

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void Compute_WaitForKeyNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            WaitForKeyNode runFunctionNode = (WaitForKeyNode)actionNode;
            string typeName = "WaitForKey";
            int tmpCommentIndex = 140;
            string detailsText = string.Format("(\"{0}\")", runFunctionNode.Key);

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void Compute_WaitForButtonNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            WaitForButtonNode runFunctionNode = (WaitForButtonNode)actionNode;
            string typeName = "WaitForButton";
            int tmpCommentIndex = 140;
            string detailsText = string.Format("(\"{0}\")", runFunctionNode.ButtonName);

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }

        static void Compute_RunFunctionNode(ActionNode actionNode, Rect placeRect, bool isActive, bool haveWarning, bool haveError)
        {
            CallFunctionNode runFunctionNode = (CallFunctionNode)actionNode;
            string typeName = "call ";
            int tmpCommentIndex = 120;
            string detailsText = runFunctionNode.FunctionToRun != null ?
                    (runFunctionNode.FunctionToRun.FunctionName.Length > 30 ? runFunctionNode.FunctionToRun.FunctionName.Substring(0, 30) + "..." : runFunctionNode.FunctionToRun.FunctionName + "()")
                    :
                    "null";

            #region show stuff
            ShowInformations_DefultStyle(actionNode,
                                        isActive,
                                        haveWarning,
                                        haveError,
                                        typeName,
                                        detailsText,
                                        placeRect,
                                        tmpCommentIndex,
                                        COLOR_SpacialNode);
            #endregion
        }
        static void ShowComment(ActionNode actionNode, int detailsTextLength, bool isActive, Rect newRect, int tmpCommentIndex)
        {
            // Font textFonts = Font.CreateDynamicFontFromOSFont("Consolas", 10);

            detailsTextLength = isActive ? 3 + detailsTextLength : 7 + detailsTextLength;
            GUIStyle commentLabelStyle = new GUIStyle();
            commentLabelStyle.normal.textColor = isActive ? ChangeAlphaColor(COLOR_COMMENTS, 1) : COLOR_COMMENTS_NOT_ENABLED;
            // commentLabelStyle.font = textFonts;

            int tmpl = (int)(detailsTextLength * 1.4f);

            // create initial gap
            StringBuilder sb_tmp = new StringBuilder();
            for (int i = 0; i < tmpl + 10; i++)
            {
                sb_tmp.Append(" ");
            }

            // set rect
            newRect.x = newRect.xMin + tmpCommentIndex;

            // show label
            GUI.Label(newRect, sb_tmp.ToString() + "// " + actionNode.Comment, commentLabelStyle); //Comment label.
        }
        static void ShowHighlights(ActionNode actionNode, bool isActive, bool haveWarning, bool haveError, Rect newRect, Color color = default(Color), string highlightSymbol = "█", bool isBold = false, int fontSize = 12, GUIStyle guiStyle = null)
        {
            Color highlightColor = GetHighlightColor(isActive, haveWarning, haveError, color);

            if (guiStyle == null)
            {
                guiStyle = new GUIStyle();
                guiStyle.normal.textColor = highlightColor;
                guiStyle.fontSize = fontSize;
                if (isBold) guiStyle.fontStyle = FontStyle.Bold;
            }

            // create highlight label
            StringBuilder sb_highlight = new StringBuilder();
            for (int i = 0; i < actionNode.name.Length; i++)
            {
                sb_highlight.Append(highlightSymbol);
            }

            GUI.Label(newRect, "     " + sb_highlight.ToString(), guiStyle); // highlight label.
        }
        static void ShowHeaders(ActionNode actionNode, bool isActive, bool haveWarning, bool haveError, Rect newRect, Color color = default(Color), string nodeHeadSymbol = "", bool isBold = false, int fontSize = 13, GUIStyle guiStyle = null)
        {
            Color headerColor = GetHeaderColor(actionNode, isActive, haveWarning, haveError, color);

            if (guiStyle == null)
            {
                guiStyle = new GUIStyle();
                guiStyle.normal.textColor = headerColor;
                guiStyle.fontSize = fontSize;
                if (isBold) guiStyle.fontStyle = FontStyle.Bold;
            }

            GUI.Label(newRect, nodeHeadSymbol, guiStyle); // head label.
        }
        static void ShowIcons(ActionNode actionNode, bool isActive, bool haveWarning, bool haveError, Rect newRect, Color color = default(Color))
        {
            Color highlightColor = GetHighlightColor(isActive, haveWarning, haveError, color);
            highlightColor = IsRuntime(actionNode) ? COLOR_Running : highlightColor;
            newRect.center += new Vector2(-4f, 2);
            newRect.width = 12;
            newRect.height = 12;
            GUI.DrawTexture(newRect, GetTexture2DFromActionNode(actionNode), ScaleMode.StretchToFill, true, 0.5f, highlightColor, 0, 0);
        }

        static string SetNodeName(ActionNode actionNode, string typeName, string detailsText, bool isActive)
        {
            string newName = typeName + detailsText;
            if (actionNode.name != newName && isActive)
                actionNode.name = typeName + detailsText;
            else if (actionNode.name != " # " + newName && !isActive)
                actionNode.name = " # " + newName;

            return newName;
        }

        static bool CheckWarning_ExecuteNode(ActionNode actionNode, bool isActive)
        {
            bool haveWarning = false;
            if (actionNode is ExecuteNode)
            {
                ExecuteNode executeNode = (ExecuteNode)actionNode;
                haveWarning = CheckWarning_EventElementLimit(actionNode, isActive, executeNode.OnExecute, 1, true, "OnExecute");
                if (haveWarning) return true;
            }

            // get all child execute nodes
            ExecuteNode[] tmpENS = actionNode.GetComponentsInChildren<ExecuteNode>();

            // check warning children of this node for OnExecute limit
            for (int i = 0; i < tmpENS.Length; i++)
            {
                if (CheckWarning_EventElementLimit(actionNode, isActive, tmpENS[i].OnExecute, 1))
                {
                    haveWarning = true;
                    break;
                }
            }
            return haveWarning;
        }
        static bool CheckWarning_EventElementLimit(ActionNode actionNode, bool isActive, UnityEvent uEvent, int limit, bool showLogWarning = false, string eventName = "event")
        {
            // warning for more than one element on event
            if (uEvent.GetPersistentEventCount() > limit)
            {
                if (showLogWarning) Debug.LogWarning(string.Format("Don't use more than {0} element on {1}!", limit, eventName), actionNode);
                return true;
            }
            return false;
        }
        static bool CheckError_ExecuteNode(ActionNode actionNode, bool isActive)
        {
            bool haveError = false;
            if (actionNode is ExecuteNode)
            {
                ExecuteNode executeNode = (ExecuteNode)actionNode;
                haveError = CheckEventError(actionNode, executeNode.OnExecute, true, "OnExecute");
                if (haveError) return true;
            }
            // get all child execute nodes
            ExecuteNode[] tmpENS = actionNode.GetComponentsInChildren<ExecuteNode>();

            // error in any children
            for (int i = 0; i < tmpENS.Length; i++)
            {
                if (CheckEventError(actionNode, tmpENS[i].OnExecute))
                {
                    haveError = true;
                    break;
                }
            }
            return haveError;
        }
        static bool CheckEventError(ActionNode actionNode, UnityEvent uEvent, bool showLogError = false, string eventName = "event")
        {
            if (
                uEvent.GetPersistentEventCount() > 0 &&
                    (
                    uEvent.GetPersistentTarget(0) == null ||
                    uEvent.GetPersistentMethodName(0) == "" ||
                    uEvent.GetPersistentTarget(0).ToString().Equals("Execute ()")
                    )
                )
            {
                if (showLogError) Debug.LogError(string.Format("Target or function is not setted on {0}", eventName), actionNode);
                return true;
            }
            return false;
        }

        static Color GetHighlightColor(bool isActive, bool haveWarning, bool haveError, Color normalColor = default(Color))
        {
            if (normalColor == default(Color)) normalColor = COLOR_NODE;

            if (haveError)
                return isActive ? COLOR_ERROR : COLOR_NULL_NOT_ENABLED;
            if (haveWarning)
                return isActive ? COLOR_WARNING : COLOR_WARNING_NOT_ENABLED;
            else
                return isActive ? normalColor : COLOR_NODE_NOT_ENABLED;
        }
        static Color GetHeaderColor(ActionNode actionNode, bool isActive, bool haveWarning, bool haveError, Color normalColor = default(Color))
        {
            if (normalColor == default(Color)) normalColor = ChangeAlphaColor(COLOR_NODE, 1);

            if (IsRuntime(actionNode)) return COLOR_Running;
            if (actionNode.IsBreakpoint) return COLOR_NULL_NOT_ENABLED;

            if (haveError)
                return isActive ? COLOR_ERROR : COLOR_NULL_NOT_ENABLED;
            else if (haveWarning)
                return isActive ? COLOR_WARNING : COLOR_WARNING_NOT_ENABLED;
            else
                return isActive ? normalColor : ChangeAlphaColor(COLOR_NODE_NOT_ENABLED, 1);
        }
        static void ShowInformations_DefultStyle(ActionNode actionNode, bool isActive, bool haveWarning, bool haveError, string typeName, string detailsText, Rect placeRect, int tmpCommentIndex, Color color = default(Color))
        {
            // set Name
            SetNodeName(actionNode, typeName, detailsText, isActive);

            // show head and highlight
            Color highlightColor = color;
            if (color != default(Color))
                highlightColor = ChangeAlphaColor(color, 0.2f);
            ShowHeaders(actionNode, isActive, haveWarning, haveError, placeRect, color);
            ShowHighlights(actionNode, isActive, haveWarning, haveError, placeRect, highlightColor);

            // show comment
            if (!actionNode.GetComponentInParent<FunctionNode>().HideComments && !string.IsNullOrEmpty(actionNode.Comment))
                ShowComment(actionNode, detailsText.ToCharArray().Length, isActive, placeRect, tmpCommentIndex);

            // show icon
            ShowIcons(actionNode, isActive, haveWarning, haveError, placeRect, color);
        }

        static Texture2D GetTexture2DFromActionNode(ActionNode actionNode)
        {
            Texture2D tmpTexture = AssetDatabase.LoadAssetAtPath("Assets/PashmakCore/Sprites/circle_One.png", typeof(Texture2D)) as Texture2D;
            return tmpTexture;
        }
        static bool IsRuntime(ActionNode actionNode)
        {
            return !((actionNode.IsDone && actionNode.IsDetected) || (!actionNode.IsDetected && !actionNode.IsDone));
        }
        static bool CheckAllParentIsActive(ActionNode actionNode)
        {
            ActionNode[] all_parents = actionNode.GetComponentsInParent<ActionNode>();
            for (int i = 0; i < all_parents.Length; i++)
            {
                if (!all_parents[i].EnabledNode) return false;
            }
            return true;
        }
        static string GetComponentNameFromUnityEventTarget(ActionNode actionNode, Object eventTarget)
        {
            if (eventTarget == null) return "";

            // Get target component name.
            if (actionNode.GetComponentInParent<FunctionNode>().ShowCompleteNodedetails)
            {
                // Get target object.
                string targetObj = eventTarget.ToString();
                return targetObj.Substring(targetObj.IndexOf('(') + 1, targetObj.IndexOf(')') - targetObj.IndexOf('(') - 1);
            }
            else
            {
                // Get target object.
                string targetObj = eventTarget.ToString();

                // Find some specific characters.
                int lastPoint = targetObj.LastIndexOf('.');
                int firstOpenPrt = targetObj.IndexOf('(');
                int lastClosePrt = targetObj.LastIndexOf(')');

                // Calculate start point.
                int startPoint = 0;
                if (lastPoint > 0)
                    startPoint = lastPoint + 1;
                else
                    startPoint = firstOpenPrt + 1;

                // Calculate end point.
                int subStringlength = 0;
                if (lastPoint > 0)
                    subStringlength = lastClosePrt - lastPoint - 1;
                else
                    subStringlength = lastClosePrt - firstOpenPrt - 1;

                // Get substring as component name.
                return targetObj.Substring(startPoint, subStringlength);
            }
        }
        static string GetGameObjectNameFromUnityEventTarget(ActionNode actionNode, Object eventTarget)
        {
            if (eventTarget == null) return "";

            string gameObjectName = eventTarget.name;
            bool checkTargetIsThisNode = false;// Show excute node and that target are the same.
            if (eventTarget is MonoBehaviour)
            {
                if (((MonoBehaviour)eventTarget).gameObject.Equals(actionNode.gameObject))
                {
                    checkTargetIsThisNode = true;
                }
            }
            else if (eventTarget is Component)
            {
                if (((Component)eventTarget).gameObject.Equals(actionNode.gameObject))
                {
                    checkTargetIsThisNode = true;
                }
            }
            else if (eventTarget is GameObject)
            {
                if (((GameObject)eventTarget).gameObject.Equals(actionNode.gameObject))
                {
                    checkTargetIsThisNode = true;
                }
            }
            if (checkTargetIsThisNode)// Check excute node and that target are the same or not.
                gameObjectName = "*";
            return gameObjectName;
        }
    }
#endif
}