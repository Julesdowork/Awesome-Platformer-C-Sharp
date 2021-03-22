using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnPlayerDie;
    public event Action OnPlayerGrabCoin;
    public event Action OnPlayerWin;

    [SerializeField] float baseSpeed = 8f;
    [SerializeField] float sprintSpeed = 12f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] float jumpTime = 0.35f;
    [SerializeField] bool canDoubleJump = true;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask movingPlatformLayer;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject winEffect;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip coinSound;
    [SerializeField] AudioClip starSound;
    [SerializeField] AudioClip loseSound;

    public int coinAmount { get; private set; }
    
    float currentSpeed;
    bool isJumping;
    float jumpTimeCounter;

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
        inputManager.OnShiftPressed += Sprint;
        inputManager.OnShiftReleased += StopSprinting;
    }

    void OnDisable()
    {
        inputManager.OnJump -= Jump;
        inputManager.OnShiftPressed -= Sprint;
        inputManager.OnShiftReleased -= StopSprinting;
    }

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Attach the player to a moving platform so they move with it
        if (transform.parent == null && IsOnTopOfMovingPlatform())
        {
            transform.SetParent(movingPlatform.transform);
            rb.interpolation = RigidbodyInterpolation2D.None;
        } // Detaches it
        else if (transform.parent != null && !IsOnTopOfMovingPlatform())
        {
            transform.SetParent(null);
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        if (IsGrounded() && InputManager.instance.shiftPressed)
            currentSpeed = sprintSpeed;

        rb.velocity = new Vector2(inputManager.horizontal * currentSpeed, rb.velocity.y);

        animator.SetBool(TagManager.IsWalking, inputManager.horizontal != 0);

        // Sets the direction the player was last facing
        if (inputManager.horizontal != 0)
            inputManager.lastMoveHorizontal = inputManager.horizontal;

        spriteRenderer.flipX = inputManager.lastMoveHorizontal < 0;

        Jump();

        animator.SetBool(TagManager.IsJumping, !IsGrounded());

        if (rb.velocity.y < -100)
            Die();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(TagManager.HazardsLayer))
        {
            gameObject.SetActive(false);
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(TagManager.CoinLayer))
        {
            Destroy(other.gameObject);
            AddCoin(1);
            OnPlayerGrabCoin?.Invoke();
            SoundManager.instance.PlaySound(coinSound);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer(TagManager.StarLayer))
        {
            Destroy(other.gameObject);
            gameObject.SetActive(false);
            GlobalVariables.totalCoins += coinAmount;
            OnPlayerWin?.Invoke();
            Instantiate(winEffect, transform.position, Quaternion.identity);
            SoundManager.instance.PlaySound(starSound);
        }
    }

    private void Sprint()
    {
        if (IsGrounded())
            currentSpeed = sprintSpeed;
    }

    private void StopSprinting()
    {
        currentSpeed = baseSpeed;
    }

    private void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown(TagManager.Jump))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        if (isJumping && Input.GetButton(TagManager.Jump))
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
                isJumping = false;
        }

        if (Input.GetButtonUp(TagManager.Jump))
            isJumping = false;
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
        SoundManager.instance.PlaySound(loseSound);
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
