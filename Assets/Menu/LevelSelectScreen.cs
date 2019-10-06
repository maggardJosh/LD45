using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour
{
    public Transform LevelSelectContainer;
    public GameObject LevelSelectButtonPrefab;

    void Start()
    {
        gameObject.SetActive(false);
        UpdateLevels();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateLevels()
    {
        foreach (var scene in GetScenes())
        {
            var go = Instantiate(LevelSelectButtonPrefab, LevelSelectContainer);
            var lsb = go.GetComponent<LevelSelectButton>();
            lsb.InitButton(scene, this);
        }
    }

    private static string[] GetScenes()
    {
        List<string> result = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            result.Add(sceneName);
        }
        return result.ToArray();
    }

}
