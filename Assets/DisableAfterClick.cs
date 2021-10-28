using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableAfterClick : MonoBehaviour
{

    public void ButtonToDisable()
    {
        Button b = GetComponent<Button>();
        b.interactable = false;
    }
}
