using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidBody2D;
    Animator myAnimator;

    bool isAlive = true;

    void Start()
    {
        if (!isAlive)
        {
            return;
        }

        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        myRigidBody2D.velocity = new Vector2(moveSpeed, 0f);
        FlipEnemyFacing(); 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        Die(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die(collision.collider);
    }

    void FlipEnemyFacing()
    { 
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);
    }

    public void Die(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float playerBottom = collision.bounds.min.y;
            float enemyTop = GetComponent<Collider2D>().bounds.max.y;

            if (playerBottom > enemyTop - 0.1f)
            {
                isAlive = false;
                myAnimator.SetTrigger("Die");
                myRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                Destroy(gameObject, myAnimator.GetCurrentAnimatorStateInfo(0).length);
            }

            else
            {
                PlayerMovement player = collision.GetComponent<PlayerMovement>();

                if (player != null)
                {
                    player.Die(GetComponent<Collider2D>());
                }
            }
        }
    }

}