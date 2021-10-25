using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PaintIn3D;
using System.IO;
using UnityEngine.Networking;


public class TouchManager : MonoBehaviour
{

    public static TouchManager Instance;
    //private Touch touch;

    private Vector2 touchPos;

    private float ShoeRotationZ, ShoeRotationY;
    private Camera Camera;

    public Transform toRotate;
    public Transform toZoom;
    public float rotationSpeedModifier = 0.5f;
    public float zoomSpeed = 3;

    public static bool isInGame = true;
    public static bool canPaintShoe = false;
    public static bool isSharingScreen = false;

    public RawImage texture;
    public Image gradTexture;
    public Vector3 originalGradTexPos;

    //RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];
    public bool clickingTex = false;
    public bool clickingGrad = false;
    public Texture paintMapGrad;

    public float timerForGetTex = 0;

    
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

    public Transform QuatMat; ///Second

    public Camera gradCam; ///Second

    public RenderTexture curTex; ///Second

    void Start()
    {
        canPaint = false;
        changingTexNow = false;
        Instance = this;
        Camera = Camera.main;
        isInGame = true;
        clickingTex = false;
        clickingGrad = false;
        chosenTex = false;
        chosenGrad = false;
        texture.gameObject.SetActive(false);
        gradTexture = null;
        isSharingScreen = false;
        //gradCam.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isInGame)
        {
            if (Input.touchCount == 0)
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

            if (Input.touchCount > 0)
            {
                if (AnimationManager.isFinishedAnimation)
                {
                    DeactivateObjectsOnScreenTouch();
                    AnimationManager.isFinishedAnimation = false;
                }
            }

            if (Input.touchCount == 1 && canPaintShoe)
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

            if (Input.touchCount >= 2 && canPaintShoe)
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
                        ShoeRotationY -= deltaX * Time.deltaTime * rotationSpeedModifier;
                        ShoeRotationZ -= deltaY * Time.deltaTime * rotationSpeedModifier;
                        ShoeRotationZ = Mathf.Clamp(ShoeRotationZ, -13, 13);
                    }
                    else
                    {
                        float deltaX = touchOne.deltaPosition.x;
                        float deltaY = touchOne.deltaPosition.y;
                        ShoeRotationY -= deltaX * Time.deltaTime * rotationSpeedModifier;
                        ShoeRotationZ += deltaY * Time.deltaTime * rotationSpeedModifier;
                        ShoeRotationZ = Mathf.Clamp(ShoeRotationZ, -13, 13);
                    }
                    toRotate.transform.eulerAngles = new Vector3(0, ShoeRotationY, ShoeRotationZ);
                }
            }
        }

        if (isSharingScreen)
        {
            if (Input.touchCount >= 2)
            {
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);

                if (touchOne.phase == TouchPhase.Moved)
                {
                    if (toRotate.transform.eulerAngles.y < 320 && toRotate.transform.eulerAngles.y > 230)
                    {
                        float deltaX = touchOne.deltaPosition.x;
                        float deltaY = touchOne.deltaPosition.y;
                        ShoeRotationY -= deltaX * Time.deltaTime * rotationSpeedModifier;
                        ShoeRotationZ -= deltaY * Time.deltaTime * rotationSpeedModifier;
                        ShoeRotationZ = Mathf.Clamp(ShoeRotationZ, -13, 13);
                    }
                    else
                    {
                        float deltaX = touchOne.deltaPosition.x;
                        float deltaY = touchOne.deltaPosition.y;
                        ShoeRotationY -= deltaX * Time.deltaTime * rotationSpeedModifier;
                        ShoeRotationZ += deltaY * Time.deltaTime * rotationSpeedModifier;
                        ShoeRotationZ = Mathf.Clamp(ShoeRotationZ, -13, 13);
                    }
                    toRotate.transform.eulerAngles = new Vector3(0, ShoeRotationY, ShoeRotationZ);
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
            HelpData hd = newDetected.GetComponent<HelpData>();
            Renderer meshRenderer = newDetected.GetComponent<MeshRenderer>();

            timerForApplyTex = 0;
            meshRenderer.material.SetColor("_FirstColor", Color.white);
            meshRenderer.material.SetColor("_SecondColor", Color.white);

            //meshRendere.material = hd.normalMat;
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
            Renderer meshRenderer = newDetected.GetComponent<MeshRenderer>();
            HelpData hd = newDetected.GetComponent<HelpData>();

            timerForApplyTex = 0;

            Gradient gradient = new Gradient();

            GradientColorKey[] ColorKey = { new GradientColorKey(ColorPickerSimple.Instacne.colorPickedFrontImage.color, 0), new GradientColorKey(ColorPickerSimple.Instacne.colorPickedBackImage.color, 1) };
            GradientAlphaKey[] AlphaKey = { new GradientAlphaKey(1, 1), new GradientAlphaKey(1, 1) };

            gradient.SetKeys(ColorKey, AlphaKey);

            float yStep = 1f / 1024f;

            var texture = new Texture2D(1024, 1024);

            for (int y = 0; y < 1024; y++)
            {
                Color color = gradient.Evaluate(y * yStep);

                for (int x = 0; x < 1024; x++)
                {
                    texture.SetPixel(Mathf.CeilToInt(x), Mathf.CeilToInt(y), color);
                }
            }

            texture.Apply();


            //connect texture to material of GameObject this script is attached to
            //meshRenderer.material.mainTexture = texture;


            //meshRenderer.material = hd.gradMat; ///First
            //paintableTexture.Texture = paintMapGrad; ///First

            //meshRenderer.material.SetColor("_FirstColor", ColorPickerSimple.Instacne.colorPickedFrontImage.color); ///First
            //meshRenderer.material.SetColor("_SecondColor", ColorPickerSimple.Instacne.colorPickedBackImage.color); ///First

            //gradCam.gameObject.SetActive(true); /// Second

            //StartCoroutine(UIManager.Instance.ManualScreenShot()); /// Second

            //gradCam.gameObject.SetActive(false); /// Second

            //StartCoroutine(LoadImage(meshRenderer)); /// Second

            meshRenderer.materials[0].SetTexture("_BaseMap", curTex);

            RefreshMap(paintableObject);
        }
        else
        {
            timerForApplyTex = 0;

        }

        changingTexNow = false;

        yield return null;

    }

    //IEnumerator LoadImage(Renderer meshRenderer) /// second
    //{
    //    yield return new WaitUntil(() => UIManager.Instance.savedImage);

    //    UIManager.Instance.savedImage = false;

    //    DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Grad");

    //    FileInfo[] Images = dir.GetFiles("*.png");

    //    foreach (FileInfo f in Images)
    //    {
    //        if (f.Name.Contains("meta"))
    //        {
    //            yield break;
    //        }
    //        else
    //        {
    //            string wwwImageFilePath = "file://" + f.FullName.ToString();


    //            UnityWebRequest www = UnityWebRequestTexture.GetTexture(wwwImageFilePath);
    //            yield return www.SendWebRequest();

    //            if (www.isNetworkError || www.isHttpError)
    //            {
    //                Debug.Log(www.error);
    //            }
    //            else
    //            {
    //                curTex = DownloadHandlerTexture.GetContent(www);

    //                meshRenderer.materials[0].SetTexture("_BaseMap", curTex); /// Second
    //            }
    //        }

    //    }

    //    gradCam.gameObject.SetActive(false); /// Second
    //}
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

    private void DeactivateObjectsOnScreenTouch()
    {
        foreach (GameObject go in UIManager.Instance.disableOnTOuch)
        {
            go.SetActive(false);
        }

        UIManager.Instance.disableOnTOuch.Clear();
    }
}

