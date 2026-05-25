using UnityEngine;

public class Shoot : MonoBehaviour
{
    public ParticleSystem shootParticles;
    public AudioSource audioSource;

    public void ShootParticles()
    {
        if(shootParticles != null)
        {
            shootParticles.Play();
        }
        if(audioSource != null)
        {
            audioSource.Play();
        }        
    }
}
