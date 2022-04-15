using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private int stackLimit = 99;

    public ToolClass start_Sword;
    public ToolClass start_Pickaxe;
    public ToolClass start_Axe;
    public ToolClass start_Hammer;
    public ToolClass start_Shovel;

    public Vector2 inventoryOffset;
    public Vector2 hotbarOffset;
    public Vector2 multiplier;


    public GameObject inventoryUI;
    public GameObject hotbarUI;
    public GameObject slotPrefab;
    public int inventoryWidth;
    public int inventoryHeight;

    public InventorySlot[,] inventorySlots;
    public InventorySlot[] hotbarSlots;

    public GameObject[,] uiSlots;
    public GameObject[] hotbarUISlots;

    private void Start()
    {
        inventorySlots = new InventorySlot[inventoryWidth, inventoryHeight];
        uiSlots = new GameObject[inventoryWidth, inventoryHeight];
        hotbarSlots = new InventorySlot[inventoryWidth];
        hotbarUISlots = new GameObject[inventoryWidth];

        SetupUI();
        UpdateInventoryUI();
        AddItem(new ItemClass(start_Sword));
        AddItem(new ItemClass(start_Pickaxe));
        AddItem(new ItemClass(start_Axe));
        AddItem(new ItemClass(start_Hammer));
        AddItem(new ItemClass(start_Shovel));
    }

    void SetupUI()
    {
        //setup inventory
        for(int x = 0; x < inventoryWidth; x++)
        {
            for(int y = 0; y < inventoryHeight; y++)
            {
                GameObject inventorySlot = Instantiate(slotPrefab, inventoryUI.transform.GetChild(0).transform);
                inventorySlot.GetComponent<RectTransform>().localPosition = new Vector3((x * multiplier.x) + inventoryOffset.x, (y * multiplier.y) + inventoryOffset.y);

                uiSlots[x, y] = inventorySlot;
                inventorySlots[x, y] = null;
            }
        }

        //setup hotbar
        for (int x = 0; x < inventoryWidth; x++)
        {
            GameObject hotbar = Instantiate(slotPrefab, hotbarUI.transform.GetChild(0).transform);
            hotbar.GetComponent<RectTransform>().localPosition = new Vector3((x * multiplier.x) + hotbarOffset.x, hotbarOffset.y);

            hotbarUISlots[x] = hotbar;
            hotbarSlots[x] = null;
        }
    }

    void UpdateInventoryUI()
    {
        //update inventory
        for (int x = 0; x < inventoryWidth; x++)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                if (inventorySlots[x, y] == null)
                {
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = false;

                    uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().text = "0";
                    uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().enabled = false;  
                }
                else
                {
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = inventorySlots[x, y].item.sprite;
                    
                    if(inventorySlots[x, y].item.itemType == ItemClass.ItemType.block)
                    {
                        if (inventorySlots[x, y].item.tile.inBackGround)
                        {
                            uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
                        }
                        else
                            uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    }

                    uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().text = inventorySlots[x, y].quanity.ToString();
                    uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().enabled = true;
                }
            }
        }

        //update hotbar
        for (int x = 0; x < inventoryWidth; x++)
        {
            
            if (inventorySlots[x, inventoryHeight - 1] == null)
            {
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().enabled = false;

                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().text = "0";
                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = false;
            }
            else
            {
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().sprite = inventorySlots[x, inventoryHeight - 1].item.sprite;

                if (inventorySlots[x, inventoryHeight - 1].item.itemType == ItemClass.ItemType.block)
                {
                    if (inventorySlots[x, inventoryHeight - 1].item.tile.inBackGround)
                    {
                        hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
                    }
                    else
                        hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                }

                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().text = inventorySlots[x, inventoryHeight - 1].quanity.ToString();
                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = true;
            }
            
        }
    }

    public Vector2Int Contains(ItemClass item)
    {
        for (int y = inventoryHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventorySlots[x, y] != null)
                {
                    if (inventorySlots[x, y].item.itemName == item.itemName)
                    {
                        if (item.isStack && inventorySlots[x, y].quanity < stackLimit)
                            return new Vector2Int(x, y);
                    }
                }
            }
        }
        return Vector2Int.one * -1; //tra? ve` (null)
    }

    public bool AddItem(ItemClass item)
    {
        Vector2Int itemPos = Contains(item);
        bool added = false;

        if(itemPos != Vector2Int.one * -1)
        {
            inventorySlots[itemPos.x, itemPos.y].quanity += 1;
            added = true;
        }

        if (!added) 
        { 
            for (int y = inventoryHeight - 1; y >= 0; y--)
            {
                if (added) 
                    break; 
                for (int x = 0; x < inventoryWidth; x++)
                {
                    if (inventorySlots[x, y] == null) 
                    {
                        inventorySlots[x, y] = new InventorySlot { item = item, position = new Vector2Int(x, y), quanity = 1 };
                        added = true;
                        break; 
                    }
                }
            }
        }
        
        UpdateInventoryUI();
        return added; 
    }

    

    public bool RemoveItem(ItemClass item)
    {

        for (int y = inventoryHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventorySlots[x, y] != null)
                {
                    if (inventorySlots[x, y].item.itemName == item.itemName) 
                    {
                        inventorySlots[x, y].quanity -= 1;

                        if (inventorySlots[x, y].quanity == 0)
                            inventorySlots[x, y] = null;

                        UpdateInventoryUI();
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
