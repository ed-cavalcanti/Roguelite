using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private PlayerDodge dashScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // Busca o script de dash que está no mesmo objeto
        dashScript = GetComponent<PlayerDodge>();
    }

    void Update()
    {
        // TRAVA: Se o personagem estiver dando dash, o movimento normal NÃO roda
        if (dashScript != null && dashScript.isDashing) 
        {
            return; 
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        // Só atualiza animação se não estiver no meio de um dash
        if (dashScript != null && !dashScript.isDashing)
        {
            animator.SetBool("isWalking", moveInput != Vector2.zero);
            animator.SetFloat("inputX", moveInput.x);
            animator.SetFloat("inputY", moveInput.y);

            if (context.canceled)
            {
                animator.SetFloat("lastInputX", moveInput.x);
                animator.SetFloat("lastInputY", moveInput.y);
            }
        }
    }
}