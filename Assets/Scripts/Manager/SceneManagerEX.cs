using KUsystem.Scene;
using KUsystem.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KUsystem.Manager
{
    public class SceneManagerEX
    {
        public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

        public void LoadScene(Define.EScene type)
        {
            CurrentScene.Clear();
            SceneManager.LoadScene(getSceneName(type));
        }

        private string getSceneName(Define.EScene type)
        {
            string name = System.Enum.GetName(typeof(Define.EScene), type);

            return name;
        }
    }
}