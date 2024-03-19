using UnityEngine;
using UnityEngine.InputSystem;

public class BuildService : MonoBehaviour
{
    [SerializeField] private CommandCenter _prefab;
    [SerializeField] private Transform _view;


    [SerializeField] private LayerMask _collisionLayers;

    [SerializeField] private Material _validMaterial;
    [SerializeField] private Material _unvalidMaterial;

    private Transform _currentObject;
    private Material[] _currentObkectMaterials;

    public bool IsActive { get; private set; } = false;

    public void StartBuild()
    {
        _currentObject = Instantiate(_view);
        IsActive = true;
    }

    private void Update()
    {
        if (_currentObject == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1001f, _collisionLayers) == true)
        {
            _currentObject.transform.position = hitInfo.point;


            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                RightClickHandle();
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                LeftClickHandle(hitInfo.point);
            }
        }
    }

    private void LeftClickHandle(Vector3 position)
    {
        Instantiate(_prefab, position, Quaternion.identity);
        DestroyTemplate();
    }

    private void RightClickHandle()
    {
        
        DestroyTemplate();
    }

    private void DestroyTemplate()
    {
        Destroy(_currentObject.gameObject);
        _currentObject = null;
        IsActive = false;
    }
}
