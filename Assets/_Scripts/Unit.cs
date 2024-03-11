using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cargoPlace;

    private Base _base;

    public void Initialize(Base @base)
    {
        _base = @base;
    }

    public void HarvestResource(Resource resource)
    {
        StartCoroutine(HarvestRoutine(resource));
    }

    private IEnumerator HarvestRoutine(Resource resource)
    {
        Vector3 targetPosition = resource.transform.position;

        yield return MoveToTarget(targetPosition);

        resource.Harvest(transform, _cargoPlace.position);

        targetPosition = _base.transform.position;

        yield return MoveToTarget(targetPosition);

        _base.CollectResource(resource.Amount);
        _base.ReturnUnit(this);
        resource.Unload();
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            yield return null;
        }
    }

}