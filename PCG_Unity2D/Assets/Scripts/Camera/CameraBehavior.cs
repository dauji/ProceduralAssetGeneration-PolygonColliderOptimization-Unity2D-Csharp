using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour 
{
	public Transform FollowThisGameObject;

	void Update () 
	{
	 	transform.position = new Vector3 (FollowThisGameObject.position.x, FollowThisGameObject.position.y, - 10);
	}
}
