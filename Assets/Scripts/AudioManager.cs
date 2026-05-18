using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource), typeof(AudioListener))]
public class AudioManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource backgroundMusicSource;
    [Header("AudioClips")]
    [SerializeField] private AudioClip mainBackgroundAudioClip;
    [SerializeField] private AudioClip hordeAudioClip;
    [SerializeField] private AudioClip winAudioClip;
    [SerializeField] private AudioClip defeatAudioClip;

    private static List<AudioClip> audioClips;
    private static AudioClip lastClipPlayed;
    private static AudioSource audioSource;
    private AudioListener audioListener;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioListener = GetComponent<AudioListener>();
        PlayBackgroundAudio();
    }

    public static void SetAudioClips(List<AudioClip> newAudioClips)
    {
        if(audioClips == newAudioClips) return;
        audioClips = newAudioClips;
    }

    public static void PlayAudio()
    {
        if(audioClips.Count > 0 && !audioSource.isPlaying)
        {            
            AudioClip clipToPlay = audioClips[Random.Range(0, audioClips.Count)];
            while (lastClipPlayed != null && clipToPlay.name.Equals(lastClipPlayed.name))
            {
                clipToPlay = audioClips[Random.Range(0, audioClips.Count)];
            }
                
            audioSource.PlayOneShot(clipToPlay);
        }
    }

    public static void CleanAudio()
    {
        audioSource.Stop();
        audioClips.Clear();
    }

    public void PlayDefeatAudio()
    {
        backgroundMusicSource.clip = defeatAudioClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayVictoryAudio()
    {
        backgroundMusicSource.clip = winAudioClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayHordeAudio()
    {
        backgroundMusicSource.clip = hordeAudioClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayBackgroundAudio()
    {
        backgroundMusicSource.clip = mainBackgroundAudioClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

}
