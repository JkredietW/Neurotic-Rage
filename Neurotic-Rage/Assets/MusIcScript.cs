using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusIcScript : MonoBehaviour
{
	public AudioSource music;
	public AudioClip[] clips;
	private AudioClip lastclip;
	public void Start()
	{
		int random = Random.Range(0, clips.Length);
		music.clip = clips[random];
		lastclip = music.clip;
		music.Play();
	}
    void Update()
    {
        if (!music.isPlaying)
        {
			int random = Random.Range(0, clips.Length);
			music.clip = clips[random];
			music.Play();
        }
    }
	public AudioClip GetClip()
	{
		int random = Random.Range(0, clips.Length);
		if (clips[random] != lastclip)
		{ 
			return clips[random];
		}
		else
		{
			GetClip();
			return clips[0];
		}
	}
}
