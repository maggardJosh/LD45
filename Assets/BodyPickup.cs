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

    public bool UsePickup(PlayerController pc)
    {
        if (pc.WillColliderHeightCollide(transform.position, Settings.GetCombinedGroupWithPlayer(pc).CharacterSettings.ColliderHeight))
        {
            FailureIndicator.ShowFailureMessage("Not Enough Room", transform.position);
            return false;
        }

        if(Settings.GetCombinedGroupWithPlayer(pc) == pc.CurrentSetting)
        {
            FailureIndicator.ShowFailureMessage("Already Have That Body Part", transform.position);
            return false;
        }

        var overlap = Settings.GetOverlapGroupWithPlayer(pc);
        Settings.ApplyToPlayer(pc);
        pc.SetPositionToPickup(this);

        if (overlap.Head || overlap.Torso || overlap.Legs)
        {
            Settings = overlap;
        }
        else
        {
            Destroy(gameObject);
        }
        AudioManager.PlayOneShot(GameSettings.PickupPartSFX);
        return true;
    }
}
