using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Game {FINDER, WACK, SKYJUMPER, MATCH, HIT, COLLECTIONS, SHOOT, HOOP}

public class SetGameScore : MonoBehaviour {

    [SerializeField] private Text currentPoints;
    
    //public Game curGame;
    //private TextMesh highScorePointsTextMesh;
	[SerializeField]private Text highScorePoints; 


	//[SerializeField] private bool isUsingTextMesh;

	//[SerializeField] private string sceneToLoadScoreFrom;

	[Header("References")]
	[SerializeField]private DataManager DATA_MANAGER;
    [SerializeField] private Points gameHighScorePoints;


    private string originalCurrentText;
    void Awake(){

        if (currentPoints == null)
        {
            currentPoints = GetComponent<Text>();
            originalCurrentText = currentPoints.text;

        }else
            originalCurrentText = currentPoints.text;

        if (highScorePoints == null)
            highScorePoints = transform.GetChild(0).GetComponent<Text>();

	}

	//void OnEnable(){
    public void SetCurrentPoints()
    {
        //if(currentPoints.text.Length > originalcurrentPointTextCount)
      
       // currentPoints.text.Remove(0,5);//originalcurrentPointTextCount-1);
         //   string.s
        currentPoints.text = originalCurrentText + " " + DATA_MANAGER.playerData.masterPlayerPoints.currentPlayerPoints.Value;

    }
    public void SetHighScore()
    {
           // DATA_MANAGER.playerData.CheckAndSaveHighScore(gameHighScorePoints);
			highScorePoints.text = "HighScore: " + gameHighScorePoints.Value;

     }

    public void ClearCurrentPointText() {
        currentPoints.text = string.Empty;
    }
    public void ClearHighScorePointText()
    {
        highScorePoints.text = string.Empty;
    }


}
