using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
//using Unity.Mathematics;

public class LookInteraction : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler {

	[SerializeField] GameObject imageGO;
	//[SerializeField] float lookTime;

	//[SerializeField] float timeUntilImageDeactivate = 5f;
	[SerializeField] bool isUnscaledTime;
	//[SerializeField] float lookDistance;
	[SerializeField] bool isLookAtPlayer = true;
    [SerializeField] bool isLookAtPlayerInverse = false;


    [SerializeField]UnityEvent onLookClick;

	//Check for bugs since I invalidated this in script
	[SerializeField] bool isItemForInventory;
    [SerializeField] public Item item;
    [SerializeField] GameObject parentOfItem;
    //[SerializeField] ItemInventoryRunTimeSet itemInventory;
    //[SerializeField] Item item;


    [SerializeField]bool isSpriteChangeOnClick = false;
	[SerializeField]private Sprite spriteToChange;

	[SerializeField]bool isColorChangeOnClick = false;
	[SerializeField]private Color colorToChange;
	[SerializeField]private Image alternativeImageToChangeColor;
	private Color originalColor;


	[SerializeField]UnityEvent onSecondaryLookClick;
	[SerializeField] bool isSecondLookEventCalled;

	public Sprite originalSprite;
	public Image image;


	private Collider thisCollider;
    private Transform thisTransform;

	[SerializeField]private bool isOnFromStart;
	[SerializeField] bool isStayOn;
	public Collider lookTriggerCollider;

	private Animator thisAnimator;
	private Camera cam;
	float timer;


    [SerializeField] UnityEvent onDisableImage;

    public bool canAppear = true;
    /// <summary>
    /// Allow For Image To Appear
    /// </summary>
    /// <param name="set">Modify Can Appar Setting</param>
    /// <returns>current status of can Appar bool</returns>
    public void SelectionAppear(bool set)
    {
    
       canAppear = set;
       
    }
 //   [Header("References")]
	//[SerializeField]private BoolVariable isSceneLoading;

	void Awake () {

        if (isItemForInventory)
            if (item.isPlayerCarrying || item.isRemoveFromGame)
            {

                if (!parentOfItem)
                    transform.parent.parent.gameObject.SetActive(false);
                else
                    parentOfItem.SetActive(false);


            }


        thisTransform = transform;
		cam = Camera.main;

        timer = Mathf.Infinity;//lookTime;
		thisCollider = GetComponent<Collider> ();
		thisAnimator = GetComponent<Animator> ();

        //if (timeUntilImageDeactivate == 0.0f)
        //    originalIndicatedDeactivationTime = Mathf.Infinity;
        //else
		//originalIndicatedDeactivationTime = timeUntilImageDeactivate;
		//if(!isItemForSlot)

		if(!isOnFromStart && lookTriggerCollider == null)
			lookTriggerCollider = transform.parent.GetComponent<Collider> ();

	
		if(image == null)
		image = GetComponentInChildren<Image> ();

		originalSprite = image.sprite;

		if (isColorChangeOnClick)
			originalColor = image.color;

      //  if(imageGO == null)
		imageGO = image.transform.parent.gameObject;

		//thisCollider.enabled = false;

		if (isOnFromStart) {
			lookTriggerCollider = null;
			imageGO.SetActive (true);
		}else
			if(!isStayOn)
			imageGO.SetActive (false);

	}

	private float timeActive;

    private bool isVisible = false;
    

	IEnumerator EnableAndDisable(){
	
		imageGO.SetActive (true);
       
        thisAnimator.SetBool("IsActive", true);

        while (true){
           // yield return new WaitForSeconds(0.01f);
            yield return new WaitForEndOfFrame();
            if (isVisible)
            {

                if (isLookAtPlayer)
                    thisTransform.LookAt(cam.transform);
                if (isLookAtPlayerInverse)
                    thisTransform.LookAt(2 * thisTransform.transform.position - cam.transform.position);
            }
            else
            {
          

                if (!isStayOn)
                    imageGO.SetActive(false);

                

                yield break;


            }


		}
       
    }

	public void DisableImage(){
		if(!isStayOn)
		imageGO.SetActive (false);
	}

	public void TriggerSelections(){


        if (isVisible || !canAppear)
            return;

       
        
        isVisible = true;
		StartCoroutine (EnableAndDisable ());


//
	}

    

    public void RestartButtonToOriginal(bool original) {

        if (original)
        {
            SetToOriginalSprite();
            isOriginalImage = false;

            if (isColorChangeOnClick)
            {
                if (alternativeImageToChangeColor)
                    alternativeImageToChangeColor.color = originalColor;
                else
                    image.color = originalColor;
            }
        }
        else
        {
            if (isOriginalImage)
            {
                ChangeSprite();
            }


            if (isColorChangeOnClick)
            {
                 if (alternativeImageToChangeColor)
                    alternativeImageToChangeColor.color = colorToChange;
                else
                    image.color = colorToChange;
            }

       }




       

    }
    public void Invoke_Call(int buttonNumber = 1)
    {
        if(buttonNumber == 1)
        {
            onLookClick.Invoke();
        }else if(buttonNumber == 2)
        {
            onSecondaryLookClick.Invoke();
        }
    }

    public void RemoveSelections(){


        if (thisAnimator == null)
            thisAnimator = GetComponent<Animator>();
        //if (!isVisible)
        //    return;
        onDisableImage.Invoke();
     
        thisAnimator.SetBool("IsActive", false);
        isVisible = false;

      

    }


	private bool isActive;
	private bool isOriginalImage = false;

	[SerializeField]private bool isCustomTime;
	[SerializeField]private float customTime;
	private float originalTime;

   

    public void OnPointerEnter(PointerEventData eventData){
		if (!EventSystem.current.IsPointerOverGameObject ()) {
		
			if (isCustomTime) {
				originalTime = GazeInputModule.GazeTimeInSeconds;
				GazeInputModule.GazeTimeInSeconds = customTime;
			}
			
			StartCoroutine (OnHover ());

		}
	}
	IEnumerator OnHover(){
		float secondsUntilClick = GazeInputModule.GazeTimeInSeconds;
		float totalTimeToWait = secondsUntilClick;

		if (isItemForInventory) {

			if (!isActive) {
				StartCoroutine (EnableAndDisable ());
			}else
				totalTimeToWait = secondsUntilClick;

		}
			
		while(true){

			if (isUnscaledTime)
				secondsUntilClick -= Time.unscaledDeltaTime;
			else
				secondsUntilClick -= Time.deltaTime;

            var timeLeft = secondsUntilClick / totalTimeToWait;

            image.fillAmount = timeLeft;


            yield return null;
        }
           
	}   
	public void OnPointerExit(PointerEventData eventData){
		StopAllCoroutines ();
		image.fillAmount = 1;

		if (isCustomTime) 
			GazeInputModule.GazeTimeInSeconds = originalTime;
		

		Debug.Log("I AM Out!!!!!!");
		timeActive = 0;


	}
	public void OnPointerClick(PointerEventData eventData){
		
		Debug.Log("I AM clickeed!!!!!!");

        isOriginalImage = !isOriginalImage;

        if (isCustomTime) 
			GazeInputModule.GazeTimeInSeconds = originalTime;

        if (isOriginalImage)
        {
            onLookClick.Invoke();
            
          //  thisAnimator.SetBool("IsActive", false);
        }
        else
        {

            if (isSecondLookEventCalled)
                onSecondaryLookClick.Invoke();

            else
            {
                onLookClick.Invoke();
            }

        }	


		

		if (isSpriteChangeOnClick) {
			ChangeSprite ();

		}

		if (isColorChangeOnClick) {

			if (!isOriginalImage) {
				if (alternativeImageToChangeColor)
					alternativeImageToChangeColor.color = originalColor;
				else
					image.color = originalColor;
			} else {
				if (alternativeImageToChangeColor)
					alternativeImageToChangeColor.color = colorToChange;
				else
					image.color = colorToChange;
			}

		}


		//isActive = false;
	
		if (!isStayOn) 
			imageGO.SetActive (false);
			

		timeActive = 0;

		if (isItemForInventory) {

            if (!parentOfItem)
            {
                var ParentOfItem = transform.parent.parent.gameObject;
               
            }

            item.itemGO = parentOfItem;
            item.itemName = parentOfItem.name;
			PlayerInventory.Instance.AddItemToSlot (item);
		//	Debug.Log (transform.parent.gameObject.name);
			var tempAS = AudioManager.Instance.GetAudioSourceReferance (AudioManager.AudioReferanceType._DIRECT,"PickUp");
			tempAS.transform.position = transform.position;
			AudioManager.Instance.PlayDirectSound ("PickUp");


		}
		StopAllCoroutines ();
		image.fillAmount = 1;
	}


	public void ChangeSprite(){

		if (isOriginalImage)
			image.sprite = spriteToChange;
		else
			image.sprite = originalSprite;


	
	
	}
	public void SetToOriginalSprite(){
	
		image.sprite = originalSprite;
		isOriginalImage = true;
	
	}

}
