using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    KeyCode robotMenuKey = KeyCode.Slash;
    KeyCode robotWPKey = KeyCode.Period;
    KeyCode combatKey = KeyCode.Mouse1;

    float playerSpeed;
    Vector2 playerDirection;

    float zoomDuration, zoomElapsed;
    float zoomExitDuration, zoomExitElapsed;
    bool zoomTransition;

    int health, stamina;
    float dayLength, nightLength;

    [HideInInspector]
    public float input_x, input_y;
    bool inputButton_x, inputButton_y;
    float playerPosX, playerPosY;
    [HideInInspector]
    public Vector2 playerPosition;
    [HideInInspector]
    public bool robotMenuActive, addRobotWP;
    [HideInInspector]
    public bool explorationMode, combatMode;

    GameObject weapon;
    GameObject weaponImage;
    GameObject firePoint;
    int angle_upperLimit;
    int angle_lowerLimit;
    float rotationZ;

    public float PlayerPositionX { get { return playerPosX; } set { playerPosX = value; } }
    public float PlayerPositionY { get { return playerPosY; } set { playerPosY = value; } }
    public bool RobotMenu { get { return robotMenuActive; } set { robotMenuActive = value; } }
    public bool RobotWayPoint { get { return addRobotWP; } set { addRobotWP = value; } }
    public bool ExplorationMode { get { return explorationMode; } set { explorationMode = value; } }
    public bool CombatMode { get { return combatMode; } set { combatMode = value; } }
    public Vector2 PlayerPosition { get { return playerPosition; } set { playerPosition = value; } }

    bool moveFirePoint = false;
    bool moveFirePoint1 = false;
    bool moveFirePoint2 = false;
    bool moveFirePoint3 = false;
    bool moveFirePoint4 = false;
    bool moveFirePoint5 = false;

    // -- Show/Hide Weapon
    void ShowHideWeapon()
    {
        if (combatMode) { Debug.Log("xx"); weapon.SetActive(true); }
        if (explorationMode) { weapon.SetActive(false); }
    }

    // -- Manage Player Input and Animations
    void ManagePlayerInputAnimation()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        inputButton_x = Input.GetButton("Vertical");
        inputButton_y = Input.GetButton("Horizontal");
        playerDirection = new Vector2(input_x, input_y).normalized;

        if (Input.GetKeyDown(combatKey) == true)
        {
            explorationMode = !explorationMode;
            combatMode = !combatMode;
        }

        if (explorationMode && !combatMode) { anim.SetBool("Combat", false); }
        if (combatMode && !explorationMode) { anim.SetBool("Combat", true); }
        if ((inputButton_x || inputButton_y))
        {
            if (explorationMode) { anim.SetBool("Walking", true); }
            if (combatMode) { anim.SetBool("Combat", true); }
            anim.SetFloat("MoveX", playerDirection.x);
            anim.SetFloat("MoveY", playerDirection.y);
        }
        else
        {
            if (explorationMode) { anim.SetBool("Walking", false); }
            if (combatMode) { anim.SetBool("Combat", false); }
        }
    }

    // -- Player Movement based on Rigidbody2D
    void PlayerMovePosition()
    {
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + playerDirection * playerSpeed * Time.deltaTime);
    }

    // -- Move gun when in combat
    void AnimateGunWhenInCombat(int direction)
    {        
        Vector2 mousePos = Input.mousePosition;
        Vector2 player_pos = Camera.main.WorldToScreenPoint(weapon.transform.position);
        mousePos.x = mousePos.x - player_pos.x;
        mousePos.y = mousePos.y - player_pos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (direction == 0)
        {
            if ((angle < 45 && (angle > -45)))
                weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        if (direction == 1)
        {
            if ((angle < 135 && (angle > 45)))
                weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    // -- Initlize values for Start()
    void Initialize()
    {
        zoomDuration = 1.0F;
        zoomElapsed = 0.0F;
        zoomExitDuration = 1.0F;
        zoomExitElapsed = 0.0F;
        zoomTransition = false;
        robotMenuActive = false;
        addRobotWP = false;
        explorationMode = true;
        combatMode = false;
        playerSpeed = 3.0F;
        health = 100;
        stamina = 100;
        explorationMode = true;
        combatMode = false;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        Camera.main.orthographic = true;

        weapon = GameObject.FindGameObjectWithTag("Weapon");
        weaponImage = GameObject.Find("WeaponImage");
        firePoint = GameObject.Find("FirePoint");

        Initialize();
    }

    void Update()
    {
        ManagePlayerInputAnimation();
        ShowHideWeapon();

        Debug.Log("Combat Mode: " + combatMode + " Exploration Mode: " + explorationMode + " Weapon: ");
        Vector2 mousePos = Input.mousePosition;
    }

    void FixedUpdate()
    {
        PlayerMovePosition();
    }
}