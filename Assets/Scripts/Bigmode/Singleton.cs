using UnityEngine;

namespace Bigmode
{
    /// <summary>
    /// Registers and manages a single instance of your class type, also
    /// manages and fires events that are relevant to your classes lifecycle.
    /// </summary>
    /// <typeparam name="T">Type of your class instance.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
    {
        /// <summary>
        /// Get the current instance of this class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }
                return instance;
            }
        }

        /// <summary>
        /// Check if the instance exists. Returns true if an instance exists, otherwise false.
        /// </summary>
        public static bool Exists => instance != null;

        private static T instance = null;

        protected virtual void Awake()
        {
            instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}
