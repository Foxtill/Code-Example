using UnityEngine;

namespace StateSystem
{
    public class AttackBull : BaseState
    {
        private float _timerAttackCooldown;

        public AttackBull(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _timerAttackCooldown = _statesData.attackData.AttackCooldown;
            //TransitionsEnter
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateLogic()
        {
            if (_thisUnit.Target != null)
            {
                if ((_thisUnit.Target.position - _thisUnit.transform.position).magnitude <= _statesData.attackData.AttackRadius)
                {
                    if (_timerAttackCooldown >= _thisUnit.Info.MovementStateData.attackData.AttackCooldown)
                    {
                        Attack();
                    }
                    _timerAttackCooldown += Time.deltaTime;
                }
                else _stateMachine.ChangeState(new RunBull(_stateMachine));
            }
            else _stateMachine.ChangeState(new IdleBull(_stateMachine));
        }

        private void Attack()
        {
            _timerAttackCooldown = 0;
            // todo: dealing damage to target
        }

        public override void UpdatePhysics()
        {

        }
    }
}
