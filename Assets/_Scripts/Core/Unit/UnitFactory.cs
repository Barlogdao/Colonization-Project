using UnityEngine;

[System.Serializable]
public class UnitFactory
{
    [SerializeField] private Unit _prefab;

    public Unit Create(Vector3 position)
    {
        Unit unit = Object.Instantiate(_prefab, position, Quaternion.identity);

        return unit;
    }
}