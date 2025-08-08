using System.Diagnostics;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Lugu.Utils.Debug
{
    public static class DebugUtil
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

        public static void Log(object message, DebugStyle style, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (HasGroup(groupName[i]))
                {
                    Log($"{ApplyStyle(message, style)}");
                    return;
                }
            }
        }

        public static void Log(object message, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (HasGroup(groupName[i]))
                {
                    Log($"{message}");
                    return;
                }
            }
        }

        public static void Log(object message, string groupName)
        {
            if (HasGroup(groupName))
                Log($"{message}");
        }

        private static void Log(object message)
        {
            UnityEngine.Debug.Log($"{message}");
        }

        #endregion

        #region LogWarning

        public static void LogWarning(object message, DebugStyle style, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (HasGroup(groupName[i]))
                {
                    LogWarning($"{ApplyStyle(message, style)}");
                    return;
                }
            }
        }

        public static void LogWarning(object message, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (HasGroup(groupName[i]))
                {
                    LogWarning($"{message}");
                    return;
                }
            }
        }

        public static void LogWarning(object message, string groupName)
        {
            if (HasGroup(groupName))
                LogWarning($"{message}");
        }

        private static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning($"{message}");
        }

        #endregion

        #region LogError

        public static void LogError(object message, DebugStyle style, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (HasGroup(groupName[i]))
                {
                    LogError($"{ApplyStyle(message, style)}");
                    return;
                }
            }
        }

        public static void LogError(object message, params string[] groupName)
        {
            for (int i = 0; i < groupName.Length; i++)
            {
                if (HasGroup(groupName[i]))
                {
                    LogError($"{message}");
                    return;
                }
            }
        }

        public static void LogError(object message, string groupName)
        {
            if (HasGroup(groupName))
                LogError($"{message}");
        }

        private static void LogError(object message)
        {
            UnityEngine.Debug.LogError($"{message}");
        }

        #endregion

        private static bool HasGroup(string groupName)
        {
            if (DebugController.Instance == null)
                return false;

            return DebugController.Instance.HasGroup(CleanText(groupName));
        }

        public static string CleanText(string text)
        {
            return text.Replace(" ", "").ToLower();
        }

        private static object ApplyStyle(object message, DebugStyle style)
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
