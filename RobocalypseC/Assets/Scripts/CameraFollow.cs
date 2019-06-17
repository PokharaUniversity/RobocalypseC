//Author @ ANUJ SHRESTHA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject player;
	public Vector3 Offset;
	public float smoother;
	[Header("verticalFollow")]
	public bool verticalMove;
	public float  verticalFollowOffset;


	public float  heightBuffer;


	void Start () {
		transform.position = player.transform.position;


	}


	void Update () {
		if (player != null) {
			if (verticalMove == true) {
				heightBuffer = player.transform.position.y  ;
			
			}
            bool PlayerFacingForward = player.GetComponent<PlayerController>().facingPositive;
			//Debug.Log (heightBuffer);
			Vector3 playerPosition =new Vector3( Offset.x,heightBuffer+Offset.y+verticalFollowOffset*System.Convert.ToInt32(verticalMove), player.transform.position.z + (PlayerFacingForward?Offset.z:-Offset.z));
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, playerPosition , smoother / 10);

		}

}
}