using UnityEngine;
using System.Collections;

public class PCG_CraterCollision : MonoBehaviour
{
    PlayerController playerController;

    [HideInInspector]
    public bool playerInsideCrater = false;
    [HideInInspector]
    public bool playerExitedCrater = false;

    private bool doOnce = false;

    void Start() 
    {
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        playerInsideCrater = false; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "MainPlayer")
        {
            playerInsideCrater = true;
            playerExitedCrater = false;
        //    playerController.speed = 0.5F;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "MainPlayer")
        {
            playerInsideCrater = false;
            playerExitedCrater = true;
          //  playerController.speed = 1.0F;
        }
    }

    void ChangePlayerSpeed()
    {

    }
}
