using Assets.Scripts.Extensions;
using System.Linq;
using UnityEngine;

namespace StateSystem
{
    public class IdleBull : BaseState
    {
        public IdleBull(StateMachine stateMachine) : base(stateMachine) { }

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
            if(_thisUnit.TryGetNearlyUnit(out Unit target))
            {
                _thisUnit.SetTarget(target.transform);
            }

            if (_thisUnit.Target != null)
            {
                var distance = Vector3.Distance(_thisUnit.transform.position, _thisUnit.Target.position);

                if (distance <= _statesData.attackData.AttackRadius)
                {
                    _stateMachine.ChangeState(new AttackBull(_stateMachine));
                }
                else if(distance <= _statesData.attackData.DetectRadius)
                {
                    _stateMachine.ChangeState(new RunBull(_stateMachine));
                }
            }
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }
    }
}
