using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Open(bool open = true)
    {
        GetComponentInChildren<Animator>().SetBool("open", open);
        if (open)
            GetComponentInChildren<ParticleSystem>().Play();
    }
}
