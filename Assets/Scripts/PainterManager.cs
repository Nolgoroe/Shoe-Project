using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;

public class PainterManager : MonoBehaviour
{
    public static PainterManager Instacne;

    public P3dPaintDecal painter;

    public Texture shapeTex;
    public Texture textureTex;

    public List<Texture> allTextures;
    public int numOfTextures;

    public GameObject textureButtonPrefab;
    public Transform textureParent;

    public P3dHitScreen hitScreenData;
    void Start()
    {
        Instacne = this;
        hitScreenData = GetComponent<P3dHitScreen>();
        hitScreenData.enabled = false;
        allTextures = new List<Texture>();
        for (int i = 1; i <= numOfTextures; i++)
        {
            allTextures.Add(Resources.Load("Textures/texture0" + i) as Texture);

            GameObject GO = Instantiate(textureButtonPrefab, textureParent);
            GO.GetComponent<RawImage>().texture = allTextures[i - 1];
            TextureHolderScript THS = GO.GetComponent<TextureHolderScript>();
            THS.heldTexture = allTextures[i - 1];
        }
    }

    //void Update()
    //{
    //    painter.Shape = shapeTex;
    //    painter.Texture = textureTex;
    //}

    public void BrushSizeChange(bool UpSize)
    {
        if (UpSize)
        {
            painter.Radius += 0.5f;
        }
        else
        {
            painter.Radius -= 0.5f;
        }

        painter.Radius = Mathf.Clamp(painter.Radius, 0.1f, 5f);
    }

    public void ThickStrongNoShape()
    {
        painter.Radius = .7f;
        painter.Hardness = 1f;
        painter.Texture = null;
    }

    public void ThickStrongCircle(Texture tex)
    {
        painter.Radius = .7f;
        painter.Hardness = 1f;
        painter.Texture = tex;
    }

    public void ThickWeakCircle(Texture tex)
    {
        painter.Radius = .7f;
        painter.Hardness = 0.3f;
        painter.Texture = tex;
    }

    public void ThinStrongNoShape()
    {
        painter.Radius = 0.3f;
        painter.Hardness = 1f;
        painter.Texture = null;

    }

    public void ThinWeakNoShape()
    {
        painter.Radius = 0.3f;
        painter.Hardness = 0.3f;
        painter.Texture = null;
    }

    public void ReallyThinWeakNoShape()
    {
        painter.Radius = 0.1f;
        painter.Hardness = 1f;
        painter.Texture = null;

    }
}
