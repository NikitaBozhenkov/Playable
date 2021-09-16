using UnityEngine;

public class TranslateMovable : FrameMovable
{
    public override void FrameMove()
    {
        transform.Translate(0, -CurrentSpeed * Time.deltaTime, 0);
    }
}