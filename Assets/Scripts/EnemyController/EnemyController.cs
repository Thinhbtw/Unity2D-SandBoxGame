using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyHealth health;
    public float speed;
    public float minRange;
    public float maxRange;
    public bool onGround;
    public Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    public bool hit;
    void Awake()
    {
        health = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();
        
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance <= maxRange && distance > minRange)
        {
            //animator.SetBool("hit", false);
            animator.SetBool("walk", true);
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            rb.velocity = new Vector2(0, rb.velocity.y);

            if (transform.position.x - player.position.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
            if (onGround)
                Invoke("ZombieWalk", 1f);
        }

        else if (distance > maxRange || distance <= minRange)
        {
            animator.SetBool("walk", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
            CancelInvoke("ZombieWalk");
        }

        if (distance <= 25f)
        {
            Invoke("ZombieSay", 5f);
        }
        else
            CancelInvoke("ZombieSay");
    }

    void ZombieWalk()
    {
        SoundManager.PlaySound("zombieWalk");
        CancelInvoke("ZombieWalk");
    }

    void ZombieSay()
    {
        SoundManager.PlaySound("zombieSay");
        CancelInvoke("ZombieSay");
    }

    
}
