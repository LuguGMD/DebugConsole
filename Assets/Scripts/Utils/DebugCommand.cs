using System;
using System.Reflection;
using UnityEngine;

public class DebugCommand : DebugCommandBase
{
    private MethodInfo m_command;
    private object[] m_parameters;

    #region Properties

    public MethodInfo command
    {
        get { return m_command; }
        private set { m_command = value; }
    }

    public object[] parameters
    {
        get { return m_parameters; }
        private set { m_parameters = value; }
    }

    #endregion

    public DebugCommand(string commandID, string commandDescription, string commandFormat, MethodInfo command, params object[] parameters) : base(commandID, commandDescription, commandFormat)
    {
        m_command = command;
        m_parameters = parameters;
    }

    public void Invoke()
    {
        command.Invoke(this, parameters);
    }
}
