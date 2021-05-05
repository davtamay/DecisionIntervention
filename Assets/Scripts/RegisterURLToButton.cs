using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RegisterURLToButton : MonoBehaviour
{
    public GameObject parentObject;
    public Button thisButton;
    void Awake()
    {
        thisButton = GetComponent<Button>();
        //  gameObject.SetActive(false);
        parentObject.SetActive(false);
       // Register("as");
    }

    public void Register(string url)
    {
        thisButton.onClick.AddListener(() => Application.ExternalEval($"window.open({url}),\"_blank\")"));
        thisButton.onClick.AddListener(() => Application.OpenURL(url));
        //   Application.OpenURL(url);
    }
    public void UnRegisterAll()
    {

        thisButton.onClick.RemoveAllListeners();
    }
       
   
}
