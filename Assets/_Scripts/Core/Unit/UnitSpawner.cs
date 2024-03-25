using UnityEngine;

[System.Serializable]
public class UnitSpawner
{
    [SerializeField] private Unit _prefab;

    public Unit Spawn(Vector3 position)
    {
        Unit unit = Object.Instantiate(_prefab, position, Quaternion.identity);

        return unit;
    }
}