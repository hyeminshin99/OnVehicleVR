using SeungHoon.Utils;
using UnityEngine;

namespace SeungHoon.Scene
{
    public class CarVRControlScene : BaseScene
    {
        public override void Clear()
        {
            
        }

        protected override void Init()
        {
            base.Init();

            SceneType = Define.EScene.CarVRControl;
        }
    }
}