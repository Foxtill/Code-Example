using UnityEngine;
using System;

public abstract class Spell : ScriptableObject
{
    [Header("Internal Part")]
    public string sequence;
    public Sprite icon;
    public Vector2 size = new Vector2(225, 225);

    [Header("Animation")]
    [SerializeField] protected State state;
    [SerializeField] protected float speed = 1;

    [Header("Game Part")]
    [SerializeField] protected ParticleSystem _effect;
    public TypeSpell typeSpell = TypeSpell.Single;

    protected Mage _player;

    public enum TypeSpell
    {
        Single,
        Splash,
    }

    public virtual void Cast(Mage player, Enemy[] _targets)
    {
        _player = player;

        for (int i = 0; i < _targets.Length; i++)
        {
            var target = _targets[i];
            if (target == null || target._isDisable) continue;

            _player.RemoveTarget(target);

            target.GetComponent<Rigidbody>().isKinematic = true;
            target.Disable();
        }
    }
}