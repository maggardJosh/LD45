using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "Custom/Character Setting")]
public class CharacterSettings : ScriptableObject
{
    public float Speed = 5;
    public float JumpStrength = 15;
}