using Assets.Library.WitUnitySdk.IOC.Context;
using Assets.Library.WitUnitySdk.Language.Constant;
using Assets.Library.WitUnitySdk.Language.Context;
using Assets.Library.WitUnitySdk.WitConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{

    /// <summary>
    /// Ӧ�ó��������ִ�д���
    /// </summary>
    internal class WitApplication : MonoBehaviour
    {
        /// <summary>
        /// Ӧ�ó���������
        /// </summary>
        public static AssemblyApplicationContext Context { get; private set; }

        /// <summary>
        /// ����汾
        /// </summary>
        public static string SoftwareVersion = "1.0.0";

        /// <summary>
        /// ���ʱ��ִ�д���
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Startup()
        {;
            // ��������
            LoadLanguage();
            // ��ʼ��������
            Context = new AssemblyApplicationContext(typeof(WitApplication).Assembly);
        }

        /// <summary>
        /// ��������
        /// </summary>
        private static void LoadLanguage()
        {
            string commandLine = Environment.CommandLine;

            print("commandLine: " + commandLine);

            if (string.IsNullOrEmpty(commandLine) == false && commandLine.Contains("lang=0"))
            {
                LanguageContext.Lang = LanguageConstant.ZHCN;
            }
            else if (string.IsNullOrEmpty(commandLine) == false && commandLine.Contains("lang=1"))
            {
                LanguageContext.Lang = LanguageConstant.EN;
            }
            else
            {
                LanguageContext.Lang = LanguageConstant.ZHCN;
            }
        }
    }
}
