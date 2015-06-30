using UnityEngine;
using System.Collections;

public class PCG_TriggerA : MonoBehaviour 
{
    [HideInInspector]
    public bool playerInsideCircleTriggerA = false;
    [HideInInspector]
    public bool playerExitedCircleTriggerA = false;

    void Start() { playerInsideCircleTriggerA = false; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "MainPlayer")
        {
            playerInsideCircleTriggerA = true;
            playerExitedCircleTriggerA = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "MainPlayer")
        {
            playerInsideCircleTriggerA = false;
            playerExitedCircleTriggerA = true;
        }
    }
}
