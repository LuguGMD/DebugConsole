using System;
using System.Reflection;
using UnityEngine;

namespace Lugu.Console
{
    public class DebugCommand : DebugCommandBase
    {
        private MethodInfo m_command;
        private Type[] m_parameters;

        #region Properties

        public MethodInfo command
        {
            get { return m_command; }
            private set { m_command = value; }
        }

        public Type[] parameters
        {
            get { return m_parameters; }
            private set { m_parameters = value; }
        }

        #endregion

        public DebugCommand(string commandID, string commandDescription, MethodInfo command, params Type[] parameters) : base(commandID, commandDescription)
        {
            m_command = command;
            m_parameters = parameters;
        }

        public void Invoke(object obj, params object[] parameters)
        {
            command.Invoke(obj, parameters);
        }
    }

}
