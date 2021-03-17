using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] bool canDoubleJump = true;

    [SerializeField] LayerMask groundLayer;

    InputManager inputManager;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        inputManager = GameObject.FindObjectOfType<InputManager>();
    }

    void OnEnable()
    {
        InputManager.OnJump += Jump;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(inputManager.horizontal * speed, rb.velocity.y);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            canDoubleJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            // Play jump sound
        }
        else if (!IsGrounded() && canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            // Play jump sound
        }
    }

    private bool IsGrounded()
    {
        Vector3 originPos = transform.position + (Vector3) boxCollider.offset;
        RaycastHit2D hit = Physics2D.BoxCast(originPos, boxCollider.size, 0, Vector2.down, 0.2f, groundLayer);

        return hit.collider != null;
    }
}
