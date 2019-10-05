using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    public Door doorToOpen;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var pressed = IsPressed();
        _animator.SetBool("pressed", pressed);
        doorToOpen.GetComponentInChildren<Animator>().SetBool("open", pressed);
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
