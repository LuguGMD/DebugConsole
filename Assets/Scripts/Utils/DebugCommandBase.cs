using UnityEngine;

public abstract class DebugCommandBase
{
    private string m_commandID;
    private string m_commandDescription;
    private string m_commandFormat;

    #region Properties

    public string commandID
    {
        get { return m_commandID; }
        private set { m_commandID = value; }
    }

    public string commandDescription
    {
        get { return m_commandDescription; }
        private set { m_commandDescription = value; }
    }

    public string commandFormat
    {
        get { return m_commandFormat; }
        private set { m_commandFormat = value; }
    }
    #endregion

    public DebugCommandBase(string commandID, string commandDescription, string commandFormat)
    {
        m_commandID = commandID;
        m_commandDescription = commandDescription;
        m_commandFormat = commandFormat;
    }
}
