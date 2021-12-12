using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class AnimatedObject
{
    public GameObject theObject;
    public Vector3 originalPos;
    public Vector3 originalScale;
    public Vector3 newPos;
    public Vector3 newScale;
    public Vector3 newRotation;
    public float timeToAnimate = 1;
    public float delatToActivate = 0;
    public Image imageToChange;
    public Sprite originalSprite;
    public Sprite targetSprite;
    public bool canChangeLanguage;
    public bool isDisableOnTouch;
}

[Serializable]
public class FadeAnimatedObjects
{
    public Image theObject;
    public RawImage theObjectRAW;
    public float origingalAlpha = 0;
    public float targetAlpha = 1;
    public float timeToAnimate = 1;
    public bool isStartNoAlpha;
}

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance;
    public Transform rootObjectOfSideScreen;

    public AnimatedObject[] objectsToAnimate;
    public FadeAnimatedObjects[] objectsToFade;

    public AnimatedObject sidePanel;
    public FadeAnimatedObjects[] fadeObjectsInfoPressed;
    public FadeAnimatedObjects[] fadeObjectsInfoClosed;
    public FadeAnimatedObjects[] fadeObjectsMovieClosed;
    public FadeAnimatedObjects[] fadeObjectsGoToGame;

    public AnimatedObject[] objectsToAnimate2nd3rd;
    public AnimatedObject[] objectsToAnimate3rd;
    public AnimatedObject[] objectsToAnimate3rdToLast;
    public AnimatedObject[] objectToAnimateLast;
    public AnimatedObject shoeToAnimateLast;

    public Button[] languageButtons;

    public Vector3 postitionForSharing;

    [HideInInspector]
    public bool isVideoOpen, isInfoOpen;
    [HideInInspector]
    //public bool isOutOfGame;

    public static bool isFinishedAnimation = false;
    void Awake()
    {
        Instance = this;

        videoAndInfoClosed();

        for (int i = 0; i < objectsToAnimate.Length; i++)
        {
            objectsToAnimate[i].originalPos = objectsToAnimate[i].theObject.transform.localPosition;
            objectsToAnimate[i].originalScale = objectsToAnimate[i].theObject.transform.localScale;

            if (objectsToAnimate[i].targetSprite)
            {
                objectsToAnimate[i].imageToChange = objectsToAnimate[i].theObject.GetComponent<Image>();
                objectsToAnimate[i].originalSprite = objectsToAnimate[i].theObject.GetComponent<Image>().sprite;
            }
        }

        for (int i = 0; i < objectsToFade.Length; i++)
        {
            if (objectsToFade[i].isStartNoAlpha)
            {
                objectsToFade[i].theObject.color = new Color(objectsToFade[i].theObject.color.r, objectsToFade[i].theObject.color.g, objectsToFade[i].theObject.color.b, 0);
            }
            else
            {
                objectsToFade[i].theObject.color = new Color(objectsToFade[i].theObject.color.r, objectsToFade[i].theObject.color.g, objectsToFade[i].theObject.color.b, 1);
            }

            objectsToFade[i].origingalAlpha = objectsToFade[i].theObject.color.a;
        }

        sidePanel.originalPos = sidePanel.theObject.transform.localPosition;

        for (int i = 0; i < fadeObjectsInfoPressed.Length; i++)
        {
            if (fadeObjectsInfoPressed[i].isStartNoAlpha)
            {
                if (fadeObjectsInfoPressed[i].theObject)
                {
                    fadeObjectsInfoPressed[i].theObject.color = new Color(fadeObjectsInfoPressed[i].theObject.color.r, fadeObjectsInfoPressed[i].theObject.color.g, fadeObjectsInfoPressed[i].theObject.color.b, 0);
                }
                else
                {
                    fadeObjectsInfoPressed[i].theObjectRAW.color = new Color(fadeObjectsInfoPressed[i].theObjectRAW.color.r, fadeObjectsInfoPressed[i].theObjectRAW.color.g, fadeObjectsInfoPressed[i].theObjectRAW.color.b, 0);
                }
            }
            else
            {
                if (fadeObjectsInfoPressed[i].theObject)
                {
                    fadeObjectsInfoPressed[i].theObject.color = new Color(fadeObjectsInfoPressed[i].theObject.color.r, fadeObjectsInfoPressed[i].theObject.color.g, fadeObjectsInfoPressed[i].theObject.color.b, 1);
                    fadeObjectsInfoPressed[i].origingalAlpha = fadeObjectsInfoPressed[i].theObject.color.a;
                }
                else
                {
                    fadeObjectsInfoPressed[i].theObjectRAW.color = new Color(fadeObjectsInfoPressed[i].theObjectRAW.color.r, fadeObjectsInfoPressed[i].theObjectRAW.color.g, fadeObjectsInfoPressed[i].theObjectRAW.color.b, 1);
                    fadeObjectsInfoPressed[i].origingalAlpha = fadeObjectsInfoPressed[i].theObjectRAW.color.a;
                }
            }

        }

        for (int i = 0; i < fadeObjectsInfoClosed.Length; i++)
        {
            if (fadeObjectsInfoClosed[i].isStartNoAlpha)
            {
                if (fadeObjectsInfoClosed[i].theObject)
                {
                    fadeObjectsInfoClosed[i].theObject.color = new Color(fadeObjectsInfoClosed[i].theObject.color.r, fadeObjectsInfoClosed[i].theObject.color.g, fadeObjectsInfoClosed[i].theObject.color.b, 0);
                }
                else
                {
                    fadeObjectsInfoClosed[i].theObjectRAW.color = new Color(fadeObjectsInfoClosed[i].theObjectRAW.color.r, fadeObjectsInfoClosed[i].theObjectRAW.color.g, fadeObjectsInfoClosed[i].theObjectRAW.color.b, 0);
                }
            }
            else
            {
                if (fadeObjectsInfoClosed[i].theObject)
                {
                    fadeObjectsInfoClosed[i].theObject.color = new Color(fadeObjectsInfoClosed[i].theObject.color.r, fadeObjectsInfoClosed[i].theObject.color.g, fadeObjectsInfoClosed[i].theObject.color.b, 1);
                    fadeObjectsInfoClosed[i].origingalAlpha = fadeObjectsInfoClosed[i].theObject.color.a;
                }
                else
                {
                    fadeObjectsInfoClosed[i].theObjectRAW.color = new Color(fadeObjectsInfoClosed[i].theObjectRAW.color.r, fadeObjectsInfoClosed[i].theObjectRAW.color.g, fadeObjectsInfoClosed[i].theObjectRAW.color.b, 1);
                    fadeObjectsInfoClosed[i].origingalAlpha = fadeObjectsInfoClosed[i].theObjectRAW.color.a;
                }
            }

        }

        for (int i = 0; i < fadeObjectsMovieClosed.Length; i++)
        {
            if (fadeObjectsMovieClosed[i].isStartNoAlpha)
            {
                if (fadeObjectsMovieClosed[i].theObject)
                {
                    fadeObjectsMovieClosed[i].theObject.color = new Color(fadeObjectsMovieClosed[i].theObject.color.r, fadeObjectsMovieClosed[i].theObject.color.g, fadeObjectsMovieClosed[i].theObject.color.b, 0);
                }
                else
                {
                    fadeObjectsMovieClosed[i].theObjectRAW.color = new Color(fadeObjectsMovieClosed[i].theObjectRAW.color.r, fadeObjectsMovieClosed[i].theObjectRAW.color.g, fadeObjectsMovieClosed[i].theObjectRAW.color.b, 0);
                }
            }
            else
            {
                if (fadeObjectsMovieClosed[i].theObject)
                {
                    fadeObjectsMovieClosed[i].theObject.color = new Color(fadeObjectsMovieClosed[i].theObject.color.r, fadeObjectsMovieClosed[i].theObject.color.g, fadeObjectsMovieClosed[i].theObject.color.b, 1);
                    fadeObjectsMovieClosed[i].origingalAlpha = fadeObjectsMovieClosed[i].theObject.color.a;
                }
                else
                {
                    fadeObjectsMovieClosed[i].theObjectRAW.color = new Color(fadeObjectsMovieClosed[i].theObjectRAW.color.r, fadeObjectsMovieClosed[i].theObjectRAW.color.g, fadeObjectsMovieClosed[i].theObjectRAW.color.b, 1);
                    fadeObjectsMovieClosed[i].origingalAlpha = fadeObjectsMovieClosed[i].theObjectRAW.color.a;
                }
            }

        }

        DisableLanguageButtons();
    }

    public void AnimateNow()
    {
        for (int i = 0; i < objectsToAnimate.Length; i++)
        {
            objectsToAnimate[i].theObject.transform.DOLocalMove(objectsToAnimate[i].newPos, objectsToAnimate[i].timeToAnimate).SetEase(Ease.OutCirc);
            objectsToAnimate[i].theObject.transform.DOScale(objectsToAnimate[i].newScale, objectsToAnimate[i].timeToAnimate).SetEase(Ease.OutCirc);

            if (objectsToAnimate[i].targetSprite)
            {
                objectsToAnimate[i].imageToChange.DOCrossfadeImage(objectsToAnimate[i].targetSprite, objectsToAnimate[i].timeToAnimate).SetEase(Ease.OutCirc);
            }
        }

        for (int i = 0; i < objectsToFade.Length; i++)
        {
            objectsToFade[i].theObject.DOFade(objectsToFade[i].targetAlpha, objectsToFade[i].timeToAnimate);
        }

        for (int i = 0; i < objectsToAnimate.Length; i++)
        {
            if (objectsToAnimate[i].canChangeLanguage)
            {
                UIManager.Instance.UiLanguages.Add(objectsToAnimate[i].theObject.GetComponent<UIElementLanguageData>());
            }
        }
        UIManager.Instance.AfterAnimation();
    }

    public void Rewind()
    {
        for (int i = 0; i < objectsToAnimate.Length; i++)
        {
            objectsToAnimate[i].theObject.transform.DOLocalMove(objectsToAnimate[i].originalPos, objectsToAnimate[i].timeToAnimate);
            objectsToAnimate[i].theObject.transform.DOScale(objectsToAnimate[i].originalScale, objectsToAnimate[i].timeToAnimate);

            if (objectsToAnimate[i].targetSprite)
            {
                objectsToAnimate[i].imageToChange.DOCrossfadeImage(objectsToAnimate[i].originalSprite, objectsToAnimate[i].timeToAnimate);
            }
        }

        for (int i = 0; i < objectsToFade.Length; i++)
        {
            objectsToFade[i].theObject.DOFade(objectsToFade[i].origingalAlpha, objectsToFade[i].timeToAnimate);
        }

        UIManager.Instance.AfterRewind();
    }

    public void AnimateSidePanel()
    {
        UIManager.Instance.videoBG.raycastTarget = true;
        //isOutOfGame = true;
        TouchManager.isInGame = false;
        isInfoOpen = true;
        EnableLanguageButtons();
        Timer.Instance.timerToPaintIsRunning = false;

        for (int i = 0; i < fadeObjectsInfoPressed.Length; i++)
        {
            if (fadeObjectsInfoPressed[i].theObject)
            {
                fadeObjectsInfoPressed[i].theObject.DOFade(fadeObjectsInfoPressed[i].targetAlpha, fadeObjectsInfoPressed[i].timeToAnimate);
            }
            else
            {
                fadeObjectsInfoPressed[i].theObjectRAW.DOFade(fadeObjectsInfoPressed[i].targetAlpha, fadeObjectsInfoPressed[i].timeToAnimate);
            }
        }
    }

    public void CallOpenInfoScreen(bool Open)
    {
        DisableLanguageButtons();
        StartCoroutine(OpenInfoScreen(Open));
    }

    public IEnumerator OpenInfoScreen(bool Open)
    {
        //isOutOfGame = true;
        TouchManager.isInGame = false;
        if (Open)
        {
            videoAndInfoClosed();

            yield return new WaitForSeconds(0.2f);
            UIManager.Instance.playerOfVideos.Stop();

            MoveScreenState(true);
            CloseInfoBar();
            CloseMovieBar();
        }
        else
        {
            MoveScreenState(false);

            //if (ReadFolderData.Instance.languageVideoClipsURL[UIManager.Instance.clickedIndexByInfo] != null)
            //{
            //    UIManager.Instance.playerOfVideos.url = ReadFolderData.Instance.languageVideoClipsURL[UIManager.Instance.clickedIndexByInfo];
            //    UIManager.Instance.playerOfVideos.Play();
            //}
            //else
            //{
            //    UIManager.Instance.playerOfVideos.url = ReadFolderData.Instance.languageVideoClipsURL[0];
            //    UIManager.Instance.playerOfVideos.Play();

            //    Debug.Log("Backup Video");
            //}

            for (int i = 0; i < fadeObjectsInfoClosed.Length; i++)
            {
                if (fadeObjectsInfoClosed[i].theObject)
                {
                    fadeObjectsInfoClosed[i].theObject.DOFade(fadeObjectsInfoClosed[i].targetAlpha, fadeObjectsInfoClosed[i].timeToAnimate);
                }
                else
                {
                    fadeObjectsInfoClosed[i].theObjectRAW.DOFade(fadeObjectsInfoClosed[i].targetAlpha, fadeObjectsInfoClosed[i].timeToAnimate);
                }
            }

            if (isVideoOpen)
            {
                CloseMovieBar();
            }
            yield return null;
        }
    }

    private void CloseInfoBar()
    {
        for (int i = 0; i < fadeObjectsInfoClosed.Length; i++)
        {
            if (fadeObjectsInfoClosed[i].theObject)
            {
                fadeObjectsInfoClosed[i].theObject.DOFade(fadeObjectsInfoClosed[i].targetAlpha, fadeObjectsInfoClosed[i].timeToAnimate);
            }
            else
            {
                fadeObjectsInfoClosed[i].theObjectRAW.DOFade(fadeObjectsInfoClosed[i].targetAlpha, fadeObjectsInfoClosed[i].timeToAnimate);
            }
        }
    }

    private void CloseMovieBar()
    {
        for (int i = 0; i < fadeObjectsMovieClosed.Length; i++)
        {
            if (fadeObjectsMovieClosed[i].theObject)
            {
                fadeObjectsMovieClosed[i].theObject.DOFade(fadeObjectsMovieClosed[i].origingalAlpha, fadeObjectsMovieClosed[i].timeToAnimate);
            }
            else
            {
                fadeObjectsMovieClosed[i].theObjectRAW.DOFade(fadeObjectsMovieClosed[i].origingalAlpha, fadeObjectsMovieClosed[i].timeToAnimate);
            }
        }
    }

    private void MoveScreenState(bool IN)
    {
        if (IN)
        {
            sidePanel.theObject.transform.DOLocalMove(sidePanel.newPos, sidePanel.timeToAnimate).SetEase(Ease.OutCirc);
        }
        else
        {
            sidePanel.theObject.transform.DOLocalMove(sidePanel.originalPos, sidePanel.timeToAnimate).SetEase(Ease.OutCirc);
        }
    }

    public void ChangeVideoLanguage(int index)
    {
        //isOutOfGame = true;
        TouchManager.isInGame = false;
        UIManager.Instance.videoButtons[index].color = ReadFolderData.Instance.translatedColorCode;

        //if (isInfoOpen)
        //{
        //    UIManager.Instance.infoButtons[index].color = ReadFolderData.Instance.translatedColorCode;
        //}
        //else
        //{
        //    UIManager.Instance.infoButtons[index].color = new Color(ReadFolderData.Instance.translatedColorCode.r, ReadFolderData.Instance.translatedColorCode.g, ReadFolderData.Instance.translatedColorCode.b, 0);
        //}


        UIManager.Instance.ChangeLanguageColorsVideo(index);

        videoAndInfoClosed();

        UIManager.Instance.clickedIndexByInfo = index;

        DisableLanguageButtons();

        UIManager.Instance.playerOfVideos.Stop();

        MoveScreenState(false);

        //playerOfVideos.clip = videoLanguages[index];
        FadeOutVideoBar(true);
        CloseInfoBar();
    }

    public void FadeOutVideoBar(bool open)
    {
        if (!open)
        {
            UIManager.Instance.videoBG.raycastTarget = true;
            isVideoOpen = true;

            EnableLanguageButtons();
            for (int i = 0; i < fadeObjectsMovieClosed.Length; i++)
            {
                if (fadeObjectsMovieClosed[i].theObject)
                {
                    fadeObjectsMovieClosed[i].theObject.DOFade(fadeObjectsMovieClosed[i].targetAlpha, fadeObjectsMovieClosed[i].timeToAnimate);
                }
                else
                {
                    fadeObjectsMovieClosed[i].theObjectRAW.DOFade(fadeObjectsMovieClosed[i].targetAlpha, fadeObjectsMovieClosed[i].timeToAnimate);
                }
            }
        }
        else
        {
            Timer.Instance.timerToPaintIsRunning = false;

            for (int i = 0; i < fadeObjectsMovieClosed.Length; i++)
            {
                if (fadeObjectsMovieClosed[i].theObject)
                {
                    fadeObjectsMovieClosed[i].theObject.DOFade(fadeObjectsMovieClosed[i].origingalAlpha, fadeObjectsMovieClosed[i].timeToAnimate);
                }
                else
                {
                    fadeObjectsMovieClosed[i].theObjectRAW.DOFade(fadeObjectsMovieClosed[i].origingalAlpha, fadeObjectsMovieClosed[i].timeToAnimate);
                }
            }
            CloseInfoBar();
            StartCoroutine(StartVidAfterFadeOut(0f));
        }
    }

    IEnumerator StartVidAfterFadeOut(float time)
    {
        yield return new WaitForSeconds(time);
        if (ReadFolderData.Instance.languageVideoClipsURL[UIManager.Instance.clickedIndexByInfo] != null)
        {
            UIManager.Instance.playerOfVideos.url = ReadFolderData.Instance.languageVideoClipsURL[UIManager.Instance.clickedIndexByInfo];
            UIManager.Instance.playerOfVideos.Play();
        }
        else
        {
            UIManager.Instance.playerOfVideos.url = ReadFolderData.Instance.languageVideoClipsURL[0];
            UIManager.Instance.playerOfVideos.Play();

            Debug.Log("No Movie Found - Default activated");
        }
    }

    public void CloseSidePanel()
    {
        UIManager.Instance.videoBG.raycastTarget = false;

        //isOutOfGame = false;
        TouchManager.isInGame = true;
        MoveScreenState(false);

        videoAndInfoClosed();

        UIManager.Instance.playerOfVideos.Stop();
        DisableLanguageButtons();

        for (int i = 0; i < fadeObjectsInfoClosed.Length; i++)
        {
            if (fadeObjectsInfoClosed[i].theObject)
            {
                fadeObjectsInfoClosed[i].theObject.DOFade(0, fadeObjectsInfoClosed[i].timeToAnimate);
            }
            else
            {
                fadeObjectsInfoClosed[i].theObjectRAW.DOFade(0, fadeObjectsInfoClosed[i].timeToAnimate);
            }
        }

        for (int i = 0; i < fadeObjectsMovieClosed.Length; i++)
        {
            if (fadeObjectsMovieClosed[i].theObject)
            {
                fadeObjectsMovieClosed[i].theObject.DOFade(0, fadeObjectsMovieClosed[i].timeToAnimate);
            }
            else
            {
                fadeObjectsMovieClosed[i].theObjectRAW.DOFade(0, fadeObjectsMovieClosed[i].timeToAnimate);
            }
        }

        for (int i = 0; i < fadeObjectsGoToGame.Length; i++)
        {
            if (fadeObjectsGoToGame[i].theObject)
            {
                fadeObjectsGoToGame[i].theObject.DOFade(fadeObjectsGoToGame[i].targetAlpha, fadeObjectsGoToGame[i].timeToAnimate);
            }
            else
            {
                fadeObjectsGoToGame[i].theObjectRAW.DOFade(fadeObjectsGoToGame[i].targetAlpha, fadeObjectsGoToGame[i].timeToAnimate);
            }
        }

        if (TouchManager.isInGame)
        {
            Timer.Instance.timerToPaintIsRunning = true;
        }
    }

    public void DisableLanguageButtons()
    {
        foreach (Button B in languageButtons)
        {
            B.interactable = false;
        }
    }

    public void EnableLanguageButtons()
    {
        foreach (Button B in languageButtons)
        {
            B.interactable = true;
        }
    }

    public void videoAndInfoClosed()
    {
        isVideoOpen = false;
        isInfoOpen = false;
    }

    public IEnumerator AnimateSecondToThird()
    {
        for (int i = 0; i < objectsToAnimate2nd3rd.Length; i++)
        {
            objectsToAnimate2nd3rd[i].theObject.transform.DOLocalMove(objectsToAnimate2nd3rd[i].newPos, objectsToAnimate2nd3rd[i].timeToAnimate).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(objectsToAnimate2nd3rd[i].delatToActivate);
        }


        UIManager.Instance.firstScreenUI.SetActive(false);

        StartCoroutine(AnimateThirdScreen());
        yield return null;
    }

    public IEnumerator AnimateThirdScreen()
    {
        for (int i = 0; i < objectsToAnimate3rd.Length; i++)
        {
            if (objectsToAnimate3rd[i].isDisableOnTouch)
            {
                UIManager.Instance.disableOnTouch.Add(objectsToAnimate3rd[i].theObject);
            }
            objectsToAnimate3rd[i].theObject.transform.DOLocalMove(objectsToAnimate3rd[i].newPos, objectsToAnimate3rd[i].timeToAnimate).SetEase(Ease.OutCirc);
            yield return new WaitForSeconds(objectsToAnimate3rd[i].delatToActivate);
        }

        UIManager.Instance.donePaintingButton.interactable = true;
        isFinishedAnimation = true;
        TouchManager.canPaintShoe = true;
        yield return null;
    }

    public IEnumerator AnimateThirdToLast()
    {
        //yield return new WaitForSeconds(1.1f);
        TouchManager.canPaintShoe = false;
        UIManager.Instance.tutorialPaintScreen.SetActive(false);

        for (int i = 0; i < objectsToAnimate3rdToLast.Length; i++)
        {
            objectsToAnimate3rdToLast[i].theObject.transform.DOLocalMove(objectsToAnimate3rdToLast[i].newPos, objectsToAnimate3rdToLast[i].timeToAnimate).SetEase(Ease.OutCirc);
            yield return new WaitForSeconds(objectsToAnimate3rdToLast[i].delatToActivate);
        }

        //UIManager.Instance.sidePanel.SetActive(false);
        StartCoroutine(AnimateLastScreenShoeScreenShot());

        yield return null;
    }

    public IEnumerator AnimateLastScreenShoeScreenShot()
    {

        //TouchManager.isInGame = false;

        shoeToAnimateLast.theObject.transform.DOLocalMove(shoeToAnimateLast.newPos, shoeToAnimateLast.timeToAnimate).SetEase(Ease.OutCirc);
        shoeToAnimateLast.theObject.transform.DORotate(shoeToAnimateLast.newRotation, shoeToAnimateLast.timeToAnimate).SetEase(Ease.OutCubic);
        shoeToAnimateLast.theObject.transform.DOScale(shoeToAnimateLast.newScale, shoeToAnimateLast.timeToAnimate).SetEase(Ease.OutCirc);
        yield return new WaitForSeconds(shoeToAnimateLast.delatToActivate);

        UIManager.Instance.firstScreenUI.SetActive(false);
        UIManager.Instance.lastScreenUI.SetActive(true);

        TouchManager.Instance.SetShoeRotation(shoeToAnimateLast.newRotation.y, shoeToAnimateLast.newRotation.z);
        //StartCoroutine(UIManager.Instance.TakeScreenShot());
        StartCoroutine(AnimatelastScren());

        //UIManager.Instance.sidePanel.SetActive(true);
        yield return null;
    }

    public IEnumerator AnimatelastScren()
    {
        for (int i = 0; i < objectToAnimateLast.Length; i++)
        {
            objectToAnimateLast[i].theObject.transform.DOLocalMove(objectToAnimateLast[i].newPos, objectToAnimateLast[i].timeToAnimate).SetEase(Ease.OutCirc);
            objectToAnimateLast[i].theObject.transform.DOScale(objectToAnimateLast[i].newScale, objectToAnimateLast[i].timeToAnimate).SetEase(Ease.OutCirc);
            yield return new WaitForSeconds(objectToAnimateLast[i].delatToActivate);
        }

        UIManager.Instance.isLastScreen = true;

        yield return null;
    }

    public void AnimateForSharing()
    {
        UIManager.Instance.lastScreenAssetsNoShoe.SetActive(false);
        UIManager.Instance.designAgainFromShareScreen.SetActive(true);
        //shoeToAnimateLast.theObject.transform.DOLocalMove(postitionForSharing, 0.8f).SetEase(Ease.OutCirc);
        //shoeToAnimateLast.theObject.transform.DORotate(Vector3.zero, 0.8f).SetEase(Ease.OutCubic);
        TouchManager.isSharingScreen = true;
        UIManager.Instance.isLastScreen = false;
        StartCoroutine(UIManager.Instance.TakeScreenShot());
    }
}
