using System.Diagnostics;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Lugu.Utils.Debug
{
    public static class DebugLogger
    {
        #region Styles
        public readonly static DebugStyle DefaultStyle = new DebugStyle()
        {
            size = 12,
            color = Color.white
        };
        public readonly static DebugStyle WarningStyle = new DebugStyle()
        {
            size = 12,
            color = Color.yellow
        };
        public readonly static DebugStyle ErrorStyle = new DebugStyle()
        {
            size = 12,
            color = Color.red
        };
        public readonly static DebugStyle InfoStyle = new DebugStyle()
        {
            size = 12,
            color = Color.cyan
        };
        public readonly static DebugStyle SuccessStyle = new DebugStyle()
        {
            size = 12,
            color = Color.green
        };
        public readonly static DebugStyle BigStyle = new DebugStyle()
        {
            size = 16,
            color = Color.white
        };
        public readonly static DebugStyle SmallStyle = new DebugStyle()
        {
            size = 7,
            color = Color.white
        };
        #endregion

        #region Log
        [HideInCallstack]
        public static void Log(object message, DebugStyle style, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (LogHasGroup(groupName[i]))
                {
                    Log($"{LogApplyStyle(message, style)}");
                    return;
                }
            }
        }
        [HideInCallstack]
        public static void Log(object message, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (LogHasGroup(groupName[i]))
                {
                    Log($"{message}");
                    return;
                }
            }
        }
        [HideInCallstack]
        public static void Log(object message, string groupName)
        {
            if (LogHasGroup(groupName))
                Log($"{message}");
        }
        [HideInCallstack]
        [DebuggerHidden]
        private static void Log(object message)
        {
            UnityEngine.Debug.Log($"{message}");
        }

        #endregion

        #region LogWarning
        [HideInCallstack]
        public static void LogWarning(object message, DebugStyle style, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (LogHasGroup(groupName[i]))
                {
                    LogWarning($"{LogApplyStyle(message, style)}");
                    return;
                }
            }
        }
        [HideInCallstack]
        public static void LogWarning(object message, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (LogHasGroup(groupName[i]))
                {
                    LogWarning($"{message}");
                    return;
                }
            }
        }
        [HideInCallstack]
        public static void LogWarning(object message, string groupName)
        {
            if (LogHasGroup(groupName))
                LogWarning($"{message}");
        }
        [HideInCallstack]
        private static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning($"{message}");
        }

        #endregion

        #region LogError

        [HideInCallstack]
        public static void LogError(object message, DebugStyle style, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (LogHasGroup(groupName[i]))
                {
                    LogError($"{LogApplyStyle(message, style)}");
                    return;
                }
            }
        }
        [HideInCallstack]
        public static void LogError(object message, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (LogHasGroup(groupName[i]))
                {
                    LogError($"{message}");
                    return;
                }
            }
        }
        [HideInCallstack]
        public static void LogError(object message, string groupName)
        {
            if (LogHasGroup(groupName))
                LogError($"{message}");
        }
        [HideInCallstack]
        private static void LogError(object message)
        {
            UnityEngine.Debug.LogError($"{message}");
        }

        #endregion
        [HideInCallstack]
        private static bool LogHasGroup(string groupName)
        {
            if (DebugController.Instance == null)
                return false;

            return DebugController.Instance.HasGroup(LogCleanText(groupName));
        }
        [HideInCallstack]
        public static string LogCleanText(string text)
        {
            return text.Replace(" ", "").ToLower();
        }
        [HideInCallstack]
        private static object LogApplyStyle(object message, DebugStyle style)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(style.color)}><size={style.size}>{message}</color=#{ColorUtility.ToHtmlStringRGB(style.color)}></size={style.size}>";
        }

    }

    public class DebugStyle
    {
        public float size = 12;
        public Color color = Color.white;
    }
}
