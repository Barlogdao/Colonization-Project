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

    public void BuildCommandCenter(Flag flag, CommandCenterSpawner commandCenterFactory)
    {
        StartCoroutine(BuildRoutine(flag, commandCenterFactory));
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

    public IEnumerator BuildRoutine(Flag flag, CommandCenterSpawner commandCenterSpawner)
    {
        yield return MoveToTarget(flag.transform);

        CommandCenter commandCenter = commandCenterSpawner.Spawn(flag.transform.position, flag.transform.rotation);
        commandCenter.BindUnit(this);

        Destroy(flag.gameObject);
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
