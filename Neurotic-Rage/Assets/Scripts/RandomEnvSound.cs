using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnvSound : MonoBehaviour
{
    public AudioSource audoClip;
    public float delayTime;
    [Header("Randomize")]
    public bool randomizeDelay;
    public float min, max;
    private float randomDelay;
    private bool isOnCooldown;
    void Start()
    {
        randomDelay = Random.Range(min, max);
    }
    void Update()
    {
		if (!audoClip.isPlaying)
		{
			if (!isOnCooldown)
			{
                StartCoroutine(SetDelay());
			}
		}
    }
    IEnumerator SetDelay()
	{
        isOnCooldown = true;

        if (randomizeDelay)
		{
            yield return new WaitForSeconds(randomDelay);
        }
		else
		{
            yield return new WaitForSeconds(delayTime);
		}
        audoClip.Play();
	}
}