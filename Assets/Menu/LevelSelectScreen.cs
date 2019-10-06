using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScreen : MonoBehaviour
{
    public Transform LevelSelectContainer;
    public Transform TutorialSelectContainer;

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
        foreach (Transform t in TutorialSelectContainer)
            Destroy(t.gameObject);//Just in case

        var scenes = GetScenes();
        foreach (var scene in scenes.Where(s => s.StartsWith("T")))
            CreateButton(TutorialSelectContainer, scene);
        foreach (var scene in scenes.Where(s => !s.StartsWith("T")))
            CreateButton(LevelSelectContainer, scene);

    }

    private void CreateButton(Transform parent, string scene)
    {
        var go = Instantiate(LevelSelectButtonPrefab, parent);
        var lsb = go.GetComponent<LevelSelectButton>();
        lsb.WireUpSceneLoaded();
        lsb.InitButton(scene, this);
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
