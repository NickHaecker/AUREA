using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Feuer Atem", menuName = "Skills/Dragon/Feuer Atem")]
public class FeuerAtem : Skill
{
    [SerializeField]
    private float magicDamageMultiplier = 1f;

    public override void Use(Damage _dmg) {
        
        _dmg.physicalDamage *= magicDamageMultiplier;
        _dmg.attackDelay = attackDelay;

        if (Player.Instance.AnimationsOn() && animation)
            animation.StartAnimation(_dmg);
        
        List<Aurea> targetList = new List<Aurea>(_dmg.targets);

        foreach (Aurea target in targetList)
        {
            Damage dmg = _dmg.Copy();
            target.TakeDamage(dmg);
        }
    }
    public override bool IsTargetValid(Aurea _target, Aurea _sender) {
        // if (_target.GetPlayer() == _sender.GetPlayer())
        //     return false;

        return true;
    }

    public override bool CheckTargets(List<Aurea> _targets, Aurea _sender) {
        if (_targets.Count > 0 && IsTargetValid(_targets[0], _sender))
            return true;

        return false;
    }
}
