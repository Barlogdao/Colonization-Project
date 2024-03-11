using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cargoPlace;

    private CommandCenter _commandCenter;

    public void BindToCommandCenter(CommandCenter commandCenter)
    {
        _commandCenter = commandCenter;
    }

    public void HarvestResource(Resource resource)
    {
        StartCoroutine(HarvestRoutine(resource));
    }

    private IEnumerator HarvestRoutine(Resource resource)
    {
        Vector3 targetPosition = resource.transform.position;
        targetPosition.y = transform.position.y;

        yield return MoveToTarget(targetPosition);

        resource.Harvest(transform, _cargoPlace.position);

        targetPosition = _commandCenter.transform.position;
        targetPosition.y = transform.position.y;

        yield return MoveToTarget(targetPosition);

        _commandCenter.CollectResource(resource.Amount);
        _commandCenter.ReturnUnit(this);
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
