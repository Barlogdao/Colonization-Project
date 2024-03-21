using UnityEngine;

public class Blueprint : MonoBehaviour
{
    [SerializeField] private Material _validMaterial;
    [SerializeField] private Material _unvalidMaterial;
    [SerializeField] private LayerMask _collisionLayer;

    private BuildingView _buildingView;

    public bool CanPlace { get; private set; }
    public bool IsActive { get; private set; } = false;

    public void Activate(BuildingView buildingView)
    {
        IsActive = true;

        _buildingView = Instantiate(buildingView, transform);

        ChangeChildLayers();
    }

    public void Deactivate()
    {
        IsActive = false;

        Destroy(_buildingView.gameObject);
    }

    public void Move(Vector3 position)
    {
        transform.position = position;
        PlacementCheck();
    }

    public void Rotate(float angle)
    {
        transform.Rotate(Vector3.up, angle);
    }

    private void PlacementCheck()
    {
        Bounds bounds = _buildingView.ColliderBounds;

        if (Physics.CheckBox(bounds.center, bounds.extents, transform.rotation, _collisionLayer) == true)
        {
            CanPlace = false;
            _buildingView.SetMaterial(_unvalidMaterial);
        }
        else
        {
            CanPlace = true;
            _buildingView.SetMaterial(_validMaterial);
        }
    }

    private void ChangeChildLayers()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = gameObject.layer;
        }
    }
}
