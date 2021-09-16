using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FinishWindowScenarioManager : MonoBehaviour
{
    [SerializeField] private Image[] _players = new Image[4];
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private Transform[] _loserPositions;
    [SerializeField] private RedirectionButton _button;

    [SerializeField] private Animator _textAnimator;
    [SerializeField] private AnimationClip endCard;

    [SerializeField] private AudioClip[] _applauseSounds;
    [SerializeField] private AudioClip _winSound;
    private Animation _animation;
    [SerializeField] private string _textAnimationName;
    private readonly RedirectionButton _redirectionButton;

    #region Initialization

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    public void FillPlayersImages(Sprite[] sprites)
    {
        if (_players.Length != sprites.Length)
        {
            throw new Exception("Players count is not equal number of sprites");
        }

        for (var i = 0; i < sprites.Length; ++i)
        {
            _players[i].sprite = sprites[i];
        }
    }

    #endregion

    #region Animations

    private void StartLoserAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.SetLoops(-1);
        _players[3].gameObject.SetActive(true);
        var loserTransform = _players[3].gameObject.transform;
        sequence.Append(loserTransform.DOMove(_loserPositions[1].position, 5f));
        sequence.Append(loserTransform.DOMove(_loserPositions[0].position, 5f));
        sequence.Play();
    }

    private IEnumerator PlayPlayerAnimation(GameObject player, AudioClip clip)
    {
        player.SetActive(true);
        var anim = player.GetComponent<Animation>();
        anim.Play();
        AudioController.Instance.PlayClip(clip);
        yield return new WaitForSeconds(Mathf.Max(anim.clip.length, clip.length));
    }

    private IEnumerator PlayWinnerAnimation(Image player, AudioClip clip)
    {
        player.gameObject.SetActive(true);
        player.color = Color.clear;
        _smoke.gameObject.SetActive(true);
        _smoke.Play();
        AudioController.Instance.PlayClip(clip);
        yield return new WaitForSeconds(_smoke.time);
        player.color = Color.white;
        yield return new WaitForSeconds(clip.length - _smoke.time);
    }

    private void ShowEndCard()
    {
        _textAnimator.gameObject.SetActive(true);
        _button.gameObject.SetActive(true);
        _textAnimator.Play(_textAnimationName);
        AudioController.Instance.PlayClip(_winSound);
        GetComponent<Animation>().clip = endCard;
        GetComponent<Animation>().Play();
    }

    #endregion


    public IEnumerator StartScenario()
    {
        _animation.Play();
        yield return new WaitForSeconds(_animation.clip.length + .5f);
        StartLoserAnimation();
        yield return PlayPlayerAnimation(_players[2].gameObject, _applauseSounds[0]);
        yield return PlayPlayerAnimation(_players[1].gameObject, _applauseSounds[1]);
        yield return PlayWinnerAnimation(_players[0], _applauseSounds[2]);
        ShowEndCard();
    }
}