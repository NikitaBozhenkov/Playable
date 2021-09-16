using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : Booster
{
    [SerializeField, Range(-1, 1)] private float _percentExtraSpeed;

    protected override void Apply(List<Movable> targets)
    {
        foreach (var movable in targets)
        {
            movable.ExtraSpeed = movable.BaseSpeed * _percentExtraSpeed;
        }
    }

    protected override void Disapply(List<Movable> targets)
    {
        var temp = _percentExtraSpeed;
        _percentExtraSpeed = 0;
        Apply(targets);
        _percentExtraSpeed = temp;
    }
}