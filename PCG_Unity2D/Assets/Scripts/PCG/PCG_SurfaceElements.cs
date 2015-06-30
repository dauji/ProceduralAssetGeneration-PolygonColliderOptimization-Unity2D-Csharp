using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PCG_SurfaceElements : PCG_Manager
{
    public PCG_SurfaceElements() : base(0, 0, 90, 0, 1, 4.12F) { }

    PCG_TriggerA trigger1 = new PCG_TriggerA();
    PCG_TriggerB trigger2 = new PCG_TriggerB();
    PCG_TriggerC trigger3 = new PCG_TriggerC();
    PCG_TriggerD trigger4 = new PCG_TriggerD();

    public GameObject _tile;

    public List<Sprite> textures_craters;
    public List<Sprite> textures_chasms;
    public List<Sprite> textures_rocks;
    public List<Sprite> textures_smallRocks;
    public List<GameObject> chasms;

    [HideInInspector]
    public bool triggerEntered2 = false;
    [HideInInspector]
    public bool addRemoveTiles2 = false;
    [HideInInspector]
    public bool initialFunctionCall = false;
    [HideInInspector]
    public bool moveTrigger2 = false;

    GameObject[] tiles = new GameObject[MAX_SIZE_SURFACE_TILES];
    float distBWplayerTiles;

    List<int> smallRocks = new List<int>();
    bool deleteSmallRocksColliders = false;

    private float areaSpan = 15.0F;


    //List<GameObject> all_chasms;

    #region OffsetsGeneration

    int RandomNosRange_DeterminingOffset(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.12F) && (rand.seedRndNos_spawning[pos] >= 0.0F)) return 0;        if ((rand.seedRndNos_spawning[pos] <= 0.24f) && (rand.seedRndNos_spawning[pos] >= 0.12f)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 0.36f) && (rand.seedRndNos_spawning[pos] >= 0.24f)) return 2;        if ((rand.seedRndNos_spawning[pos] <= 0.48f) && (rand.seedRndNos_spawning[pos] >= 0.36f)) return 3;
        if ((rand.seedRndNos_spawning[pos] <= 0.60f) && (rand.seedRndNos_spawning[pos] >= 0.48f)) return 4;        if ((rand.seedRndNos_spawning[pos] <= 0.72f) && (rand.seedRndNos_spawning[pos] >= 0.60f)) return 5;
        if ((rand.seedRndNos_spawning[pos] <= 0.84f) && (rand.seedRndNos_spawning[pos] >= 0.72f)) return 6;        if ((rand.seedRndNos_spawning[pos] <= 1.00f) && (rand.seedRndNos_spawning[pos] >= 0.84f)) return 7;
        else return -1;
    }

    void AddOffset(int pos)
    {
        switch (RandomNosRange_DeterminingOffset(pos))
        {
            case -1: Debug.LogError("RndNosOutOfBounds"); break;
            case 0: tiles[pos].transform.position += new Vector3(.45F, 0.0F, 0.0F); break;            case 1: tiles[pos].transform.position += new Vector3(-.45F, 0.0F, 0.0F); break;
            case 2: tiles[pos].transform.position += new Vector3(0.0F, .45F, 0.0F); break;            case 3: tiles[pos].transform.position += new Vector3(0.0F, -.45F, 0.0F); break;
            case 4: tiles[pos].transform.position += new Vector3(.45F, .45F, 0.0F); break;            case 5: tiles[pos].transform.position += new Vector3(-.45F, -.45F, 0.0F); break;
            case 6: tiles[pos].transform.position += new Vector3(.45F, -.45F, 0.0F); break;            case 7: tiles[pos].transform.position += new Vector3(-.45F, .45F, 0.0F); break;
            default: break;
        }
    }

    #endregion

    int RandomNosRange_ChangingSpritesOverall(int pos)
    {
        if ((rand.seedRndNos_spawning[pos] <= 0.50F) && (rand.seedRndNos_spawning[pos] >= 0.00F)) return 0;
        if ((rand.seedRndNos_spawning[pos] <= 0.75F) && (rand.seedRndNos_spawning[pos] >= 0.50F)) return 1;
        if ((rand.seedRndNos_spawning[pos] <= 1.00F) && (rand.seedRndNos_spawning[pos] >= 0.75F)) return 2;
        else return -1;
    }

    void ChangeSprites(GameObject asset, int pos)
    {
        switch (RandomNosRange_ChangingSpritesOverall(pos))
        {
            case -1: Debug.LogError("RndNosOutOfBounds"); break;
            case 0: Change_Textures(textures_craters, asset, pos); break;
            case 1: Change_Textures(textures_chasms, asset, pos); break;
            case 2: Change_Textures(textures_rocks, asset, pos); break;
            default: break;
        }
    }

    void AddRemoveTiles()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        if (triggerEntered2 == true || initialFunctionCall == false)
        {
            initialFunctionCall = true;
            if (!addRemoveTiles2)
            {
                addRemoveTiles2 = true;
                triggerEntered2 = false;
                moveTrigger2 = false;

                GameObject[] gameObjectTile = GameObject.FindGameObjectsWithTag("CratersOnScreen");
                List<GameObject> gameObjectTileList = gameObjectTile.ToList();
                List<GameObject> finalList = gameObjectTileList.Distinct().ToList();

                GameObject[] gameObjectTile2 = GameObject.FindGameObjectsWithTag("SmallRocksOnScreen");
                List<GameObject> gameObjectTileList2 = gameObjectTile2.ToList();
                List<GameObject> finalList2 = gameObjectTileList2.Distinct().ToList();

                for (int i = 0; i < 11071; i++)
                {
                    distBWplayerTiles = Vector2.Distance(new Vector2(X_GRID[i], Y_GRID[i]), mainPlayerPos);
                    if (distBWplayerTiles < areaSpan)
                    {
                        tiles[i] = Instantiate(_tile, new Vector3(X_GRID[i], Y_GRID[i], 0), transform.rotation) as GameObject;
                        tiles[i].tag = "CratersOnScreen";
                        tiles[i].name = "Craters " + i;
						tiles[i].transform.parent = GameObject.Find("PCG_World").transform;

                        if (tiles[i].name == "Craters 90") { tiles[i].GetComponentInChildren<SpriteRenderer>().enabled = false; }
                        ChangeSprites(tiles[i], i);
                        RND_DeleteExtraSpawns(tiles[i], i);
                        AddOffset(i);

                        foreach (GameObject item in finalList)
                        {
                            if (item.name == tiles[i].name) { Destroy(tiles[i]); }
                        }

                        foreach (GameObject item in finalList2)
                        {
                            if (item.name == tiles[i].name) { Destroy(tiles[i]); }
                        }
                    }

                    if (i == 11070) { deleteSmallRocksColliders = true;  }
                }

                StartCoroutine(CHANGE_COLLIDERS());
                StartCoroutine(OPTIMIZE_POLYGON_COLLIDER());

                GameObject[] allTexturesMyFriend = GameObject.FindGameObjectsWithTag("CratersOnScreen");
                foreach (GameObject texture in allTexturesMyFriend)
                {
                    var distance2 = Vector2.Distance(texture.transform.position, mainPlayerPos);
                    if (distance2 > areaSpan)
                    {
                        GameObject.Destroy(texture);
                    }
                }

                GameObject[] allTexturesMyFriend2 = GameObject.FindGameObjectsWithTag("SmallRocksOnScreen");
                foreach (GameObject texture in allTexturesMyFriend2)
                {
                    var distance2 = Vector2.Distance(texture.transform.position, mainPlayerPos);
                    if (distance2 > areaSpan)
                    {
                       GameObject.Destroy(texture);
                    }
                }
            }
        }
    }

    IEnumerator CHANGE_COLLIDERS()
    {
        yield return new WaitForSeconds(2.0F);
        GameObject[] all_elements_on_screen = GameObject.FindGameObjectsWithTag("CratersOnScreen");

        foreach (GameObject element in all_elements_on_screen)
        {
            foreach (Transform child in element.transform)
            {
                if (textures_craters.Contains(child.GetComponent<SpriteRenderer>().sprite))
                {
                    Destroy(child.GetComponent<PolygonCollider2D>());
                    Destroy(child.GetComponent<PCG_OptimizePolygonCollider2D>());
                    Destroy(child.GetComponent<BoxCollider2D>());
                    child.GetComponent<CircleCollider2D>().enabled = true;
                }

                else if (textures_chasms.Contains(child.GetComponent<SpriteRenderer>().sprite))
                {
                    Destroy(child.GetComponent<PolygonCollider2D>());
                    Destroy(child.GetComponent<PCG_OptimizePolygonCollider2D>());
                    Destroy(child.GetComponent<BoxCollider2D>());
                    Destroy(child.GetComponent<CircleCollider2D>());

                    foreach (EdgeCollider2D edgeCollider in child.GetComponents<EdgeCollider2D>())
                    {
                        if (edgeCollider.edgeCount == 4 && child.GetComponent<SpriteRenderer>().sprite == textures_chasms[0])
                        {
                            edgeCollider.enabled = true;
                        }
                        if (edgeCollider.edgeCount == 5 && child.GetComponent<SpriteRenderer>().sprite == textures_chasms[1])
                        {
                            edgeCollider.enabled = true;
                        }
                        if (edgeCollider.edgeCount == 6 && child.GetComponent<SpriteRenderer>().sprite == textures_chasms[2])
                        {
                            edgeCollider.enabled = true;
                        }
                    }

                }
                else
                {
                    Destroy(child.GetComponent<PolygonCollider2D>());
                    child.gameObject.AddComponent<PolygonCollider2D>();
                    Destroy(child.GetComponent<PCG_OptimizePolygonCollider2D>());
                    child.gameObject.AddComponent<PCG_OptimizePolygonCollider2D>();
                }

                #region ALTERNATE_CHECK
                //if (child.GetComponent<SpriteRenderer>().sprite == _tileA || child.GetComponent<SpriteRenderer>().sprite == _tileB ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileC || child.GetComponent<SpriteRenderer>().sprite == _tileD ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileE || child.GetComponent<SpriteRenderer>().sprite == _tileF ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileG || child.GetComponent<SpriteRenderer>().sprite == _tileH ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileI || child.GetComponent<SpriteRenderer>().sprite == _tileJ ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileK || child.GetComponent<SpriteRenderer>().sprite == _tileL ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileM || child.GetComponent<SpriteRenderer>().sprite == _tileN ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileO || child.GetComponent<SpriteRenderer>().sprite == _tileP ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileQ || child.GetComponent<SpriteRenderer>().sprite == _tileR ||
                //    child.GetComponent<SpriteRenderer>().sprite == _tileS || child.GetComponent<SpriteRenderer>().sprite == _tileT)
                //{
                //    Destroy(child.GetComponent<PolygonCollider2D>());
                //    Destroy(child.GetComponent<PCG_OptimizePolygonCollider2D>());
                //    Destroy(child.GetComponent<BoxCollider2D>());
                //    child.GetComponent<CircleCollider2D>().enabled = true;
                //}
                //else
                //{
                //    Destroy(child.GetComponent<PolygonCollider2D>());
                //    child.gameObject.AddComponent<PolygonCollider2D>();
                //    Destroy(child.GetComponent<PCG_OptimizePolygonCollider2D>());
                //    child.gameObject.AddComponent<PCG_OptimizePolygonCollider2D>();
                //}
                #endregion
            }
        }
    }

    IEnumerator OPTIMIZE_POLYGON_COLLIDER()
    {
        yield return new WaitForSeconds(3.0F);
        GameObject[] all_elements_on_screen = GameObject.FindGameObjectsWithTag("CratersOnScreen");

        foreach (GameObject element in all_elements_on_screen)
        {
            foreach (Transform child in element.transform)
            {
                if (!textures_chasms.Contains(child.GetComponent<SpriteRenderer>().sprite))
                {
                    Destroy(child.GetComponent<PCG_OptimizePolygonCollider2D>());
                    child.gameObject.AddComponent<PCG_OptimizePolygonCollider2D>();
                }

            }
        }
    }

    int RandonNosRangeToExactValuesForDeletingExtraSpawns(int pos)
    {
        if ((rand.seedRndNos_deleting[pos] <= 0.60F) && (rand.seedRndNos_deleting[pos] >= 0.00F)) return 0;
        if ((rand.seedRndNos_deleting[pos] <= 0.70f) && (rand.seedRndNos_deleting[pos] >= 0.60f)) return 1;
        if ((rand.seedRndNos_deleting[pos] <= 1.00f) && (rand.seedRndNos_deleting[pos] >= 0.70f)) return 2;
        else return -1;
    }

    void RND_DeleteExtraSpawns(GameObject asset, int pos)
    {
        switch (RandonNosRangeToExactValuesForDeletingExtraSpawns(pos))
        {
            case -1: Debug.LogError("RndNosOutOfBounds"); break;
            case 0: Destroy(tiles[pos]); break;
            case 1: Change_Textures(textures_smallRocks, asset, pos);
                    smallRocks.Add(pos);
                    tiles[pos].tag = "SmallRocksOnScreen"; break;
            case 2: break;
            default: return;
        }
    }

    void DeletingColliders_SmallRocks()
    {
        if (deleteSmallRocksColliders)
        {
            deleteSmallRocksColliders = false;
            foreach (int _pos in smallRocks)
            {
                GameObject[] surfaceElements = GameObject.FindGameObjectsWithTag("SmallRocksOnScreen");
                foreach (GameObject element in surfaceElements)
                {
                    if (element.name == "Craters " + _pos)
                    {
                        foreach (Transform child in element.transform)
                        {
                            Destroy(child.GetComponent<PolygonCollider2D>());
                            Destroy(child.GetComponent<BoxCollider2D>());
                            Destroy(child.GetComponent<CircleCollider2D>());
                        }
                    }
                }
            }
        }
    }

    void Reposition_Triggers()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        if (trigger1.playerInsideCircleTriggerA == true || trigger2.playerInsideCircleTriggerA1 == true || trigger3.playerInsideCircleTriggerA2 == true || trigger4.playerInsideCircleTriggerA3 == true)
        {
            if (!moveTrigger)
            {
                moveTrigger2 = true;
                addRemoveTiles2 = false;
                triggerEntered2 = true;
                trigger1.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 2.5F);
                trigger2.transform.position = new Vector2(mainPlayerPos.x - 2.5F, mainPlayerPos.y);
                trigger3.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 2.5F);
                trigger4.transform.position = new Vector2(mainPlayerPos.x + 2.5F, mainPlayerPos.y);
            }
        }
    }

    void Start()
    {
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;
        trigger1 = FindObjectOfType(typeof(PCG_TriggerA)) as PCG_TriggerA;
        trigger2 = FindObjectOfType(typeof(PCG_TriggerB)) as PCG_TriggerB;
        trigger3 = FindObjectOfType(typeof(PCG_TriggerC)) as PCG_TriggerC;
        trigger4 = FindObjectOfType(typeof(PCG_TriggerD)) as PCG_TriggerD;

        Generate_Grid("SurfaceTiles&&SurfaceElements");
    }
     
    void FixedUpdate()
    {
        DeletingColliders_SmallRocks();
    }

    void Update()
    {
        AddRemoveTiles();
        Reposition_Triggers();
    }
}
