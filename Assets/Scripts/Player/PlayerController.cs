using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private new Camera camera;
    [SerializeField] private CameraConfig cameraConfig;
    [SerializeField] private Transform targetTransform;
    [Header("Layers")]
    [SerializeField] private LayerMask selectableUnitsLayers;
    [SerializeField] private LayerMask floorLayers;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Transform cameraMovementTransform;

    // Events
    [Header("Events")]
    public UnitSelectedEvent selectUnitEvent;
    public UnitDeselectEvent deselectUnitEvent;
    public ActionExecuted actionExecuted;
    public ActionClicked actionClicked;
    public MinimapClickEvent minimapClickEvent;

    public float edgeSize = 50f;

    private Vector3 cameraStartPosition;

    // Unit Management
    private List<ISelectable> selectedUnits = new List<ISelectable>(12);
    private Vector2 startingMousePosition;
    public List<CommonActions> playerUnits = new List<CommonActions>(24);
    private List<CommonActions> addedUnits = new(24);
    private float zoom;
    private bool clickOnUI = false;

    // Actions
    private BaseAction selectedAction;

    private void Awake()
    {
        selectUnitEvent.Register(SelectedUnit);
        deselectUnitEvent.Register(DeselectUnit);
        //actionExecuted.Register(ExecuteAction);
        actionClicked.Register(ActionClicked);
        minimapClickEvent.Register(MinimapClicked);
        zoom = camera.transform.localPosition.y;
        cameraStartPosition = camera.transform.position;
    }

    private void Update()
    {
        zoom = camera.transform.localPosition.y;
        DragMouse();
        CameraZoom();
        CameraMovement();
        RigthClick();
        ResetCameraPosition();
    }

    private void OnDestroy()
    {
        selectUnitEvent.Unregister(SelectedUnit);
        deselectUnitEvent.Unregister(DeselectUnit);
        actionClicked.Unregister(ActionClicked);
        minimapClickEvent.Unregister(MinimapClicked);
    }

    private void SelectedUnit(CommonActions action)
    {
        if (!selectedUnits.Contains(action))
        {
            selectedUnits.Add(action);
        }
        Debug.Log("Selected Unit");
    }

    private void DeselectUnit(CommonActions action)
    {
        selectedUnits.Remove(action);
        Debug.Log("Deselected Unit");
    }

    private void CameraMovement()
    {

        Vector3 dir = Vector3.zero;

        // Keyboard
        if (Keyboard.current.upArrowKey.isPressed)
        {
            dir += cameraMovementTransform.transform.forward;
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            dir -= cameraMovementTransform.transform.forward;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            dir += cameraMovementTransform.transform.right;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            dir -= cameraMovementTransform.transform.right;
        }

        // Mouse
        if (Mouse.current.position.x.value >= Screen.width - edgeSize)
            dir += cameraMovementTransform.transform.right;
        if (Mouse.current.position.x.value <= edgeSize)
            dir -= cameraMovementTransform.transform.right;
        if (Mouse.current.position.y.value >= Screen.height - edgeSize)
            dir += cameraMovementTransform.transform.forward;
        if (Mouse.current.position.y.value <= edgeSize)
            dir -= cameraMovementTransform.transform.forward;

        // Map edges
        /*Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, mapLimitX.x, mapLimitX.y);
        pos.z = Mathf.Clamp(pos.z, mapLimitZ.x, mapLimitZ.y);*/

        // Apply movement
        cameraMovementTransform.transform.position += dir * cameraConfig.MoveSpeed * Time.deltaTime;
    }

    private void CameraZoom()
    {
        // Check mouse magnitude => wheel is moving
        if(Mouse.current.scroll.magnitude > 0)
        {
            zoom = camera.transform.localPosition.y;
            // Scroll.value => -1 or 1
            float scroll = Mouse.current.scroll.value.y;
            // Need to be framerate indepenendent
            zoom -= scroll * cameraConfig.ZoomSpeed * Time.deltaTime;
            zoom = Mathf.Clamp(zoom, cameraConfig.MinZoom, cameraConfig.MaxZoom);

            // Only update coordinates y and z
            Vector3 pos = camera.transform.localPosition;
            pos.y = zoom;
            pos.z = -zoom;

            // Update camera position
            camera.transform.localPosition = pos;

        }
    }

    private void ResetCameraPosition()
    {
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            camera.transform.position = cameraStartPosition;
        }
    }

    private void RigthClick()
    {
        if(selectedUnits.Count == 0 || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        // IssueRightClick
        if(Mouse.current.rightButton.wasReleasedThisFrame 
            && Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, floorLayers | interactableLayers))
        {
            List<BaseUnit> baseUnits = new List<BaseUnit>(selectedUnits.Count);
            foreach(ISelectable selectable in selectedUnits)
            {
                if(selectable is BaseUnit)
                {
                    baseUnits.Add((BaseUnit)selectable);
                }
            }

            foreach(BaseUnit unit in baseUnits)
            {
                ActionInfo actionInfo = new ActionInfo(unit, hitInfo, baseUnits.IndexOf(unit));
                
                foreach(IAction action in unit.Actions)
                {
                    if (action.CanExecute(actionInfo))
                    {
                        action.Execute(actionInfo);
                        if (action.IsSingleUnitCommand) return;
                        break;
                    }
                }
            }
        }
    }

    private void LeftClick()
    {
        if (camera == null) return;

        Ray cameraRay = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (addedUnits.Count == 0
            && Physics.Raycast(cameraRay, out RaycastHit hit, float.MaxValue, selectableUnitsLayers)
            && hit.collider.TryGetComponent(out ISelectable selectable)
            && selectedAction == null)
        {
            selectable.Select();
        }
        else if(selectedAction != null
            && !EventSystem.current.IsPointerOverGameObject()
            && Physics.Raycast(cameraRay, out hit, float.MaxValue, interactableLayers | floorLayers))
        {
            ExecuteAction(hit);
        }
    }

    private void DeselectUnits()
    {
        foreach (var unit in selectedUnits)
        {
            unit.Deselect();
        }
        selectedUnits.Clear();
    }

    private void DragMouse()
    {
        // Avoid error if selection box is not provided
        if (selectionBox == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Start of the possible drag.
            MouseDown();
        }
        else if (Mouse.current.leftButton.isPressed && !Mouse.current.leftButton.wasPressedThisFrame)
        {
            // We are in the drag and drop. Need to show and resize the selection box
            Drag();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            // Finish of drag and drop. Select units and hide selection box
            MouseUp();
        }
    }

    private void MouseDown()
    {
        selectionBox.sizeDelta = Vector2.zero;
        selectionBox.gameObject.SetActive(true);
        startingMousePosition = Mouse.current.position.ReadValue();
        addedUnits.Clear();
        clickOnUI = EventSystem.current.IsPointerOverGameObject();
    }

    private void Drag()
    {
        if (selectedAction != null || clickOnUI) return;
        Bounds selectionBoxBounds = ResizeSelectionBox();
        foreach (CommonActions unit in playerUnits)
        {
            if (!unit.gameObject.activeInHierarchy) continue;

            Vector2 unitPosition = camera.WorldToScreenPoint(unit.transform.position);
            if (selectionBoxBounds.Contains(unitPosition))
            {
                if (!addedUnits.Contains(unit))
                {
                    addedUnits.Add(unit);
                }                
            }
        }
    }

    private void MouseUp()
    {
        if(!clickOnUI && selectedAction == null)
        {
            // Deselect all units
            ISelectable[] test = selectedUnits.ToArray();
            foreach (ISelectable item in test)
            {
                item.Deselect();
            }
        }
        
        LeftClick();
        // Select all units inside de square
        foreach (ISelectable item in addedUnits)
        {     
            item.Select();
        }
        // Hide selection box
        selectionBox.gameObject.SetActive(false);
    }

    private Bounds ResizeSelectionBox()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        float width = mousePosition.x - startingMousePosition.x;
        float heigth = mousePosition.y - startingMousePosition.y;

        selectionBox.anchoredPosition = startingMousePosition + new Vector2(width / 2, heigth / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(heigth));

        return new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);
    }

    private void ActionClicked(BaseAction actionClicked)
    {
        selectedAction = actionClicked;
        if (!selectedAction.UseClickToExecute)
        {
            ExecuteAction(new RaycastHit());
        }
    }

    private void MinimapClicked(MinimapEventInfo info)
    {
        if (info.MouseButton == MouseButton.Right)
        {
            ExecuteAction(info.RaycastHit);
        }
        else if (info.MouseButton == MouseButton.Left)
        {
            ExecuteAction(info.RaycastHit);
        }
    }

    private void ExecuteAction(RaycastHit hit)
    {
        List<CommonActions> actions = selectedUnits.Where(unit => unit is CommonActions).Cast<CommonActions>().ToList();
        if (actions.Count == 0 || selectedAction == null) return;
        foreach (CommonActions action in actions)
        {
            ActionInfo actionInfo = new(action, hit, actions.IndexOf(action));
            if (selectedAction.CanExecute(actionInfo))
            {
                selectedAction.Execute(actionInfo);
                if (selectedAction.IsSingleUnitCommand)
                {
                    break;
                }
            }
        }
        Debug.Log("Execute action");
        actionExecuted.Raise(selectedAction);

        selectedAction = null;
    }
}
