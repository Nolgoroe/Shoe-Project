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

    private float /*rotationY,*/ rotationX;
    private Camera Camera;

    public Transform toRotate;
    public Transform toZoom;
    public float rotationSpeedModifier = 0.5f;
    public float zoomSpeed = 3;

    public static bool isInGame = false;

    public RawImage texture;

    RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];
    public bool clickingTex = false;
    public float timerForGetTex = 0;
    bool stopCheck = false;
    [HideInInspector]
    public bool chosenTex = false;
    Vector3 screenPos;
    public LayerMask hitLayer3D;

    public ScrollRect textureScrollRect;
    Transform previouslyDetectedPiece = null;
    float timerForApplyTex = 0;
    bool changingTexNow = false;
    private P3dInputManager inputManager = new P3dInputManager();

    void Start()
    {
        changingTexNow = false;
        Instance = this;
        Camera = Camera.main;
        isInGame = false;
        clickingTex = false;
        chosenTex = false;
        texture.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isInGame)
        {
            if (Input.touchCount == 1)
            {
                PainterManager.Instacne.hitScreenData.enabled = false;

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    HitsBuffer[0] = new RaycastHit2D();
                    timerForGetTex = 0;
                    screenPos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));

                    int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, Mathf.Infinity);

                    if(HitsBuffer[0].transform)
                    {
                        if (HitsBuffer[0].transform.CompareTag("TexturePrefab"))
                        {
                            clickingTex = true;
                        }
                    }
                }

                if(touch.phase == TouchPhase.Stationary)
                {
                    PainterManager.Instacne.hitScreenData.enabled = false;

                    if (clickingTex && !stopCheck)
                    {
                        stopCheck = false;
                        timerForGetTex += Time.deltaTime;

                        if (timerForGetTex > 0.3f)
                        {
                            textureScrollRect.enabled = false;
                            chosenTex = true;
                            timerForApplyTex = 0;
                            stopCheck = true;
                            TextureHolderScript THS = HitsBuffer[0].transform.GetComponent<TextureHolderScript>();

                            texture.gameObject.SetActive(true);
                            texture.texture = THS.heldTexture;

                            Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y));
                            mousePos.z = texture.transform.position.z;
                            //Debug.DrawRay(Camera.transform.position, mousePos, Color.red);

                            texture.transform.position = mousePos;
                            //Debug.Log("IN TEX");
                        }
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    PainterManager.Instacne.hitScreenData.enabled = true;

                    clickingTex = false;
                    timerForGetTex = 0;
                    timerForApplyTex = 0;
                    //if (chosenTex)
                    //{
                    //    Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                    //    mousePos.z = texture.transform.position.z;
                    //    texture.transform.position = mousePos;

                    //    RaycastHit hit;
                    //    Ray ray = Camera.ScreenPointToRay(touch.position);

                    //    //Debug.DrawRay(Camera.transform.position, mousePos, Color.red);
                    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    //    {
                    //        if (hit.transform.CompareTag("ShoePiece"))
                    //        {
                    //            timerForApplyTex += Time.deltaTime;

                    //            if (timerForApplyTex >= 0.3f)
                    //            {
                    //                Transform newDetected = hit.transform;

                    //                P3dPaintable paintableObject = hit.transform.GetComponent<P3dPaintable>();

                    //                if (newDetected != previouslyDetectedPiece)
                    //                {
                    //                    timerForApplyTex = 0;

                    //                    if (previouslyDetectedPiece)
                    //                    {
                    //                        Renderer previouslyDetectedPieceRenderer = previouslyDetectedPiece.transform.GetComponent<Renderer>();
                    //                        previouslyDetectedPieceRenderer.materials[0].SetTexture("_BaseMap", null);
                    //                        Debug.Log("Previous: " + previouslyDetectedPieceRenderer.transform.name);
                    //                    }

                    //                    Renderer newDetectedPieceRenderer = newDetected.transform.GetComponent<Renderer>();
                    //                    newDetectedPieceRenderer.materials[0].SetTexture("_BaseMap", texture.texture);
                    //                    Debug.Log("New: " + newDetectedPieceRenderer.transform.name);

                    //                    previouslyDetectedPiece = newDetected;
                    //                    RefreshMap(paintableObject);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        timerForApplyTex = 0;

                    //        if (previouslyDetectedPiece)
                    //        {
                    //            Renderer previouslyDetectedPieceRenderer = previouslyDetectedPiece.transform.GetComponent<Renderer>();
                    //            previouslyDetectedPieceRenderer.material.SetTexture("_BaseMap", null);
                    //            previouslyDetectedPiece = null;
                    //        }
                    //    }
                    //}
                }

                if(touch.phase == TouchPhase.Ended)
                {
                    //PainterManager.Instacne.hitScreenData.BreakHits(PainterManager.Instacne.hitScreenData);
                    //PainterManager.Instacne.hitScreenData.ResetConnections();
                    //PainterManager.Instacne.hitScreenData.ClearHitCache();
                    //inputManager.Fingers.Clear();

                    PainterManager.Instacne.hitScreenData.enabled = true;

                    textureScrollRect.enabled = true;
                    timerForApplyTex = 0;
                    timerForGetTex = 0;
                    previouslyDetectedPiece = null;
                    stopCheck = false;
                    clickingTex = false;
                    chosenTex = false;
                    texture.gameObject.SetActive(false);

                }

                StartCoroutine(ChangeTexOnPiece(touch));
            }

            if (Input.touchCount >= 2)
            {
                PainterManager.Instacne.hitScreenData.enabled = false;
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);

                if (touchOne.phase == TouchPhase.Moved)
                {
                    float deltaX = touchOne.deltaPosition.x;
                    float deltaY = touchOne.deltaPosition.y;
                    rotationX -= deltaX * Time.deltaTime * rotationSpeedModifier;
                    //rotationY += deltaY * Time.deltaTime * rotationSpeedModifier;
                    toRotate.transform.eulerAngles = new Vector3(0 , -rotationX/*, rotationY*/);
                }

                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

                float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
                float currentMagnitude = (touchOne.position - touchTwo.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.01f);
            }

            //if (Input.GetMouseButton(1))
            //{
            //    Debug.Log("rotate");
            //    float speed = 130; //how fast the object should rotate

            //    toRotate.transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
            //}
        }
    }

    private void zoom(float v)
    {

        Vector3 newxtpos = toZoom.position += toZoom.transform.forward * v * zoomSpeed;

        newxtpos.z = Mathf.Clamp(newxtpos.z, -40, -20);
        toZoom.position = newxtpos;
    }

    public void RefreshMap(P3dPaintable paintableObject)
    {
        paintableObject.Invoke("Activate", 0.1f);
        paintableObject.Activate();
    }
    public IEnumerator ChangeTexOnPiece(Touch touch)
    {
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
                if (hit.transform.CompareTag("ShoePiece"))
                {
                    texture.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 1);
                    texture.transform.localScale = new Vector3(0.2f, 0.2f, 1);

                    timerForApplyTex += Time.deltaTime;

                    P3dPaintable paintableObject = hit.transform.GetComponent<P3dPaintable>();

                    if (paintableObject)
                    {
                        if (timerForApplyTex >= 0.05f)
                        {
                            Transform newDetected = hit.transform;


                            if (newDetected != previouslyDetectedPiece)
                            {
                                timerForApplyTex = 0;

                                if (previouslyDetectedPiece)
                                {
                                    Renderer previouslyDetectedPieceRenderer = previouslyDetectedPiece.transform.GetComponent<Renderer>();
                                    previouslyDetectedPieceRenderer.materials[0].SetTexture("_BaseMap", null);
                                    Debug.Log("Previous: " + previouslyDetectedPieceRenderer.transform.name);
                                    yield return null;
                                    yield return null;
                                }

                                Renderer newDetectedPieceRenderer = newDetected.transform.GetComponent<Renderer>();
                                newDetectedPieceRenderer.materials[0].SetTexture("_BaseMap", texture.texture);
                                Debug.Log("New: " + newDetectedPieceRenderer.transform.name);
                                previouslyDetectedPiece = newDetected;

                                RefreshMap(paintableObject);

                            }
                        }
                    }
                }
            }
            else
            {
                timerForApplyTex = 0;

                if (previouslyDetectedPiece)
                {
                    Renderer previouslyDetectedPieceRenderer = previouslyDetectedPiece.transform.GetComponent<Renderer>();
                    previouslyDetectedPieceRenderer.material.SetTexture("_BaseMap", null);
                    previouslyDetectedPiece = null;
                }
            }

            changingTexNow = false;

            yield return null;
        }
    }
}
