using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Editor.ButlerScripts
{
    public class ButlerWindow : EditorWindow
    {
        [MenuItem("Window/Butler")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ButlerWindow));
        }
        string status = "";
        bool isRefreshing
        {
            get
            {
                try { return p != null && p.HasExited == false; }
                catch (Exception) { return false; }
            }
        }
        bool done = false;
        string refreshStatus = "";
        float count = 0;
        public bool autoRefresh = true;
        public int refreshSeconds = 2;

        public string ReleaseMessage = "";
        void OnGUI()
        {
            GUILayout.Space(10);
            if (isRefreshing)
            {
                refreshStatus = "Refreshing";
                for (int i = 0; i < (count * 10) % 3; i++)
                    refreshStatus += ".";
                Repaint();
            }
            GUILayout.Label(refreshStatus);
            GUILayout.Label(status, GUILayout.MinHeight(80));

            autoRefresh = GUILayout.Toggle(autoRefresh, "Auto-Refresh");
            refreshSeconds = EditorGUILayout.IntSlider("Refresh Seconds", refreshSeconds, 1, 15);
            if (GUILayout.Button(isRefreshing ? "Refreshing" : "Refresh"))
                RefreshStatus();

            GUILayout.Space(30);

            if (GUILayout.Button("Build All"))
                BuildScript.BuildAll();
            GUILayout.BeginHorizontal();
            if (ButlerSettings.Instance.BuildWeb)
                if (GUILayout.Button("Build Web"))
                    BuildScript.WebBuild();
            if (ButlerSettings.Instance.BuildPC)
                if (GUILayout.Button("Build PC"))
                    BuildScript.WindowsBuild();
            if (ButlerSettings.Instance.BuildAndroid)
                if (GUILayout.Button("Build Android"))
                    BuildScript.AndroidBuild();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (ButlerSettings.Instance.BuildWeb)
                if (GUILayout.Button("Push Web"))
                    BuildScript.WebPush();
            if (ButlerSettings.Instance.BuildPC)
                if (GUILayout.Button("Push PC"))
                    BuildScript.WindowsPush();
            if (ButlerSettings.Instance.BuildAndroid)
                if (GUILayout.Button("Push Android"))
                    BuildScript.AndroidPush();
            GUILayout.EndHorizontal();

            ReleaseMessage = GUILayout.TextArea(ReleaseMessage);
            if (GUILayout.Button("New Release"))
            {
                BuildScript.NewRelease(ReleaseMessage);
                ReleaseMessage = "";
            }
        }
        Process p;
        float nextRefresh = 0;
        private void Update()
        {
            count = Time.realtimeSinceStartup;

            if (autoRefresh && nextRefresh < Time.realtimeSinceStartup)
            {
                RefreshStatus();
                nextRefresh = Time.realtimeSinceStartup + 60;
            }
            if (done)
            {
                refreshStatus = "Refreshed at " + DateTime.Now.ToLongTimeString();
                nextRefresh = Time.realtimeSinceStartup + refreshSeconds;
                Repaint();
                done = false;
            }
            if (!isRefreshing && nextRefresh > Time.realtimeSinceStartup + refreshSeconds)
                nextRefresh = Time.realtimeSinceStartup + refreshSeconds;
        }
        private void RefreshStatus()
        {
            if (!isRefreshing)
            {
                status = "";
                p = new Process();
                p.StartInfo.FileName = "cmd";
                p.StartInfo.Arguments = $"/c butler status {ButlerSettings.Instance.UserName}/{ButlerSettings.Instance.GameName}";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.EnableRaisingEvents = true;

                p.OutputDataReceived += (s, e) =>
                {
                    string[] strings = e.Data.Split('|');
                    if (strings.Length > 1)
                    {

                        foreach (string st in strings)
                            if (!String.IsNullOrEmpty(st))
                                status += st + "\t";
                        status += "\n";
                    }
                };
                p.ErrorDataReceived += (s, e) =>
                {
                    UnityEngine.Debug.Log(e.Data);
                };
                p.Exited += (s, e) =>
                {
                    done = true;
                };
                p.Start();
                p.BeginOutputReadLine();
            }
        }

    }
}