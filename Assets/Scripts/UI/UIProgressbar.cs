using UnityEngine;
using UnityEngine.UI;

public class UIProgressbar : MonoBehaviour
{
    [SerializeField][Range(0, 1)] private float progress;
    [SerializeField] private Image progressbarImage;

    private void Awake()
    {
        progress = 0;
    }

    private void Update()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {        
        progressbarImage.fillAmount = progress;
    }

    public void UpdateProgress(float amount)
    {
        progress = amount;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
