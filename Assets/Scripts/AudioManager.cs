using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;


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
    [SerializeField] private StartWaveEvent startWaveEvent;
    [SerializeField] private FinishWaveEvent finishWaveEvent;

    private static List<AudioClip> audioClips;
    private static AudioClip lastClipPlayed;
    private static AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        victoryEvent.Register(PlayVictoryAudio);
        gameOverEvent.Register(PlayDefeatAudio);
        startWaveEvent.Register(PlayHordeAudio);
        finishWaveEvent.Register(PlayBackgroundAudio);
        PlayBackgroundAudio(null);
    }

    private void OnDestroy()
    {
        victoryEvent.Unregister(PlayVictoryAudio);
        gameOverEvent.Unregister(PlayDefeatAudio);
        startWaveEvent.Unregister(PlayHordeAudio);
        finishWaveEvent.Unregister(PlayBackgroundAudio);
    }

    public static void SetAudioClips(List<AudioClip> newAudioClips)
    {
        audioSource.Stop();
        if (audioClips == newAudioClips) return;        
        audioClips = newAudioClips;
    }

    public static void PlayAudio()
    {
        if(audioClips.Count > 0 && !audioSource.isPlaying)
        {                       
            AudioClip clipToPlay = audioClips[Random.Range(0, audioClips.Count)];
            if(audioClips.Count > 1)
            {
                while (lastClipPlayed != null && clipToPlay.name.Equals(lastClipPlayed.name))
                {
                    clipToPlay = audioClips[Random.Range(0, audioClips.Count)];
                }
            }
                            
            audioSource.PlayOneShot(clipToPlay);
            lastClipPlayed = clipToPlay;
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

    public void PlayHordeAudio(int wave)
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

    public void ShowInfo()
    {
        Debug.Log("IsPlaying: " + backgroundMusicSource.isPlaying);
        Debug.Log("Volume: " + backgroundMusicSource.volume);
        Debug.Log("Mute: " + backgroundMusicSource.mute);
        Debug.Log("Clip: " + backgroundMusicSource.clip.name);
    }
}
