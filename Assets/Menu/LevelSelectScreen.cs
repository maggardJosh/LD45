using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour
{
    public Transform LevelSelectContainer;
    public GameObject LevelSelectButtonPrefab;

    void Awake()
    {
        gameObject.SetActive(false);
        UpdateLevels();
    }

    public void UpdateLevels()
    {
        foreach (Transform t in LevelSelectContainer)
            Destroy(t.gameObject);//Just in case
        foreach (var scene in GetScenes())
        {
            var go = Instantiate(LevelSelectButtonPrefab, LevelSelectContainer);
            var lsb = go.GetComponent<LevelSelectButton>();
            lsb.WireUpSceneLoaded();
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
