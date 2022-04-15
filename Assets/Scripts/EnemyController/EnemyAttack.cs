using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform player;
    public float verticalKnockbackForce;
    public float horizontallKnockbackForce;
    private Animator animator;
    public bool attack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (attack)
            {
                rb.AddForce(Vector2.up * verticalKnockbackForce);
                if (transform.position.x > player.position.x)
                    rb.AddForce(Vector2.left * horizontallKnockbackForce);
                else
                    rb.AddForce(Vector2.right * horizontallKnockbackForce);
                player.GetComponent<Health>().TakeDamge();
                
                attack = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attack = true;
    }
}
