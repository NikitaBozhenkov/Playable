using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public void Play(string name)
    {
        AudioController.Instance.PlaySound(name);
    }
}
