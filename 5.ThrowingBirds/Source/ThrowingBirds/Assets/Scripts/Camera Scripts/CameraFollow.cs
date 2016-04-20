using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	[HideInInspector]
	public Vector3 startingPoisition;

	public float minCameraX = 0f, maxCameraX = 19f;

	[HideInInspector]
	public bool isFllowing;

	[HideInInspector]
	public Transform birdToFollow;



	// Use this for initialization
	void Awake () {

		startingPoisition = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (isFllowing) {
			
			if (birdToFollow != null) {
				
				var birdPosition = birdToFollow.position;

				float x = Mathf.Clamp (birdPosition.x, minCameraX, maxCameraX);

				transform.position = new Vector3 (x, startingPoisition.y, startingPoisition.z);

			} else {
				isFllowing = false;
			}
		}
	
	}
}
