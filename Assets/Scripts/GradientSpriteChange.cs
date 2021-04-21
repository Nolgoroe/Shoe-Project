using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientSpriteChange : MonoBehaviour
{
    public Material GradientMat;
    void Start()
    {
        GradientMat.SetColor("_Color", Color.black); // the left color
        GradientMat.SetColor("_Color2", Color.white); // the right color
    }
}
