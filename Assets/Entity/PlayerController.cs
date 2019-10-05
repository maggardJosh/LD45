
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BaseEntity))]
public class PlayerController : MonoBehaviour
{
    private BaseEntity _entity;
    public SkeletonSettingGrouping CurrentSetting;

    public SkeletonSettingGrouping ApplySetting;

    private Animator _animController;
    public IInputProvider _inputProvider;
    private BoxCollider2D _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        ApplySetting?.ApplyToPlayer(this, ApplySetting);
        _entity = GetComponent<BaseEntity>();
        _animController = GetComponent<Animator>();
        _sRend = GetComponent<SpriteRenderer>();
        if (_inputProvider == null)
            _inputProvider = new PlayerInputProvider();
    }

    internal void SetPositionToPickup(BodyPickup bodyPickup)
    {
        _sRend.flipX = bodyPickup.GetComponent<SpriteRenderer>().flipX;
        transform.position = bodyPickup.transform.position;
        _entity.SetYVelocity(0);
        _entity.SetXVelocity(0);
    }

    private SpriteRenderer _sRend;
    private bool isFacingLeft = false;

    private float lastYInput = 0;
    float xInput = 0;
    float yInput = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        CheckSetting();
        xInput = _inputProvider.GetXInput();
        yInput = _inputProvider.GetYInput();

        if (_inputProvider.GetUseInput())
            TryUse();
    }

    private void TryUse()
    {
        foreach (Interactable interactableObject in FindObjectsOfType<Interactable>())
        {
            if (interactableObject.Use(this))
                return;
        }

        if (_entity._lastHitResult.hitDown && CurrentSetting != GameSettings.GhostSetting)
        {
            GoGhost();
            if(CurrentSetting == GameSettings.GhostSetting)
                _entity.SetYVelocity(5);
        }
    }

    internal void SetBoxColliderHeight(float colliderHeight)
    {
        var bcSize = _boxCollider.size;
        bcSize.y = colliderHeight;
        _boxCollider.size = bcSize;
        _boxCollider.offset = new Vector2(0, colliderHeight / 2f);
    }

    private void GoGhost()
    {
        int ind = 1;
        if (CurrentSetting.Head)
        {
            SpawnPickup(SkeletonSettingGrouping.FindGroupingSetting(true, false, false, false), ind++);
            SkeletonSettingGrouping.FindGroupingSetting(false, CurrentSetting.Torso, CurrentSetting.Legs, CurrentSetting.Wings).ForceApplyToPlayer(this);
            return;
        }
        if (CurrentSetting.Torso)
        {
            SpawnPickup(SkeletonSettingGrouping.FindGroupingSetting(false, true, false, false), ind++);
            SkeletonSettingGrouping.FindGroupingSetting(CurrentSetting.Head, false, CurrentSetting.Legs, CurrentSetting.Wings).ForceApplyToPlayer(this);
            return;
        }
        if (CurrentSetting.Legs)
        {
            SpawnPickup(SkeletonSettingGrouping.FindGroupingSetting(false, false, true, false), ind++);
            SkeletonSettingGrouping.FindGroupingSetting(CurrentSetting.Head, CurrentSetting.Torso, false, CurrentSetting.Wings).ForceApplyToPlayer(this);
            return;
        }
        if (CurrentSetting.Wings)
        {
            SpawnPickup(SkeletonSettingGrouping.FindGroupingSetting(false, false, false, true), ind++);
            SkeletonSettingGrouping.FindGroupingSetting(CurrentSetting.Head, CurrentSetting.Torso, CurrentSetting.Legs, false).ForceApplyToPlayer(this);
            return;
        }

        //Just in case
        GameSettings.GhostSetting.ForceApplyToPlayer(this);
    }

    public float xRandSpawnVel = 1;
    public float minYRandSpawnVel = 2;
    public float maxYRandSpawnVel = 5;
    private void SpawnPickup(SkeletonSettingGrouping setting, int ind)
    {
        var go = Instantiate(GameSettings.PickupPrefab);
        go.GetComponent<BodyPickup>().Settings = setting;
        go.transform.position = transform.position;
        go.GetComponent<SpriteRenderer>().flipX = _sRend.flipX;
        var be = go.GetComponent<BaseEntity>();
        float xVel = (_sRend.flipX ? 1 : -1 ) * (ind % 2 == 0 ? 1 : -1) * (Mathf.CeilToInt(ind / 2f)) * xRandSpawnVel;  //Leaving in "throw body away" code just in case I need it later...
        xVel *= Random.Range(.8f, 1.2f);
        be.SetXVelocity(xVel);
        be.SetYVelocity(Random.Range(minYRandSpawnVel, maxYRandSpawnVel));
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

        float yValue = 0;
        if(lastYInput != yInput || CurrentSetting.CharacterSettings.CanFly)
        {
            yValue = yInput;
            lastYInput = yInput;
        }

        if (!_animController)
            return;
        _animController.SetFloat("xMove", xInput);
        _animController.SetBool("grounded", _entity._lastHitResult.hitDown);

        if (_sRend)
            _sRend.flipX = isFacingLeft;

        if (CurrentSetting.CharacterSettings.InstantVelocity)
        {
            HandleInstantVelocity(xInput, yValue);
        }
        else
        {
            _entity.AddToVelocity(new Vector3(xInput * CurrentSetting.CharacterSettings.Speed, CurrentSetting.CharacterSettings.CanFly ? yValue * CurrentSetting.CharacterSettings.JumpStrength : 0, 0));
        }
    }

    private void HandleInstantVelocity(float xValue, float yValue)
    {
        _entity.SetXVelocity(xValue * CurrentSetting.CharacterSettings.Speed);

        if (CurrentSetting.CharacterSettings.CanFly)
        {
            _entity.SetYVelocity(yValue * CurrentSetting.CharacterSettings.JumpStrength);
        }
        else
        {
            if (_entity._lastHitResult.hitDown && yValue > 0)
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
