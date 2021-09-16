using System;
using UnityEngine;

public abstract class Movable : MonoBehaviour
{
    [SerializeField] private float _baseSpeed;
    [SerializeField] private Transform _aimPoint;

    public float BaseSpeed
    {
        get => _baseSpeed;
        protected set => _baseSpeed = value;
    }

    public float ExtraSpeed;
    public float CurrentSpeed => _baseSpeed + ExtraSpeed;
    public float FinishY => _aimPoint.position.y;

    public event Action<Movable> Finished;

    public void Construct(float baseSpeed, Transform aimPoint)
    {
        if (baseSpeed < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(baseSpeed));
        }

        _baseSpeed = baseSpeed;
        _aimPoint = aimPoint;
    }

    protected void OnFinish()
    {
        Finished?.Invoke(this);
    }

    public abstract void StartMoving();
    public abstract void StopMoving();
}