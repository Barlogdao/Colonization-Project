using System.Collections;
using UnityEngine;
using RB.Extensions.Vector;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cargo;

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
        Vector3 targetPosition = resource.transform.position.WithY(transform.position.y);

        yield return MoveToTarget(targetPosition);

        resource.Harvest(transform, _cargo.position);

        targetPosition = _commandCenter.transform.position.WithY(transform.position.y);

        yield return MoveToTarget(targetPosition);

        UnloadCargo(resource);
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            yield return null;
        }
    }

    private void UnloadCargo(Resource resource)
    {
        _commandCenter.AddResource(resource);
        _commandCenter.ReturnUnit(this);
        resource.Unload();
    }
}
