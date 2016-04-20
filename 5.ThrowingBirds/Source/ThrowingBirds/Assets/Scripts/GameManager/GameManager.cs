using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;


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
		gameState = GameState.Start;

		slingShot.enabled = false;

		bricks = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Brick"));

		birds = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Bird"));

		pigs = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Pig"));
	
	}

	void OnEnable ()
	{
		slingShot.birdThrown += SlingShootBirtThrown;

	}

	void OnDisable ()
	{
		slingShot.birdThrown -= SlingShootBirtThrown;
		
	}
	
	// Update is called once per frame
	void Update () {

		switch (gameState) {
		case GameState.Start:
			if (Input.GetMouseButtonUp (0)) {

				AnimateBirdToSlingShoot ();

			}
			break;
		case GameState.Playing:
			if (slingShot.slingShootState == SlingshootState.BirdFlying && (BricksBirdsPigsStoppedMoving () ||
			    Time.time - slingShot.timeSinceThrown > 5f)) {
				slingShot.enabled = false;

				AnimateCameraToStartPosition ();

				gameState = GameState.BirdMovingToSlingshoot;
			}

			break;
		case GameState.Won:

			break;
		case GameState.Lost:
			if (Input.GetMouseButtonDown (0)) {
			
				SceneManager.LoadScene ("Gameplay");
			}
			break;

		}

		
	
	
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

	bool BricksBirdsPigsStoppedMoving ()
	{
		foreach (var item in bricks.Union(birds).Union(pigs)) 
		{
			if (item != null && item.GetComponent<Rigidbody2D> ().velocity.sqrMagnitude > GameVariables.MinVelocity){
			
				return false;
			
			}
		}
		return true;
	}

	private bool AllPigsAreDestroyed ()
	{
		return pigs.All (x => x == null);
	}

	private void AnimateCameraToStartPosition ()
	{
		float duration = Vector2.Distance (Camera.main.transform.position, cameraFollow.startingPoisition) / 10f;

		if (duration == 0.0f)
			duration = 0.1f;

		Camera.main.transform.positionTo (duration, cameraFollow.startingPoisition).
		setOnCompleteHandler ((x) => {

			cameraFollow.isFllowing = false;

			if (AllPigsAreDestroyed()) {
				gameState = GameState.Won;

			} else if (currentBirdIndex == birds.Count - 1) {
				gameState = GameState.Lost;
			} else {
				slingShot.slingShootState = SlingshootState.Idle;

				currentBirdIndex++;

				AnimateBirdToSlingShoot ();
			}

		});

	}

	private void SlingShootBirtThrown ()
	{
		cameraFollow.birdToFollow = birds [currentBirdIndex].transform;

		cameraFollow.isFllowing = true;
	}

}
