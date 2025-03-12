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
        Die();
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;

    }

    void FlipEnemyFacing()
    {
      
        transform.localScale = new Vector2(Mathf.Sign(myRigidBody2D.velocity.x), 1f);

    }

    void Die()
    {
        if (myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Die");
            myRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            Destroy(gameObject, myAnimator.GetCurrentAnimatorStateInfo(0).length);

        }
    }

}