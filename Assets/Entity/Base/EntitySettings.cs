using UnityEngine;

[CreateAssetMenu(fileName = "EntitySettings", menuName = "Custom/Entity Setting")]
public class EntitySettings : ScriptableObject
{
    [Range(-1, 0)]
    public float BounceValue = -.5f;
    public float MaxXVel = 5;
    public float MinYVel = -5;
    public float MaxYVel = 10;
    public LayerMask CollideMask;
}