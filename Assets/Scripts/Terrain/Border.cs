using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public TerrainGeneration world;
    // Start is called before the first frame update
    void Start()
    {
        GameObject border = new GameObject();
        border.transform.parent = this.transform;
        border.name = "LeftBorder";
        border.AddComponent<BoxCollider2D>();
        border.GetComponent<BoxCollider2D>().size = Vector2.one;
        border.transform.position = new Vector2(-0.5f, 0);
        border.transform.localScale = new Vector2(1f, world.worldSize);

        GameObject border2 = new GameObject();
        border2.transform.parent = this.transform;
        border2.name = "RightBorder";
        border2.AddComponent<BoxCollider2D>();
        border2.GetComponent<BoxCollider2D>().size = Vector2.one;
        border2.transform.position = new Vector2(world.worldSize + 0.5f, 0);
        border2.transform.localScale = new Vector2(1f, world.worldSize);

        GameObject border3 = new GameObject();
        border3.transform.parent = this.transform;
        border3.name = "SkyCap";
        border3.AddComponent<BoxCollider2D>();
        border3.GetComponent<BoxCollider2D>().size = Vector2.one;
        border3.transform.position = new Vector2(world.worldSize / 2, world.worldSize / 2 + 0.5f);
        border3.transform.localScale = new Vector2(world.worldSize, 1f);


    }


}
