using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemClass
{
    public enum ItemType
    {
        block, 
        tool
    }

    public enum ToolType
    {
        axe,
        pickaxe,
        hammer,
        shovel,
        none,
        Unbreakable,
        sword
    }

    public ItemType itemType;
    public ToolType toolType;

    public string itemName;
    public Sprite sprite;
    public bool isStack;
    public TileClass tile;
    public ToolClass tool;
    public ItemClass (TileClass _tile)
    {
        itemName = _tile.tileName;
        sprite = _tile.tileDrop.tileSprites[0];
        isStack = _tile.isStackAble;
        itemType = ItemType.block;
        tile = _tile;
    }

    public ItemClass(ToolClass _tool)
    {
        itemName = _tool.name;
        sprite = _tool.sprite;
        isStack = false;
        itemType = ItemType.tool;
        toolType = _tool.toolType;
        tool = _tool;
    }
}
