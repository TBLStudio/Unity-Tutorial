using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

    public void OnPointerDown (PointerEventData data)
    {
        if (PlayerJumpScripts.instance != null)
        {
            PlayerJumpScripts.instance.SetPower(true);
        }
        
    }

    public void OnPointerUp (PointerEventData data)
    {
        if (PlayerJumpScripts.instance != null)
        {
            PlayerJumpScripts.instance.SetPower(false);
        }
    }

	
}
