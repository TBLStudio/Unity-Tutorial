using UnityEngine;
using System.Collections;
using System.Xml.Linq;

public class SlingShot : MonoBehaviour {


	private Vector3 slingShootMiddleVector;

	[HideInInspector]
	public SlingshootState slingShootState;

	public Transform leftSlingShootOrigin, rightSlingShootOrigin;

	public LineRenderer slingShootLineRenderer1, slingShootLineRenderer2, trajectoryLineRenderer;

	[HideInInspector]
	public GameObject birdToThrown;

	public Transform birdWaitPosition;

	public float thrownSpeed;

	[HideInInspector]
	public float timeSinceThrown;

	public delegate void BirdThrown ();

	public event BirdThrown birdThrown;



	void Awake () {

		slingShootLineRenderer1.sortingLayerName = "Foreground";

		slingShootLineRenderer2.sortingLayerName = "Foreground";

		trajectoryLineRenderer.sortingLayerName = "Foreground";

		slingShootState = SlingshootState.Idle;

		slingShootLineRenderer1.SetPosition (0, leftSlingShootOrigin.position);

		slingShootLineRenderer2.SetPosition (0, rightSlingShootOrigin.position);

		slingShootMiddleVector = new Vector3 ((leftSlingShootOrigin.position.x + rightSlingShootOrigin.position.x) / 2, 
											  (leftSlingShootOrigin.position.y + rightSlingShootOrigin.position.y) / 2, 0);
		
	
	}
	
	// Update is called once per frame
	void Update () {

		switch (slingShootState)
		{
		case SlingshootState.Idle:

			InitializeBird ();

			DisplaySlingshootLineRenderer ();

			if (Input.GetMouseButtonDown (0)) 
			{
				Vector3 location = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				if (birdToThrown.GetComponent <CircleCollider2D> () == Physics2D.OverlapPoint (location)){
					slingShootState = SlingshootState.UserPulling;
				}
				
			}
			break;

		case SlingshootState.UserPulling:

			DisplaySlingshootLineRenderer ();

			if (Input.GetMouseButton (0)) {
				
				Vector3 location = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				location.z = 0;

				if (Vector3.Distance (location, slingShootMiddleVector) > 1.5f) {
					
					var maxPosition = (location - slingShootMiddleVector).normalized * 1.5f + slingShootMiddleVector;

					birdToThrown.transform.position = maxPosition;

				} else {
					
					birdToThrown.transform.position = location;

				}
				var distance = Vector3.Distance (slingShootMiddleVector, birdToThrown.transform.position);

				DisplayTrajectoryLineRenderer (distance);

			} else {
				
				SetTrajectoryLineRendererActive (true);

				timeSinceThrown = Time.time;

				float distance = Vector3.Distance (slingShootMiddleVector, birdToThrown.transform.position);

				if (distance > 1) {

					SetSlingShootLineRenderersActive (false);

					slingShootState = SlingshootState.BirdFlying;

					ThrownBird (distance);
				
				} else {
					birdToThrown.transform.positionTo (distance /10, birdWaitPosition.position);
					InitializeBird ();
				}
			
			}

			break;

		}
	
	}

	void InitializeBird ()
	{
		birdToThrown.transform.position = birdWaitPosition.position;
		slingShootState = SlingshootState.Idle;
		
	}

	void SetSlingShootLineRenderersActive (bool active)
	{
		slingShootLineRenderer1.enabled = active;
		slingShootLineRenderer2.enabled = active;
	}

	void DisplaySlingshootLineRenderer ()
	{
		slingShootLineRenderer1.SetPosition (1, birdToThrown.transform.position);
		slingShootLineRenderer2.SetPosition (1, birdToThrown.transform.position);

	}

	void SetTrajectoryLineRendererActive (bool active)
	{
		trajectoryLineRenderer.enabled = active;
	}

	void DisplayTrajectoryLineRenderer (float distance)
	{
		SetTrajectoryLineRendererActive (true);

		Vector3 v2 = slingShootMiddleVector - birdToThrown.transform.position;

		int segementCount = 15;

		Vector2[] segments = new Vector2[segementCount];

		segments [0] = birdToThrown.transform.position;

		Vector2 segVelocity = new Vector2 (v2.x, v2.y) * thrownSpeed * distance;

		for (int i = 1; i < segementCount; i++) {

			float time = i * Time.fixedDeltaTime * 5f;

			segments[i] = segments[0] + segVelocity * time + 0.5f * Physics2D.gravity * Mathf.Pow(time, 2);

		}

		trajectoryLineRenderer.SetVertexCount (segementCount);

		for (int i = 0; i < segementCount; i++) 
		{
			trajectoryLineRenderer.SetPosition (i, segments[i]);
		}
	}

	private void ThrownBird (float distance)
	{
		Vector3 velocity = slingShootMiddleVector - birdToThrown.transform.position;

		birdToThrown.GetComponent <Bird>().OnThrown ();

		birdToThrown.GetComponent <Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) * thrownSpeed * distance;

		if (birdThrown != null)
			birdThrown ();
	}
}
