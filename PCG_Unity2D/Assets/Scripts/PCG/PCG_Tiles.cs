using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PCG_Tiles : PCG_Manager
{
    public PCG_Tiles() : base(0, 0, 90, 0, 1, 5.12F) { }

    PCG_TriggerA trigger1 = new PCG_TriggerA();
    PCG_TriggerB trigger2 = new PCG_TriggerB();
    PCG_TriggerC trigger3 = new PCG_TriggerC();
    PCG_TriggerD trigger4 = new PCG_TriggerD();

    public GameObject _tile;
    public List<Sprite> textures;

    GameObject[] tiles = new GameObject[MAX_SIZE_SURFACE_TILES];

    public float areaSpan;
    int[] extractableValue = new int[MAX_SIZE_SURFACE_TILES];

    public int[] ExtractableValues
    {
        get { return extractableValue; }
        set { extractableValue = value; }
    }

    void Position_Triggers()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;
        trigger1.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 2.5F);
        trigger2.transform.position = new Vector2(mainPlayerPos.x - 2.5F, mainPlayerPos.y);
        trigger3.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 2.5F);
        trigger4.transform.position = new Vector2(mainPlayerPos.x + 2.5F, mainPlayerPos.y);
    }

    void Reposition_Triggers()
    {
        GameObject mainPlayer = GameObject.Find("MainPlayer");
        Vector2 mainPlayerPos = mainPlayer.transform.position;

        if (trigger1.playerInsideCircleTriggerA == true || trigger2.playerInsideCircleTriggerA1 == true || trigger3.playerInsideCircleTriggerA2 == true || trigger4.playerInsideCircleTriggerA3 == true)
        {
            if (!moveTrigger)
            {
                moveTrigger = true;
                addRemove = false;
                triggerEntered = true;
                trigger1.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y + 2.5F);
                trigger2.transform.position = new Vector2(mainPlayerPos.x - 2.5F, mainPlayerPos.y);
                trigger3.transform.position = new Vector2(mainPlayerPos.x, mainPlayerPos.y - 2.5F);
                trigger4.transform.position = new Vector2(mainPlayerPos.x + 2.5F, mainPlayerPos.y);
            }
        }
    }

    void Initialization()
    {
        areaSpan = 15.0F;
    }

    void Start()
    {
        rand = FindObjectOfType(typeof(PCG_Rand)) as PCG_Rand;  // this is important, has to be in each extended class which uses rand
        trigger1 = FindObjectOfType(typeof(PCG_TriggerA)) as PCG_TriggerA;
        trigger2 = FindObjectOfType(typeof(PCG_TriggerB)) as PCG_TriggerB;
        trigger3 = FindObjectOfType(typeof(PCG_TriggerC)) as PCG_TriggerC;
        trigger4 = FindObjectOfType(typeof(PCG_TriggerD)) as PCG_TriggerD;

        Initialization();
        Position_Triggers();
        Generate_Grid("SurfaceTiles&&SurfaceElements");
    }

    void Update()
    {
        Generate_Content(_tile, tiles, areaSpan, textures, "FinalTextures", "SurfaceTiles&&SurfaceElements", MAX_SIZE_SURFACE_TILES, extractableValue);
        Reposition_Triggers();
    }
}
