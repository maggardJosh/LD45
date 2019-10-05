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
    bool yInput = false;

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

        if (_sRend)
            _sRend.flipX = isFacingLeft;
        _entity.SetXVelocity(xInput * Settings.Speed);

        if (_entity._lastHitResult.hitDown && yInput)
            _entity.SetYVelocity(Settings.JumpStrength);
    }
}

public interface IInputProvider
{
    bool GetYInput();
    float GetXInput();
}

public class PlayerInputProvider : IInputProvider
{
    public bool GetYInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public float GetXInput()
    {
        float xInput = 0;
        if (Input.GetKey(KeyCode.A))
        {
            xInput -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            xInput += 1;
        }
        return xInput;
    }
}
