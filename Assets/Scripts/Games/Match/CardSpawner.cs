using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Difficulty{easy,medium,hard}
public class CardSpawner : MonoBehaviour {



	public Texture2D[] images;
	public Sprite[] sprites;

	//[SerializeField] Vector3 cardScale
	[SerializeField] float topPadding = 1.5f;
	[SerializeField] float sidePadding = -2;

    [SerializeField] private LookSelect lookSelect;

	public Transform tier1trans;
	public Transform tier2trans;
	public Transform tier3trans;


    [SerializeField] private Transform tier1StartPosition;
    [SerializeField] private Transform tier2StartPosition;
    [SerializeField] private Transform tier3StartPosition;


    //[SerializeField] private Vector3 tier1StartPos;
    //[SerializeField] private Vector3 tier2StartPos;
    //[SerializeField] private Vector3 tier3StartPos;




	public bool isTier1;
	public bool	isTier2;
	public bool isTier3;

	[SerializeField] List<GameObject> cardObjects;

    [Header("References")]
    [SerializeField] private IntVariable currentCardsAvailable;

	private int cardsToSpawn;
	public int GetSpawned{

        //get{return cardsToSpawn;}
        get { return currentCardsAvailable.Value; }

    }

	private int wave;
	public int GetWave {

		get{ return wave;}

	}

    //private void Awake()
    //{
            
    //}
    //private void Awake()
    //{
    //    //if (isTier1 && !isTier2 && !isTier3)
    //    //{
    //        tier1StartPos = tier1Position.position;
    //        tier2StartPos = tier2Position.position;
    //        tier3StartPos = tier3Position.position;
    //          tier1trans.position = tier1StartPos;
    //         tier2trans.position = tier2StartPos;
    //         tier3trans.position = tier3StartPos;
    //    //}

    //}

    
    public void OnStart () {

        if (isTier1)
        
            ChangeWave(Difficulty.easy);

        
        if (isTier2)
        
            ChangeWave(Difficulty.medium);
        
        if (isTier3)

            ChangeWave(Difficulty.hard);
       
        



		

	}
  

    public void RemoveRound()
    {
       
        foreach (GameObject gO in cardObjects)
        {
            Destroy(gO);
        }
        cardObjects.Clear();

        if (this.gameObject.activeInHierarchy)
            this.gameObject.SetActive(false);

    }

    public void StartNewRound()
    {
        this.gameObject.SetActive(true);

    }



    public void ChangeWave (Difficulty diff){
       
		switch (diff) {

            
			case Difficulty.easy:

                currentCardsAvailable.SetValue(16);
				
				break;

			case Difficulty.medium:
             
                currentCardsAvailable.SetValue(32);
            
				break;

			case Difficulty.hard:
          
                currentCardsAvailable.SetValue(48);
             
				break;


		}

		

		Shuffle (sprites);
		for (int i = 0; i < currentCardsAvailable.Value; i++) {
			images [i] = Extensions.textureFromSprite (sprites [i]);
			images [i].name = sprites [i].name;
		}

		InstantiateCards ();




	}



	public void InstantiateCards (){

      
        int cardNumber = 0;

		for (int i = 0; i < currentCardsAvailable.Value; i++){

			GameObject topCard = GameObject.CreatePrimitive (PrimitiveType.Quad);
			GameObject bottomCard = GameObject.CreatePrimitive (PrimitiveType.Quad);

            topCard.GetComponent<MeshCollider> ().convex = true;

			topCard.name = images[i].name;

			topCard.transform.tag = "Card";

			topCard.GetComponent<Renderer> ().material.color = Color.blue;
			bottomCard.GetComponent<Renderer> ().material.SetTexture ("_MainTex", images[i]);

			cardObjects.Add (topCard);
			cardObjects.Add (bottomCard);

            if (i < 16) {
                //if (i <= 15) {

                if (i == 0)
                {
                    tier1trans.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    cardNumber = 0;
                    tier1trans.position = tier1StartPosition.position;

                }

                topCard.transform.localRotation = tier1trans.transform.rotation;
				topCard.transform.localPosition = tier1trans.position + ((sidePadding * Vector3.right) * cardNumber);
				
				bottomCard.transform.localPosition = tier1trans.position + ((sidePadding * Vector3.right) * cardNumber);

                ++cardNumber;
                
                if (0 == cardNumber % 5)
                {
                        topCard.transform.position = tier1trans.position + (-topCard.transform.up * topPadding);
                        bottomCard.transform.position = tier1trans.position + (-topCard.transform.up * topPadding);

                        tier1trans.position = topCard.transform.position;

                    cardNumber = 1;

                 }



                } else if (i > 15 && i <= 31 ) {

                if (i == 16)
                {
                    tier2trans.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    cardNumber = 0;
                    tier2trans.position = tier2StartPosition.position;
                }

				topCard.transform.localRotation = tier2trans.transform.rotation;
				topCard.transform.localPosition = tier2trans.position + ((sidePadding * Vector3.right) * cardNumber);

                bottomCard.transform.localPosition = tier2trans.position + ((sidePadding * Vector3.right) * cardNumber);

                ++cardNumber;
                
                if (0 == cardNumber % 5)
                {
                    topCard.transform.position = tier2trans.position + (-topCard.transform.up * topPadding);
                    bottomCard.transform.position = tier2trans.position + (-topCard.transform.up * topPadding);

                    tier2trans.position = topCard.transform.position;

                    cardNumber = 1;

                }


            } else if (i > 31 && i <= 47) {


                if (i == 32)
                {
                    tier3trans.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    cardNumber = 0;
                    tier3trans.position = tier3StartPosition.position;
                }


				topCard.transform.rotation = tier3trans.transform.rotation;
				topCard.transform.position = tier3trans.position + ((sidePadding * Vector3.right) * cardNumber);
			
				bottomCard.transform.position = tier3trans.position + ((sidePadding * Vector3.right) * cardNumber) + new Vector3 (0, 0, 0);


                ++cardNumber;
                if (0 == cardNumber % 5)
                {
                    topCard.transform.position = tier3trans.position + (-topCard.transform.up * topPadding);
                    bottomCard.transform.position = tier3trans.position + (-topCard.transform.up * topPadding);

                    tier3trans.position = topCard.transform.position;

                    cardNumber = 1;

                }
            }
			bottomCard.transform.parent = topCard.transform;
            
            topCard.transform.localScale = tier1trans.transform.localScale;

			bottomCard.transform.localRotation = Quaternion.Euler (0, 180, 0);

          //  cardNumber++;

            //Start new Row
			//if (0 == cardNumber % 5) {

			//	if (isTier1) {

   //                 topCard.transform.position = tier1trans.position + (-topCard.transform.up * topPadding );
			//		bottomCard.transform.position = tier1trans.position + (-topCard.transform.up *topPadding);

   //                 tier1trans.position = topCard.transform.position;
				
			//	} else if (isTier2) {

   //                 topCard.transform.position = tier2trans.position + (-topCard.transform.up * topPadding);
			//		bottomCard.transform.position = tier2trans.position + (-topCard.transform.up * topPadding);
			//		tier2trans.position = topCard.transform.position;
				

			//	} else if (isTier3) {

   //              //   tier3trans.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        
                    
			//		topCard.transform.position = tier3trans.position + (-topCard.transform.up * topPadding);
			//		bottomCard.transform.position = tier3trans.position + (-topCard.transform.up * topPadding);
			//		tier3trans.position = topCard.transform.position;
				
			//	}
   //             //restart counting row
			//	cardNumber = 1;
                
   //         }
            //tier1trans.position = tear1StartPos;
            //tier2trans.position = tear2StartPos;
            //tier3trans.position = tear3StartPos;

        }
        //tier1trans.position = tier1StartPos;
        //tier2trans.position = tier2StartPos;
        //tier3trans.position = tier3StartPos;


    }


    void Shuffle (Sprite [] array){

		for (int t = 0; t < currentCardsAvailable.Value; t++){

			Sprite tmp = array [t];
			int r = Random.Range (t, currentCardsAvailable.Value);
			array [t] = array [r];
			array [r] = tmp;
		}

	}
	

}
