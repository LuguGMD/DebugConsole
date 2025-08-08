using UnityEngine;

namespace Lugu.Utils
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T m_instance;

        #region Properties

        public static T Instance
        {
            get
            {
                return m_instance;
            }

            private set
            {
                m_instance = value;
            }
        }

        #endregion

        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}