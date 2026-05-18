using UnityEngine;

public class Fade : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 2.1f);
    }
}
