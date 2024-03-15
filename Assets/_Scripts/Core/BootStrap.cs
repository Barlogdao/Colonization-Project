using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BootStrap : MonoBehaviour
{
    [SerializeField] private CommandCenter _commandCenter;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private HUD _hud;

    private PlayerInput _playerInput;


    private void Awake()
    {
        _playerInput = new PlayerInput();
        _commandCenter.Initialize(_playerInput);
        _hud.Initialize(_commandCenter);

        foreach (var unit in _units)
        {
            _commandCenter.BindUnit(unit);
        }
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnValidate()
    {
        _units = FindObjectsByType<Unit>(sortMode: FindObjectsSortMode.None).ToList();
    }
}