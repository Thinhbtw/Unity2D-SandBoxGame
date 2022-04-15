using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoJump : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyController enemy;
    public bool groundCheck;
    public Rigidbody2D rb;
    private void Awake()
    {
        enemy = transform.parent.GetComponent<EnemyController>();
        rb = enemy.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            if (groundCheck)
            {
                rb.AddForce(Vector2.up * 120f);
            }
    }


    private void Update()
    {
        groundCheck = enemy.onGround;
    }
}
