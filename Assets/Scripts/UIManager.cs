using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button letsStartButton, startDesignButton;
    public Button donePaintingButton;

    public static UIManager Instance;

    public GameObject firstScreenUI, lastScreenUI, lastScreenAssetsNoShoe ,sidePanel;
    public GameObject mainCanvas, cameraCanvas;
    public RawImage videoBG;

    public Transform shoeGO;

    public RawImage infoTextImage;
    public Image coloredBar;

    public VideoPlayer playerOfVideos;

    public int clickedIndexByInfo;

    public Image[] infoButtons, videoButtons;

    public Sprite[] whiteLetters, blackLetters;


    [HideInInspector]
    public bool isLastScreen;

    public string screenshotSaveFolderPath;
    public string manualSaveFolder;

    public List<UIElementLanguageData> UiLanguages;

    public List<GameObject> disableOnTOuch;

    [HideInInspector]
    public bool savedImage;
    private void Start()
    {
        Instance = this;

        isLastScreen = false;
        savedImage = false;
        startDesignButton.interactable = false;
        startDesignButton.GetComponent<Image>().raycastTarget = false;

        letsStartButton.interactable = true;
        letsStartButton.GetComponent<Image>().raycastTarget = true;

        lastScreenUI.SetActive(false);
        firstScreenUI.SetActive(true);
        sidePanel.SetActive(true);
        videoBG.raycastTarget = false;
        disableOnTOuch = new List<GameObject>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        Debug.Log("Pic");
    //        StartCoroutine(ManualScreenShot());
    //    }
    //}
    public void AfterAnimation()
    {
        startDesignButton.interactable = true;
        startDesignButton.GetComponent<Image>().raycastTarget = true;

        letsStartButton.interactable = false;
        letsStartButton.GetComponent<Image>().raycastTarget = false;
    }

    public void AfterRewind()
    {
        startDesignButton.interactable = false;
        startDesignButton.GetComponent<Image>().raycastTarget = false;

        letsStartButton.interactable = true;
        letsStartButton.GetComponent<Image>().raycastTarget = true;
    }

    public void GoToGame()
    {
        TouchManager.isInGame = true;
        Timer.Instance.timerIsRunning = true;

        ColorPickerSimple.Instacne.colorPickedFrontImage.color = PainterManager.Instacne.painter.Color;
        ColorPickerSimple.Instacne.gradMaterial.SetColor("_Color", ColorPickerSimple.Instacne.colorPickedFrontImage.color);
        ColorPickerSimple.Instacne.gradMaterial.SetColor("_Color2", Color.black);


        Renderer r = TouchManager.Instance.QuatMat.GetComponent<Renderer>(); /// Second

        r.material.SetColor("_FirstColor", ColorPickerSimple.Instacne.colorPickedFrontImage.color); /// Second
        r.material.SetColor("_SecondColor", ColorPickerSimple.Instacne.colorPickedBackImage.color); /// Second


        StartCoroutine(AnimationManager.Instance.AnimateSecondToThird());
        //PainterManager.Instacne.hitScreenData.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeLocalizationImages(int index)
    {
        infoButtons[index].color = ReadFolderData.Instance.translatedColorCode;
        SetUILanguage(index);
        //if (AnimationManager.Instance.isVideoOpen)
        //{
        //    videoButtons[index].color = ReadFolderData.Instance.translatedColorCode;
        //}
        //else
        //{
        //    videoButtons[index].color = new Color(ReadFolderData.Instance.translatedColorCode.r, ReadFolderData.Instance.translatedColorCode.g, ReadFolderData.Instance.translatedColorCode.b,0);
        //}

        ChangeLanguageColorsInfo(index);

        clickedIndexByInfo = index;

        if (ReadFolderData.Instance.languageTextures[clickedIndexByInfo] != null)
        {
            infoTextImage.texture = ReadFolderData.Instance.languageTextures[index];
        }
        else
        {
            infoTextImage.texture = ReadFolderData.Instance.languageTextures[0];

            Debug.Log("Backup Image");
        }
    }

    //public IEnumerator ManualScreenShot()
    //{

    //    shoeGO.gameObject.SetActive(false);
    //    mainCanvas.SetActive(false);
    //    cameraCanvas.SetActive(false);

    //    string path = Application.streamingAssetsPath + "/Grad";
    //    DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Grad");

    //    if (!dir.Exists)
    //    {
    //        Directory.CreateDirectory(path);
    //    }

    //    ScreenCapture.CaptureScreenshot(path + "/Screenshot.png");

    //    yield return new WaitForEndOfFrame();

    //    shoeGO.gameObject.SetActive(true);
    //    mainCanvas.SetActive(true);
    //    cameraCanvas.SetActive(true);

    //    savedImage = true;
    //}
    public IEnumerator TakeScreenShot()
    {
        //yield return new WaitForSeconds(1.1f);
        //shoeGO.transform.rotation = Quaternion.Euler(4.5f,145,-13.5f);
        //shoeGO.transform.position = Vector3.zero;

        //shotPicScreenShot.transform.localPosition = new Vector3(40, 50, 0);
        //shotPicScreenShot.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);



        //for (int i = 0; i < 100000; i++)
        //{
        //    if (!System.IO.File.Exists(Application.streamingAssetsPath + "/Screenshot" + i + ".png"))
        //    {
        //        ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Screenshot" + i + ".png");
        //        break;
        //    }
        //}

        DateTime theTime = DateTime.Now;
        string date = theTime.ToString("yyyy-MM-dd");
        string time = theTime.ToString("HH-mm-ss");
        string datetime = theTime.ToString("yyyy-MM-dd , HH-mm-ss");

        if (!Directory.Exists(screenshotSaveFolderPath))
        {
            Directory.CreateDirectory(screenshotSaveFolderPath);
        }

        if (!File.Exists(screenshotSaveFolderPath + "/Screenshot" + " " + datetime + ".png"))
        {
            ScreenCapture.CaptureScreenshot(screenshotSaveFolderPath + "/Screenshot"+ " " + datetime + ".png");
        }

        yield return null;
        StartCoroutine(AnimationManager.Instance.AnimatelastScren());
        //shotPicScreenShot.transform.localPosition = new Vector3(40, 180, 0);
        //shotPicScreenShot.transform.localScale = Vector3.one;

        //lastScreenAssetsNoShoe.SetActive(true);
        //sidePanel.SetActive(true);
    }


    public void ChangeLanguageColorsVideo(int index)
    {
        foreach (Image i in videoButtons)
        {
            LanguageButtonInfo childLetterImageData = i.transform.GetComponent<LanguageButtonInfo>();
            Image childLetterImage = i.transform.GetChild(0).GetComponent<Image>();

            if (i != videoButtons[index])
            {
                if (AnimationManager.Instance.isVideoOpen)
                {
                    i.color = Color.white;
                    childLetterImage.sprite = childLetterImageData.blackImage;
                }
                else
                {
                    i.color = new Color(255, 255, 255, 0);
                    childLetterImage.sprite = childLetterImageData.blackImage;
                }
            }
            else
            {
                childLetterImage.sprite = childLetterImageData.whiteImage;
            }
        }
    }
    public void ChangeLanguageColorsInfo(int index)
    {
        foreach (Image i in infoButtons)
        {
            LanguageButtonInfo childLetterImageData = i.transform.GetComponent<LanguageButtonInfo>();
            Image childLetterImage = i.transform.GetChild(0).GetComponent<Image>();

            if (i != infoButtons[index])
            {
                if (AnimationManager.Instance.isInfoOpen)
                {
                    i.color = Color.white;
                    childLetterImage.sprite = childLetterImageData.blackImage;
                }
                else
                {
                    i.color = new Color(255, 255, 255, 0);
                    childLetterImage.sprite = childLetterImageData.blackImage;
                }
            }
            else
            {
                childLetterImage.sprite = childLetterImageData.whiteImage;
            }
        }
    }


    public void SetUILanguage(int index)
    {
        foreach (UIElementLanguageData UIL in UiLanguages)
        {
            UIL.ChangeLanguageSprite(index);
            //UIL.transform.localPosition = Vector3.zero;
        }

        foreach (AnimatedObject AM in AnimationManager.Instance.objectsToAnimate)
        {
            if (AM.canChangeLanguage)
            {
                AM.targetSprite = AM.theObject.GetComponent<UIElementLanguageData>().languagesInOrderEHAC[index];
            }
        }
    }
    
    public void CloseScreen(GameObject toClose)
    {
        toClose.SetActive(false);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
