using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LoadingScreen : MonoBehaviour
{
    private Image _image;

    private static LoadingScreen _instance;
    public static LoadingScreen Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LoadingScreen>();
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _image = GetComponent<Image>();
    }

    private Action loadedAction;

    private enum State
    {
        IN,
        GOING_OUT,
        OUT,
        GOING_IN
    }

    private State currentState = State.IN;
    private float count = 0;

    void Update()
    {
        switch (currentState)
        {
            case State.IN:
                loadedAction?.Invoke();
                currentState = State.GOING_OUT;
                count = 0;
                break;
            case State.GOING_OUT:
                count += Time.deltaTime;
                float t = count / GameSettings.LoadSceneFadeTime;
                Color imageColor = _image.color;
                imageColor.a = GameSettings.LoadOutCurve.Evaluate(t);
                _image.color = imageColor;
                if (t >= 1)
                    currentState = State.OUT;
                break;
            case State.OUT:
                count = 0;
                //Do nothing
                break;
            case State.GOING_IN:
                count += Time.deltaTime;
                t = count / GameSettings.LoadSceneFadeTime;
                imageColor = _image.color;
                imageColor.a = GameSettings.LoadInCurve.Evaluate(t);
                _image.color = imageColor;
                if (t >= 1)
                    currentState = State.IN;
                break;
        }
    }

    public static void Show(Action finishLoadingAction)
    {
        if (_instance.currentState != State.OUT)
            return;
        _instance.currentState = State.GOING_IN;
        _instance.loadedAction = finishLoadingAction;
    }
}
