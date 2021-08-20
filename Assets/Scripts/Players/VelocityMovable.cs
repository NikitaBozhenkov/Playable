using System.Collections;
using UnityEngine;

public class VelocityMovable : PhysicsMovable
{
    public override void StartMoving()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.velocity = new Vector2(0, -CurrentSpeed);
    }
}