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
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
       }
    }

    public static void CleanAudio()
    {
        audioSource.Stop();
        audioClips.Clear();
    }

}
