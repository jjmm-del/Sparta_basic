using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");

    protected Animator animator;
    
    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"Animator가 {gameObject.name}에서 발견되지않음");
        }
    }
    public virtual void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > .5f);
    }
}
