using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusIcScript : MonoBehaviour
{
	public AudioSource music;
    public AudioClip clip1, clip2;
	public void Start()
	{
		music.Play();
	}
    void Update()
    {
        if (!music.isPlaying)
        {
			if (music.clip = clip1)
			{
				music.clip = clip2;
			}
			else
			{
				music.clip = clip1;
			}
            music.Play();
        }
    }
}
