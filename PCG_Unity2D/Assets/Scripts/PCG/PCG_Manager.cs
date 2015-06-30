using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PCG_Manager : MonoBehaviour
{
    public PCG_Rand rand = new PCG_Rand();

    public const int MAX_SIZE_SURFACE_TILES = 11071;
    public const int MAX_SIZE_MINEABLE_ROCKS = 2040;
    public List<float> X_GRID = new List<float>(MAX_SIZE_SURFACE_TILES);
    public List<float> Y_GRID = new List<float>(MAX_SIZE_SURFACE_TILES);

    public int Y_MAX_incrementor;
    public int _Y_MAX_incrementor;
    public int Y_MAX;
    public float Y_increment;
    public float X_increment;
    public float size;
    public float distance_bw_player_asset;

    public bool addRemove;
    public bool initial_call;
    public bool triggerEntered;
    public bool moveTrigger;

    public PCG_Manager() { }

    public PCG_Manager(int y_max_incrementor, int _y_max_incrementor, int y_max, float y_increment, float x_increment, float _size)
    {
        this.Y_MAX_incrementor = y_max_incrementor;
        this._Y_MAX_incrementor = _y_max_incrementor;
        this.Y_MAX = y_max;
        this.Y_increment = y_increment;
        this.X_increment = x_increment;
        this.size = _size;
    }

    public void Generate_Grid(string assetType)
    {
        switch (assetType)
        {
            case "SurfaceTiles&&SurfaceElements":

                for (int i = 0; i < MAX_SIZE_SURFACE_TILES; i++)
                {
                    Y_GRID[i] = size + Y_increment;
                    Y_increment = Y_increment + size;

                    if (i == Y_MAX || i == Y_MAX + Y_MAX_incrementor)
                    {
                        Y_MAX_incrementor = Y_MAX_incrementor + Y_MAX;
                        Y_increment = 0;
                    }
                }

                for (int j = 0; j < MAX_SIZE_SURFACE_TILES; j++)
                {
                    X_GRID[j] = size + X_increment;

                    if (j == Y_MAX || j == Y_MAX + _Y_MAX_incrementor)
                    {
                        _Y_MAX_incrementor = _Y_MAX_incrementor + Y_MAX;
                        X_increment = X_increment + size;
                    }
                }
                break;

            case "MineableRocks":

                for (int i = 0; i < MAX_SIZE_MINEABLE_ROCKS; i++)
                {
                    X_GRID[i] = UnityEngine.Random.Range(Y_increment, Y_increment + 40);
                    Y_increment = Y_increment + (int)size;
                    if (i == 60 || i == 60 + Y_MAX_incrementor)
                    {
                        Y_MAX_incrementor = Y_MAX_incrementor + 60;
                        Y_increment = 0;
                    }
                }

                for (int j = 0; j < MAX_SIZE_MINEABLE_ROCKS; j++)
                {
                    Y_GRID[j] = UnityEngine.Random.Range(X_increment, X_increment + 40);
                    if (j == Y_MAX || j == Y_MAX + _Y_MAX_incrementor)
                    {
                        _Y_MAX_incrementor = _Y_MAX_incrementor + Y_MAX;
                        X_increment = X_increment + (int)size;
                    }
                }
                break;
        }
    }

    public void Generate_Content(GameObject asset, GameObject[] assets, float areaSpan, List<Sprite> textures, string tagName, string assetType, int max_size, params int[] assetValue)
    {
        GameObject player = GameObject.Find("MainPlayer");
        Vector2 playerPos = player.transform.position;

        if (triggerEntered == true || initial_call == false)
        {
            initial_call = true;
            if (!addRemove)
            {
                addRemove = true;
                triggerEntered = false;
                moveTrigger = false;

                GameObject[] gameObjectTile = GameObject.FindGameObjectsWithTag(tagName);
                List<GameObject> gameObjectTileList = gameObjectTile.ToList();
                List<GameObject> finalList = gameObjectTileList.Distinct().ToList();

                for (int i = 0; i < max_size; i++)
                {
                    distance_bw_player_asset = Vector2.Distance(new Vector2(X_GRID[i], Y_GRID[i]), playerPos);
                    if (distance_bw_player_asset < areaSpan)
                    {
                        assets[i] = Instantiate(asset, new Vector3(X_GRID[i], Y_GRID[i], 0), transform.rotation) as GameObject;
                        assets[i].tag = tagName;
                        assets[i].name = tagName + " " + i;
						assets[i].transform.parent = GameObject.Find("PCG_World").transform;

                        assetValue[i] = UnityEngine.Random.Range(0, 10);
                        Change_Textures(textures, assets[i], i);

                        foreach (GameObject item in finalList) { if (item.name == assets[i].name) { Destroy(assets[i]); } }
                    }
                }

                GameObject[] allTexturesMyFriend = GameObject.FindGameObjectsWithTag(tagName);
                foreach (GameObject texture in allTexturesMyFriend)
                {
                    var distance = Vector2.Distance(texture.transform.position, playerPos);
                    if (distance > areaSpan)
                    {
                        GameObject.Destroy(texture);
                    }
                }
            }
        }
    }

    public void Generate_Content_Rocks(GameObject asset, GameObject[] assets, float areaSpan, List<Sprite> textures, string tagName, string assetType, int max_size, params int[] assetValue)
    {
        GameObject player = GameObject.Find("MainPlayer");
        Vector2 playerPos = player.transform.position;

        if (triggerEntered == true || initial_call == false)
        {
            initial_call = true;
            if (!addRemove)
            {
                addRemove = true;
                triggerEntered = false;
                moveTrigger = false;

                GameObject[] gameObjectTile = GameObject.FindGameObjectsWithTag(tagName);
                List<GameObject> gameObjectTileList = gameObjectTile.ToList();
                List<GameObject> finalList = gameObjectTileList.Distinct().ToList();

                for (int i = 0; i < max_size; i++)
                {
                    distance_bw_player_asset = Vector2.Distance(new Vector2(X_GRID[i], Y_GRID[i]), playerPos);
                    if (distance_bw_player_asset < areaSpan)
                    {
                        assets[i] = Instantiate(asset, new Vector3(X_GRID[i], Y_GRID[i], 0), transform.rotation) as GameObject;
                        assets[i].tag = tagName;
                        assets[i].name = tagName + " " + i;
                        assets[i].transform.parent = GameObject.Find("PCG_World").transform;
                        assetValue[i] = UnityEngine.Random.Range(0, 10);
                        Change_Textures(textures, assets[i], i);

                        foreach (GameObject item in finalList) { if (item.name == assets[i].name) { Destroy(assets[i]); } }
                    }
                }

                GameObject[] allTexturesMyFriend = GameObject.FindGameObjectsWithTag(tagName);
                foreach (GameObject texture in allTexturesMyFriend)
                {
                    var distance = Vector2.Distance(texture.transform.position, playerPos);
                    if (distance > areaSpan)
                    {
                        GameObject.Destroy(texture);
                    }
                }

                GameObject[] deletedMinerals = GameObject.FindGameObjectsWithTag("DeletedMinerals");

                foreach(GameObject mineral in deletedMinerals)
                {
                    foreach(GameObject texture in allTexturesMyFriend)
                    {
                        if (texture.name == mineral.name)
                        {
                            GameObject.Destroy(texture);
                        }
                    }
                }
            }
        }
    }

    public void Change_Textures(List<Sprite> textures, GameObject asset, int pos)
    {
        float remainder = 1.00F / textures.Count;
        for (float i = 0; i <= 1.00F; i += remainder)
        {
            if ((rand.seedRndNos_spawning[pos] <= i + remainder) && (rand.seedRndNos_spawning[pos] >= i))
            {
                asset.GetComponentInChildren<SpriteRenderer>().sprite = textures[(int)((i) * textures.Count)];
            }
        }

    }

    void Initialization()
    {
        addRemove = false;
        initial_call = false;
        triggerEntered = false;
        moveTrigger = false;
    }

    void Start()
    {
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;

        Initialization();
    }
}
