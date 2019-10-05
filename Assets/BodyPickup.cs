using UnityEngine;

[ExecuteInEditMode]
public class BodyPickup : MonoBehaviour
{
    public SkeletonSettingGrouping Settings;
    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        GetComponent<Interactable>().interactAction = UsePickup;
    }

    public void Update()
    {
        _animator.SetBool("Head", Settings.Head);
        _animator.SetBool("Torso", Settings.Torso);
        _animator.SetBool("Legs", Settings.Legs);
        _animator.SetBool("Wings", Settings.Wings);

        _animator.Update(Time.deltaTime);
    }

    public void UsePickup(PlayerController pc)
    {
        Settings.ApplyToPlayer(pc);
        pc.SetPositionToPickup(this);
        Destroy(gameObject);
    }
}
