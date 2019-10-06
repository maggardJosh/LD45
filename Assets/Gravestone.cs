using UnityEngine;
using UnityEngine.SceneManagement;

public class Gravestone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().interactAction = (pc) =>
        {
            GameSettings.SetLevelCompleted(SceneManager.GetActiveScene().name);
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (SceneManager.GetSceneByBuildIndex(i - 1) == SceneManager.GetActiveScene())
                {
                    LoadSceneByIndex(i);
                    return true;
                }

            }
            LoadSceneByIndex(0);
            return true;
        };
    }

    private static void LoadSceneByIndex(int i)
    {
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(i);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
        AudioManager.PlayOneShot(GameSettings.VictorySFX);
        LoadingScreen.Show(() => SceneManager.LoadScene(sceneName));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
