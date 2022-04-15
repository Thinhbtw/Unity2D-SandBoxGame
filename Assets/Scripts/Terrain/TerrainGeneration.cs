using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public PlayerController player;
    public CamController cam;
    public GameObject tileDrop;
    public GameObject enemy;
    public bool spawn;

    [Header("Block")]
    public Blocks blocks;
    public float seed;

    public BiomeClass[] biomes;


    [Header("Biome Settings")]
    public float biomeFrequency;
    public Gradient biomeGradient;
    public Texture2D biomeMap;

    [Header("Generation Settings")]
    public int chunkSize = 16;
    public int worldSize = 100;
    public bool generateCaves = true;
    public int heightAddition = 25;

    [Header("Noise Settings")]
    public Texture2D caveNoiseTexture;
    public float terrainFreq = 0.05f;
    public float caveFreq = 0.04f;


    [Header("Ore Settings")]
    public OreClass[] ores;

    private GameObject[] worldChunks;

    private GameObject[,] world_ForeGroundObject;
    private GameObject[,] world_BackGroundObject;
    private TileClass[,] world_BackgroundTiles;
    private TileClass[,] world_ForegroundTiles;
    private int[] maxY;

    private BiomeClass curBiome;

    public void Start()
    {
        world_ForegroundTiles = new TileClass[worldSize, worldSize];
        world_BackgroundTiles = new TileClass[worldSize, worldSize];
        world_ForeGroundObject = new GameObject[worldSize, worldSize];
        world_BackGroundObject = new GameObject[worldSize, worldSize];
        
        maxY = new int[worldSize];


        seed = Random.Range(-10000, 10000);


        DrawOreTexture();
        DrawBiomeMap();
        DrawCaves();
        CreateChunk();
        GenerateTerrain();

        cam.Spawn(new Vector3(player.spawnPos.x, player.spawnPos.y, cam.transform.position.z));
        cam.worldSize = worldSize;
        player.Spawn();
    }

    private void OnValidate()
    {
        DrawBiomeMap();
        DrawCaves();
        DrawOreTexture();
    }
    public void DrawBiomeMap()
    {
        float b;
        Color col;
        biomeMap = new Texture2D(worldSize, worldSize);
        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                b = Mathf.PerlinNoise((x + seed) * biomeFrequency, seed * biomeFrequency);
                col = biomeGradient.Evaluate(b);
                biomeMap.SetPixel(x, y, col);
            }
        }

        biomeMap.Apply();
    }

    public void DrawCaves()
    {
        caveNoiseTexture = new Texture2D(worldSize, worldSize);
        for (int x = 0; x < caveNoiseTexture.width; x++)
        {
            for (int y = 0; y < caveNoiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * caveFreq, (y + seed) * caveFreq);
                if (v > terrainFreq)
                    caveNoiseTexture.SetPixel(x, y, Color.white); 
                else
                    caveNoiseTexture.SetPixel(x, y, Color.black);
            }
        }

        caveNoiseTexture.Apply();
    }

    public void DrawOreTexture()
    {
        for(int i = 0;i < 4; i++)
        {
            ores[i].veinTexture = new Texture2D(worldSize, worldSize);
            for (int x = 0; x < ores[i].veinTexture.width; x++)
            {
                for (int y = 0; y < ores[i].veinTexture.height; y++)
                {
                    float v = Mathf.PerlinNoise((x + seed) * ores[i].rarity, (y + seed) * ores[i].rarity);
                    if (v > ores[i].size)
                        ores[i].veinTexture.SetPixel(x, y, Color.white);
                    else
                        ores[i].veinTexture.SetPixel(x, y, Color.black);
                }
            }
            ores[i].veinTexture.Apply();
        }
        
    }

    public void CreateChunk()
    {
        int numChunk = worldSize / chunkSize;
        worldChunks = new GameObject[numChunk];
        for (int i = 0; i < numChunk; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            worldChunks[i] = newChunk;
        }
    }

    public BiomeClass GetCurrentBiome(int x, int y)
    {

        for (int i = 0; i < biomes.Length; i++)
        {
            if (biomes[i].biomeColor == biomeMap.GetPixel(x, y))
            {
                return biomes[i];
            }
        }

        return curBiome;
    }

    public void GenerateTerrain()
    {
        TileClass tileClass;
        for (int x = 0; x <= worldSize - 1; x++)
        {
            float height;

            for (int y = 0; y <= worldSize; y++)
            {
                curBiome = GetCurrentBiome(x, y);
                height = Mathf.PerlinNoise((x + seed) * curBiome.surfaceValue, seed * curBiome.surfaceValue) * curBiome.heightMultiplier + heightAddition;

                if (y >= height)
                    break;
                if (y < height - curBiome.dirtLayerHeight)
                {
                    tileClass = curBiome.blocks.stone;

                    if (ores[0].veinTexture.GetPixel(x, y).r > 0.5f && height - y > ores[0].startSpawnHeight)
                        tileClass = blocks.coal;
                    if (ores[1].veinTexture.GetPixel(x, y).r > 0.5f && height - y > ores[1].startSpawnHeight)
                        tileClass = blocks.iron;
                    if (ores[2].veinTexture.GetPixel(x, y).r > 0.5f && height - y > ores[2].startSpawnHeight)
                        tileClass = blocks.gold;
                    if (ores[3].veinTexture.GetPixel(x, y).r > 0.5f && height - y > ores[3].startSpawnHeight)
                        tileClass = blocks.diamond;
                }
                else if (y < height - 1)
                {
                    tileClass = curBiome.blocks.dirt;
                }
                else
                {
                    tileClass = curBiome.blocks.grass;
                    maxY[x] = y; 
                }

                if (y == 0)
                {
                    tileClass = blocks.bedrock;
                }

                if (generateCaves && y > 0)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f) 
                    {
                        PlaceTile(tileClass, x, y, true);
                    }
                    else if (tileClass.wallVariant != null)
                    {
                        PlaceTile(tileClass.wallVariant, x, y, true); 
                    }
                }
                else
                {
                    PlaceTile(tileClass, x, y, true);
                }

                if (y > height - 1)
                {
                    if (x == worldSize / 2)
                        player.spawnPos = new Vector2(x, height + 3);

                    int t = Random.Range(0, curBiome.treeChance);
                    if (t == 1)
                    {
                        if (GetTileFromWorld(x, y))
                            GenerateTree(Random.Range(curBiome.minTreeHeight, curBiome.maxTreeHeight), x, y + 1);
                    }
                    else
                    {
                        int i = Random.Range(0, curBiome.tallGrassChance);
                        if (i == 1)
                        {
                            if (GetTileFromWorld(x, y))
                                if (curBiome.blocks.tallGrass != null)
                                    PlaceTile(curBiome.blocks.tallGrass, x, y + 1, true);
                        }
                    }
                } 
            }
        }
    } 

    void GenerateTree(int treeHeight, int x, int y)
    {
        if (x >= 0 && x <= worldSize - 3 && y >= 0 && y <= worldSize)
        {
            for (int i = 0; i <= treeHeight; i++)
            {
                if (x <= worldSize - 3)
                    PlaceTile(curBiome.blocks.log, x, y + i, true);
            }

            for (int i = 1; i <= 3; i++)
            {
                if (curBiome.blocks.leaf != null)
                {
                    PlaceTile(curBiome.blocks.leaf, x, y + treeHeight + i, true);
                    PlaceTile(curBiome.blocks.leaf, x - 1, y + treeHeight + i - 1, true);
                    PlaceTile(curBiome.blocks.leaf, x + 1, y + treeHeight + i - 1, true);
                }
            }
            for (int i = 1; i <= 2; i++)
            {
                if (curBiome.blocks.leaf != null)
                {
                    PlaceTile(curBiome.blocks.leaf, x - 2, y + treeHeight + i - 1, true);
                    PlaceTile(curBiome.blocks.leaf, x + 2, y + treeHeight + i - 1, true);
                }
            }

        }

    }


    public void PlaceTile(TileClass tile, int x, int y, bool isNaturallyPlaced)
    {

        if (x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            GameObject newTile = new GameObject();

            int chunkCord = (x / chunkSize) * chunkSize; // 19/20 kieu? int = 0
            chunkCord /= chunkSize;
            newTile.transform.parent = worldChunks[chunkCord].transform;  


            newTile.AddComponent<SpriteRenderer>(); 
            int spriteIndex = Random.Range(0, tile.tileSprites.Length);
            newTile.GetComponent<SpriteRenderer>().sprite = tile.tileSprites[spriteIndex]; 

            if (tile.inBackGround)
            {
                newTile.GetComponent<SpriteRenderer>().sortingOrder = -10;
                if (tile.name.ToUpper().Contains("WALL"))
                    newTile.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
            }
            else
            {
                newTile.GetComponent<SpriteRenderer>().sortingOrder = -5;
                newTile.AddComponent<BoxCollider2D>();
                newTile.GetComponent<BoxCollider2D>().size = Vector2.one;
                newTile.tag = "Ground";
            }
            if (isNaturallyPlaced == false && tile.name.ToUpper().Contains("LOG") || isNaturallyPlaced == false && tile.name.ToUpper().Contains("CACTUS"))
            {
                newTile.GetComponent<SpriteRenderer>().sortingOrder = -5;
                newTile.AddComponent<BoxCollider2D>();
                newTile.GetComponent<BoxCollider2D>().size = Vector2.one;
                newTile.tag = "Ground";
            }

            newTile.name = tile.tileName;
            newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

            TileClass newTileClass = TileClass.CreateInstance(tile, isNaturallyPlaced);       

            AddObjectToWorld(x, y, newTile, newTileClass);
            AddTileToWorld(x, y, newTileClass);
        }
    }

    public bool CheckTiles(TileClass tile, int x, int y, bool isNaturallyPlaced)
    {
        if (x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            if (tile.inBackGround) //dat background
            {
                if (GetTileFromWorld(x + 1, y) || GetTileFromWorld(x - 1, y) ||
                    GetTileFromWorld(x, y + 1) || GetTileFromWorld(x, y - 1))
                {
                    if (!GetTileFromWorld(x, y))
                    {
                        GameSound(tile);
                        PlaceTile(tile, x, y, isNaturallyPlaced);
                        return true;
                    }
                }
            }
            else //dat block
            {
                if (GetTileFromWorld(x + 1, y) || GetTileFromWorld(x - 1, y) ||
                    GetTileFromWorld(x, y + 1) || GetTileFromWorld(x, y - 1))
                {
                    if (!GetTileFromWorld(x, y)) 
                    {
                        GameSound(tile);
                        PlaceTile(tile, x, y, isNaturallyPlaced);
                        return true;
                    }
                    else
                    {
                        if (GetTileFromWorld(x, y).inBackGround) 
                        {
                            GameSound(tile);
                            PlaceTile(tile, x, y, isNaturallyPlaced);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void RemoveTile(int x, int y)
    {
        if (GetTileFromWorld(x, y) && x >= 0 && x <= worldSize && y >= 0 & y <= worldSize)
        {
            TileClass tile = GetTileFromWorld(x, y);
            RemoveTileFromWorld(x, y);

            if (tile.wallVariant != null)
            {
                if (tile.naturallyPlaced)
                {
                    PlaceTile(tile.wallVariant, x, y, true);
                }
            }

            if (tile.tileDrop) 
            {
                GameObject newtileDrop = Instantiate(tileDrop, new Vector2(x, y + 0.5f), Quaternion.identity) as GameObject;
                newtileDrop.GetComponent<SpriteRenderer>().sprite = tile.tileDrop.tileSprites[0];
                //newtileDrop.transform.localScale = Vector3.one;

                ItemClass tileDropItem = new ItemClass(tile.tileDrop); //nhat block
                newtileDrop.GetComponent<TileDropController>().item = tileDropItem;
            }

            Destroy(GetObjectFromWorld(x, y));
        }

    }

    public bool BreakTileWith(int x, int y, ItemClass item)
    {
        if (GetTileFromWorld(x, y) && x >= 0 && x <= worldSize && y >= 0 & y <= worldSize)
        {
            TileClass tile = GetTileFromWorld(x, y);
            if (tile.toolToBreak == ItemClass.ToolType.none)
            {
                if (tile.tileName == "TallGrass" || tile.tileName == "DeadBush"
                    || tile.tileName == "Leaf")
                {
                    SoundManager.PlaySound("mineTallGrass");
                }
                RemoveTile(x, y);
                return true;
            }
            else
            {
                if (item != null) //neu' cam` item
                {
                    if (item.itemType == ItemClass.ItemType.tool) 
                    {
                        if (tile.toolToBreak == item.toolType)
                        {
                            GameSound(tile);
                            RemoveTile(x, y);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    TileClass GetTileFromWorld(int x, int y)
    {
        if (world_ForegroundTiles[x, y] != null)   //co block thi them vao` world_BackgorundTiles
        {
            return world_ForegroundTiles[x, y];
        }
        else if (world_BackgroundTiles[x, y] != null)
        {
            return world_BackgroundTiles[x, y];
        }
        return null;
    }

    void AddTileToWorld(int x, int y, TileClass tile)
    {
        if (tile.inBackGround)
        {
            world_BackgroundTiles[x, y] = tile;
        }
        else
        {
            world_ForegroundTiles[x, y] = tile;
        }
    }

    void RemoveTileFromWorld(int x, int y)
    {
        if (world_ForegroundTiles[x, y] != null) 
        {
            world_ForegroundTiles[x, y] = null;
        }
        else if (world_BackgroundTiles[x, y] != null)  
        {
            world_BackgroundTiles[x, y] = null;
        }
    }


    GameObject GetObjectFromWorld(int x, int y)
    {
        if (world_ForeGroundObject[x, y] != null)
        {
            return world_ForeGroundObject[x, y];
        }
        else if (world_BackGroundObject[x, y] != null)
        {
            return world_BackGroundObject[x, y];
        }
        return null;
    }

    void AddObjectToWorld(int x, int y, GameObject tileObject, TileClass tile)
    {
        if (tile.inBackGround)
        {
            world_BackGroundObject[x, y] = tileObject;
        }
        else
        {
            world_ForeGroundObject[x, y] = tileObject;
        }
    }

    void GameSound(TileClass tile)
    {
        if (tile.tileName.Contains("Grass"))
        {
            SoundManager.PlaySound("mineGrass");
        }
        if (tile.tileName.Contains("Dirt"))
        {
            SoundManager.PlaySound("mineDirt");
        }
        if (tile.tileName == "Sand")
        {
            SoundManager.PlaySound("mineSand");
        }
        if (tile.tileName.Contains("Stone") || tile.tileName == "Coal" || tile.tileName == "Gold"
            || tile.tileName == "Iron" || tile.tileName == "Diamond")
        {
            SoundManager.PlaySound("mineStone");
        }
        if (tile.tileName.Contains("Snow"))
        {
            SoundManager.PlaySound("mineSnow");
        }
        if (tile.tileName.Contains("Ice"))
        {
            SoundManager.PlaySound("mineIce");
        }
        if (tile.tileName.Contains("Log"))
        {
            SoundManager.PlaySound("mineLog");
        }
        if (tile.tileName.Contains("Cactus"))
        {
            SoundManager.PlaySound("mineCactus");
        }
    }

    void RefreshChunk()
    {    
        Vector2 a = new Vector2(player.transform.position.x, 0);       
        for (int i = 0; i < worldChunks.Length; i++)
        {
            if (Vector2.Distance(new Vector2((i * chunkSize) + (chunkSize / 2), 0), a) > Camera.main.orthographicSize * 5f)
                worldChunks[i].SetActive(false);
            else
            {
                worldChunks[i].SetActive(true);
                int t = Random.Range(0, 1000);
                if (t == 1)
                {
                    SpawnZombie();
                }
                int rnd = Random.Range(30, 35) * ((Random.Range(0, 2) == 0) ? 1 : -1);
                int test = Mathf.RoundToInt(player.transform.position.x + rnd);
                if (spawn && test < worldSize && test > 0)
                {
                    Vector2 b = new Vector2(player.transform.position.x + rnd, maxY[test] + 5f);
                    Instantiate(enemy, b, Quaternion.identity);
                    spawn = false;                 
                }
            }
        }
    }

    void SpawnZombie()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            spawn = true;
        }
    }


    private void Update()
    {
        RefreshChunk();
    }

}
