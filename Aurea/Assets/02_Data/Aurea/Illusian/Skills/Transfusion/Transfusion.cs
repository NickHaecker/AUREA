﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transfusion", menuName = "Skills/Illusian/Transfusion")]
public class Transfusion : Skill
{
    public override bool IsTargetValid(Aurea _target, Aurea _sender) {
        return true;
    }

    public override bool CheckTargets(List<Aurea> _targets, Aurea _sender) {
        return true;
    }

    public override void Use(Damage _dmg) {

    }
}
