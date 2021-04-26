using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Pashmak.Action
{
    public abstract class ActionNode : MonoBehaviour
    {
        // variable________________________________________________________________
        [SerializeField] private bool m_enabledNode = true;
        [SerializeField] private bool m_isBreakpoint = false;
        [SerializeField] private string m_comment;
        private ActionNode m_parentNode;
        protected FunctionNode m_parentFunction;
        protected Queue<ActionNode> m_subActionNodes = new Queue<ActionNode>();
        protected ActionNode[] m_subActionNodesTemp;
        protected bool m_isDetected = false;
        protected bool m_isDone = false;


        // property________________________________________________________________
        public bool EnabledNode { get => m_enabledNode; set => m_enabledNode = value; }
        public string Comment { get => m_comment; private set => m_comment = value; }
        public ActionNode ParentNode { get => m_parentNode == null ? this : m_parentNode; private set => m_parentNode = value; }
        public FunctionNode ParentFunction { get => m_parentFunction; }
        public bool IsDetected { get => m_isDetected; }
        public bool IsDone { get => m_isDone; }
        public bool IsBreakpoint { get => m_isBreakpoint; }


        // monoBehaviour___________________________________________________________
        protected virtual void Awake()
        {
            //Set sub action nodes.
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).gameObject.activeInHierarchy) continue;
                ActionNode tmpAN = transform.GetChild(i).GetComponent<ActionNode>();
                if (tmpAN != null)
                {
                    m_subActionNodes.Enqueue(tmpAN);
                }
            }
            m_subActionNodesTemp = m_subActionNodes.ToArray();

            //Set parent node.
            m_parentNode = transform.parent.GetComponent<ActionNode>();

            //Set function.
            m_parentFunction = transform.GetComponentInParent<FunctionNode>();
        }
        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (IsDetected || IsDone)
            {
                this.name = this.name;
            }
#endif
        }


        // function________________________________________________________________
        public virtual void Execute()
        {
            if (m_isBreakpoint) return;
            if (EnabledNode && m_parentFunction.IsDetected)
            {
                Initialize();
                if (m_parentFunction.DebugMode)
                {
                    GameObject obj = this.gameObject;
                    string str = "";
                    while (true)
                    {
                        if (obj.GetComponent<FunctionNode>() != null &&
                            obj.GetComponent<FunctionNode>().Equals(m_parentFunction))
                        {
                            break;
                        }
                        else
                        {
                            obj = obj.transform.parent.gameObject;
                            str += "            ";
                        }
                        if (str.Length > 1000) break;
                    }
                    Debug.Log(str + this.name, this);
                }

                m_isDetected = true;
            }
            else
            {
                m_isDetected = true;
                ActionIsDone();
            }
        }
        protected virtual void ExecuteNextChildActionNode()
        {
            if (m_isBreakpoint) return;
            if (m_subActionNodes.Count == 0)
            {
                ActionIsDone();
            }
            else
                m_subActionNodes.Dequeue().Execute();
        }
        protected virtual void ExecuteNextActionNodeInParent()
        {
            if (m_isBreakpoint) return;
            m_parentNode.ExecuteNextChildActionNode();
        }
        public virtual void Initialize()
        {
            if (m_isBreakpoint) return;
            m_isDetected = false;
            m_isDone = false;
            if (m_subActionNodes.Count == m_subActionNodesTemp.Length) return;
            m_subActionNodes.Clear();
            for (int i = 0; i < m_subActionNodesTemp.Length; i++)
            {
                m_subActionNodes.Enqueue(m_subActionNodesTemp[i]);
            }
        }
        public void ActionIsDone()
        {
            if (m_isBreakpoint) return;
            m_isDone = true;
            ExecuteNextActionNodeInParent();
        }
    }
}