using UnityEngine;

namespace Lugu.Utils.Debug
{
    public static class DebugUtil
    {
        public static void Log(string message, string groupName)
        {
            if (HasGroup(groupName))
                UnityEngine.Debug.Log(message);
        }

        public static void LogWarning(string message, string groupName)
        {
            if (HasGroup(groupName))
                UnityEngine.Debug.LogWarning(message);
        }

        public static void LogError(string message, string groupName)
        {
            if (HasGroup(groupName))
                UnityEngine.Debug.LogError(message);
        } 

        private static bool HasGroup(string groupName)
        {
            if(DebugController.Instance == null)
                return false;

            return DebugController.Instance.HasGroup(CleanText(groupName));
        }
        
        public static string CleanText(string text)
        {
            return text.Replace(" ", "").ToLower();
        }

    }
}
