using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BaseEntity : MonoBehaviour
{
    public bool DebugEntity = false;

    public EntitySettings Settings;
    public override string ToString()
    {
        return $"pos: {transform.position}, hit: {_lastHitResult}, v: {_velocity}";
    }

    public MoveResult _lastHitResult { get; private set; } = new MoveResult();
    private Vector3 _velocity = Vector3.zero;
    private BoxCollider2D _bCollider;

    private Animator _animController;
    private void Start()
    {
        _bCollider = GetComponent<BoxCollider2D>();
        _animController = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        HandleFixedUpdate();
    }

    public void SetXVelocity(float xVel)
    {
        _velocity.x = xVel;
        ClampVelocity();
    }

    public void SetYVelocity(float yVel)
    {
        _velocity.y = yVel;
        ClampVelocity();
    }

    public float GetYVelocity() { return _velocity.y; }

    public void AddToVelocity(Vector3 vel)
    {
        _velocity += vel;
        ClampVelocity();
    }

    private void ClampVelocity()
    {
        _velocity.x = Mathf.Clamp(_velocity.x, -Settings.MaxXVel, Settings.MaxXVel);
        _velocity.y = Mathf.Clamp(_velocity.y, Settings.MinYVel, Settings.MaxYVel);
    }

    protected virtual void HandleFixedUpdate()
    {
        TryMove();
        HandleFriction();
        if (Settings.ObeysGravity)
            AddToVelocity(Vector3.down * GameSettings.Gravity * Time.fixedDeltaTime * (1 / .02f));
    }
    protected virtual void TryMove()
    {
        ForceOutOfWalls();

        _lastHitResult = GetMoveTester().GetMoveResult(transform.position, _velocity * Time.fixedDeltaTime);
        transform.position = _lastHitResult.newPos;

        if (_lastHitResult.hitDown)
            _velocity.y *= Settings.BounceValue;

        if (_lastHitResult.hitLeft || _lastHitResult.hitRight)
            _velocity.x *= Settings.BounceValue;
    }

    private EntityMoveTester GetMoveTester()
    {
        return new EntityMoveTester(new EntityDebugLogger(this), _bCollider, Settings.CollideMask);
    }

    private void ForceOutOfWalls()
    {
        MoveResult hitResult = GetMoveTester().GetHorizontalRaycastFromCenter(transform.position);
        transform.position = hitResult.newPos;
    }

    private void HandleFriction()
    {
        if (_lastHitResult.hitDown)
            _velocity.x *= .8f;
        else
            _velocity.x *= Settings.Friction;

        if (!Settings.ObeysGravity)
            _velocity.y *= Settings.Friction;
    }
}
