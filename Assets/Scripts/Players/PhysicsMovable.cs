using UnityEngine;

public abstract class PhysicsMovable : Movable
{
    protected Rigidbody2D _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();

        if(_rigidbody == null) {
            _rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }

        _rigidbody.sleepMode = RigidbodySleepMode2D.StartAsleep;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        _rigidbody.gravityScale = .01f;
        _rigidbody.mass = 1f;
    }

    private void FixedUpdate() {
        if(!(FinishY > transform.position.y)) return;

        OnFinish();
        StopMoving();
    }

    public override void StopMoving() {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}