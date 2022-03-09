using UnityEngine;

namespace StateSystem
{
    public abstract class BaseState
    {
        public readonly string Name;
        protected readonly StateMachine _stateMachine;
        protected readonly Unit _thisUnit;
        protected readonly MovementStateData _statesData;

        public BaseState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _thisUnit = _stateMachine.Unit;
            _statesData = _thisUnit.Info.MovementStateData;
            Name = GetType().Name;
        }
        public virtual void Enter() { }
        public virtual void UpdatePhysics() { }
        public virtual void UpdateLogic() { }
        public virtual void Exit() { }
    }
}
