using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateMovable : NonPhysicsMovable
{
    public override void FrameMove()
    {
        transform.Translate(0, -CurrentSpeed * Time.deltaTime, 0);
    }
}
