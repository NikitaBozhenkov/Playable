using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] private AnimationClip _tapDownAnimation;
    [SerializeField] private AnimationClip _tapUpAnimation;
    [SerializeField] private Transform _clickTransform;
    [SerializeField] private float _dragObjectScale;

    private Animator _animator;
    private Vector3 _offset;

    public UnityEvent Clicked;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _offset = _clickTransform.transform.position;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        Clicked.RemoveAllListeners();
    }

    public IEnumerator ImitateTap(Vector3 position)
    {
        transform.position = position - _offset;
        gameObject.SetActive(true); 
        _animator.Play(_tapDownAnimation.name);
        yield return new WaitForSeconds(_tapDownAnimation.length);
        _animator.Play(_tapUpAnimation.name);
        yield return new WaitForSeconds(_tapUpAnimation.length);
        Clicked?.Invoke();
        gameObject.SetActive(false);
    }

    public IEnumerator ImitateDrag(Vector3 originPosition, Transform movable, Image dragObject, float speed)
    {
        transform.position = originPosition - _offset;
        gameObject.SetActive(true);
        _animator.Play(_tapDownAnimation.name);
        yield return new WaitForSeconds(_tapDownAnimation.length);
        var image = Instantiate(new GameObject()).AddComponent<SpriteRenderer>();
        image.sprite = dragObject.sprite;
        image.transform.position = originPosition;
        image.transform.localScale = Vector3.one * _dragObjectScale;
        image.sortingOrder = 4;
        image.transform.SetParent(transform);

        var temp1 = new Vector2(originPosition.x, originPosition.y);
        var temp2 = new Vector2(movable.position.x, movable.position.y);
        var time = (temp2 - temp1).magnitude / speed;

        for(float t = 0; t < time; t += Time.deltaTime) {
            transform.position = Vector2.Lerp(originPosition, movable.position, t / time);
            yield return null;
        }

        _animator.Play(_tapUpAnimation.name);
        Destroy(image);
        Clicked?.Invoke();
        gameObject.SetActive(false);
    }
}
