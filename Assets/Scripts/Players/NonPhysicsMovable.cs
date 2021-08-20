public abstract class NonPhysicsMovable : Movable
{
    private bool _moving;

    private void Update() {
        if(!_moving) return;
        FrameMove();
        if(!(transform.position.y <= FinishY)) return;
        StopMoving();
        OnFinish();
    }

    public abstract void FrameMove();

    public override void StartMoving() {
        _moving = true;
    }

    public override void StopMoving() {
        _moving = false;
    }
}