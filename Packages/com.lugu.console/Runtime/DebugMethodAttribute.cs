using System;
using UnityEngine;


namespace Lugu.Console
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DebugMethodAttribute : Attribute
    {
        public string commandID;
        public string commandDescription;

        public DebugMethodAttribute(string commandID, string commandDescription = "")
        {
            this.commandID = commandID;
            this.commandDescription = commandDescription;
        }
    }
}