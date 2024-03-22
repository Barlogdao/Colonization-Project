using UnityEngine;

public class CommandCenterSpawner 
{
    private readonly CommandCenter.Factory _factory;

    public CommandCenterSpawner(CommandCenter.Factory factory)
    {
        _factory = factory;
    }

    public CommandCenter Spawn(Vector3 position,Quaternion rotation)
    {
        CommandCenter commandCenter = _factory.Create();

        commandCenter.transform.SetPositionAndRotation(position, rotation);

        return commandCenter;
    }
}