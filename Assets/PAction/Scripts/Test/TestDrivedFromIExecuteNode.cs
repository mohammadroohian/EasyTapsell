using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pashmak.Action;

namespace Pashmak.Action
{
    public class TestDrivedFromIExecuteNode : MonoBehaviour, IExecuteNode
    {
        public string str = "";

        public void ExcuteScript(ActionNode sender)
        {
            print(str);
            print(sender.gameObject.name);
        }
    }
}