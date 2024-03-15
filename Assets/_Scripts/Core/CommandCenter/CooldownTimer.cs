using UnityEngine;

public class CooldownTimer
{
    private readonly float _cooldown;
    private float _elapsedTime;

    public CooldownTimer(float cooldown)
    {
        _cooldown = cooldown;
        _elapsedTime = 0f;
    }

    public bool IsReady => _elapsedTime > _cooldown;

    public void Update()
    {
        _elapsedTime += Time.deltaTime;
    }

    public void Reset()
    {
        _elapsedTime = 0f;
    }
}
