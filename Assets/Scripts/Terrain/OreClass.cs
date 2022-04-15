using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class OreClass
{
    public string name;
    [Range(0f, 1f)]
    public float rarity;
    [Range (0f, 1f)]
    public float size;
    public int startSpawnHeight;
    public Texture2D veinTexture;
}
