using System.Collections;
using UnityEngine;
using RB.Extensions.Vector;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cargo;

    public void HarvestResource(CommandCenter commandCenter, Resource resource)
    {
        StartCoroutine(HarvestRoutine(commandCenter, resource));
    }

    private IEnumerator HarvestRoutine(CommandCenter commandCenter, Resource resource)
    {
        Vector3 targetPosition = resource.transform.position.WithY(transform.position.y);

        yield return MoveToTarget(targetPosition);

        resource.Harvest(transform, _cargo.position);

        targetPosition = commandCenter.transform.position.WithY(transform.position.y);

        yield return MoveToTarget(targetPosition);

        UnloadCargo(commandCenter,resource);
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        transform.LookAt(targetPosition);

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            yield return null;
        }
    }

    private void UnloadCargo(CommandCenter commandCenter, Resource resource)
    {
        commandCenter.AcceptResource(resource);
        commandCenter.BindUnit(this);
    }
}
