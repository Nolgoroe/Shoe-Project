using UnityEngine;
using UnityEngine.UI;

public class ColorPickerSimple : MonoBehaviour
{
    public static ColorPickerSimple Instacne;
    Color[] Data;
    public bool Selected;

    SpriteRenderer SpriteRenderer;
    GameObject ColorPicker;
    GameObject Selector;
    CircleCollider2D Collider;

    public int Width { get { return SpriteRenderer.sprite.texture.width; } }
    public int Height { get { return SpriteRenderer.sprite.texture.height; } }

    public Color Color;
    Camera Camera;

    RaycastHit2D[] HitsBuffer = new RaycastHit2D[1];


    public Image colorPickedFrontImage, colorPickedBackImage;

    public Scrollbar HSVbar;

    float valueHSV = 0;
    void Awake() {
        Instacne = this;
        Camera = Camera.main;

        ColorPicker = transform.Find("ColorPicker").gameObject;
        SpriteRenderer = ColorPicker.GetComponent<SpriteRenderer>();
        Selector = transform.Find("Selector").gameObject;
        Collider = ColorPicker.GetComponent<CircleCollider2D>();

        Data = SpriteRenderer.sprite.texture.GetPixels();

        Color = Color.white;
    }


    void Update() {

        if (TouchManager.isInGame && !TouchManager.Instance.chosenTex && !PainterManager.Instacne.hasErased)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                //if (touch.phase == TouchPhase.Began)
                //{
                //    Vector3 screenPos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100f));

                //    int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, Mathf.Infinity);

                //    for (int i = 0; i < hitCount; i++)
                //    {
                //        if (HitsBuffer[i].collider == Collider)
                //        {
                //            Selected = true;
                //            Debug.Log("IN");
                //            PainterManager.Instacne.hitScreenData.enabled = false;

                //            screenPos.z = 75;

                //            screenPos.x = Mathf.Clamp(screenPos.x, transform.position.x, transform.position.x + transform.lossyScale.x);
                //            screenPos.y = Mathf.Clamp(screenPos.y, transform.position.y - transform.lossyScale.y, transform.position.y);


                //            //get color data
                //            screenPos.x = screenPos.x - ColorPicker.transform.position.x;
                //            screenPos.y = ColorPicker.transform.position.y - screenPos.y;

                //            int x = (int)(screenPos.x * Width / transform.lossyScale.x);
                //            int y = Height - (int)(screenPos.y * Height / transform.lossyScale.y);

                //            if (x == Width)
                //                x -= 1;
                //            if (y == Height)
                //                y -= 1;

                //            if (x >= 0 && x < Width && y >= 0 && y < Height)
                //            {
                //                Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                //                mousePos.z = Selector.transform.position.z;
                //                Selector.transform.position = mousePos;

                //                Color = Data[y * Width + x];

                //                Color = new Color(Color.r, Color.g, Color.b, 1);

                //                PainterManager.Instacne.painter.Color = Color;
                //                Debug.Log(Color);
                //            }
                //        }
                //    }
                //}

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 screenPos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100f));

                    int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, 0);

                    for (int i = 0; i < hitCount; i++)
                    {
                        if (HitsBuffer[i].collider == Collider)
                        {
                            Selected = true;
                            Debug.Log("IN");
                            //PainterManager.Instacne.hitScreenData.enabled = false;

                            screenPos.z = 75;

                            screenPos.x = Mathf.Clamp(screenPos.x, transform.position.x, transform.position.x + transform.lossyScale.x);
                            screenPos.y = Mathf.Clamp(screenPos.y, transform.position.y - transform.lossyScale.y, transform.position.y);


                            //get color data
                            screenPos.x = screenPos.x - ColorPicker.transform.position.x;
                            screenPos.y = ColorPicker.transform.position.y - screenPos.y;

                            int x = (int)(screenPos.x * Width / transform.lossyScale.x);
                            int y = Height - (int)(screenPos.y * Height / transform.lossyScale.y);

                            if (x == Width)
                                x -= 1;
                            if (y == Height)
                                y -= 1;

                            if (x >= 0 && x < Width && y >= 0 && y < Height)
                            {
                                Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 100));
                                mousePos.z = Selector.transform.position.z;
                                Selector.transform.position = mousePos;

                                Color = Data[y * Width + x];

                                Color = new Color(Color.r, Color.g, Color.b, 1);

                                colorPickedFrontImage.color = Color;

                                ChangeHSVWheel();

                            }
                        }
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Selected = false;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 screenPos = Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));

                    int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, Mathf.Infinity);

                    for (int i = 0; i < hitCount; i++)
                    {
                        if (HitsBuffer[i].collider == Collider)
                        {
                            Selected = true;
                            Debug.Log("IN");
                            //PainterManager.Instacne.hitScreenData.enabled = false;
                        }
                    }

                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Selected = false;
                    //PainterManager.Instacne.hitScreenData.enabled = true;
                }

                if (Selected && Input.GetMouseButton(0))
                {
                    Vector3 screenPos = Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));
                    screenPos.z = 75;

                    int hitCount = Physics2D.RaycastNonAlloc(screenPos, Vector2.zero, HitsBuffer, Mathf.Infinity);

                    for (int i = 0; i < hitCount; i++)
                    {
                        if (HitsBuffer[i].collider == Collider)
                        {
                            screenPos.x = Mathf.Clamp(screenPos.x, transform.position.x, transform.position.x + ColorPicker.transform.lossyScale.x);
                            screenPos.y = Mathf.Clamp(screenPos.y, transform.position.y - ColorPicker.transform.lossyScale.y, transform.position.y);

                            Vector3 CPCenter = new Vector3(ColorPicker.transform.position.x + (ColorPicker.transform.lossyScale.x / 2),
                                 ColorPicker.transform.position.y - (ColorPicker.transform.lossyScale.y / 2), 0);


                            //get color data
                            screenPos.x = screenPos.x - ColorPicker.transform.position.x;
                            screenPos.y = ColorPicker.transform.position.y - screenPos.y;

                            int x = (int)(screenPos.x * Width / transform.lossyScale.x);
                            int y = Height - (int)(screenPos.y * Height / transform.lossyScale.y);

                            if (x == Width)
                                x -= 1;
                            if (y == Height)
                                y -= 1;

                            if (x >= 0 && x < Width && y >= 0 && y < Height)
                            {
                                Vector3 mousePos = Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
                                mousePos.z = Selector.transform.position.z;
                                Selector.transform.position = mousePos;
                                Color = Data[y * Width + x];

                                Color = new Color(Color.r, Color.g, Color.b, 1);

                                colorPickedFrontImage.color = Color;

                                ChangeHSVWheel();
                                Debug.Log(Color);
                            }
                        }
                    }
                }
            }
        }
    }


    public void SwitchColorPics()
    {
        if (!TouchManager.Instance.chosenTex)
        {
            Color temp;
            temp = colorPickedFrontImage.color;
            colorPickedFrontImage.color = colorPickedBackImage.color;
            colorPickedBackImage.color = temp;


            PainterManager.Instacne.painter.Color = colorPickedFrontImage.color;
        }
    }

    public void ChangeHSV()
    {
        if (!TouchManager.Instance.chosenTex)
        {
            float h, s;

            Color.RGBToHSV(colorPickedFrontImage.color, out h, out s, out valueHSV);

            valueHSV = HSVbar.value;

            if (valueHSV <= 0.01f)
            {
                valueHSV = 0.01f;
            }
            colorPickedFrontImage.color = Color.HSVToRGB(h, s, valueHSV);

            PainterManager.Instacne.painter.Color = colorPickedFrontImage.color;
        }

    }
    public void ChangeHSVWheel()
    {
        if (!TouchManager.Instance.chosenTex)
        {
            float h, s;

            Color.RGBToHSV(colorPickedFrontImage.color, out h, out s, out valueHSV);

            valueHSV = HSVbar.value;

            if (valueHSV <= 0.01f)
            {
                valueHSV = 0.01f;
            }
            colorPickedFrontImage.color = Color.HSVToRGB(h, s, valueHSV);

            PainterManager.Instacne.painter.Color = colorPickedFrontImage.color;
        }
    }
} 

