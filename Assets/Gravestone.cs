using UnityEngine;
using UnityEngine.SceneManagement;

public class Gravestone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().interactAction = (pc) =>
        {
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if (SceneManager.GetSceneByBuildIndex(i - 1) == SceneManager.GetActiveScene())
                {
                    string pathToScene = SceneUtility.GetScenePathByBuildIndex(i);
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
                    AudioManager.PlayOneShot(GameSettings.VictorySFX);
                    LoadingScreen.Show(() => SceneManager.LoadScene(sceneName));
                    return true;
                }

            }
            FailureIndicator.ShowFailureMessage("Out of levels... sorry!", transform.position);
            return false;
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
