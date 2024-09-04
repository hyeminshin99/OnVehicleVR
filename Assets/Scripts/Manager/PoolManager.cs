using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using KUsystem.Utils;

namespace KUsystem.Manager
{
    public class PoolManager
    {
        #region Pool
        class Pool
        {
            public GameObject Original { get; private set; }
            public Transform Root { get; set; }

            private Stack<Poolable> mPoolStack = new Stack<Poolable>();

            public void Init(GameObject original, int count = 5)
            {
                Original = original;
                Root = new GameObject().transform;
                Root.name = $"{original.name}";

                for(int i = 0; i < count; ++i)
                {
                    Push(create());
                }
            }

            public Poolable Pop(Transform parent)
            {
                Poolable poolable;

                if(mPoolStack.Count >0)
                {
                    poolable = mPoolStack.Pop();
                }
                else
                {
                    poolable = create();
                }

                poolable.gameObject.SetActive(true);

                if(parent == null)
                {
                    poolable.transform.parent = Managers.Scene.CurrentScene.transform;
                }

                poolable.transform.parent = parent;
                poolable.IsUsing = true;

                return poolable;
            }
            
            public Poolable Pop (Vector3 position, Transform parent)
            {
                Poolable poolable;

                if(mPoolStack.Count > 0)
                {
                    poolable = mPoolStack.Pop();
                }
                else
                {
                    poolable = create(position);
                }

                poolable.gameObject.SetActive(true);

                if (parent == null)
                {
                    poolable.transform.parent = Managers.Scene.CurrentScene.transform;
                }

                poolable.transform.parent = parent;
                poolable.IsUsing = true;

                return poolable;
            }

            public void Push(Poolable poolable)
            {
                if(poolable == null)
                {
                    return;
                }

                poolable.transform.parent = Root;
                poolable.gameObject.SetActive(false);
                poolable.IsUsing = false;

                mPoolStack.Push(poolable);
            }

            private Poolable create()
            {
                GameObject go = Object.Instantiate<GameObject>(Original);
                go.name = Original.name;
                return go.GetOrAddComponent<Poolable>();
            }

            private Poolable create(Vector3 position)
            {
                GameObject go = Object.Instantiate<GameObject>(Original, position, Quaternion.identity);
                go.name = Original.name;
                return go.GetOrAddComponent<Poolable>();
            }
        }
        #endregion

        private Dictionary<string, Pool> mPool = new Dictionary<string, Pool>();
        private Transform mRoot;

        public void Clear()
        {
            foreach(Transform child in mRoot)
            {
                GameObject.Destroy(child.gameObject);
            }

            mPool.Clear();
        }

        public void CreatePool(GameObject original, int count = 5)
        {
            Pool pool = new Pool();
            pool.Init(original, count);
            pool.Root.parent = mRoot;

            mPool.Add(original.name, pool);
        }

        public void Init()
        {
            if(mRoot == null)
            {
                mRoot = new GameObject { name = "@Pool_Root" }.transform;
                Object.DontDestroyOnLoad(mRoot);
            }
        }

        public GameObject GetOriginal(string name)
        {
            if(mPool.ContainsKey(name) == false)
            {
                return null;
            }

            return mPool[name].Original;
        }

        public Poolable Pop(GameObject original, Transform parent = null)
        {
            if(mPool.ContainsKey(original.name) == false)
            {
                CreatePool(original);
            }

            return mPool[original.name].Pop(parent);
        }

        public Poolable Pop(GameObject original, Vector3 position, Transform parent = null)
        {
            if (mPool.ContainsKey(original.name) == false)
            {
                CreatePool(original);
            }

            return mPool[original.name].Pop(position, parent);
        }

        public void Push(Poolable poolable)
        {
            string name = poolable.gameObject.name;

            if(mPool.ContainsKey(name) == false)
            {
                GameObject.Destroy(poolable.gameObject);
                return;
            }

            mPool[name].Push(poolable);
        }
    }
}


