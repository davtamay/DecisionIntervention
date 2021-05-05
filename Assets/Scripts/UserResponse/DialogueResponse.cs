using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueResponse : MonoBehaviour
{

    [Header("Initial Interaction References")]
    [SerializeField] private LookInteraction initialSpeakInteraction;
    [SerializeField] private bool isUnscaledTime = true;


    [Header("Global Set Settings")]
    //[SerializeField] private bool hasTextOptions = true;
    [SerializeField] private float responseDelay;
    [Space]
    [SerializeField] private bool isWrong = false;
    [SerializeField] private string wrongMessage;
    [SerializeField] private UnityEvent onInCompleteDialogue;
    [Space]
    [SerializeField] private bool isCompleted = false;
    [SerializeField] private bool isResetCompletedAfterDialogue = false;
    [SerializeField] private string correctMessage;
    [SerializeField] private UnityEvent onCompleteDialogue;
    [Space]

    [Space]
    public Dialogue_Data[] currentDialougeMessageSet;
    [Space]

    [Header("Main Dialogue References")]
    [SerializeField] private Dialogue_Change mainDialogueToShow;
    [SerializeField] private GameObject mainInteraction;
    [SerializeField] private Text mainText;
    [SerializeField] private Image mainImageDisplayReference;
    // [Multiline] [SerializeField] private string[] mainDiscussion;

    [Space]

    [Header("Dialogue Options References")]
    [SerializeField] private bool isOnFromStart = false;

    [Space]

    [SerializeField] private LookInteraction continueInteraction_Option;
    [SerializeField] private LookInteraction lookInteraction_Option1;
    private Text textOption1;
   // [Multiline] [SerializeField] private string[] discussionOptions1;

    [Space]

    [SerializeField] private LookInteraction lookInteraction_Option2;
    private Text textOption2;
   

    [Header("Stressed References")]
    [SerializeField] private int resilienceNeededForHelping;
    [SerializeField] private Dialogue_Change dialogueToShowOnStressed;
    [SerializeField] private bool isStressed;
    [SerializeField] private UnityEvent onStressedDialogue;

    [Space]
    [Header("Debug.Info")]
    public int level;
    [Space]

    [Header("References")]
    [SerializeField] private DataManager DATA_MANAGER;



    // public enum DialogueOptions {First_Option_Continue, Second_Option}
    //[SerializeField] private DialogueOptions[] answerKey2;

    private Animator mainInteractionAnimator;
    private int isActiveHash = Animator.StringToHash("IsActive");


    //[SerializeField] private LocalizationManager LOCALIZATION_MANAGER;

    public void Start()
    {

        mainInteractionAnimator = mainInteraction.GetComponent<Animator>();

        if (mainText == null)
            mainText = mainInteraction.GetComponentInChildren<Text>();

        //mainText.text = mainDiscussion[level];
        mainText.text = currentDialougeMessageSet[level].mainText;


       mainInteractionAnimator.SetBool(isActiveHash, true);
        mainInteractionAnimator.SetBool(isActiveHash, false);

        if (lookInteraction_Option1 != null)
        {
            textOption1 = lookInteraction_Option1.GetComponentInChildren<Text>(true);
            // textOption1.text = discussionOptions1[level];

            textOption1.text = currentDialougeMessageSet[level].discussion_text_Option1;
        }

        if (lookInteraction_Option2 != null)
        {
            textOption2 = lookInteraction_Option2.GetComponentInChildren<Text>(true);
            //textOption2.text = discussionOptions2[level];

            textOption2.text = currentDialougeMessageSet[level].discussion_text_Option2;
        }

       if(isOnFromStart)
        TurnOnDialogue();
        

    }
    private bool hasBranch = false;
    //  private BoolVariable

    public void TurnOnDialogue()
    {
        isStressed = DATA_MANAGER.playerData.playerResilienceHealth.resilienceHealth < resilienceNeededForHelping;

     //   Debug.Log(currentDialougeMessageSet[level].imageToDisplay + " " + level);
        if (mainImageDisplayReference != null)
        {
            
            if (currentDialougeMessageSet[level].imageToDisplay != null)
            {
                mainImageDisplayReference.gameObject.SetActive(true);
                mainImageDisplayReference.sprite = currentDialougeMessageSet[level].imageToDisplay;

            }
            else
            {
                mainImageDisplayReference.gameObject.SetActive(false);
                mainImageDisplayReference.sprite = null;

            }
        }



        if (isStressed)
            ChangeDisplayedText(dialogueToShowOnStressed);
        else
        {
            if (mainDialogueToShow != null)
                ChangeDisplayedText(mainDialogueToShow);

        }

        if (initialSpeakInteraction)
            initialSpeakInteraction.canAppear = false;


        if (isCompleted)
        {
            level = currentDialougeMessageSet.Length - 1;
            mainText.text = correctMessage;
            StartCoroutine(ChangeOptions(false));
            return;
        }
        else
        {
          
        
            //  var imageBeingDisplayed = currentDialougeMessageSet[level].imageToDisplay.gameObject.SetActive(true); 
            //currentDialougeMessageSet[level].imageToDisplay.gameObject.SetActive(false);

            //if (currentDialougeMessageSet[level].imageToDisplay != null)
            //currentDialougeMessageSet[level].imageToDisplay..SetActive(true);

            mainText.text = currentDialougeMessageSet[level].mainText;


            if (lookInteraction_Option1 != null)
                textOption1.text = currentDialougeMessageSet[level].discussion_text_Option1;

            if (lookInteraction_Option2 != null)
                textOption2.text = currentDialougeMessageSet[level].discussion_text_Option2;


            if (currentDialougeMessageSet[level].isOptionChoiceAvailable)
            {

                lookInteraction_Option1.TriggerSelections();

                if (lookInteraction_Option2 != null)
                    lookInteraction_Option2.TriggerSelections();
            }
            else
            {
                if (continueInteraction_Option != null)
                    continueInteraction_Option.TriggerSelections();

            }
        }
        

        mainInteractionAnimator.SetBool(isActiveHash, true);
    }
    

    public void TurnOffDialogue(bool keepLevel = false)
    {
    


        if (initialSpeakInteraction)
        {
            initialSpeakInteraction.canAppear = true;
            initialSpeakInteraction.RemoveSelections();

        }
        
        if (!isCompleted && !keepLevel)
            level = 0;


        if (isCompleted && !isStressed)
            onCompleteDialogue.Invoke();



        if (!mainInteractionAnimator)
            mainInteractionAnimator = mainInteraction.GetComponent<Animator>();

        mainInteractionAnimator.SetBool(isActiveHash, false);

        if (lookInteraction_Option1 != null)
            lookInteraction_Option1.RemoveSelections();

        if (continueInteraction_Option != null)
            continueInteraction_Option.RemoveSelections();

        if (lookInteraction_Option2 != null)
            lookInteraction_Option2.RemoveSelections();


        //ChangeDisplayedText(currentDialougeMessageSet[level].dialogueChange_option_1);
  

    }

    public void ChangeDisplayedText(Dialogue_Change dialogueChange)
    {

        TurnOffDialogue();
        level = 0;

        var newLength = dialogueChange.dialogueDataMessages.Length;
       
        currentDialougeMessageSet = new Dialogue_Data[newLength];

        currentDialougeMessageSet = dialogueChange.dialogueDataMessages;

        for (int i = 0; i < newLength; i++)
        {
            
            currentDialougeMessageSet[i].mainText = dialogueChange.dialogueDataMessages[i].mainText;
         
            currentDialougeMessageSet[i].discussion_text_Option1 = dialogueChange.dialogueDataMessages[i].discussion_text_Option1;
           // discussionOptions1[i] = dialogueChange.dialogueDataMessages[i].discussion_text_Option1;

            if (lookInteraction_Option2 != null)
               currentDialougeMessageSet[i].discussion_text_Option2 = dialogueChange.dialogueDataMessages[i].discussion_text_Option2;

             currentDialougeMessageSet[i].isOptionChoiceAvailable = dialogueChange.dialogueDataMessages[i].isOptionChoiceAvailable;

            currentDialougeMessageSet[i].imageToDisplay = dialogueChange.dialogueDataMessages[i].imageToDisplay;

        }

        correctMessage = dialogueChange.correctMessage;
        wrongMessage = dialogueChange.inCorrectMessage;
        this.isCompleted = dialogueChange.isComplete;

        //NEW 4/6/20 WORKS WELL IN SKIPPING MESSAGES SUDENLY
        StopAllCoroutines();

    }
   [SerializeField] private Dialogue_Change dataBranching1;
   [SerializeField] private Dialogue_Change databranching2;
    private int correctOption;
 //   private bool hasBranching;
    public IEnumerator ChangeOptions(bool isLastDialogueDelay = true)
    {
        #region TURN_EVERYTHING_OFF
        mainInteractionAnimator.SetBool(isActiveHash, false);

        if (continueInteraction_Option != null)
        {
            continueInteraction_Option.image.raycastTarget = false;
            continueInteraction_Option.RemoveSelections();
        }
        if (lookInteraction_Option1 != null)
        {
            lookInteraction_Option1.image.raycastTarget = false;
            lookInteraction_Option1.RemoveSelections();
        }

        if (lookInteraction_Option2 != null)
        {
            lookInteraction_Option2.image.raycastTarget = false;
            lookInteraction_Option2.RemoveSelections();
        }

        #endregion

        #region SETUP_COMPLETED_OR_WRONG_OR_STRESSED_MESSAGE

       

        if (isCompleted || isWrong || isStressed)
        {
            if (isLastDialogueDelay)
            {
                if (isUnscaledTime)
                    yield return new WaitForSecondsRealtime(responseDelay);
                else
                    yield return new WaitForSeconds(responseDelay);

            }




            if (isCompleted)
            {
                mainText.text = correctMessage;
              //  mainInteractionAnimator.SetBool(isActiveHash, true);
                // onCompleteDialogue.Invoke();
            }
            else if (isWrong)
            {
                mainText.text = wrongMessage;
                isWrong = false;

            }
            else if (isStressed)
            {

                onStressedDialogue.Invoke();
                level = 0;

                if (initialSpeakInteraction)
                    initialSpeakInteraction.canAppear = true;

               /// continueInteraction_Option.image.raycastTarget = true;


                //yield break;

            }

            mainInteractionAnimator.SetBool(isActiveHash, true);

            if (isUnscaledTime)
                yield return new WaitForSecondsRealtime(responseDelay);
            else;
               yield return new WaitForSeconds(responseDelay);

            mainInteractionAnimator.SetBool(isActiveHash, false);


            //if (isCompleted)
            //    onCompleteDialogue.Invoke();

            if (initialSpeakInteraction)
                initialSpeakInteraction.canAppear = true;

            if (isResetCompletedAfterDialogue)
            {
                level = 0;
                isCompleted = false;
            }

            if(lookInteraction_Option1 != null)
                lookInteraction_Option1.image.raycastTarget = true;
            if (lookInteraction_Option2 != null)
                lookInteraction_Option2.image.raycastTarget = true;
            if(continueInteraction_Option != null)
                continueInteraction_Option.image.raycastTarget = true;

            yield break;


        }
        #endregion

        #region TURN_OPTIONS_ON

     
        // Debug.Log("THIS IS HAPPENING::::::::::::::::::::::::::");
        if (isUnscaledTime)
            yield return new WaitForSecondsRealtime(responseDelay);
        else
            yield return new WaitForSeconds(responseDelay);


        if (currentDialougeMessageSet[level].isOptionChoiceAvailable)
        {

            lookInteraction_Option1.image.raycastTarget = true;
            textOption1.text = currentDialougeMessageSet[level].discussion_text_Option1;
            lookInteraction_Option1.TriggerSelections();


            if (lookInteraction_Option2 != null)
            {
                lookInteraction_Option2.image.raycastTarget = true;

                textOption2.text = currentDialougeMessageSet[level].discussion_text_Option2;

                lookInteraction_Option2.TriggerSelections();
            }

        }
        else
        {

            if (continueInteraction_Option != null)
            {
                continueInteraction_Option.image.raycastTarget = true;
                continueInteraction_Option.TriggerSelections();

            }
        }

        #endregion

        #region SETUP_TURNON_MAININTERACTION

        mainInteractionAnimator.SetBool(isActiveHash, true);

        mainText.text = currentDialougeMessageSet[level].mainText;

        if (mainImageDisplayReference != null)
        {

            if (currentDialougeMessageSet[level].imageToDisplay != null)
            {
                mainImageDisplayReference.gameObject.SetActive(true);
                mainImageDisplayReference.sprite = currentDialougeMessageSet[level].imageToDisplay;

            }
            else
            {
                mainImageDisplayReference.gameObject.SetActive(false);
                mainImageDisplayReference.sprite = null;

            }
        }
        #endregion

    }
    //public void CheckSelected(DialogueOptions dO) { }
    //1 is true //0 is false
    public void CheckSelectedOptions(int value)
    {
       

        currentDialougeMessageSet[level].onSelectOption.Invoke();


        //Debug.Log(currentDialougeMessageSet[level].CorrectOption);
        if (currentDialougeMessageSet[level].isBranch)
        {
          

            if (value == 1)
                ChangeDisplayedText(currentDialougeMessageSet[level].dialogueChange_option_1);
            else if (value == 2)
                ChangeDisplayedText(currentDialougeMessageSet[level].dialogueChange_option_2);

            level = 0;
            StartCoroutine(ChangeOptions());

           
            return;
        }


        if (currentDialougeMessageSet[level].CorrectOption == value)
        {

           



            ++level;

            // if (answerKey.Length == level && !isStressed)
            if (currentDialougeMessageSet.Length == level && !isStressed)
            {
                isCompleted = true;

            }

        }
        else
        {
        //    Debug.Log(currentDialougeMessageSet[level].CorrectOption);
          

            level = 0;

            isWrong = true;

            onInCompleteDialogue.Invoke();
        }

        StartCoroutine(ChangeOptions());

    }

    public void AddLevel(int level)
    {
        currentDialougeMessageSet[this.level].onSelectOption.Invoke();

        this.level += level;
      //  currentDialougeMessageSet[level].onSelectOption.Invoke();

        if (currentDialougeMessageSet.Length == this.level)
                isCompleted = true;


    }


}
