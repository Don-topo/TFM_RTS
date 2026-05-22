using UnityEngine;

public class EnemyController : BaseAttacker
{
    [Header("Death Components")]
    [SerializeField] private bool explodes;
    [SerializeField] private GameObject deathExplotion;

    [SerializeField] private GameObject target;

    protected override void Start()
    {
        base.Start();
        if(target != null )
        {
            Attack(target.transform.position);
        }        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(explodes && deathExplotion != null)
        {
            Instantiate(deathExplotion, transform);
        }
    }
}
