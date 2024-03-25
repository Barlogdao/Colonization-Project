using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cargo;
    [SerializeField] private float _reachTargetOffset;
    [SerializeField] private View _view;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
    }

    private void Start()
    {
        _view.ShowSpawn();
    }

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
        Vector3 targetPosition = resource.transform.position;

        yield return MoveToTarget(targetPosition);

        resource.Harvest(transform, _cargo.position);

        targetPosition = commandCenter.GetClosestPoint(transform);

        yield return MoveToTarget(targetPosition);

        UnloadCargo(commandCenter,resource);
    }

    public IEnumerator BuildRoutine(Flag flag, Action<Unit> onReachFlag)
    {
        Vector3 targetPosition = flag.transform.position;

        yield return MoveToTarget(targetPosition);

        onReachFlag?.Invoke(this);
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        yield return new WaitUntil(() => _agent.SetDestination(targetPosition) == true);
        yield return new WaitUntil(() => _agent.hasPath == true);
        yield return new WaitUntil(() => _agent.remainingDistance <= _reachTargetOffset);
    }

    private void UnloadCargo(CommandCenter commandCenter, Resource resource)
    {
        commandCenter.AcceptResource(resource);
        commandCenter.BindUnit(this);
    }
}
