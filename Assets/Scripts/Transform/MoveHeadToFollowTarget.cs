using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHeadToFollowTarget : MonoBehaviour
{
    Transform thisTransform;
    Animator thisAnimator;
    [Header("Head and Target References")]
    [SerializeField] private Transform headBone;
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] bool isOnUnscaledTime = false;
    [SerializeField] Transform setObjectToKnowFowardDir;
    [SerializeField] private Vector3 additionalRotation;
    [SerializeField] private float weight;
    [SerializeField] private float dampTime;
    [SerializeField] private float maxRadiansDelta;

    [SerializeField][Range(-1, 1)]private float angleDOT;

    [Header("DEBUG")]
    [SerializeField] private bool isLooking;
    [SerializeField] private bool isInsideTrigger;

    void Start()
    {
        thisTransform = transform;
        thisAnimator = GetComponent<Animator>();

        forwardDir = setObjectToKnowFowardDir.forward;

        if (isOnUnscaledTime)
            StartCoroutine(RotateHeadToTargetCoroutine());
    }

    private Vector3 lookDirection;
    private Vector3 finalLookVector;
    private Quaternion rotation;

    Vector3 forwardDir;


    //int NormalizeEulerRotationToNegAndPos1(int number)
    //{
    //    return (number + 1 / 1 + 1) ;

    //}

    //LATE UPDATE IF DOING ANIMATIONS
    //void LateUpdate()

    IEnumerator RotateHeadToTargetCoroutine()
    {

        while (isInsideTrigger)
        {
            RotateHeadToTarget();
            yield return null;
        }
    }

    void RotateHeadToTarget()
    {

        if (weight <= 0f)
            return;

        forwardDir = setObjectToKnowFowardDir.forward;


        Vector3 dampVelocity = Vector3.zero;

        lookDirection = Vector3.SmoothDamp(lookDirection, target.position - headBone.position, ref dampVelocity, dampTime);



        if (Vector3.Dot(forwardDir.normalized, lookDirection.normalized) < angleDOT)
        {
            finalLookVector = Vector3.RotateTowards(thisTransform.forward, lookDirection, Mathf.Deg2Rad * maxRadiansDelta, 0.5f);
            // thisAnimator.SetBool("IsLooking", false);
            isLooking = false;
        }
        else
        {
            finalLookVector = lookDirection;
            //   thisAnimator.SetBool("IsLooking", true);
            isLooking = true;
        }

        if (!isLooking)
            return;

        if (finalLookVector != Vector3.zero)
        {
            rotation = Quaternion.LookRotation(finalLookVector) * Quaternion.Euler(additionalRotation);
            headBone.rotation = Quaternion.Lerp(headBone.rotation, rotation, weight);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        isInsideTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInsideTrigger = false;
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    if (isOnUnscaledTime)
    //    {
    //      //  RotateHeadToTarget();
    //        thisAnimator.SetLookAtWeight(1);
    //        thisAnimator.SetLookAtPosition(target.position);

    //    }
       
    //}
    void OnTriggerStay()
    {
        if (!isOnUnscaledTime)
            RotateHeadToTarget();
    }
}