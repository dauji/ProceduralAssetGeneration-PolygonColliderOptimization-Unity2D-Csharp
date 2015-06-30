using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PCG_Rand : MonoBehaviour 
{
    // Make changes to these values in the editor.
    [HideInInspector]
    public List<float> seedRndNos_spawning = new List<float>(20000);
    [HideInInspector]
    public List<float> seedRndNos_deleting = new List<float>(20000);

    public List<float> SeedRndNosSpawning
    {
        get { return seedRndNos_spawning; }
        set { seedRndNos_spawning = value; }
    }

    public List<float> SeedRndNosDeleting
    {
        get { return seedRndNos_deleting; }
        set { seedRndNos_deleting = value; }
    }


	void Start () 
    {
        SeedRndNos_Spawning();
        SeedRndNos_Deleting();
	}

    public void SeedRndNos_Spawning()
    {
        Random.seed = System.Environment.TickCount;
        for (int i = 0; i < seedRndNos_spawning.Count; i++) { seedRndNos_spawning[i] = Random.value; }
    }

    //-- Set the other seed to a different value from the first seed because if they are same it is deleting the same surface elements whose sprites I am trying to change since for both
    // spawning and deleting, it's giving out the same set of values.
    public void SeedRndNos_Deleting()
    {
        Random.seed = Random.Range(System.Environment.TickCount, System.Environment.TickCount * System.Environment.TickCount);
        for (int i = 0; i < seedRndNos_deleting.Count; i++) { seedRndNos_deleting[i] = Random.value; }
    }
}
