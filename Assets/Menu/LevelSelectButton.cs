using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public string sceneName = "";
    public LevelSelectScreen screen;
    public GameObject levelCompletedInd;
    public GameObject currentLevelInd;

    public void WireUpSceneLoaded()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void LoadLevel()
    {
        screen.gameObject.SetActive(false);
        AudioManager.PlayOneShot(GameSettings.ResetSFX);
        LoadingScreen.Show(() => SceneManager.LoadScene(sceneName));
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UpdateButton();
    }

    public void InitButton(string sceneName, LevelSelectScreen screen)
    {
        this.screen = screen;
        GetComponentInChildren<Text>().text = sceneName;
        this.sceneName = sceneName;
        UpdateButton();
    }

    public void UpdateButton()
    {
        SetCurrentLevelInd();
        SetCompletedInd();
    }

    private void SetCompletedInd()
    {
        levelCompletedInd.SetActive(GameSettings.LevelCompleted(sceneName));
    }

    private void SetCurrentLevelInd()
    {
        currentLevelInd.SetActive(SceneManager.GetActiveScene().name.Equals(sceneName, StringComparison.CurrentCultureIgnoreCase));
    }
}
