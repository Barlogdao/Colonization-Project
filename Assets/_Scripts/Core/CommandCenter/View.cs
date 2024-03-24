using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class View : MonoBehaviour
{
    private Collider _collider;
    private int _collisionAmount = 0;
    public bool IsCollide => _collisionAmount > 0;

    protected void Awake()
    {
        _collider = GetComponent<Collider>();

        _collider.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        _collisionAmount++;
    }

    private void OnTriggerExit(Collider other)
    {
        _collisionAmount--;
    }
}