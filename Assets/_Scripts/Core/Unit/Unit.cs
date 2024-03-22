using System;
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

    public void BuildConstruction(Flag flag, Action<Unit> onReachFlag)
    {
        StartCoroutine(BuildRoutine(flag, onReachFlag));
    }

    private IEnumerator HarvestRoutine(CommandCenter commandCenter, Resource resource)
    {
        yield return MoveToTarget(resource.transform);

        resource.Harvest(transform, _cargo.position);

        yield return MoveToTarget(commandCenter.transform);

        UnloadCargo(commandCenter,resource);
    }

    public IEnumerator BuildRoutine(Flag flag, Action<Unit> onReachFlag)
    {
        yield return MoveToTarget(flag.transform);

        onReachFlag?.Invoke(this);
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        while (transform.position != target.position.WithY(transform.position.y))
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position.WithY(transform.position.y), _speed * Time.deltaTime);
            transform.LookAt(target.position.WithY(transform.position.y));

            yield return null;
        }
    }

    private void UnloadCargo(CommandCenter commandCenter, Resource resource)
    {
        commandCenter.AcceptResource(resource);
        commandCenter.BindUnit(this);
    }
}
