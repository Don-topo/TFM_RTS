using System.Collections;
using UnityEngine;

public class UIBuildSelectedBuilding : MonoBehaviour
{
    [SerializeField] private UIProgressbar progressBar;
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    private IEnumerator UpdateProgressBar(BaseBuilding baseBuilding)
    {
        //progressBar.UpdateProgress();
        yield return null;
    }
}
