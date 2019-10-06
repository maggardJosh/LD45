using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (LoadingScreen.Instance == null)
            Instantiate(GameSettings.EssentialsPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
