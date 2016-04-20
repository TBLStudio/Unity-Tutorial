using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

    private BirdState birdState { get; set;}

    private TrailRenderer lineRenderer;

	private Rigidbody2D myBody;

    private CircleCollider2D myCollider;

    private AudioSource audioSource;

	// Use this for initialization
	void Awake () {

		InitializeVariables ();


	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (birdState == BirdState.Thrown && myBody.velocity.sqrMagnitude <= GameVariables.MinVelocity) {
			StartCoroutine (DestroyAfterDelay (2f));
		}
	
	}

	void InitializeVariables ()
	{
		lineRenderer = GetComponent<TrailRenderer>();

		myBody = GetComponent <Rigidbody2D>();

		myCollider = GetComponent <CircleCollider2D>();

		audioSource = GetComponent<AudioSource> ();

		lineRenderer.enabled = false;

		lineRenderer.sortingLayerName = "Foreground";

		myBody.isKinematic = true;

		myCollider.radius = GameVariables.BirdColliderRadiusBig;

		birdState = BirdState.BeforeThrown;
		
	}

	public void OnThrown()
	{
		audioSource.Play ();

		lineRenderer.enabled = true;

		myBody.isKinematic = false;

		myCollider.radius = GameVariables.BirdColliderRadiusNomal;

		birdState = BirdState.Thrown;
	}

	IEnumerator DestroyAfterDelay (float delay)
	{
		yield return new WaitForSeconds (delay);

		Destroy (gameObject);
	}
}



















