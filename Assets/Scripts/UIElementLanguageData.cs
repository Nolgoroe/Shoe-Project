using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElementLanguageData : MonoBehaviour
{
    public Sprite[] languagesInOrderEHAC;

    public Image thisImage;

    public Vector3 leftToRightWorldPos;
    public Vector3 leftToRightWorldScale;
    public Vector3 rightToLeftWorldPos;
    public Vector3 rightToLeftWorldScale;

    public bool partOfWorld;
    //private void Start()
    //{
    //    thisImage = GetComponent<Image>();
    //}
    public void ChangeLanguageSprite(int index)
    {
        if(languagesInOrderEHAC[index] != null)
        {
            thisImage.sprite = languagesInOrderEHAC[index];
        }
        
        if(index == 0 && partOfWorld)
        {
            transform.localPosition = leftToRightWorldPos;
            transform.localScale = leftToRightWorldScale;
        }

        if (index != 0 && partOfWorld)
        {
            transform.localPosition = rightToLeftWorldPos;
            transform.localScale = rightToLeftWorldScale;
        }
    }
}
