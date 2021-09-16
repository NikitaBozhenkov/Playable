using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Booster : MonoBehaviour
{
    [SerializeField] private BoosterUsageType _type;

    public BoosterUsageType Type
    {
        get => _type;
        protected set => _type = value;
    }

    [SerializeField] protected Color EnabledColor;
    [SerializeField] protected Color UnabledColor;
    [SerializeField] protected Image Image;
    [SerializeField] private Image _dragImage;

    public Image DragImage
    {
        get => _dragImage;
        protected set => _dragImage = value;
    }

    [SerializeField] protected float CooldownTime;
    [SerializeField] protected float ActionTime;
    [SerializeField] protected AudioClip UsageAudio;

    public bool Enabled { get; set; } = true;

    protected abstract void Apply(List<Movable> targets);
    protected abstract void Disapply(List<Movable> targets);

    protected IEnumerator Cancel(List<Movable> targets)
    {
        yield return new WaitForSeconds(ActionTime);
        Disapply(targets);
    }

    public void Use(List<Movable> targets)
    {
        if (!Enabled) return;

        Apply(targets);
        AudioController.Instance.PlayClip(UsageAudio);
        StartCoroutine(StartCooldown());
        StartCoroutine(Cancel(targets));
    }

    protected IEnumerator StartCooldown()
    {
        Enabled = false;
        Image.color = UnabledColor;
        yield return new WaitForSeconds(CooldownTime);
        Enabled = true;
        Image.color = EnabledColor;
    }
}