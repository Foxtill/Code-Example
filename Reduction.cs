using UnityEngine;

[CreateAssetMenu(fileName = "Reduction", menuName = "ScriptableObjects/Spells/Reduction")]
public class Reduction : Spell
{
    public override void Cast(Mage player, Enemy[] _targets)
    {
        foreach (var target in _targets)
        {
            if (target._isDisable) continue;

            target._animController.SetState(state);
            target._animController.ChangeSpeedAnimation(speed);
        }
        base.Cast(player, _targets);
    }
}
