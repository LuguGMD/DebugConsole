using System;
using UnityEngine;

namespace Lugu.Utils
{
    public abstract class Singleton<T>
    {
        private static T m_instance;

        #region Properties

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = (T)Activator.CreateInstance(typeof(T));
                }

                return m_instance;
            }

            private set
            {
                m_instance = value;
            }
        }

        #endregion
    }
}
