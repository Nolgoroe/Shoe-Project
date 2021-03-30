using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextureHolderScript : MonoBehaviour, IPointerDownHandler
{
    public Texture heldTexture;

    public void OnPointerDown(PointerEventData eventData)
    {
        TouchManager.Instance.clickingTex = true;
        TouchManager.Instance.currentTHS = this;
        Debug.Log(transform.name);
    }
}
