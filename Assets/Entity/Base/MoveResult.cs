using UnityEngine;

public class MoveResult
{
    public bool hitRight = false;
    public bool hitLeft = false;
    public bool hitUp = false;
    public bool hitDown = false;
    public Vector3 newPos;

    public override string ToString()
    {
        return $"l:{(hitLeft ? 1:0)} u:{(hitUp ? 1:0)} r:{(hitRight ? 1:0)} d:{(hitDown ? 1:0)}";
    }
}
