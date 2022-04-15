using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Health health;
    private Inventory inventory;
    public bool invenShowing = false;
    public bool hotbarShowing = true;

    public GameObject holdItem;
    public GameObject hotbarSelector;   
    public int selectedSlotIndex = 0;
    public bool isDead = false;

    public TerrainGeneration terrainGenerator;

    public ItemClass selectedItem;


    public int playerRange;
    

    public float moveSpeed;
    public float jumpForce;
    public bool onGround;
    public float maxVelocity;

    private Rigidbody2D rb;
    private Animator animator;
    public float horizontal;
    public float jump;
    public bool hit, place;

    [HideInInspector]
    public Vector2Int mousePos;
    public Vector2 spawnPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
    }
    public void Spawn()
    {
        GetComponent<Transform>().position = spawnPos;
    }
    
    private void FixedUpdate()
    {
        
        jump = Input.GetAxisRaw("Jump");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        
        

        //xoay nhan vat
        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);

        if(jump >0.1f)
        {
            if (onGround)
            {
                movement.y = jumpForce;
            }
        }   

        rb.velocity = movement;

    }

    private void Update()
    {
        if (!isDead)
        {
            horizontal = Input.GetAxis("Horizontal");
            hit = Input.GetMouseButton(0);
            place = Input.GetMouseButton(1);


            //chuyen o hotbar
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                //scroll up
                if (selectedSlotIndex < inventory.inventoryWidth - 1)
                    selectedSlotIndex += 1;
                else
                    selectedSlotIndex = 0;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (selectedSlotIndex > 0)
                    selectedSlotIndex -= 1;
                else
                    selectedSlotIndex = inventory.inventoryWidth - 1;
            }

            //set selected item
            if (inventory.inventorySlots[selectedSlotIndex, inventory.inventoryHeight - 1] != null)
                selectedItem = inventory.inventorySlots[selectedSlotIndex, inventory.inventoryHeight - 1].item;
            else
                selectedItem = null;

            //set selected slot UI
            hotbarSelector.transform.position = inventory.hotbarUISlots[selectedSlotIndex].transform.position;

            //set item dang cam`
            if (selectedItem != null)
            {
                holdItem.GetComponent<SpriteRenderer>().sprite = selectedItem.sprite;
                if (selectedItem.itemType == ItemClass.ItemType.block)
                    holdItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                else
                    holdItem.transform.localScale = new Vector3(1f, 1f, 1f);
                if (selectedItem.toolType == ItemClass.ToolType.sword)
                    holdItem.GetComponentInChildren<EdgeCollider2D>().enabled = true;
                else
                    holdItem.GetComponentInChildren<EdgeCollider2D>().enabled = false;
            }
            else
            {
                holdItem.GetComponent<SpriteRenderer>().sprite = null;
                holdItem.GetComponentInChildren<EdgeCollider2D>().enabled = false;
            }



            if (Input.GetKeyDown(KeyCode.E))
            {
                invenShowing = !invenShowing;
                hotbarShowing = !hotbarShowing;
            }

            //pha' block + dat block
            if (Vector2.Distance(transform.position, mousePos) <= playerRange &&
                Vector2.Distance(transform.position, mousePos) > 1f) //ko dat duoc block o? player
            {

                if (place)
                {
                    if (selectedItem != null)
                    {
                        if (selectedItem.itemType == ItemClass.ItemType.block)
                        {
                            if (terrainGenerator.CheckTiles(selectedItem.tile, mousePos.x, mousePos.y, false))
                                inventory.RemoveItem(selectedItem); 
                        }

                    }
                }
            }

            if (Vector2.Distance(transform.position, mousePos) <= playerRange)
            {
                if (hit)
                {
                    terrainGenerator.BreakTileWith(mousePos.x, mousePos.y, selectedItem);
                }
            }

            //set VT cua? chuot.
            mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f); //khi add block thi vi tri +0.5f
            mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

            inventory.inventoryUI.SetActive(invenShowing);
            inventory.hotbarUI.SetActive(hotbarShowing);

            animator.SetFloat("horizontal", horizontal);
            animator.SetBool("hit", hit || place);
        }
    }
}
