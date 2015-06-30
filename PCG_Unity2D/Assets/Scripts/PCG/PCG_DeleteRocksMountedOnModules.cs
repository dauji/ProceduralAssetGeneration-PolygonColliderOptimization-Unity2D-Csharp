using UnityEngine;
using System.Collections;

public class PCG_DeleteRocksMountedOnModules : MonoBehaviour
{
    public bool rockInsidePolygonTrigger = false;

    private bool doOnce = false;

    void Start() { rockInsidePolygonTrigger = false; }

    void OnTriggerStay2D(Collider2D other)
    {
		if ((other.gameObject.tag == "Modules" ) || (other.gameObject.tag == "HabitatModule") || (other.gameObject.tag == "HealthStaminaModule") || (other.gameObject.tag == "RefineryModule")
		    || (other.gameObject.tag == "Airlock") || (other.gameObject.tag.Contains("Connect Point")) || (other.gameObject.tag == "BuildingModule") || (other.gameObject.tag == "InitialTerrainTrigger"))
        {
			rockInsidePolygonTrigger = true;
            Destroy(gameObject);
        }
    }
}
