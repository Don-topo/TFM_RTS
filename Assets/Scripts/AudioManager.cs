using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private static AudioSource audioSource;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private AudioMixer audioMixer;

    private static List<AudioClip> audioClips;
    private static AudioClip lastClipPlayed;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioListener = GetComponent<AudioListener>();
        audioMixer = GetComponent<AudioMixer>();
    }

    public static void SetAudioClips(List<AudioClip> newAudioClips)
    {
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

}
