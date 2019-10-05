using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public Door doorToOpen;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().interactAction = (pc) =>
        {
            doorToOpen.GetComponent<Animator>().SetTrigger("open");
        };
    }
}
