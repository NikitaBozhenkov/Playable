using System.Collections.Generic;

public class StopAllBooster : Booster
{
    protected override void Apply(List<Movable> targets)
    {
        foreach (var target in targets)
        {
            target.StopMoving();
        }
    }

    protected override void Disapply(List<Movable> targets)
    {
        foreach (var target in targets)
        {
            target.StartMoving();
        }
    }
}