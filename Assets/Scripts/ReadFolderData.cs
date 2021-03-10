using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
using System;

public class ReadFolderData : MonoBehaviour
{
    public static ReadFolderData Instance;
    //public LocalizedTexture localTex;
    public string colorCode;

    public Texture[] languageTextures;
    public string[] languageVideoClipsURL;

    public Color translatedColorCode;

    public Button[] languageButtons;
    void Start()
    {
        Instance = this;

        languageTextures = new Texture[4];
        languageVideoClipsURL = new string[4];
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);

        FileInfo[] colorCode = dir.GetFiles("*.txt");

        foreach (FileInfo f in colorCode)
        {
            if (f.ToString().Contains("Color"))
            {
                print(f.Name);
                StartCoroutine(LoadColorCode(f));
            }
        }

        FileInfo[] Images = dir.GetFiles("*.png");

        foreach (FileInfo f in Images)
        {
            if (f.ToString().Contains("INFO"))
            {
                StartCoroutine(LoadImages(f));
            }
        }

        FileInfo[] moveis = dir.GetFiles("*.mp4");

        foreach (FileInfo f in moveis)
        {
            if (f.ToString().Contains("VIDEO"))
            {
                StartCoroutine(LoadMovies(f));
            }
        }
    }


    IEnumerator LoadMovies(FileInfo GameData)
    {
        //1 ignore meata files
        if (GameData.Name.Contains("meta"))
        {
            yield break;
        }
        else
        {
            string wwwVideoFilePath = "file://" + GameData.FullName.ToString();
            UnityWebRequest www = new UnityWebRequest(wwwVideoFilePath);
            //UnityWebRequest www = new UnityWebRequest(wwwImageFilePath);
            yield return www;

            int num = 0;
            if (int.TryParse(GameData.Name[0].ToString(), out num))
            {
                languageVideoClipsURL[num] = www.url;

                if (num == 0)
                {
                    UIManager.Instance.playerOfVideos.url = languageVideoClipsURL[0];
                    UIManager.Instance.playerOfVideos.Play();
                    UIManager.Instance.playerOfVideos.SetDirectAudioVolume(0, 0);
                    StartCoroutine(SetVideo());
                }
            }
            else
            {
                Debug.Log("Number 0 - 3 have to be at start of file name!");
            }

            //languageVideoClipsURL.Add(www.url);
        }
    }
    IEnumerator LoadColorCode(FileInfo GameData)
    {
        //1 ignore meata files
        if (GameData.Name.Contains("meta"))
        {
            yield break;
        }
        else
        {
            string wwwTextFilePath = "file://" + GameData.FullName.ToString();
            UnityWebRequest www = new UnityWebRequest(wwwTextFilePath);
            yield return www;

            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/ColorData.txt");

            colorCode = reader.ReadToEnd();

            Color color = new Color();
            ColorUtility.TryParseHtmlString("#"+colorCode, out color);
            translatedColorCode = color;
            UIManager.Instance.coloredBar.color = translatedColorCode;


            foreach (Button b in languageButtons)
            {
                ColorBlock colors = b.colors;
                colors.pressedColor =translatedColorCode;
                b.colors = colors;
            }


            reader.Close();
        }

    }
    IEnumerator LoadImages(FileInfo GameData)
    {
        //1 ignore meata files
        if (GameData.Name.Contains("meta"))
        {
            yield break;
        }
        else
        {
            string wwwImageFilePath = "file://" + GameData.FullName.ToString();
            Debug.Log(GameData.Name[0]);

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(wwwImageFilePath);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                int num = 0;
                if (int.TryParse(GameData.Name[0].ToString(), out num))
                {
                    languageTextures[num] = DownloadHandlerTexture.GetContent(www);
                }
                else
                {
                    Debug.Log("Number 0 - 3 have to be at start of file name!");
                }
                //languageTextures.Add(DownloadHandlerTexture.GetContent(www));
            }
        }
    }

    IEnumerator SetVideo()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Stopped");
        UIManager.Instance.playerOfVideos.Stop();
        UIManager.Instance.playerOfVideos.SetDirectAudioVolume(0,1);
    }
}
