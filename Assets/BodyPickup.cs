using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPickup : MonoBehaviour
{
    public bool Head = true;
    public bool Torso = false;
    public bool Legs = false;
    public bool Wings = false;

    private Animator _animator;
    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        _animator.SetBool("Head", Head);
        _animator.SetBool("Torso", Head);
        _animator.SetBool("Legs", Head);
        _animator.SetBool("Wings", Head);
    }
}
