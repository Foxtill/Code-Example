using Assets.Scripts.Extensions;
using System.Linq;
using UnityEngine;

namespace StateSystem
{
    public class RunBull : BaseState
    {
        private float _cooldownForCheckEnemy = 0.25f;
        private float _timerForCheckEnemy;

        public RunBull(StateMachine stateMachine) : base(stateMachine) { }
        public override void Enter()
        {
            base.Enter();
            //TransitionsEnter
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            if (Time.time >= _timerForCheckEnemy)
            {
                if (_thisUnit.TryGetNearlyUnit(out Unit enemy))
                {
                    _thisUnit.SetTarget(enemy.transform);
                }
                _timerForCheckEnemy = _cooldownForCheckEnemy + Time.time;
            }

            if (_thisUnit.Target != null)
            {
                var distance = (_thisUnit.Target.position - _thisUnit.transform.position).magnitude;
                if (distance <= _statesData.attackData.AttackRadius)
                {
                    _stateMachine.ChangeState(new AttackBull(_stateMachine));
                }
            }
            else _stateMachine.ChangeState(new IdleBull(_stateMachine));
        }

        public override void UpdatePhysics()
        {
            if(_thisUnit.Target != null)
            {
                _thisUnit.transform.position = Vector3.MoveTowards(_thisUnit.transform.position, _thisUnit.Target.position, _statesData.moveData.Speed * Time.fixedDeltaTime);
            }
        }
    }
}
