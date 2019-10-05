using UnityEngine;

[RequireComponent(typeof(BaseEntity))]
public class PlayerController : MonoBehaviour
{
    private BaseEntity _entity;
    public CharacterSettings Settings;

    private Animator _animController;
    public IInputProvider _inputProvider;
    private void Start()
    {
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
        xInput = _inputProvider.GetXInput();
        yInput = _inputProvider.GetYInput();
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

        if (Settings.InstantVelocity)
        {
            HandleInstantVelocity();
        }
        else
        {
            _entity.AddToVelocity(new Vector3(xInput * Settings.Speed, Settings.CanFly ? yInput * Settings.JumpStrength : 0, 0));
        }
    }

    private void HandleInstantVelocity()
    {
        _entity.SetXVelocity(xInput * Settings.Speed);

        if (Settings.CanFly)
        {
            _entity.SetYVelocity(yInput * Settings.JumpStrength);
        }
        else
        {
            if (_entity._lastHitResult.hitDown && yInput > 0)
                _entity.SetYVelocity(Settings.JumpStrength);
        }
    }
}

public interface IInputProvider
{
    float GetYInput();
    float GetXInput();
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
}
