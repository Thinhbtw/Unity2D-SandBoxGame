using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]

public class TileClass : ScriptableObject
{
    public string tileName;
    public TileClass wallVariant;
    //public Sprite tileSprite;
    public Sprite[] tileSprites;
    public bool inBackGround = false;
    public TileClass tileDrop;
    public bool naturallyPlaced = true;
    public bool isStackAble;
    public ItemClass.ToolType toolToBreak;


    public static TileClass CreateInstance(TileClass t, bool isNaturallyPlaced)
    {
        var thisTile = ScriptableObject.CreateInstance<TileClass>();
        thisTile.Init(t, isNaturallyPlaced);
        return thisTile;
    }

    public void Init (TileClass t, bool isNaturallyPlaced)
    {
        tileName = t.tileName;
        wallVariant = t.wallVariant;
        tileSprites = t.tileSprites;
        inBackGround = t.inBackGround;
        tileDrop = t.tileDrop;
        isStackAble = t.isStackAble;
        toolToBreak = t.toolToBreak;
        naturallyPlaced = isNaturallyPlaced;
    }
}