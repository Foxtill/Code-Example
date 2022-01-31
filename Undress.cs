using UnityEngine;

[CreateAssetMenu(fileName = "Undress", menuName = "ScriptableObjects/Spells/Undress")]
public class Undress : Spell
{
    public override void Cast(Mage player, Enemy[] _targets)
    {
        foreach (var target in _targets)
        {
            if (target._isDisable) continue;

            var effect = Object.Instantiate(_effect, target.transform.position, Quaternion.identity, Level.Instance.transform);

            var wardrobe = target.GetComponent<Wardrobe>();
            wardrobe.structureCloth.Change(System.Array.Find(wardrobe.ClothDataBase.structuresCloth, x => x.typeCloth == StructureCloth.TypeCloth.Undressed));

            target._animController.SetState(state);
        }
        base.Cast(player, _targets);
    }
}
