using UnityEngine;

namespace Utility
{
    /** Assume that the object prefab will be initialized in the first scene. Never creates a new object. */
    public abstract class SingletonScene<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();

        public static bool HasInstance
        {
            get
            {
                return _instance != null;
            }
        }

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (_lock)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                }
                if (_instance == null)
                {
                    Debug.LogError("[SingletonScene] Please initialized the instance of " + typeof(T)
                        + " by dragging prefab in first scene. Child class must call base Awake");
                }
                return _instance;
            }
        }

        /** You must call base Awake */
        public virtual void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this as T;
                onDestroyed = false;
            }
        }


        private bool onDestroyed = false;
        public virtual void OnDestroy()
        {
            if (!onDestroyed)
            {
                onDestroyed = true;
                Save();
            }
        }

        public void OnApplicationQuit()
        {
            if (!onDestroyed)
            {
                onDestroyed = true;
                Save();
            }
        }

        void OnApplicationPause(bool value)
        {
            if (value)
            {
                Save();
            }
            else
            {
                RefreshPause();
            }
        }


        protected virtual void Save()
        {
        }

        protected virtual void RefreshPause()
        {

        }
    }
}
