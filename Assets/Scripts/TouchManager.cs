using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;
    //private Touch touch;

    private Vector2 touchPos;

    private float rotationZ, rotationX;
    private Camera Camera;

    public Transform toRotate;
    public Transform toZoom;
    public float rotationSpeedModifier = 0.5f;
    public float zoomSpeed = 3;

    public static bool isInGame = false;

    public RawImage texture;
    public Image gradTexture;
    public Vector3 originalGradTexPos;

    //RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];
    public bool clickingTex = false;
    public bool clickingGrad = false;
    public Texture paintMapGrad;

    public float timerForGetTex = 0;

    [HideInInspector]
    public bool chosenTex = false;
    [HideInInspector]
    public bool chosenGrad = false;

    Vector3 screenPos;
    public LayerMask hitLayer3D;

    public ScrollRect textureScrollRect;
    Transform previouslyDetectedPiece = null;
    public float timerForApplyTex = 0;
    bool changingTexNow = false;

    private P3dPaintable paintableObject;
    private P3dHitScreen hitScreen;
    private P3dPaintableTexture paintableTexture;

    [HideInInspector]
    public bool canPaint;

    public float tileX, tileY;
    public float offsetX, offsetY;

    public Touch touch;

    public TextureHolderScript currentTHS;
    void Start()
    {
        canPaint = false;
        changingTexNow = false;
        Instance = this;
        Camera = Camera.main;
        isInGame = false;
        clickingTex = false;
        clickingGrad = false;
        chosenTex = false;
        chosenGrad = false;
        texture.gameObject.SetActive(false);
        gradTexture = null;
    }

    void Update()
    {
        if (isInGame)
        {
            if(Input.touchCount == 0)
            {
                PainterManager.Instacne.hitScreenData.enabled = false;

                textureScrollRect.enabled = true;
                timerForApplyTex = 0;
                timerForGetTex = 0;
                previouslyDetectedPiece = null;
                clickingTex = false;
                clickingGrad = false;
                chosenTex = false;
                chosenGrad = false;
                texture.gameObject.SetActive(false);
                gradTexture = null;
                paintableObject = null;
                paintableTexture = null;
                canPaint = true;
                currentTHS = null;
                DisconnectPaint3DTouches();
            }

            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);

                if (!PainterManager.Instacne.hitScreenData.enabled)
                {
                    Invoke("EnableHitScreen", 0.1f);
                }

                if (touch.phase == TouchPhase.Stationary)
                {
                    if (clickingTex && !chosenTex)
                    {
                        timerForGetTex += Time.deltaTime;

                        if (timerForGetTex > 0.1f)
                        {
                            textureScrollRect.enabled = false;
                            chosenTex = true;
                            timerForApplyTex = 0;
                            TextureHolderScript THS = currentTHS.transform.GetComponent<TextureHolderScript>();

                            texture.gameObject.SetActive(true);
                            texture.texture = THS.heldTexture;

                            Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                            mousePos.z = texture.transform.position.z;
                            //Debug.DrawRay(Camera.transform.position, mousePos, Color.red);

                            texture.transform.position = mousePos;
                            texture.transform.localScale = new Vector3(1, 1, 1);

                        }
                    }

                    if (clickingGrad && !chosenGrad)
                    {
                        timerForGetTex += Time.deltaTime;

                        if (timerForGetTex > 0.1f)
                        {
                            //textureScrollRect.enabled = false;
                            chosenGrad = true;
                            timerForApplyTex = 0;
                            //TextureHolderScript THS = currentTHS.transform.GetComponent<TextureHolderScript>();

                            //texture.gameObject.SetActive(true);
                            //texture.texture = THS.heldTexture;

                            Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                            mousePos.z = gradTexture.transform.position.z - 1;
                            //Debug.DrawRay(Camera.transform.position, mousePos, Color.red);

                            gradTexture.transform.position = mousePos;
                            //texture.transform.position = mousePos;
                            //texture.transform.localScale = new Vector3(1, 1, 1);

                        }
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    timerForGetTex = 0;
                    timerForApplyTex = 0;
                    
                    if (chosenTex && !changingTexNow)
                    {
                        changingTexNow = true;
                        Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                        mousePos.z = 75;
                        texture.transform.position = mousePos;
                        texture.transform.localScale = new Vector3(1, 1, 1);
                        RaycastHit hit;
                        Ray ray = Camera.ScreenPointToRay(touch.position);

                        //Debug.DrawRay(Camera.transform.position, mousePos, Color.red);
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            if (hit.transform.CompareTag("ShoePiece") || hit.transform.CompareTag("ShoePieceNonPaintable"))
                            {

                                texture.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 1);
                                texture.transform.localScale = new Vector3(0.2f, 0.2f, 1);

                                timerForApplyTex += Time.deltaTime;

                                if (hit.transform.CompareTag("ShoePiece"))
                                {
                                    paintableObject = hit.transform.GetComponent<P3dPaintable>();
                                    paintableTexture = hit.transform.GetComponent<P3dPaintableTexture>();
                                }

                            }
                        }
                        else
                        {
                            paintableObject = null;
                            paintableTexture = null;
                            timerForApplyTex = 0;

                            //if (previouslyDetectedPiece)
                            //{
                            //    Renderer previouslyDetectedPieceRenderer = previouslyDetectedPiece.transform.GetComponent<Renderer>();
                            //    previouslyDetectedPieceRenderer.material.SetTexture("_BaseMap", null);
                            //    previouslyDetectedPiece = null;
                            //}
                        }

                        changingTexNow = false;
                    }

                    if (chosenGrad && !changingTexNow)
                    {
                        changingTexNow = true;
                        Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                        mousePos.z = 74;
                        gradTexture.transform.position = mousePos;
                        gradTexture.transform.localScale = new Vector3(1, 1, 1);
                        RaycastHit hit;
                        Ray ray = Camera.ScreenPointToRay(touch.position);

                        //Debug.DrawRay(Camera.transform.position, mousePos, Color.red);
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            if (hit.transform.CompareTag("ShoePiece") || hit.transform.CompareTag("ShoePieceNonPaintable"))
                            {
                                gradTexture.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 1);
                                gradTexture.transform.localScale = new Vector3(0.2f, 0.2f, 1);

                                timerForApplyTex += Time.deltaTime;

                                if (hit.transform.CompareTag("ShoePiece"))
                                {
                                    paintableObject = hit.transform.GetComponent<P3dPaintable>();
                                    paintableTexture = hit.transform.GetComponent<P3dPaintableTexture>();
                                }
                            }
                        }
                        else
                        {
                            paintableObject = null;
                            paintableTexture = null;
                            timerForApplyTex = 0;
                        }
                        changingTexNow = false;
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    DisconnectPaint3DTouches();

                    if (chosenTex)
                    {
                        StartCoroutine(ChangeTexOnPiece(touch));
                    }

                    if (chosenGrad)
                    {
                        gradTexture.transform.localPosition = originalGradTexPos;
                        gradTexture.transform.localScale = new Vector3(1, 1, 1);
                        StartCoroutine(ChangeGradOnPiece(touch));
                    }
                }

            }

            if (Input.touchCount >= 2)
            {
                PainterManager.Instacne.hitScreenData.enabled = false;

                canPaint = false;
                //PainterManager.Instacne.hitScreenData.enabled = false;
                //PainterManager.Instacne.hitScreenData.Paint()


                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);

                if (touchOne.phase == TouchPhase.Moved)
                {
                    if (toRotate.transform.eulerAngles.y < 320 && toRotate.transform.eulerAngles.y > 230)
                    {
                        float deltaX = touchOne.deltaPosition.x;
                        float deltaY = touchOne.deltaPosition.y;
                        rotationX -= deltaX * Time.deltaTime * rotationSpeedModifier;
                        rotationZ -= deltaY * Time.deltaTime * rotationSpeedModifier;
                        rotationZ = Mathf.Clamp(rotationZ, -13, 13);
                    }
                    else
                    {
                        float deltaX = touchOne.deltaPosition.x;
                        float deltaY = touchOne.deltaPosition.y;
                        rotationX -= deltaX * Time.deltaTime * rotationSpeedModifier;
                        rotationZ += deltaY * Time.deltaTime * rotationSpeedModifier;
                        rotationZ = Mathf.Clamp(rotationZ, -13, 13);
                    }
                    toRotate.transform.eulerAngles = new Vector3(0, rotationX, rotationZ);
                }
            }
        }
    }


    public void RefreshMap(P3dPaintable paintableObject)
    {
        paintableObject.Activate();
    }

    public IEnumerator ChangeTexOnPiece(Touch touch)
    {
        if (paintableObject)
        {
            Transform newDetected = paintableObject.transform;
            Mesh newMesh = newDetected.GetComponent<MeshFilter>().mesh;
            HelpData hd = newDetected.GetComponent<HelpData>();
            Renderer meshRendere = newDetected.GetComponent<MeshRenderer>();

            timerForApplyTex = 0;

            meshRendere.material = hd.normalMat;
            //paintableTexture.Texture = meshRendere.material.GetTexture("_BaseMap");

            Renderer newDetectedPieceRenderer = newDetected.transform.GetComponent<Renderer>();
            newDetectedPieceRenderer.materials[0].SetTexture("_BaseMap", texture.texture);
            previouslyDetectedPiece = newDetected;

            RefreshMap(paintableObject);
        }
        else
        {
            timerForApplyTex = 0;

        }

        changingTexNow = false;

        yield return null;

    }
    public IEnumerator ChangeGradOnPiece(Touch touch)
    {
        if (paintableObject)
        {
            Transform newDetected = paintableObject.transform;
            Renderer meshRendere = newDetected.GetComponent<MeshRenderer>();
            HelpData hd = newDetected.GetComponent<HelpData>();

            timerForApplyTex = 0;

            meshRendere.material = hd.gradMat;
            //paintableTexture.Texture = paintMapGrad;

            meshRendere.material.SetColor("_Color", ColorPickerSimple.Instacne.colorPickedFrontImage.color);
            meshRendere.material.SetColor("_Color2", ColorPickerSimple.Instacne.colorPickedBackImage.color);

            //meshRendere.materials[0].SetTexture("_BaseMap", paintMapGrad);

            RefreshMap(paintableObject);
        }
        else
        {
            timerForApplyTex = 0;

        }

        changingTexNow = false;

        yield return null;

    }

    private void DisconnectPaint3DTouches()
    {
        var fingers = PainterManager.Instacne.hitScreenData.inputManager.Fingers;
        for (var i = fingers.Count - 1; i >= 0; i--)
        {
            var finger = fingers[i];

#if !USE_LEAN_TOUCH
            if (finger.StartedOverGui == true)
            {
                continue;
            }
#endif

            PainterManager.Instacne.hitScreenData.Paint(finger, false, true);

        }

        for (int i = 0; i < PainterManager.Instacne.hitScreenData.links.Count; i++)
        {
            PainterManager.Instacne.hitScreenData.BreakHits(PainterManager.Instacne.hitScreenData.links[i]);
        }

        PainterManager.Instacne.hitScreenData.links.Clear();
    }

    private void EnableHitScreen()
    {
        PainterManager.Instacne.hitScreenData.enabled = true;
    }
}

