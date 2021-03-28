using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button letsStartButton, startDesignButton;

    public static UIManager Instance;

    public GameObject firstScreenUI, lastScreenUI, sidePanel;

    public Transform shoeGO;

    public RawImage infoTextImage;
    public Image coloredBar;

    public VideoPlayer playerOfVideos;

    public int clickedIndexByInfo;

    public Image[] infoButtons, videoButtons;

    public Sprite[] whiteLetters, blackLetters;

    [HideInInspector]
    public bool isLastScreen;

    private void Start()
    {
        Instance = this;
        isLastScreen = false;

        startDesignButton.interactable = false;
        startDesignButton.GetComponent<Image>().raycastTarget = false;

        letsStartButton.interactable = true;
        letsStartButton.GetComponent<Image>().raycastTarget = true;

        lastScreenUI.SetActive(false);
        firstScreenUI.SetActive(true);
        sidePanel.SetActive(true);
    }


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
        firstScreenUI.SetActive(false);
        TouchManager.isInGame = true;
        Timer.Instance.timerIsRunning = true;

        ColorPickerSimple.Instacne.colorPickedFrontImage.color = PainterManager.Instacne.painter.Color;

        //PainterManager.Instacne.hitScreenData.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeLocalizationImages(int index)
    {
        infoButtons[index].color = ReadFolderData.Instance.translatedColorCode;

        if (AnimationManager.Instance.isVideoOpen)
        {
            videoButtons[index].color = ReadFolderData.Instance.translatedColorCode;
        }
        else
        {
            videoButtons[index].color = new Color(ReadFolderData.Instance.translatedColorCode.r, ReadFolderData.Instance.translatedColorCode.g, ReadFolderData.Instance.translatedColorCode.b,0);
        }

        ChangeLanguageColors(index);

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
    public IEnumerator GoLastScreen()
    {
        yield return new WaitForSeconds(1.1f);
        shoeGO.transform.rotation = Quaternion.Euler(4.5f,145,-13.5f);
        shoeGO.transform.position = Vector3.zero;
        TouchManager.isInGame = false;

        firstScreenUI.SetActive(false);
        lastScreenUI.SetActive(true);

        yield return new WaitForSeconds(2);
        isLastScreen = true;
        //for (int i = 0; i < 100000; i++)
        //{
        //    if (!System.IO.File.Exists(Application.streamingAssetsPath + "/Screenshot" + i + ".png"))
        //    {
        //        ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Screenshot" + i + ".png");
        //        break;
        //    }
        //}

        string date = System.DateTime.Now.ToString();
        date = date.Replace("/", "-");
        date = date.Replace(" ", "_");
        date = date.Replace(":", "-");

        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/Screenshot" + " " + date + ".png"))
        {
            ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Screenshot"+ " " + date + ".png");
        }
    }


    public void ChangeLanguageColors(int index)
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
                    i.color = new Color (255,255,255,0);
                    childLetterImage.sprite = childLetterImageData.blackImage;
                }
            }
            else
            {
                childLetterImage.sprite = childLetterImageData.whiteImage;
            }
        }

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
}
