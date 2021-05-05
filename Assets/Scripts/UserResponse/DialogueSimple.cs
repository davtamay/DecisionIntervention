using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueSimple : MonoBehaviour
{


    [SerializeField] private float responseDelay;

    [SerializeField] private GameObject mainInteraction;
    [SerializeField] private Text mainText;
    [SerializeField] private string[] mainDiscussion;


    [SerializeField] private LookInteraction lookInteraction_Option1;
    private Text textOption1;

    [SerializeField] private string[] discussionOptions1;


    [SerializeField] private LookInteraction lookInteraction_Option2;
    private Text textOption2;
    [SerializeField] private string[] discussionOptions2;

    [SerializeField] private string wrongMessage;
    [SerializeField] private bool isCompleted = false;
    [SerializeField] private string correctMessage;

    [SerializeField] private int[] answerKey;


    [SerializeField] private int level;

    [SerializeField] private UnityEvent onInCompleteDialogue;
    [SerializeField] private UnityEvent onCompleteDialogue;

    private Animator mainInteractionAnimator;
    private int isActiveHash = Animator.StringToHash("IsActive");

    public void Awake()
    {
        textOption1 = lookInteraction_Option1.GetComponentInChildren<Text>(true);
        textOption2 = lookInteraction_Option2.GetComponentInChildren<Text>(true);

        mainInteractionAnimator = mainInteraction.GetComponent<Animator>();

        mainText = mainInteraction.GetComponentInChildren<Text>();

        //mainInteractionAnimator.SetBool(isActiveHash, false);
        //lookInteraction_Option1.RemoveSelections();
        //lookInteraction_Option2.RemoveSelections();

        mainText.text = mainDiscussion[level];
        textOption1.text = discussionOptions1[level];
        textOption2.text = discussionOptions2[level];

    }

    public void Start()
    {
        mainInteractionAnimator.SetBool(isActiveHash, true);
        mainInteractionAnimator.SetBool(isActiveHash, false);
        // lookInteraction_Option1.RemoveSelections();
        //lookInteraction_Option2.RemoveSelections();

        // enabled = false;
    }

    public void TurnOnDialogue()
    {
        if (level == 0)
        {
            mainText.text = mainDiscussion[level];
            textOption1.text = discussionOptions1[level];
            textOption2.text = discussionOptions2[level];

        }

        if (isCompleted)
            mainInteractionAnimator.SetBool(isActiveHash, true);
        else
        {

            mainInteractionAnimator.SetBool(isActiveHash, true);
            lookInteraction_Option1.TriggerSelections();
            lookInteraction_Option2.TriggerSelections();
        }


    }

    public void TurnOffDialogue()
    {





        mainInteractionAnimator.SetBool(isActiveHash, false);
        lookInteraction_Option1.RemoveSelections();
        lookInteraction_Option2.RemoveSelections();
        //level = 0;
        // isOptionsLeftOut = false;
    }

    private bool isOptionsLeftOut;
    IEnumerator ChangeOptions()
    {
        mainInteractionAnimator.SetBool(isActiveHash, false);

        lookInteraction_Option1.image.raycastTarget = false;
        lookInteraction_Option2.image.raycastTarget = false;

        lookInteraction_Option1.RemoveSelections();
        lookInteraction_Option2.RemoveSelections();
        yield return new WaitForSeconds(responseDelay);

        lookInteraction_Option1.image.raycastTarget = true;
        lookInteraction_Option2.image.raycastTarget = true;

        if (!isOptionsLeftOut)
        {

            textOption1.text = discussionOptions1[level];
            textOption2.text = discussionOptions2[level];

            lookInteraction_Option1.TriggerSelections();
            lookInteraction_Option2.TriggerSelections();

            mainInteractionAnimator.SetBool(isActiveHash, true);
            mainText.text = mainDiscussion[level];
        }
        else
        {
            if (!isCompleted)
            {
                mainText.text = wrongMessage;
                level = 0;

                onInCompleteDialogue.Invoke();

            }
            else
            {
                mainText.text = correctMessage;
                isOptionsLeftOut = true;
                onCompleteDialogue.Invoke();
            }

            mainInteractionAnimator.SetBool(isActiveHash, true);
            yield return new WaitForSeconds(responseDelay);

            //lookInteraction_Option1.image.raycastTarget = true;
            //lookInteraction_Option2.image.raycastTarget = true;

            mainInteractionAnimator.SetBool(isActiveHash, false);
            yield return new WaitForSeconds(responseDelay);


        }

    }
    public void CheckSelectedOptions(int value)
    {



        if (answerKey[level] == value)
        {
            ++level;

            if (answerKey.Length == level)
            {
                isOptionsLeftOut = true;
                isCompleted = true;

            }
            else
                isOptionsLeftOut = false;


        }
        else
        {
            isOptionsLeftOut = true;
            level = 0;
        }


        StartCoroutine(ChangeOptions());



    }




}

