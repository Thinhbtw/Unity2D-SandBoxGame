using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float curHealth { get; private set; } //get duoc o? script khac' nhung chi? set dc o? script nay`
    private Rigidbody2D rb;
    public EnemyController enemy;
    private Animator animator;
    public Transform player;
    public bool canTakeDMG = true;
    public bool getHurt = false;
    [SerializeField] private float maxYvelocity = 0;
    public float yVelocity;
    public int multiply = 1;


    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        enemy = GetComponent<EnemyController>();
        curHealth = startingHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void TakeFallDamage()
    {
        yVelocity = Mathf.Round(rb.velocity.y);
        if (maxYvelocity > yVelocity)
        {
            maxYvelocity = yVelocity;
        }
        for (int i = -17; i >= maxYvelocity; i--)
            if (i % 2 == 0)
                multiply++;

        if (rb.velocity.y < -17f && canTakeDMG == true && Vector3.Distance(transform.position, player.position) <= 25f)
        {
            SoundManager.PlaySound("zombieHurt");
            curHealth = Mathf.Clamp(curHealth - (0.5f * multiply), 0, startingHealth);
            canTakeDMG = false;
            getHurt = true;
            StartCoroutine(GetHurt());
        }

        
    }

    public void TakeDamge()
    {
        SoundManager.PlaySound("zombieHurt");
        curHealth = Mathf.Clamp(curHealth - 1, 0, startingHealth);
        getHurt = true;
        StartCoroutine(GetHurt());

    }

    private IEnumerator GetHurt()
    {
        yield return new WaitForSeconds(0.5f);
        getHurt = false;
    }

    public void Update()
    {
        animator.SetBool("getHurt", getHurt);      
        if(curHealth == 0)
        {
            Destroy(this.gameObject);
            SoundManager.PlaySound("zombieDeath");
        }
        if (enemy.onGround)
        {
            canTakeDMG = true;
            multiply = 1;
            maxYvelocity = 0;
        }
    }
}
