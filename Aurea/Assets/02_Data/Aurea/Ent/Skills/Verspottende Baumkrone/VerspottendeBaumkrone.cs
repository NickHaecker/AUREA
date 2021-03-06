using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VerspottendeBaumkrone", menuName = "Skills/Ent/VerspottendeBaumkrone")]
public class VerspottendeBaumkrone : Skill
{
    public string modifierName = "Baumkrone";
    public override void Use(Damage _dmg) {
        
        _dmg.attackDelay = attackDelay;

        if (Player.Instance.AnimationsOn() && animation)
            animation.StartAnimation(_dmg);

        System.Type modifierScript = System.Type.GetType(modifierName);
        Component component = _dmg.sender.gameObject.GetComponent(modifierScript);

        if (component)
            component.SendMessage("Kill");

        _dmg.sender.gameObject.AddComponent(modifierScript);
    }
    public override bool IsTargetValid(Aurea _target, Aurea _sender) {
        // if (_target.GetPlayer() == _sender.GetPlayer())
        //     return false;

        return true;
    }

    public override bool CheckTargets(List<Aurea> _targets, Aurea _sender) {
        // if (_targets.Count > 0 && IsTargetValid(_targets[0], _sender))
        //     return true;

        return true;
    }
}