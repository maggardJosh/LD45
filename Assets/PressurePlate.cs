using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    public Door[] doorsToOpen;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    bool _lastPressed = false;
    // Update is called once per frame
    void Update()
    {
        var pressed = IsPressed();
        if(pressed != _lastPressed)
            AudioManager.PlayOneShot(GameSettings.DoorOpenSFX);
        _lastPressed = pressed;
        _animator.SetBool("pressed", pressed);
        foreach (var door in doorsToOpen)
            door.GetComponentInChildren<Animator>().SetBool("open", pressed);
    }

    private bool IsPressed()
    {
        foreach (var pickup in FindObjectsOfType<BodyPickup>())
        {
            if (!pickup.Settings.Head)
                continue;
            if (pickup.GetComponent<BoxCollider2D>().bounds.Intersects(_boxCollider.bounds))
            {

                return true;
            }
        }
        return false;
    }
}
