using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Instance = this;
    }

    public void PlaySound(string clipName)
    {
        _audioSource.Stop();
        var clipToPlay = _clips.FirstOrDefault(clip => clip.name == clipName);

        if (clipToPlay == null)
        {
            throw new KeyNotFoundException(nameof(clipToPlay));
        }

        _audioSource.PlayOneShot(clipToPlay, .1f);
    }

    public void PlayClip(AudioClip clip) {
        _audioSource.Stop();

        _audioSource.PlayOneShot(clip, .1f);
    }
}
