using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { AudioManager.PlayOneShot(GameSettings.MenuBlipSFX); });
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
