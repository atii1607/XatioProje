using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float rollSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip jumpSound;

    private Coroutine speedBoostCoroutine;
    private Coroutine jumpBoostCoroutine;
    private Coroutine invincibilityCoroutine;
    private LivesCountText livesCountText;
    public static PlayerMovement instance { get; private set; }

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    BoxCollider2D myBodyCollider;

    bool isAlive = true;
    bool isDying = false;
    public bool isInvincible = false;
    void Start()
    {
        if(!isAlive) { return; }
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        livesCountText = FindObjectOfType<LivesCountText>();

    }

    void Update()
    {
        Run();
        FlipSprite();
    }
    public void LoadData(GameData gameData)
    {
        this.transform.position = gameData.playerPosition;
    }
    public void SaveData (GameData gameData)
    {
        if (this == null) return;
        gameData.playerPosition = transform.position;

    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if(value.isPressed == true)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position);
        }
    }

    public void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playHasSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playHasSpeed);

    }

    void OnRoll(InputValue value)
    {

        if (value.isPressed)
        {
            StartCoroutine(Roll());
        }
    }

    private IEnumerator Roll()
    {
        myAnimator.SetBool("isRolling", true);

        float originalYVelocity = myRigidbody.velocity.y;
        myRigidbody.velocity = new Vector2(transform.localScale.x * rollSpeed, originalYVelocity);

        yield return new WaitForSeconds(0.5f);
        myRigidbody.velocity = new Vector2(transform.localScale.x * runSpeed, myRigidbody.velocity.y);
        myAnimator.SetBool("isRolling", false);
    }

    void FlipSprite()
    {
        bool playHasSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playHasSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    public void Die(Collider2D collision)
    {
        if (isDying || !isAlive) return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isAlive = false;
            myAnimator.SetBool("isDead", true);
            myRigidbody.velocity = deathKick;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), true);
            livesCountText.ProcessPlayerDeath();
        }
    }

    public void ResetPlayer(Vector3 newPosition)
    {
        myRigidbody.velocity = Vector2.zero;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        myAnimator.SetBool("isDead", false);
        myAnimator.SetBool("isIdle", true);
        transform.position = newPosition;
        isAlive = true;
        isDying = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), false);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
            Die(collision);
        }
    }

    public void ActivateSpeedBoost(float boostSpeed, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }
        speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(boostSpeed, duration));
    }

    public void ActivateJumpBoost(float boostJump, float duration)
    {
        if (jumpBoostCoroutine != null)
        {
            StopCoroutine(jumpBoostCoroutine);
        }
        jumpBoostCoroutine = StartCoroutine(JumpBoostRoutine(boostJump, duration));
    }
    public void ActivateInvincibility(float duration)
    {
        if (invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
        }
        invincibilityCoroutine = StartCoroutine(InvincibilityRoutine(duration));
    }

    private IEnumerator SpeedBoostRoutine(float boostSpeed, float duration)
    {
        float originalSpeed = runSpeed;
        runSpeed = boostSpeed;
        yield return new WaitForSeconds(duration);
        runSpeed = originalSpeed;
        speedBoostCoroutine = null;
    }

    private IEnumerator JumpBoostRoutine(float boostMultiplier, float duration)
    {
        float originalJump = jumpSpeed;
        jumpSpeed *= boostMultiplier;

        yield return new WaitForSeconds(duration);

        jumpSpeed = originalJump;
        jumpBoostCoroutine = null;
    }
    private IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), true);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float lowAlpha = 0.5f; 

        Color c = spriteRenderer.color;
        c.a = lowAlpha;
        spriteRenderer.color = c;
        yield return new WaitForSeconds(5f);
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            c.a = 1f;
            spriteRenderer.color = c;
        }

        isInvincible = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), false);
        invincibilityCoroutine = null;
    }

}
