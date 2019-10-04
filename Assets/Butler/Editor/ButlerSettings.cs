using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.ButlerScripts
{

    [CreateAssetMenu(fileName = "ButlerSettings", menuName = "Butler/CreateButlerScriptableObject", order = 1)]
    public class ButlerSettings : ScriptableObject
    {
        private static ButlerSettings butlerAsset;
        public static ButlerSettings Instance
        {
            get
            {
                if (butlerAsset == null)
                    butlerAsset = AssetDatabase.LoadAssetAtPath<ButlerSettings>("Assets/ButlerSettings.asset");
                if (butlerAsset == null)
                    throw new Exception("Unable to find ButlerSettings asset file");
                return butlerAsset;
            }
        }

        public string GameName = "square-slide";
        public string UserName = "maggardJosh";
        public string[] Scenes;
        public bool BuildAndroid = false;
        public bool BuildWeb = false;
        public bool BuildPC = false;
    }
}