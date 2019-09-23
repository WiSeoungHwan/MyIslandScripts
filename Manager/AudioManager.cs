using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    
    public static AudioManager Instance
    {
        get { return instance; }
    }
    private static AudioManager instance = null;
    [SerializeField]
	private AudioSource sfxSouce = null;
    [SerializeField]
	private AudioSource bgmSouce = null;

    private string[] bgmList = {"happy_pirate_accordion"};
    private string[] sfxList = {"button"};
    
    // private bool mute = false;
    protected void Awake()
    {
		if(instance == null)
		{
            instance = this;
		}
        else
        {
			DestroyImmediate(gameObject);
        }
    }
    public void PlaySfx(SFX _sfxType)
    {
        int sfxNumber = (int)_sfxType;

        AudioClip audioClip = Resources.Load("Sfx/" + this.sfxList[sfxNumber], typeof(AudioClip)) as AudioClip;
        this.sfxSouce.clip = audioClip;
        this.sfxSouce.PlayOneShot(audioClip);
    }

    public void PlayBgm(BGM _bgmNumber)
	{
        int bgmNumber = (int)_bgmNumber;
        AudioClip audioClip = Resources.Load("Bgm/" + this.bgmList[bgmNumber]) as AudioClip;
        this.bgmSouce.clip = audioClip;
        this.bgmSouce.Play();
	}
	private void stopSound(EVENT_TYPE eventType, Component sender, object value = null)
	{
	}

    private void onMute(EVENT_TYPE eventType, Component sender, object value = null)
    {
        // this.mute = !this.mute;
        // AudioListener.volume = this.mute ? 0 : 1;
    }
}
