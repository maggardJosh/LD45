using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonScript : MonoBehaviour
{
    public void Restart()
    {
        AudioManager.PlayOneShot(GameSettings.ResetSFX);
        LoadingScreen.Show(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}
