using UnityEngine;


namespace Lugu.Console
{
    public abstract class DebugCommandBase
    {
        private string m_commandID;
        private string m_commandDescription;

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
        #endregion

        public DebugCommandBase(string commandID, string commandDescription)
        {
            m_commandID = commandID;
            m_commandDescription = commandDescription;
        }
    }
}