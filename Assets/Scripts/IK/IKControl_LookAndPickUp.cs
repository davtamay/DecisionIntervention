using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class IKControl_LookAndPickUp : MonoBehaviour
{

    protected Animator animator;
    protected Transform thisTransform;

    public bool ikActive = false;

    [Header("Angle And Pos Reference For IK")]
    public Transform leftHandDirAndPosIKRef = null;

    //[Header("Looking References")]
    //public Transform defaultLookObject = null;
    //public Transform lookObj = null;

    [Header("Weight_IK_Limits")]
    [SerializeField] private bool isIkHeadControlOnly = false;
    [SerializeField] private Transform boneDirRef;
    [SerializeField] private Transform target;
    [SerializeField] Transform setObjectToKnowFowardDir;
    Vector3 forwardDir;
    [SerializeField] [Range(-1, 1)] private float angleDOT;

    [SerializeField] private float dampTime;
    //[SerializeField] private float maxRadiansDelta;
    [SerializeField] private Vector3 additionalRotation;
    [SerializeField] private float weight;

    private Vector3 lookDirection;
    private Vector3 finalLookVector;
    private Quaternion rotation;


    private bool isInRuntime = false;

    private void Awake()
    {
        isInRuntime = true;   
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        forwardDir = (forwardDir + setObjectToKnowFowardDir.forward) + Quaternion.Euler(additionalRotation).eulerAngles;


    }

    public void ToggleIK(bool isActive = true)
    {
        if(isInRuntime)
        ikActive = isActive;
    }
    public void ToggleIK_Active()
    {
        if (isInRuntime)
            ikActive = true;
    }

    public void Toggle_Active_HeadOnly_IK()
    {
        if (isInRuntime)
            isIkHeadControlOnly = true;
        //ikActive = true;
    }
    public void Toggle_InActive_HeadOnly_IK()
    {
        if (isInRuntime)
            isIkHeadControlOnly = false;
        //ikActive = true;
    }

    void OnAnimatorIK()
    {
        
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                Vector3 dampVelocity = Vector3.zero;
                lookDirection = Vector3.SmoothDamp(lookDirection, target.position - boneDirRef.position, ref dampVelocity, dampTime);

       
                if (Vector3.Dot(forwardDir.normalized, lookDirection.normalized) < angleDOT)
                    weight = Mathf.Clamp01(Mathf.Lerp(weight, 0f, Time.unscaledDeltaTime * 2));
                else
                    weight = Mathf.Lerp(weight, 1f, Time.unscaledDeltaTime * 2);


                if (!isIkHeadControlOnly || animator.GetIKPositionWeight(AvatarIKGoal.LeftHand) >= 0.05f)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, target.position);

                    
                }

                animator.SetLookAtWeight(weight);
                animator.SetLookAtPosition(target.position);

                //if (!isLooking)
                //    return;

                //if (finalLookVector != Vector3.zero)
                //{
                //    rotation = Quaternion.LookRotation(finalLookVector) * Quaternion.Euler(additionalRotation);
                //    headBone.rotation = Quaternion.Lerp(headBone.rotation, rotation, weight);
                //}


                //if (angleDOT < 0.2f)
                //{
                //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                //    //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, animator.GetFloat("Grab"));//1);
                //    //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, animator.GetFloat("Grab"));//1);
                //    //animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandDirAndPosIKRef.position);
                //    //animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandDirAndPosIKRef.rotation);
                //}
                //if (angleDOT > 0.2f)
                //{

                //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

                //    //animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, animator.GetFloat("Grab"));//1);
                //    animator.SetIKPosition(AvatarIKGoal.LeftHand, defaultLookObject.position);

                //    animator.SetLookAtWeight(1);
                //    animator.SetLookAtPosition(defaultLookObject.position);

                //    // animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandDirAndPosIKRef.rotation);



                //}



                // }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            //else
            //{
            //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            //    //animator.SetLookAtWeight(animator.GetFloat("Grab"));

            //    //if (animator.GetFloat("Grab") <= tresholdToStartParenting)
            //    //    leftHandDirAndPosIKRef.parent = originalParent;
            //}
        }
    }


}
