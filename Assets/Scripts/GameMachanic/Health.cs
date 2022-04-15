 using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float curHealth { get; private set; } //get duoc o? script khac' nhung chi? set dc o? script nay`
    private Rigidbody2D rb;
    public PlayerController player;
    public bool isTakeDMG = true;
    private Animator animator;
    public bool getHurt = false;
    [SerializeField]private float maxYvelocity = 0;
    public float yVelocity;
    public int multiply = 1;
    
    public Canvas canvas;
    public GameObject backGroundAudio;
    public GameObject gameSound;

    private void Awake()
    {
        curHealth = startingHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canvas.enabled = false;
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

        if (rb.velocity.y < -17f && isTakeDMG == true)
        {
            SoundManager.PlaySound("getHit");
            curHealth = Mathf.Clamp(curHealth - (0.5f * multiply), 0, startingHealth);
            isTakeDMG = false;
            getHurt = true;
            StartCoroutine(GetHurt());
        }
        PlayerDeath();

    }

    public void TakeDamge()
    {
        SoundManager.PlaySound("getHit");
        curHealth = Mathf.Clamp(curHealth - 1, 0, startingHealth);
        getHurt = true;
        StartCoroutine(GetHurt());
        PlayerDeath();

    }

    public void RegenHealth()
    {
        curHealth = Mathf.Clamp(curHealth + 1, 0, startingHealth);
        CancelInvoke("RegenHealth");
    }

    public void PlayerDeath()
    {
        if (curHealth == 0)
        {
            Time.timeScale = 0;
            canvas.enabled = true;
            backGroundAudio.SetActive(false);
            gameSound.SetActive(false);
            player.isDead = true;
            CancelInvoke("RegenHealth");
        }
    }

    public void PlayerRespawn()
    {
        canvas.enabled = false;
        player.Spawn();
        Time.timeScale = 1;
        curHealth = startingHealth;
        backGroundAudio.SetActive(true);
        gameSound.SetActive(true);
        player.isDead = false;
    }

    private IEnumerator GetHurt()
    {
        yield return new WaitForSeconds(0.3f);
        getHurt = false;
    }

    public void Update()
    {
        animator.SetBool("getHurt", getHurt);

        if (curHealth < startingHealth)
            Invoke("RegenHealth", 5f);
       
        if (player.onGround)
        {
            isTakeDMG = true;
            multiply = 1;
            maxYvelocity = 0;
        }
    }



}
