using UnityEngine;
using System.Collections;

public class NeedleHeadScript : MonoBehaviour {

    void OnTriggerEnter2D (Collider2D target)
    {
        if (target.tag == "Needle Head")
        {
            print("Game Over");

            Time.timeScale = 0f;
        }
    }
}
