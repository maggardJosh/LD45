using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public string sceneName = "";
    public LevelSelectScreen screen;

    public void LoadLevel()
    {
        screen.gameObject.SetActive(false);
        AudioManager.PlayOneShot(GameSettings.ResetSFX);
        LoadingScreen.Show(() => SceneManager.LoadScene(sceneName));
    }

    public void InitButton(string sceneName, LevelSelectScreen screen)
    {
        this.screen = screen;
        GetComponentInChildren<Text>().text = sceneName;
        this.sceneName = sceneName;
    }
}
