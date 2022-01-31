using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effectGettingDamage;

    public ParticleSystem ThisPS { get; private set; }

    private bool _isEmitting = false;
    private List<ParticleSystem.Particle> _enter = new List<ParticleSystem.Particle>();
    private ParticleSystem[] _childrenPS;

    private void Awake()
    {
        ThisPS = GetComponent<ParticleSystem>();
        _childrenPS = GetComponentsInChildren<ParticleSystem>();
    }
    public void SetTrigger(Collider targetCollider, int index = 0)
    {
        ThisPS.trigger.SetCollider(index, targetCollider);
        foreach(var child in _childrenPS)
        {
            child.trigger.SetCollider(index, targetCollider);
        }
    }

    public void SetTrigger(Collider [] targetColliders)
    {
        for(int i = 0; i < targetColliders.Length; i++) 
        {
            SetTrigger(targetColliders[i], i);
        }
    }

    private void OnParticleTrigger()
    {
        int numEnter = ThisPS.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enter);
        if(numEnter > 0 && !_isEmitting)
        {
            StartCoroutine(ReturnSpell());
        }
    }

    private IEnumerator ReturnSpell()
    {
        _isEmitting = true;
        float timer = 0;
        var effect = Instantiate(_effectGettingDamage, _enter[0].position, Quaternion.identity, Level.Instance.transform);
        while (timer < 0.2f)
        {
            if (ThisPS.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enter) == 0)
                timer += Time.deltaTime;
            else timer = 0;
            yield return null;
        }
        _isEmitting = false;
        effect.Stop();
    }

    public void Stop()
    {
        ThisPS.Stop();
    }

    public void Play()
    {
        ThisPS.Play();
    }
}
