using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHoopPoints : MonoBehaviour {

	[SerializeField] private int points;
	[SerializeField] private bool isPersist;

	[SerializeField] private int livesUntilDisable = 1;

    [SerializeField] private bool isMakeNoice = false;

    [SerializeField] private bool isUseMaterialFlikerOnDamage;
    private Material thisMaterial;
    private Color originalColor;

    private void Awake()
    {
        if (isUseMaterialFlikerOnDamage)
        {
            thisMaterial = GetComponentInChildren<MeshRenderer>().material;
            originalColor = thisMaterial.color;

        }
    }
    private void OnCollisionEnter(Collision other)
    {
       
        if (other.collider.CompareTag("Bullet"))
        {
            if (isPersist)
            {
                if (isMakeNoice)
                    AudioManager.Instance.PlayDirectSound("SmallWin");
                PlayerManager.Instance.points = points;
                other.gameObject.SetActive(false);

            }
            else
            {
                --livesUntilDisable;
                StartCoroutine(DamageFlicker());
                //other.GetComponent


                if (livesUntilDisable < 1)
                {
                    //Resources.Load<Particlest>
                    if(isMakeNoice)
                    AudioManager.Instance.PlayDirectSound("SmallWin");

                    StartCoroutine(GameController.Instance.HitEffectLocation(other.transform.position));
                    PlayerManager.Instance.points = points;
                    other.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
            }
        }

    }

    IEnumerator DamageFlicker()
    {
        for (int i = 0; i < 3; i++)
        {
            thisMaterial.color = Color.red;

            yield return new WaitForSeconds(0.1f) ;

            thisMaterial.color = originalColor;

            yield return new WaitForSeconds(0.1f);

        }
       


    }
    void OnTriggerEnter(Collider other){

		if(other.CompareTag("Bullet")){
		if (isPersist) {

                if (isMakeNoice)
                    AudioManager.Instance.PlayDirectSound("SmallWin");
				PlayerManager.Instance.points = points;
				other.gameObject.SetActive (false);

		} else {
			--livesUntilDisable;
			if (livesUntilDisable < 1) {

                    if (isMakeNoice)
                        AudioManager.Instance.PlayDirectSound("SmallWin");

                StartCoroutine(GameController.Instance.HitEffectLocation(other.transform.position));
                PlayerManager.Instance.points = points;
				other.gameObject.SetActive (false);
				gameObject.SetActive (false);
			}
		}
		}
	}
}
