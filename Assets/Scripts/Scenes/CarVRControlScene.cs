using KUsystem.Utils;
using UnityEngine;

namespace KUsystem.Scene
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