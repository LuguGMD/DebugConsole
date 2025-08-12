using UnityEngine;

namespace Lugu.Console
{
    public class DebugConsoleMethods : MonoBehaviour
    {
        [DebugMethod("TIME_SCALE")]
        public static void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }

        public static void ConsoleHelp()
        {

        }
    }
}
