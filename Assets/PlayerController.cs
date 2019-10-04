using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sRend;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _sRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        _animator.SetFloat("xSpeed", hInput);
        if (hInput < 0)
            _sRend.flipX = true;
        else if (hInput > 0)
            _sRend.flipX = false;

    }
}
