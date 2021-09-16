using UnityEngine;

public class ForceMovable : PhysicsMovable
{
    public override void StartMoving()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _rigidbody.AddForce(new Vector2(0, -CurrentSpeed * Time.deltaTime));
    }
}