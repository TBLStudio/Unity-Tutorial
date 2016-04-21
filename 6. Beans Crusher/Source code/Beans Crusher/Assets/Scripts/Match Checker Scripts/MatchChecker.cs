using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MatchChecker {

	public static IEnumerator AnimatePontentialMatches (IEnumerable<GameObject> pontentialMatches)
	{
		for (float i = 1.0f; i >= 0.3f; i -= 0.1f) {
			foreach (var item in pontentialMatches) {
				Color c = item.GetComponent <SpriteRenderer> ().color;
				c.a = i;
				item.GetComponent <SpriteRenderer>().color = c;

			}
			yield return new WaitForSeconds (GameVariables.OpacityAnimationDelay);
		}
		for (float i = 0.3f; i <= 1.0f; i += 0.1f) {
			foreach (var item in pontentialMatches) {
				Color c = item.GetComponent <SpriteRenderer> ().color;
				c.a = i;
				item.GetComponent <SpriteRenderer>().color = c;

			}
			yield return new WaitForSeconds (GameVariables.OpacityAnimationDelay);
		}
	}

	public static bool AreHorizontalOrVerticalNeighbors (Candy c1, Candy c2)
	{
		return (c1.Column == c2.Column || c1.Row == c2.Row) && Mathf.Abs (c1.Column - c2.Column) <= 1 && Mathf.Abs (c1.Row - c2.Row) <= 1;
	}




}
