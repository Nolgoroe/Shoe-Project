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
    public float timeToAnimate = 1;
    public Image imageToChange;
    public Sprite originalSprite;
    public Sprite targetSprite;
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

    public Button[] languageButtons;

    [HideInInspector]
    public bool isVideoOpen, isInfoOpen;
    [HideInInspector]
    public bool isOutOfGame;
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
        isOutOfGame = true;
        isInfoOpen = true;
        EnableLanguageButtons();
        Timer.Instance.timerIsRunning = false;

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

            if (ReadFolderData.Instance.languageVideoClipsURL[UIManager.Instance.clickedIndexByInfo] != null)
            {
                UIManager.Instance.playerOfVideos.url = ReadFolderData.Instance.languageVideoClipsURL[UIManager.Instance.clickedIndexByInfo];
                UIManager.Instance.playerOfVideos.Play();
            }
            else
            {
                UIManager.Instance.playerOfVideos.url = ReadFolderData.Instance.languageVideoClipsURL[0];
                UIManager.Instance.playerOfVideos.Play();

                Debug.Log("Backup Video");
            }

            for (int i = 0; i < fadeObjectsInfoClosed.Length; i++)
            {
                if (fadeObjectsInfoClosed[i].theObject)
                {
                    fadeObjectsInfoClosed[i].theObject.DOFade(fadeObjectsInfoClosed[i].origingalAlpha, fadeObjectsInfoClosed[i].timeToAnimate);
                }
                else
                {
                    fadeObjectsInfoClosed[i].theObjectRAW.DOFade(fadeObjectsInfoClosed[i].origingalAlpha, fadeObjectsInfoClosed[i].timeToAnimate);
                }
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
                fadeObjectsInfoClosed[i].theObject.DOFade(fadeObjectsInfoClosed[i].origingalAlpha, fadeObjectsInfoClosed[i].timeToAnimate);
            }
            else
            {
                fadeObjectsInfoClosed[i].theObjectRAW.DOFade(fadeObjectsInfoClosed[i].origingalAlpha, fadeObjectsInfoClosed[i].timeToAnimate);
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
        UIManager.Instance.videoButtons[index].color = ReadFolderData.Instance.translatedColorCode;

        if (isInfoOpen)
        {
            UIManager.Instance.infoButtons[index].color = ReadFolderData.Instance.translatedColorCode;
        }
        else
        {
            UIManager.Instance.infoButtons[index].color = new Color(ReadFolderData.Instance.translatedColorCode.r, ReadFolderData.Instance.translatedColorCode.g, ReadFolderData.Instance.translatedColorCode.b, 0);
        }


        UIManager.Instance.ChangeLanguageColors(index);

        videoAndInfoClosed();

        UIManager.Instance.clickedIndexByInfo = index;

        DisableLanguageButtons();

        UIManager.Instance.playerOfVideos.Stop();

        MoveScreenState(false);

        //playerOfVideos.clip = videoLanguages[index];
        FadeOutVideoBar(true);
    }

    public void FadeOutVideoBar(bool open)
    {
        isOutOfGame = true;

        if (!open)
        {
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
            Timer.Instance.timerIsRunning = false;

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
            StartCoroutine(StartVidAfterFadeOut(fadeObjectsMovieClosed[0].timeToAnimate + 0.1f));
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
        isOutOfGame = false;

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
            Timer.Instance.timerIsRunning = true;
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
}
