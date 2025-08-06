using UnityEngine;
namespace Lugu.Utils
{
    public abstract class SingletonMonoPersistent<T> : SingletonMono<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            base.Awake();

            if (m_instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}