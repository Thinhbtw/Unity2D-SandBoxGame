using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDropController : MonoBehaviour
{
    public ItemClass item;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if(col.GetComponent<Inventory>().AddItem(item))
            {
                Destroy(this.gameObject);
            }
            SoundManager.PlaySound("pickUp");

        }
    }

}
