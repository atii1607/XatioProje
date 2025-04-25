using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidBody2D;
    BoxCollider2D myCollider2D;
    CapsuleCollider2D myCapsuleCollider2D;
    [SerializeField] float moveSpeed = 1f;
    Animator myAnimator;
    bool isAlive = true;

    void Start()
    {
        if (!isAlive) { return; }
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
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
                PlayerMovement player = collision.GetComponent<PlayerMovement>(); // Or whatever script your player uses
                if (player != null)
                {
                    player.Die(GetComponent<Collider2D>()); // Pass enemy collider as per your method
                }
            }
        }
    }

}