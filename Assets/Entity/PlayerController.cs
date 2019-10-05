using UnityEngine;

[RequireComponent(typeof(BaseEntity))]
public class PlayerController : MonoBehaviour
{
    private BaseEntity _entity;
    public SkeletonSettingGrouping CurrentSetting;

    public SkeletonSettingGrouping ApplySetting;

    private Animator _animController;
    public IInputProvider _inputProvider;
    private void Start()
    {
        ApplySetting?.ApplyToPlayer(this, ApplySetting);
        _entity = GetComponent<BaseEntity>();
        _animController = GetComponent<Animator>();
        _sRend = GetComponent<SpriteRenderer>();
        if (_inputProvider == null)
            _inputProvider = new PlayerInputProvider();
    }

    private SpriteRenderer _sRend;
    private bool isFacingLeft = false;

    float xInput = 0;
    float yInput = 0;

    private void Update()
    {
        CheckSetting();
        xInput = _inputProvider.GetXInput();
        yInput = _inputProvider.GetYInput();

        if (_inputProvider.GetUseInput())
            TryUse();
    }

    private void TryUse()
    {
        foreach (BodyPickup pickup in FindObjectsOfType<BodyPickup>())
        {
            if (pickup.interactIndicator.activeInHierarchy)
            {
                transform.position = pickup.transform.position;
                _sRend.flipX = pickup.GetComponent<SpriteRenderer>().flipX;
                pickup.Settings.ApplyToPlayer(this, CurrentSetting);
                _entity.SetYVelocity(0);
                _entity.SetXVelocity(0);
                Destroy(pickup.gameObject);
                return;
            }
        }

        if (_entity._lastHitResult.hitDown && CurrentSetting != GameSettings.GhostSetting)
            GoGhost();
    }

    private void GoGhost()
    {
        var go = Instantiate(GameSettings.PickupPrefab);
        go.GetComponent<BodyPickup>().Settings = CurrentSetting;
        go.transform.position = transform.position;
        go.GetComponent<SpriteRenderer>().flipX = _sRend.flipX;
        GameSettings.GhostSetting.ForceApplyToPlayer(this);
        _entity.SetYVelocity(5);
    }
    private void CheckSetting()
    {
        if (ApplySetting != CurrentSetting)
        {
            CurrentSetting.ApplyToPlayer(this, ApplySetting);
        }
    }

    private void FixedUpdate()
    {
        if (xInput < 0)
            isFacingLeft = true;
        else if (xInput > 0)
            isFacingLeft = false;

        if (!_animController)
            return;
        _animController.SetFloat("xMove", xInput);
        _animController.SetBool("grounded", _entity._lastHitResult.hitDown);

        if (_sRend)
            _sRend.flipX = isFacingLeft;

        if (CurrentSetting.CharacterSettings.InstantVelocity)
        {
            HandleInstantVelocity();
        }
        else
        {
            _entity.AddToVelocity(new Vector3(xInput * CurrentSetting.CharacterSettings.Speed, CurrentSetting.CharacterSettings.CanFly ? yInput * CurrentSetting.CharacterSettings.JumpStrength : 0, 0));
        }
    }

    private void HandleInstantVelocity()
    {
        _entity.SetXVelocity(xInput * CurrentSetting.CharacterSettings.Speed);

        if (CurrentSetting.CharacterSettings.CanFly)
        {
            _entity.SetYVelocity(yInput * CurrentSetting.CharacterSettings.JumpStrength);
        }
        else
        {
            if (_entity._lastHitResult.hitDown && yInput > 0)
                _entity.SetYVelocity(CurrentSetting.CharacterSettings.JumpStrength);
        }
    }
}

public interface IInputProvider
{
    float GetYInput();
    float GetXInput();
    bool GetUseInput();
}

public class PlayerInputProvider : IInputProvider
{
    public float GetYInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public float GetXInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public bool GetUseInput()
    {
        return Input.GetButtonDown("Use");
    }
}
