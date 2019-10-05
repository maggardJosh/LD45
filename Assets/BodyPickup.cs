﻿using UnityEngine;

[ExecuteInEditMode]
public class BodyPickup : MonoBehaviour
{
    public SkeletonSettingGrouping Settings;
    public GameObject interactIndicator;

    private Animator _animator;
    public void Awake()
    {
        _animator = GetComponent<Animator>();
        interactIndicator.SetActive(false);
    }

    public void Update()
    {
        _animator.SetBool("Head", Settings.Head);
        _animator.SetBool("Torso", Settings.Torso);
        _animator.SetBool("Legs", Settings.Legs);
        _animator.SetBool("Wings", Settings.Wings);

        _animator.Update(Time.deltaTime);
        if (Application.isPlaying)
        {
            var o = FindObjectOfType<PlayerController>();
            var bc = GetComponent<BoxCollider2D>();
            interactIndicator.SetActive(bc.bounds.Intersects(o.GetComponent<BoxCollider2D>().bounds));
        }
    }
}