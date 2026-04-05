using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Light lightDay;
    // Day info
    [SerializeField] private int maxDays = 10;
    [SerializeField] private float dayDuration = 15f;

    private int currentDay = 0;
    private float currentDayTime = 0f;

    private void Update()
    {
        currentDayTime += Time.deltaTime / dayDuration;
        currentDayTime %= 1;

        float angle = currentDayTime * 360f;
        lightDay.transform.rotation = Quaternion.Euler(angle - 90f, 170f, 0f);
    }
}
