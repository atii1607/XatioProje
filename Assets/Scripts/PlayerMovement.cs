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
    private Coroutine speedBoostCoroutine;
    private Coroutine JumpBoostCoroutine;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;

    bool isAlive = true;

    void Start()
    {
        if(!isAlive) { return; }
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();

    }

    void Update()
    {
        Run();
        FlipSprite();
        Die();
    }
    public void LoadData(GameData gameData)
    {
        this.transform.position = gameData.playerPosition;
    }
    public void SaveData (GameData gameData)
    {
        gameData.playerPosition = this.transform.position;
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
        }
    }

    void Run()
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

    void Die()
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies"))) 
        {
            isAlive = false;
            myAnimator.SetTrigger("Die");
            myRigidbody.velocity = deathKick;
            myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), true);
            //FindObjectOfType<GameSession>().ProcessPlayerDeath();


        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        }
    }

    public void ActivateSpeedBoost(float boostedSpeed, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }
        speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(boostedSpeed, duration));
    }
    public void ActivateJumpBoost(float jumpBoosted, float duration)
    {
        if (JumpBoostCoroutine != null)
        {
            StopCoroutine(JumpBoostCoroutine);
        }
        JumpBoostCoroutine = StartCoroutine(JumpBoostRoutine(jumpBoosted, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boostedSpeed, float duration)
    {
        runSpeed = boostedSpeed;
        yield return new WaitForSeconds(duration);
        runSpeed = 5f;
        speedBoostCoroutine = null;
    }
    private IEnumerator JumpBoostRoutine(float jumpBoosted, float duration)
    {
        jumpSpeed = jumpBoosted;
        yield return new WaitForSeconds(duration);
        jumpSpeed = 5f;
        JumpBoostCoroutine = null;
    }

    public void RespawnPlayer(GameData gameData)
    {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            gameData.playerPosition = this.transform.position;
            this.transform.position = gameData.respawnPosition; //TODO - Respawn position not working properly.
        }
    }

}
