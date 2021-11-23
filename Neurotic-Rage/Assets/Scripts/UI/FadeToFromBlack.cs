using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToFromBlack : MonoBehaviour
{
    public Image fadeOutImage;

    public void FadeToBlack(float fadeSpeed)
    {
        StartCoroutine(IEFadeToBlack(true, fadeSpeed));
    }

    public void FadeFromBlack(float fadeSpeed)
    {
        StartCoroutine(IEFadeToBlack(false, fadeSpeed));
    }

    public IEnumerator IEFadeToBlack(bool fadeOutToBlack, float fadeSpeed)
    {
        if (fadeOutToBlack)
        {
            if(fadeOutImage.gameObject.activeSelf == false)
            {
                fadeOutImage.gameObject.SetActive(true);
            }
            while (fadeOutImage.color.a < 1)
            {
                fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, (fadeOutImage.color.a + (fadeSpeed * Time.deltaTime)));
                yield return null;
            }
        }
        else
        {
            while (fadeOutImage.color.a > 0)
            {
                fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, (fadeOutImage.color.a - (fadeSpeed * Time.deltaTime)));
                if(fadeOutImage.color.a <= 0)
                {
                    fadeOutImage.gameObject.SetActive(false);
                }
                yield return null;
            }
        }
    }
}
