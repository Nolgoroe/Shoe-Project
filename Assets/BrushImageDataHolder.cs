using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushImageDataHolder : MonoBehaviour
{
    public Sprite black, white;

    Image brushImage;

    void Start()
    {
        brushImage = GetComponent<Image>();
    }

    public void SetToWhite()
    {
        brushImage.sprite = white;
    }

    public void SetToBlack()
    {
        brushImage.sprite = black;
    }
}
