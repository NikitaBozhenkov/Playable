using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScenarioManager : MonoBehaviour
{
    [Serializable]
    private struct PlayerScenario
    {
        public Movable movable;
        public float speed;
    }

    [Serializable]
    private struct BoosterScenario
    {
        public Booster booster;
        public float usageDelay;
        public Movable[] targets;
    }

    [SerializeField] private Animation _counterAnimation;

    [Space] [Header("Scenario")] [SerializeField]
    private string _playersSpritesPath;

    [SerializeField] private PlayerScenario[] _playerScenarios;
    [SerializeField] private Transform _finishPoint;
    [SerializeField] private BoosterScenario[] _boostersScenarios;

    [Space] [Header("Hand imitation")] [SerializeField]
    private Hand _handPrefab;

    [SerializeField] private float _dragSpeed;

    [Space] [Header("Finish")] [SerializeField]
    private AudioClip _allFinishedSound;

    [SerializeField] private float _delayAfterAllFinished;
    [SerializeField] private Canvas _finishWindow;

    private Loader<Sprite> _spriteLoader;
    private Hand _hand;
    private readonly List<Movable> _finishedMovables = new List<Movable>();

    #region Initialization

    private void Awake()
    {
        _spriteLoader = new LoaderResources<Sprite>();
    }

    private void FillPlayersImages()
    {
        var playersSprites = _spriteLoader.LoadAll(_playersSpritesPath);

        if (playersSprites == null)
        {
            throw new DirectoryNotFoundException(nameof(_playersSpritesPath));
        }
        else if (_playerScenarios.Length > playersSprites.Length)
        {
            throw new Exception("Players count is greater than number of sprites");
        }

        for (var i = 0; i < _playerScenarios.Length; ++i)
        {
            _playerScenarios[i].movable.gameObject.AddComponent<Image>().sprite = playersSprites[i];
            _playerScenarios[i].movable.Construct(_playerScenarios[i].speed, _finishPoint);
        }
    }

    #endregion

    #region Finish

    private void OnMovableFinish(Movable movable)
    {
        _finishedMovables.Add(movable);
        movable.Finished -= OnMovableFinish;
        AudioController.Instance.PlaySound("finish_2");

        if (_finishedMovables.Count == _playerScenarios.Length)
        {
            StartCoroutine(OnAllFinished());
        }
    }

    private IEnumerator OnAllFinished()
    {
        yield return new WaitForSeconds(_delayAfterAllFinished);
        AudioController.Instance.PlayClip(_allFinishedSound);
        yield return new WaitForSeconds(_allFinishedSound.length - 1f);
        _finishWindow.gameObject.SetActive(true);
        var finishWindow = _finishWindow.GetComponentInChildren<FinishWindowScenarioManager>();
        var sprites = _finishedMovables.Select(finishedMovable => finishedMovable.GetComponent<Image>().sprite)
            .ToArray();
        finishWindow.FillPlayersImages(sprites);
        StartCoroutine(finishWindow.StartScenario());
    }

    #endregion


    private void Start()
    {
        FillPlayersImages();
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(_counterAnimation.clip.length);
        foreach (var player in _playerScenarios)
        {
            player.movable.StartMoving();
            player.movable.Finished += OnMovableFinish;
        }

        _hand = Instantiate(_handPrefab);

        foreach (var scenario in _boostersScenarios)
        {
            StartCoroutine(UseBooster(scenario.usageDelay, scenario.booster, scenario.targets.ToList()));
        }
    }

    private IEnumerator UseBooster(float delay, Booster booster, List<Movable> targets)
    {
        yield return new WaitForSeconds(delay);

        switch (booster.Type)
        {
            case BoosterUsageType.Tap:
            {
                _hand.gameObject.SetActive(true);
                StartCoroutine(_hand.ImitateTap(booster.transform.position));
                booster.Use(targets);
                break;
            }
            case BoosterUsageType.Drag:
            {
                if (targets.Count != 1)
                {
                    throw new ArgumentException(nameof(targets));
                }

                var target = targets[0];
                _hand.gameObject.SetActive(true);
                _hand.Clicked.AddListener(() => { booster.Use(new List<Movable> {target}); });

                StartCoroutine(
                    _hand.ImitateDrag(
                        booster.transform.position,
                        target.transform,
                        booster.DragImage.sprite,
                        _dragSpeed));
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}