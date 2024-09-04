using KUsystem.Manager;
using KUsystem.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KUsystem.Scene
{
    public abstract class BaseScene: MonoBehaviour
    {
        #region PoissonDiskSampling
        [SerializeField]
        private float mRadius = 2.0f;

        [SerializeField]
        private Vector3 mRegionSize = new Vector3(30, 3, 30);

        [SerializeField]
        private int mRejectionSamples = 30;

        private List<Vector3> mPoints = new List<Vector3>();
        #endregion
        public Define.EScene SceneType { get; protected set; } = Define.EScene.Unknown;

        private void Awake()
        {
            Init(); 
        }

        public abstract void Clear();

        protected void GeneratePoints()
        {      
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();            

            mPoints = PoissonDiskSampling.GeneratePoints(mRadius, mRegionSize, mRejectionSamples);

            if(mPoints != null)
            {
                foreach(Vector3 point in mPoints)
                {
                    Managers.Resource.InstantiateWithPosition("Vection/Cube", point);
                }
            }

            stopwatch.Stop();
            float elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0f;
            UnityEngine.Debug.Log("Elapsed Time: " + stopwatch.ElapsedMilliseconds + " ms"); //경과시간
            UnityEngine.Debug.Log("Generated Number of points: " + mPoints.Count); //생성된 점의 개수
            UnityEngine.Debug.Log("Generated points Number per second: " + (mPoints.Count / elapsedSeconds / 100)); //초당 생성된 점의 개수

        }

        protected virtual void Init()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(mRegionSize / 2, mRegionSize);
        }
    }
}
