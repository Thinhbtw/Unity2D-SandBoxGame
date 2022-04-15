using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeClass 
{
    public string biomeName;
    public Color biomeColor;
    public Blocks blocks;

    [Header("Generation Settings")]
    public int dirtLayerHeight = 5;
    public float surfaceValue = 0.25f;
    public float heightMultiplier = 4f;

    [Header("Tree")]
    public int treeChance = 10;
    public int minTreeHeight = 3;
    public int maxTreeHeight = 6;

    [Header("Decoration")]
    public int tallGrassChance = 10;
}
