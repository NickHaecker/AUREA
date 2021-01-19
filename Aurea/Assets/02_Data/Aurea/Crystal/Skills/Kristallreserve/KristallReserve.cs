﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "KristallReserve", menuName = "Skills/Crystal/KristallReserve")]
public class KristallReserve : Skill
{
    [SerializeField]
    private float physicalDamageMultiplier = 1.3f;

    [SerializeField]
    private GameObject attackPrefab = null;

    //[SerializeField]
    //private float attackDelay = 1.8f;
    public override void Use(Damage dmg)
    {
        dmg.physicalDamage *= physicalDamageMultiplier;
        GameObject attack = Instantiate(attackPrefab, dmg.sender.transform);
        ErdbebenController controller = attack.GetComponent<ErdbebenController>();
        controller.TakeInformations(dmg);
    }

    public override bool IsTargetValid(Aurea _aurea, Aurea _sender)
    {
        return true;
    }

    public override bool CheckTargets(List<Aurea> _targets, Aurea _sender)
    {
        return true;
    }
}