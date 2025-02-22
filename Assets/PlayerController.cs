using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField]
    private float moveSpeed = 5f;

    private Vector2 moveInput;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
        rb2d.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
