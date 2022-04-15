using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip mineDirt, mineStone, mineSnow, mineIce, mineLog, mineSand, mineGrass, 
                            getHit, mineCactus, mineTallGrass, pickUp, zombieDeath, zombieHurt, zombieWalk,
                            walkOnGrass, walkOnDirt, walkOnSnow, walkOnSand, walkOnStone,
                            walkOnCactus, walkOnWood, zombieSay, calm3;
    static AudioSource audioSrc;

    private void Start()
    {
        mineDirt = Resources.Load<AudioClip>("gravel1");
        mineStone = Resources.Load<AudioClip>("stone4");
        mineSnow = Resources.Load<AudioClip>("snow1");
        mineIce = Resources.Load<AudioClip>("glass3");
        mineLog = Resources.Load<AudioClip>("wood4");
        mineSand = Resources.Load<AudioClip>("sand3");
        mineGrass = Resources.Load<AudioClip>("grass3");
        getHit = Resources.Load<AudioClip>("hit3");
        mineCactus = Resources.Load<AudioClip>("cloth4");
        mineTallGrass = Resources.Load<AudioClip>("grass1");
        pickUp = Resources.Load<AudioClip>("pop");

        calm3 = Resources.Load<AudioClip>("calm3");

        zombieDeath = Resources.Load<AudioClip>("death_zombie");
        zombieHurt = Resources.Load<AudioClip>("hurt_zombie");
        zombieWalk = Resources.Load<AudioClip>("zombie_walk");
        zombieSay = Resources.Load<AudioClip>("say_zombie");

        walkOnGrass = Resources.Load<AudioClip>("Walk_grass");
        walkOnDirt = Resources.Load<AudioClip>("walk_dirt");
        walkOnSnow = Resources.Load<AudioClip>("walk_snow");
        walkOnSand = Resources.Load<AudioClip>("walk_sand");
        walkOnStone = Resources.Load<AudioClip>("walk_stone");
        walkOnCactus = Resources.Load<AudioClip>("walk_cactus");
        walkOnWood = Resources.Load<AudioClip>("walk_wood");

        audioSrc = GetComponent<AudioSource>();
    }


    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "pickUp":
                audioSrc.PlayOneShot(pickUp);
                break;
            case "mineTallGrass":
                audioSrc.PlayOneShot(mineTallGrass);
                break;
            case "mineDirt":
                audioSrc.PlayOneShot(mineDirt);
                break;
            case "mineGrass":
                audioSrc.PlayOneShot(mineGrass);
                break;
            case "mineStone":
                audioSrc.PlayOneShot(mineStone);
                break;
            case "mineSnow":
                audioSrc.PlayOneShot(mineSnow);
                break;
            case "mineIce":
                audioSrc.PlayOneShot(mineIce);
                break;
            case "mineLog":
                audioSrc.PlayOneShot(mineLog);
                break;
            case "mineSand":
                audioSrc.PlayOneShot(mineSand);
                break;
            case "getHit":
                audioSrc.PlayOneShot(getHit);
                break;
            case "mineCactus":
                audioSrc.PlayOneShot(mineCactus);
                break;
            case "zombieSay":
                audioSrc.PlayOneShot(zombieSay);
                break;
            case "zombieDeath":
                audioSrc.PlayOneShot(zombieDeath);
                break;
            case "zombieHurt":
                audioSrc.PlayOneShot(zombieHurt);
                break;
            case "zombieWalk":
                audioSrc.PlayOneShot(zombieWalk);
                break;
            case "walkOnGrass":
                audioSrc.PlayOneShot(walkOnGrass);
                break;
            case "walkOnDirt":
                audioSrc.PlayOneShot(walkOnDirt);
                break;
            case "walkOnSnow":
                audioSrc.PlayOneShot(walkOnSnow);
                break;
            case "walkOnSand":
                audioSrc.PlayOneShot(walkOnSand);
                break;
            case "walkOnStone":
                audioSrc.PlayOneShot(walkOnStone);
                break;
            case "walkOnCactus":
                audioSrc.PlayOneShot(walkOnCactus);
                break;
            case "walkOnWood":
                audioSrc.PlayOneShot(walkOnWood);
                break;
        }
    }
}
