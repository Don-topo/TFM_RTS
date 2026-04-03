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
        UpdateProgress(progress);
    }

    public void UpdateProgress(float progress)
    {
        progressbarImage.fillAmount = progress;
    }
}
