using System;
using System.Collections.Generic;
using UnityEngine;
namespace Lugu.Utils.Debug
{
    public class DebugController : SingletonMonoPersistent<DebugController>
    {
        [SerializeField] private List<DebugGroup> m_debugGroups = new List<DebugGroup>();

        private Dictionary<string, bool> m_groupStatus = new Dictionary<string, bool>();

        #region Properties

        public List<DebugGroup> debugGroups
        {
            get { return m_debugGroups; }
            private set { m_debugGroups = value; }
        }

        public Dictionary<string, bool> groupStatus
        {
            get { return m_groupStatus; }
            private set { m_groupStatus = value; }
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();


            UpdateDebugGroups();
        }

        public bool HasGroup(string groupName)
        {
            m_groupStatus.TryGetValue(groupName, out bool isEnabled);
            return isEnabled;
        }

        [ContextMenu("Update Groups")]
        public void UpdateDebugGroups()
        {
            UnityEngine.Debug.Log("Updating Debug Groups...");

            for (int i = 0; i < m_debugGroups.Count; i++)
            {
                DebugGroup group = m_debugGroups[i];
                m_groupStatus[DebugUtil.CleanText(group.name)] = group.isEnabled;
            }

            DebugUtil.Log("Debug Groups Updated", "Debug1");
            DebugUtil.Log("Debug Groups Updated", DebugUtil.WarningStyle,"Debug2");

        }

    }
}


