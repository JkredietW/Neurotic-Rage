using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalFade : MonoBehaviour
{
    Renderer fadeShader;
    float value;

    private void Start()
    {
        fadeShader = GetComponent<Renderer>();
        StartCoroutine(FadeAway());
    }
    IEnumerator FadeAway()
    {
        fadeShader.material.SetFloat("FadeValue", value);
        yield return new WaitForSeconds(0.1f);
        value += 0.1f;
        StartCoroutine(FadeAway());
    }
}
