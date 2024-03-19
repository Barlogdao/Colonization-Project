using UnityEngine;
using UnityEngine.InputSystem;


public class BuildService : MonoBehaviour
{
    [SerializeField] private CommandCenter _prefab;
    [SerializeField] private Blueprint _blueprintPrefab;

    [SerializeField] private float _rotateSpeed;

    [SerializeField] private LayerMask _placementLayer;

    private Blueprint _bluePrint;

    public bool IsActive { get; private set; } = false;
    private System.Action<Vector3, Quaternion> _buildCallback;
    public void StartBuild(System.Action<Vector3, Quaternion> buildCallback)
    {
        _bluePrint = Instantiate(_blueprintPrefab);
        _bluePrint.Initialize(_prefab.BuildingView);
        _buildCallback = buildCallback;
        IsActive = true;
    }

    private void Update()
    {
        if (_bluePrint == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, _placementLayer) == true)
        {

            _bluePrint.Move(hitInfo.point);

            RotationHandle();
            _bluePrint.PlacementCheck();

            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RightClickHandle();
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LeftClickHandle();
        }
    }

    private void RotationHandle()
    {
        float mouseWheelInput = Mouse.current.scroll.y.value;
        _bluePrint.Rotate(mouseWheelInput * _rotateSpeed);
    }

    private void LeftClickHandle()
    {
        if (_bluePrint.CanPlace == true)
        {
            _buildCallback.Invoke(_bluePrint.transform.position, _bluePrint.transform.rotation);
            DestroyBlueprint();
        }
    }

    private void RightClickHandle()
    {
        DestroyBlueprint();
    }

    private void DestroyBlueprint()
    {
        _bluePrint.Remove();
        _buildCallback = null;
        _bluePrint = null;
        IsActive = false;
    }
}
