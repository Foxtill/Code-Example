using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    protected State _state = State.Idle;

    [Header("Tracking")]
    [SerializeField] private float _speed;
    [SerializeField] private float _raduisDetecting;
    [Header("Attack")]
    [SerializeField] private float _attackRange;
    [SerializeField] private float _damage;
    [Header("Health")]
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    public Outline outline { get; private set; }
    public RagdollController ragdollController {get; private set;}
    public AnimController _animController { get; private set; }
    public float CurrentHealth
    {
        get
        {
            if (_currentHealth > _maxHealth)
                _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            return _currentHealth;
        }
    }
    public float MaxHealth => _maxHealth;
    public float RaduisDetecting => _raduisDetecting;

    public bool IsDisable { get; protected set; } = false;
    public bool IsStopAttack { get; protected set; } = false;
    public bool IsDetected { get; protected set; } = false;
    public bool InAttackRange { get; protected set; } = false;

    private float _attackInSecond = 2.5f;
    private float _attackCooldown;
    private float _distance;

    private Coroutine _moveCoroutine;
    private Coroutine _rotationCoroutine;

    private Vector3 _difVectors;

    protected Mage _player;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawAssistance(Color.white, raduisDetecting);
        DrawAssistance(Color.red, _attackRange);
    }

    private void DrawAssistance(Color color, float radius)
    {
        Handles.color = color;
        Handles.DrawWireDisc(transform.position, new Vector3(0, 1, 0), radius);
        Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), radius);

        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
#endif

    public void Enable()
    {
        IsDisable = false;
    }

    public void Disable()
    {
        StopMove();
        if(outline != null) outline.enabled = false;
        IsDisable = true;
    }

    public bool IsCanAttack()
    {
        return _attackRange >= _distance;
    }

    public void StopMove()
    {
        GeneralizedFunc.Stop(_moveCoroutine);
        GeneralizedFunc.Stop(_rotationCoroutine);
    }

    public void Idle()
    {
        _animController.SetState(_state);
    }

    public void Damage(float damage)
    {
        if(_state == State.Attack)
        {
            if (IsCanAttack())
            {
                _player.TakeDamage(damage);
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if(_player.currentHealth > 0)
            _currentHealth -= damage;
    }

    private void Awake()
    {
        ragdollController = GetComponent<RagdollController>();
        outline = GetComponent<Outline>();
        _animController = GetComponent<AnimController>();
        _player = FindObjectOfType<Mage>();
    }

    protected virtual void FixedUpdate()
    {
        if (IsDisable) return;

        _difVectors = Vector3.Scale(new Vector3(1, 1, 1), _player.transform.position - transform.position);
        _distance = _difVectors.magnitude;
        IsDetected = _distance < _raduisDetecting ? true : false;

        if (IsDetected)
        {
            if (!IsCanAttack())
            {
                LookToTarget();
                MoveToTarget();
            }
            else
            {
                if (_attackCooldown <= 0)
                {
                    if (_player.currentHealth > 0)
                    {
                        StopMove();
                        Attack();
                    }
                    else
                    {
                        Disable();
                    }
                    _attackCooldown = _attackInSecond;
                }
                else _attackCooldown -= Time.fixedDeltaTime;
            }
        }
    }

    protected virtual void Attack()
    {
        _state = State.Attack;
        var randomAttack = Random.Range(0, 2);
        _animController.SetState(_state, (float)randomAttack);
    }


    protected void LookToTarget()
    {
        var quat = Quaternion.LookRotation(Vector3.Scale(new Vector3(1,0,1), _player.hand.pointForFollower.position - transform.position), new Vector3(0, 1, 0));
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, Time.fixedDeltaTime * 10f);
    }
    protected void MoveToTarget()
    {
        _state = State.Run;
        _animController.SetState(_state);
        transform.position = Functions.Equate
            (transform.position, 
            Vector3.MoveTowards(transform.position, _player.transform.position - _difVectors.normalized * (_attackRange - 0.2f), 
            _speed * Time.fixedDeltaTime), 
            Axes.XZ);
    }
}
