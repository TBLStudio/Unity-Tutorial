using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;


public class GameManager : MonoBehaviour {

	public CameraFollow cameraFollow;

	int currentBirdIndex;

	public SlingShot slingShot;

	[HideInInspector]
	public static GameState gameState;

	private List<GameObject> bricks;
	private List<GameObject> birds;
	private List<GameObject> pigs;

	// Use this for initialization
	void Awake () {
		bricks = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Brick"));

		birds = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Brick"));

		pigs = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Brick"));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}

	void AnimateBirdToSlingShoot ()
	{
		gameState = GameState.BirdMovingToSlingshoot;

		birds [currentBirdIndex].transform.positionTo (Vector2.Distance (birds [currentBirdIndex].transform.position / 10, 
														slingShot.birdWaitPosition.position) / 10, 
														slingShot.birdWaitPosition.position).
		setOnCompleteHandler ((x) => { 
			x.complete ();
			x.destroy ();
			gameState = GameState.Playing;
			slingShot.enabled = true;
			slingShot.birdToThrown = birds[currentBirdIndex];
		});
	}
}
