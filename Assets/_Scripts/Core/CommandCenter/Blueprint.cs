using UnityEngine;

public class Blueprint : MonoBehaviour
{
    [SerializeField] private Material _validMaterial;
    [SerializeField] private Material _unvalidMaterial;

    [SerializeField] private LayerMask _collisionLayer;

    private BuildingView _buildingView;
    private Vector3 _colliderOffset;
    private float _colliderRadius;

    public bool CanPlace { get; private set; }
    public bool IsActive { get; private set; } = false;

    public void Activate(BuildingView buildingView)
    {
        IsActive = true;

        _buildingView = Instantiate(buildingView, transform);
        _colliderOffset = _buildingView.SphereCollider.center;
        _colliderRadius = _buildingView.SphereCollider.radius;

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
    }

    public void Rotate(float angle)
    {
        transform.Rotate(Vector3.up, angle);
    }

    public void PlacementCheck()
    {
        if (Physics.CheckSphere(transform.position + _colliderOffset, _colliderRadius, _collisionLayer) == true)
        {
            CanPlace = false;
            _buildingView.ChangeMaterials(_unvalidMaterial);
           
        }
        else
        {
            CanPlace = true;
            _buildingView.ChangeMaterials(_validMaterial);
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
