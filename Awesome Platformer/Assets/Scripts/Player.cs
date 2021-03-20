using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnPlayerDie;
    public event Action OnPlayerGrabCoin;
    public event Action OnPlayerWin;

    [SerializeField] float speed = 12f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] bool canDoubleJump = true;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask movingPlatformLayer;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject winEffect;

    public int coinAmount { get; private set; }

    Collider2D movingPlatform;

    InputManager inputManager;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        inputManager = GameObject.FindObjectOfType<InputManager>();
    }

    void OnEnable()
    {
        inputManager.OnJump += Jump;
    }

    void OnDisable()
    {
        inputManager.OnJump -= Jump;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null && IsOnTopOfMovingPlatform())
        {
            transform.SetParent(movingPlatform.transform);
            rb.interpolation = RigidbodyInterpolation2D.None;
        }
        else if (transform.parent != null && !IsOnTopOfMovingPlatform())
        {
            transform.SetParent(null);
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        rb.velocity = new Vector2(inputManager.horizontal * speed, rb.velocity.y);

        animator.SetBool("isWalking", inputManager.horizontal != 0);

        if (inputManager.horizontal != 0)
            inputManager.lastMoveHorizontal = inputManager.horizontal;

        animator.SetBool("isJumping", !IsGrounded());

        spriteRenderer.flipX = inputManager.lastMoveHorizontal < 0;

        if (rb.velocity.y < -100)
            Die();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hazards"))
        {
            gameObject.SetActive(false);
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            Destroy(other.gameObject);
            AddCoin(1);
            OnPlayerGrabCoin?.Invoke();
            // Play coin sound
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Star"))
        {
            Destroy(other.gameObject);
            gameObject.SetActive(false);
            GlobalVariables.totalCoins += coinAmount;
            OnPlayerWin?.Invoke();
            Instantiate(winEffect, transform.position, Quaternion.identity);
            // Play star sound
        }
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

    private void AddCoin(int amount)
    {
        coinAmount += amount;
    }

    private void Die()
    {
        gameObject.SetActive(false);
        OnPlayerDie?.Invoke();
        Instantiate(deathEffect, transform.position, Quaternion.identity);
    }

    private bool IsOnTopOfMovingPlatform()
    {
        Vector3 originPos = transform.position + (Vector3) boxCollider.offset;
        RaycastHit2D hit = Physics2D.BoxCast(originPos, boxCollider.size, 0, Vector2.down, 0.1f, movingPlatformLayer);

        if (hit.collider != null)
        {
            movingPlatform = hit.collider;
            return true;
        }
        else
        {
            movingPlatform = null;
            return false;
        }
    }
}
