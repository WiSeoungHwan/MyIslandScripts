using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : DontDestroy<AudioManager>
{

    [SerializeField]
	private AudioSource sfxSouce = null;
    [SerializeField]
	private AudioSource bgmSouce = null;

    [SerializeField]
    private AudioClip[] audioClip;
    private Dictionary<string, AudioClip> audioClipsDic;
    
    private bool mute = false;


    #region MonoBehaviour
    protected override void OnAwake(){
        audioClipsDic = new Dictionary<string, AudioClip>();
        foreach(AudioClip a in audioClip){
            audioClipsDic.Add(a.name,a);
        }
    }
    #endregion
    public void PlaySfx(string audioName,float volume = 1f)
    {
        if (audioClipsDic.ContainsKey(audioName) == false)
        {
            Debug.Log(audioName + " is not Contained audioClipsDic");
            return;
        }
        this.sfxSouce.volume = volume;
        this.sfxSouce.PlayOneShot(audioClipsDic[audioName], volume);
    }
    public void PlaySfx(AudioClip audioClip, float volume = 0.8f){
        if(audioClip == false){
            Debug.Log("Sfx is null");
            return;
        }
        sfxSouce.volume = volume;
        sfxSouce.PlayOneShot(audioClip,volume);
    }


    public void PlayBgm(string audioName, float volume = 0.3f)
	{
        if (audioClipsDic.ContainsKey(audioName) == false)
        {
            Debug.Log(audioName + " is not Contained audioClipsDic");
            return;
        }
        this.bgmSouce.clip = audioClipsDic[audioName];
        this.bgmSouce.volume = volume;
        this.bgmSouce.loop = true;
        this.bgmSouce.Play();
	}
	public void StopBGM()
	{
        this.bgmSouce.Stop();
	}

    private void OnMute()
    {
        this.mute = !this.mute;
        AudioListener.volume = this.mute ? 0 : 1;
    }
}
