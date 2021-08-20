using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GreetingsSceneManager : MonoBehaviour
{
    [SerializeField] private RectTransform[] _players;
    [SerializeField] private string _playersSpritesPath;
    private Animation _animation;
    private Loader<Sprite> _spriteLoader;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _spriteLoader = new LoaderResources<Sprite>();
    }

    private void FillPlayersImages()
    {
        var playersSprites = _spriteLoader.LoadAll(_playersSpritesPath);
        (playersSprites[1], playersSprites[0]) = (playersSprites[0], playersSprites[1]);

        if(_players.Length > playersSprites.Length) {
            throw new Exception("Players count is greater than number of sprites");
        }

        for(var i = 0; i < _players.Length; ++i) {
            _players[i].gameObject.AddComponent<Image>().sprite = playersSprites[i];
        }
    }

    private void StartAnimation()
    {
        _animation.Play();
    }

    private void Start()
    {
        FillPlayersImages();
        StartAnimation();
    }
}
