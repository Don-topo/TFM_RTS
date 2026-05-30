using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private new Camera camera;
    [SerializeField] private CameraConfig cameraConfig;
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
    public ShowResourceAreaEvent showResourceAreaEvent;
    public ShowResourceAreaEvent hideResourceAreaEvent;
    public UnitRecruitedEvent recruitedEvent;
    [Header("Construction Materials")]
    [field: SerializeField] public Material OkPlaceMaterial { get; private set; }
    [field: SerializeField] public Material KoPlaceMaterial { get; private set; }
    [Header("Audio")]
    [SerializeField] private AudioClip invalidAction;
    [Header("Cursor")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private GameObject moveIndicator;


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

    // Building
    private GameObject placeBuildingInstance;
    private Renderer renderers;

    private void Awake()
    {
        LoadConfig();
        selectUnitEvent.Register(SelectedUnit);
        deselectUnitEvent.Register(DeselectUnit);
        actionClicked.Register(ActionClicked);
        minimapClickEvent.Register(MinimapClicked);
        recruitedEvent.Register(AddUnit);
        zoom = camera.transform.localPosition.y;
        cameraStartPosition = cameraMovementTransform.transform.position;
        hideResourceAreaEvent.Raise(null);

    }

    private void Update()
    {
        zoom = camera.transform.localPosition.y;
        DragMouse();
        CameraZoom();
        BuildingPlacement();
        CameraMovement();
        RigthClick();
        ResetCameraPosition();
        FocusCameraOnSelectedUnit();
    }

    private void OnDestroy()
    {
        selectUnitEvent.Unregister(SelectedUnit);
        deselectUnitEvent.Unregister(DeselectUnit);
        actionClicked.Unregister(ActionClicked);
        minimapClickEvent.Unregister(MinimapClicked);
        recruitedEvent.Unregister(AddUnit);
    }

    private void LoadConfig()
    {

    }

    private void SelectedUnit(CommonActions action)
    {
        if (!selectedUnits.Contains(action))
        {
            selectedUnits.Add(action);
        }
    }

    private void DeselectUnit(CommonActions action)
    {
        selectedUnits.Remove(action);
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
        if (Keyboard.current.tabKey.wasReleasedThisFrame)
        {
            camera.transform.position = cameraStartPosition;
        }
    }

    private void RigthClick()
    {
        if (addedUnits.Any(unit => unit.CompareTag("Enemy"))) return;
        if (selectedUnits.Any(unit => ((CommonActions)unit).CompareTag("Enemy"))) return;

        if (selectedUnits.Count == 0 || EventSystem.current.IsPointerOverGameObject())
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
                
                foreach(IAction action in GetAvailableCommands(unit))
                {
                    if (action.CanExecute(actionInfo))
                    {
                        action.Execute(actionInfo);
                        if (action.IsSingleUnitAction) return;
                        break;
                    }
                }
            }
            ShowClickAnimation(hitInfo);
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
            ShowClickAnimation(hit);
        }
    }

    private void ShowClickAnimation(RaycastHit hit)
    {
        GameObject clickResponse = Instantiate(
                    moveIndicator,
                    hit.point + Vector3.up * 0.02f,
                    Quaternion.Euler(90, 0, 0)
            );
        clickResponse.GetComponent<MeshRenderer>().material.SetFloat("_StartTime", Time.time);
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
        if(!clickOnUI && selectedAction == null && !Keyboard.current.leftShiftKey.isPressed)
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
        if(selectedAction.CursorIcon != null)
        {
            Cursor.SetCursor(selectedAction.CursorIcon, Vector2.zero, CursorMode.Auto);
        }
        if (!selectedAction.UseClickToExecute)
        {
            ExecuteAction(new RaycastHit());
        }
        else if(actionClicked is BuildBuildingAction)
        {
            placeBuildingInstance = Instantiate(((BuildBuildingAction)actionClicked).PlaceBuilding);
            if(((BuildBuildingAction)actionClicked).BuildingToBuild.UnitPrefab.GetComponent<ProductionBuilding>() != null)
            {
                showResourceAreaEvent.Raise(((BuildBuildingAction)actionClicked).BuildingToBuild.UnitPrefab.GetComponent<ProductionBuilding>().Resource);
            }            
        }
    }

    private void MinimapClicked(MinimapEventInfo info)
    {
        if ((info.MouseButton == MouseButton.Left && selectedAction != null))
        {
            ExecuteAction(info.RaycastHit);
        }
       /* else if (info.MouseButton == MouseButton.Right)
        {
            if(selectedUnits.All(unit => unit is BaseUnit)){
                ExecuteAction(info.RaycastHit);
            }
        }*/
    }

    private void ExecuteAction(RaycastHit hit)
    {
        bool canExecuteAction = false;

        if(placeBuildingInstance != null)
        {
            Destroy(placeBuildingInstance);
            placeBuildingInstance = null;
            hideResourceAreaEvent.Raise(null);
        }

        List<CommonActions> actions = selectedUnits.Where(unit => unit is CommonActions).Cast<CommonActions>().ToList();
        if (actions.Count == 0) return;
        foreach (CommonActions action in actions)
        {
            ActionInfo actionInfo = new(action, hit, actions.IndexOf(action));
            if(selectedAction == null)
            {
                if(action.Actions.Where(act => act.Name == "Move").Count() != 0)
                {
                    selectedAction = action.Actions.Where(act => act.Name == "Move").First();
                }
                else
                {
                    break;
                }                
            }
            if (selectedAction.CanExecute(actionInfo))
            {
                selectedAction.Execute(actionInfo);
                canExecuteAction = true;
                if (selectedAction.IsSingleUnitAction)
                {
                    break;
                }
            }            
        }

        if (canExecuteAction)
        {

            PlaySound(selectedAction.ExecuteAudio);      
        }
        else
        {
            PlaySound(new List<AudioClip> { invalidAction });
        }

        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        actionExecuted.Raise(selectedAction);

        selectedAction = null;
    }

    private void PlaySound(List<AudioClip> audioClips)
    {
        if(audioClips.Count > 0)
        {
            AudioManager.SetAudioClips(audioClips);
            AudioManager.PlayAudio();
        }       
    }

    private void BuildingPlacement()
    {
        if (placeBuildingInstance == null) return;

        if (Keyboard.current.escapeKey.wasReleasedThisFrame)
        {
            Destroy(placeBuildingInstance);
            hideResourceAreaEvent.Raise(null);
            placeBuildingInstance = null;
            selectedAction = null;
            return;
        }

        Ray cameraRay = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(cameraRay, out RaycastHit hit, float.MaxValue, floorLayers))
        {
            placeBuildingInstance.transform.position = hit.point;
            placeBuildingInstance.GetComponentsInChildren<Renderer>().All(rend => 
                rend.material = selectedAction.CheckRestrictions(hit.point) ? OkPlaceMaterial : KoPlaceMaterial);            
        }
    }

    private void AddUnit(BaseUnit unit)
    {
        if(playerUnits.Contains(unit)) return;
        playerUnits.Add(unit);
    }

    private void FocusCameraOnSelectedUnit()
    {
        if(Keyboard.current.spaceKey.wasReleasedThisFrame && selectedUnits.Count > 0)
        {
            Vector3 unitPosition = ((CommonActions)selectedUnits[0]).transform.position;
            cameraMovementTransform.transform.position = new Vector3(
                unitPosition.x, 
                cameraMovementTransform.transform.position.y, 
                unitPosition.z - 18f
            );
        }
    }

    private List<BaseAction> GetAvailableCommands(CommonActions unit)
    {
        ReplaceActions[] overrideCommandsCommands = unit.Actions
            .Where(command => command is ReplaceActions)
            .Cast<ReplaceActions>()
            .ToArray();

        List<BaseAction> allAvailableCommands = new();
        foreach (ReplaceActions overrideCommand in overrideCommandsCommands)
        {
            allAvailableCommands.AddRange(overrideCommand.Commands
                .Where(command => command is not ReplaceActions)
            );
        }

        allAvailableCommands.AddRange(unit.Actions
            .Where(command => command is not ReplaceActions)
        );

        return allAvailableCommands;
    }
}
