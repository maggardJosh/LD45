using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public Door[] doorsToOpen;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Interactable>().interactAction = (pc) =>
        {
            foreach (var doorToOpen in doorsToOpen)
                doorToOpen.GetComponentInChildren<Animator>().SetBool("open", true);
            AudioManager.PlayOneShot(GameSettings.DoorOpenSFX);
            return true;
        };
    }
}
