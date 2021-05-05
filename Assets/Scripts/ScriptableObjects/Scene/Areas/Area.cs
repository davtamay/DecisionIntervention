using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Area", menuName = "CustomSO/Scene/Area")]
public class Area : ScriptableObject
{
    public bool isAvailable;
    public bool isShowing;
    public Sprite areaImage;
    public string areaName;
}
