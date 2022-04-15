using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Health health;
    public PlayerController player;
    public bool grass, dirt, stone, wood, cactus, snow, sand;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            player.onGround = true;

        if(collision.name.Contains("Grass") && player.horizontal > 0 || collision.name.Contains("Grass") && player.horizontal < 0)
            grass = true;
        else
            grass = false;

        if (collision.name == "Dirt" && player.horizontal > 0 || collision.name == "Dirt" && player.horizontal < 0)
            dirt = true;
        else
            dirt = false;

        if (collision.name == "Snow" && player.horizontal > 0 || collision.name == "Snow" && player.horizontal < 0)
            snow = true;
        else
            snow = false;

        if (collision.name == "Sand" && player.horizontal > 0 || collision.name == "Sand" && player.horizontal < 0)
            sand = true;
        else
            sand = false;

        if (collision.name.Contains("Log") && player.horizontal > 0 || collision.name.Contains("Log") && player.horizontal < 0)
            wood = true;
        else
            wood = false;

        if (collision.name.Contains("Cactus") && player.horizontal > 0 || collision.name.Contains("Cactus") && player.horizontal < 0)
            cactus = true;
        else
            cactus = false;

        if (collision.name.Contains("Stone") && player.horizontal > 0 || collision.name.Contains("Stone") && player.horizontal < 0)
            stone = true;
        else
            stone = false;

        if(player.jump >0.1f)
        {
            grass = false;
            dirt = false;
            snow = false;
            sand = false;
            wood = false;
            cactus = false;
            stone = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            player.onGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            health.TakeFallDamage();
        }

    }

    void WalkingSound()
    {
        if (grass)
        {
            SoundManager.PlaySound("walkOnGrass");
        }
        if (dirt)
        {
            SoundManager.PlaySound("walkOnDirt");
        }
        if (snow)
        {
            SoundManager.PlaySound("walkOnSnow");
        }
        if (sand)
        {
            SoundManager.PlaySound("walkOnSand");
        }
        if (wood)
        {
            SoundManager.PlaySound("walkOnWood");
        }
        if (cactus)
        {
            SoundManager.PlaySound("walkOnCactus");
        }
        if (stone)
        {
            SoundManager.PlaySound("walkOnStone");
        }
        CancelInvoke("WalkingSound");
        
    }

    private void Update()
    {
        if (player.jump > 0.1f)
            CancelInvoke("WalkingSound");
        else
            Invoke("WalkingSound", 0.5f);
    }
}
