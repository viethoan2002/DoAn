using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMovement : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TargetAnimationByName(string nameAnimation, float duration = 0)
    {
        animator.CrossFade(nameAnimation, duration);
    }
}
