using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyExplotion : MonoBehaviour
{
    private AudioSource audioSource;
    private SO_AttackInfo attackInfo;

    private void Awake()
    {
        Destroy(gameObject, 3f);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetInfo(SO_AttackInfo attackInfo)
    {
        this.attackInfo = attackInfo;
        Explode();
    }

    private void Explode() 
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Get nearby enemies
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackInfo.AttackRange * 2f);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag("Player") || enemyCollider.CompareTag("CommandPost"))
            {
                if (enemyCollider.TryGetComponent<IAttackable>(out var attackable))
                {
                    attackable.ApplyDamage(attackInfo.AttackDamage);
                }
            }
        }
    }
}
