using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool attack = true;
    private bool hit;
    public float verticalKnockbackForce;
    public float horizontallKnockbackForce;
    private void Update()
    {
        hit = Input.GetMouseButton(0);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            Transform enemy = FindObjectOfType<EnemyController>().transform;
            if (enemyHealth.curHealth > 0)
            {
                if (hit)
                {
                    if (attack)
                    {
                        rb.AddForce(Vector2.up * verticalKnockbackForce);
                        if (enemy.position.x < transform.position.x)
                            rb.AddForce(Vector2.left * horizontallKnockbackForce);
                        else
                            rb.AddForce(Vector2.right * horizontallKnockbackForce);

                        enemyHealth.TakeDamge();
                        attack = false;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        attack = true;
    }
}
