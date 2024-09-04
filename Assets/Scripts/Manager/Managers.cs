using UnityEngine;

namespace SeungHoon.Manager
{
    public class Managers: MonoBehaviour
    {
        private static Managers msInstance;

        private static Managers Instance
        {
            get
            {
                checkAndInitManager();
                return msInstance;
            }
        }

        private PoolManager mPoolInstance = new PoolManager();
        public static PoolManager Pool
        {
            get
            {
                return Instance.mPoolInstance;
            }
        }

        private ResourceManager mResourceInstance = new ResourceManager();
        public static ResourceManager Resource
        {
            get
            {
                return Instance.mResourceInstance;
            }
        }

        private SceneManagerEX mSceneInstance = new SceneManagerEX();
        public static SceneManagerEX Scene
        {
            get
            {
                return Instance.mSceneInstance;
            }
        }

        private SerialManager mSerialInstance = new SerialManager();
        public static SerialManager Serial
        {
            get
            {
                return Instance.mSerialInstance;
            }
        }

        public static void Clear()
        {
            Pool.Clear();
        }

        private static void checkAndInitManager()
        {
            if (msInstance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                    DontDestroyOnLoad(go);
                }
                msInstance = go.GetComponent<Managers>();

                msInstance.mPoolInstance.Init();
            }
        }
    }
}
