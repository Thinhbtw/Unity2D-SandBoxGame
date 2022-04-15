using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    public EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = transform.parent.GetComponent<EnemyHealth>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            transform.parent.GetComponent<EnemyController>().onGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            transform.parent.GetComponent<EnemyController>().onGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            enemyHealth.TakeFallDamage();
        }
    }
}
