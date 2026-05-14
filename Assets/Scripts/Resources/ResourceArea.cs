using UnityEngine;

public class ResourceArea : MonoBehaviour
{
    [Header("Mandatory Info")]
    [SerializeField] private ShowResourceAreaEvent showAreaEvent;
    [SerializeField] private ShowResourceAreaEvent hideAreaEvent;
    [field: SerializeField] public SO_Resource ResourceInArea { get; private set; }
    [SerializeField] private Transform areaPosition;
    [SerializeField] private GameObject areaGameObject;


    private void Awake()
    {
        showAreaEvent.Register(ShowArea);
        hideAreaEvent.Register(HideArea);
        HideArea(ResourceInArea);
    }

    private void Update()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        if (box != null && areaGameObject.activeSelf)
        {
            Vector3 size = box.size;

            areaPosition.localPosition = new Vector3(
                box.center.x,
                -box.size.y / 2f + 0.01f,
                box.center.z
            );

            areaPosition.localRotation = Quaternion.Euler(90, 0, 0);

            areaPosition.localScale = new Vector3(
                size.x,
                size.z,
                1
            );
        }
    }

    private void OnDestroy()
    {
        showAreaEvent.Unregister(ShowArea);
        hideAreaEvent.Unregister(HideArea);
    }

    private void ShowArea(SO_Resource resource)
    {
        if(this.ResourceInArea.ResourceTypes != resource.ResourceTypes) return;
        areaGameObject.SetActive(true);
    }

    private void HideArea(SO_Resource resource)
    {
        areaGameObject.SetActive(false);
    }
}
