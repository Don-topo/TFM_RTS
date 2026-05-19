using System.Collections.Generic;
using Unity.VisualScripting;
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
    [Header("Events")]
    [SerializeField] private VictoryEvent victoryEvent;
    [SerializeField] private GameOverEvent gameOverEvent;

    private static List<AudioClip> audioClips;
    private static AudioClip lastClipPlayed;
    private static AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        victoryEvent.Register(PlayVictoryAudio);
        gameOverEvent.Register(PlayDefeatAudio);
        PlayBackgroundAudio(null);
    }

    private void OnDestroy()
    {
        victoryEvent.Unregister(PlayVictoryAudio);
        gameOverEvent.Unregister(PlayDefeatAudio);
    }

    public static void SetAudioClips(List<AudioClip> newAudioClips)
    {
        if(audioClips == newAudioClips) return;
        audioSource.Stop();
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

    public void PlayDefeatAudio(Null @null)
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = defeatAudioClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayVictoryAudio(Null @null)
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = winAudioClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayHordeAudio(Null @null)
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = hordeAudioClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayBackgroundAudio(Null @null)
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = mainBackgroundAudioClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

}
