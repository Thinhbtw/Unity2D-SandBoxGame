using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileBlocks", menuName = "Blocks")]
public class Blocks : ScriptableObject
{
    [Header("Enviroment")]
    public TileClass grass;
    public TileClass dirt;
    public TileClass stone;
    public TileClass log;
    public TileClass leaf;
    public TileClass tallGrass;
    public TileClass bedrock;

    [Header("Ore")]
    public TileClass coal;
    public TileClass iron;
    public TileClass gold;
    public TileClass diamond;
}
