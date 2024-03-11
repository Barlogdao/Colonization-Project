using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BootStrap : MonoBehaviour
{
    [SerializeField] private Base _base;

    [SerializeField] private List<Unit> _units;


    private void Awake()
    {
        _base.Initialize(_units);
    }

    private void OnValidate()
    {
        _units = FindObjectsByType<Unit>(sortMode: FindObjectsSortMode.None).ToList();
    }
}