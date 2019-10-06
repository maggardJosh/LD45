using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailureIndicator : MonoBehaviour
{

    private static FailureIndicator _instance;
    public static FailureIndicator Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<FailureIndicator>();
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public static void ShowFailureMessage(string message, Vector3 position)
    {
        AudioManager.PlayOneShot(GameSettings.FailSFX);
        Instance.GetComponentInChildren<Animator>().SetTrigger("StartFade");
        Instance.GetComponentInChildren<Text>().text = message;
        position.x = Mathf.Clamp(position.x, 1, 19);
        ((RectTransform)Instance.transform).anchoredPosition = position + Vector3.up;
    }
}
