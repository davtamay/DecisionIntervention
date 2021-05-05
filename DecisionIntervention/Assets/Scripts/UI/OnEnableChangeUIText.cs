using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnEnableChangeUIText : MonoBehaviour
{
    [Multiline]
    public string[] textToApply;
    public Text uIText;

    public void Awake()
    {
        uIText = GetComponent<Text>();
    }
    public void OnEnable()
    {
        uIText.text = textToApply[Random.RandomRange(0, textToApply.Length)];
    }
}
