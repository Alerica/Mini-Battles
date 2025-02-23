using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float moveSpeed = 5f;

    private Vector2 moveInput;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();   
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(moveInput == Vector2.zero)
        {
            animator.SetBool(StringManager.isMoving, false);
        }
        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        rb2d.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        animator.SetBool(StringManager.isMoving,true);
    }
}
