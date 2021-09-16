using UnityEngine;

public class MoveTowardsMovable : FrameMovable
{
    public override void FrameMove()
    {
        var temp = transform.position;
        temp.y = Mathf.MoveTowards(temp.y, FinishY, CurrentSpeed * Time.deltaTime);
        transform.position = temp;
    }
}