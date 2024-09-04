using SeungHoon.Manager;
using UnityEngine;

namespace SeungHoon.Scene
{
    public class CarVRCircularLinearScene : BaseScene
    {
        public override void Clear()
        {
            
        }

        protected override void Init()
        {
            base.Init();

            SceneType = Utils.Define.EScene.CarVRCircularLinear;

            GeneratePoints();
        }
    }

}