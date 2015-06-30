using UnityEngine;
using System.Collections;

public class PCG_TriggerB : MonoBehaviour 
{
    [HideInInspector]
    public bool playerInsideCircleTriggerA1 = false;
    [HideInInspector]
    public bool playerExitedCircleTriggerA1 = false;

    void Start() { playerInsideCircleTriggerA1 = false; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "MainPlayer")
        {
            playerInsideCircleTriggerA1 = true;
            playerExitedCircleTriggerA1 = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "MainPlayer")
        {
            playerInsideCircleTriggerA1 = false;
            playerExitedCircleTriggerA1 = true;
        }
    }
}
