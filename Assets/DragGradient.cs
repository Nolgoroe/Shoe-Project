using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragGradient : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        TouchManager.Instance.clickingGrad = true;
        TouchManager.Instance.gradTexture = GetComponent<Image>();
        TouchManager.Instance.originalGradTexPos = transform.localPosition;
        Debug.Log(transform.name);
    }
}
