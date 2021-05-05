using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimAndTurnOnLights_Lerp : MonoBehaviour
{
    public Light light;
    public float lerpMagnitude = 0.1f;
    

    private void Start()=> light = GetComponent<Light>();

    public void ToogleLights(bool isOn) => StartCoroutine(ToogleLightsEnum(isOn));

    public IEnumerator ToogleLightsEnum(bool isOn) {

    float amountUntilOne = 0;
  //  float amountUntilZero = 1;



        if (isOn)
            while (light.intensity < 0.98f) { yield return null; amountUntilOne += Time.deltaTime * lerpMagnitude;  light.intensity = Mathf.Lerp(0, 1, amountUntilOne); }
        else
            while (light.intensity > 0.02f) { yield return null; amountUntilOne += Time.deltaTime * lerpMagnitude; light.intensity = Mathf.Lerp(1, 0, amountUntilOne); }
    }

}
