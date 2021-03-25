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
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

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

        // Player is sprinting
        if (IsGrounded() && InputManager.instance.shiftPressed)
            currentSpeed = sprintSpeed;
        rb.velocity = new Vector2(inputManager.horizontal * currentSpeed, rb.velocity.y);

        // Sets the direction the player was last facing
        if (inputManager.horizontal != 0)
            inputManager.lastMoveHorizontal = inputManager.horizontal;

        // Animate character
        spriteRenderer.flipX = inputManager.lastMoveHorizontal < 0;
        animator.SetBool(TagManager.IsWalking, inputManager.horizontal != 0);
        animator.SetBool(TagManager.IsJumping, !IsGrounded());

        // Land on the ground
        FallDown();

        // Kill the player if they fall to their death
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
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    private void FallDown()
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton(TagManager.Jump))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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
