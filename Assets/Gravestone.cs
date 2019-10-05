using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gravestone : MonoBehaviour
{
    public string nextScene = "";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().interactAction = (pc) => { SceneManager.LoadScene(nextScene); };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
