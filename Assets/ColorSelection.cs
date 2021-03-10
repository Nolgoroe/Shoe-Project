using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelection : MonoBehaviour
{
    public Image colorPickerImage;

    public Color chosenColor;
    void Start()
    {
        colorPickerImage.alphaHitTestMinimumThreshold = 0.9f;
    }

    void Update()
    {

    }
    public void CheckClickAngle()
    {
        float adjacentRad = 130;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 SA = (Vector3)touch.position - colorPickerImage.transform.position;
            float angle = Mathf.Cos(SA.magnitude / adjacentRad) * Mathf.Rad2Deg;
        }
        else
        {
            Vector3 SL = colorPickerImage.transform.position - new Vector3(130,-130,0);

            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)), out hit))
                return;

            Vector3 SLToPixel = Vector3.zero;

            SLToPixel.x = (hit.point - SL).x;
            SLToPixel.y = (SL - hit.point).y;
            SLToPixel.z = 0;


            Image rend = hit.transform.GetComponent<Image>();
            //MeshCollider meshCollider = hit.collider as MeshCollider;

            if (rend == null /*|| meshCollider == null*/)
                return;

            Debug.Log(hit.point);
            Texture2D tex = rend.sprite.texture;
            Vector2 pixelUV = hit.textureCoord;
            SLToPixel.x *= tex.width;
            SLToPixel.y *= tex.height;

            chosenColor = tex.GetPixel((int)SLToPixel.x, (int)SLToPixel.y);
            chosenColor.a = 1;
            tex.Apply();
        }

    }
}
