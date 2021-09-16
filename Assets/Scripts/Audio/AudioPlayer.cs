using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public void Play(string name)
    {
        AudioController.Instance.PlaySound(name);
    }
}