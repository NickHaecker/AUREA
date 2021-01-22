﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Animator))]
public class Aurea : MonoBehaviour
{
    public Action<Damage> StartAttack;
    public Action<Damage> BeforeGettingHit;
    public Action<Damage> AfterGettingHit;
    public Action<int> TookDamage;
    public Action ChangedLifepoints;
    public Action<Aurea> Died;
    public Action<Aurea> Selected;
    public Action<List<Aurea>> ChangedTargets;
    public Action SkillCancled;
    public Action GotHit;


    [SerializeField]
    private AureaData data = null;

    [SerializeField]
    public int level = 0;

    [SerializeField]
    private float lifePointsLeft = 0;

    public List<Component> modifier = new List<Component>();
    public List<ItemData> activeItems = new List<ItemData>();

    [SerializeField]
    private PlayerController player = null;
    private Skill _activeSkill;
    public Skill activeSkill
    {
        get
        {
            return _activeSkill;
        }
        set
        {
            if (value && player.GetAPLeft() >= value.GetCosts())
            {
                _activeSkill = value;
                Selected?.Invoke(this);
                Debug.Log("Selected Skill");
            }
            else {
                CancelSkill();
            }

            if (_activeSkill && _activeSkill.CheckTargets(targets, this))
                UseSkill();
        }
    }

    public List<Aurea> targets = new List<Aurea>();

    public void Init(int initLevel, PlayerController aureaPlayer)
    {
        level = initLevel;
        player = aureaPlayer;
        lifePointsLeft = data.levels[level - 1].lifePoints;
    }

    public bool IsAlive() { return lifePointsLeft > 0; }

    public void TakeTarget(Aurea _aurea)
    {
        if(!_aurea) {
            Debug.Log("Aurea is null");
        }
        if (!activeSkill)
        {
            Debug.LogError("Selected Target but no skill active!");
            return;
        }

        Debug.Log("Took Target and have skill active: " + activeSkill.name);

        foreach (Aurea target in targets)
        {
            if (target == _aurea)
            {
                targets.Remove(_aurea);
                ChangedTargets?.Invoke(targets);
                Debug.Log("Changed Targets");

                if (targets.Count <= 0)
                {
                    CancelSkill();
                }
                return;
            }
        }

        if (activeSkill.IsTargetValid(_aurea, this))
        {
            targets.Add(_aurea);
            ChangedTargets?.Invoke(targets);
            Debug.Log("Changed Targets");

            if (activeSkill.CheckTargets(targets, this))
                UseSkill();
        }
        else
            CancelSkill();
    }

    private void UseSkill()
    {
        Debug.Log("Using SKill : " + activeSkill.name + "on n targets: " + targets.Count);
        Damage dmg = GetDamage(targets, activeSkill);
        StartAttack?.Invoke(dmg);
        player.RemoveAP(activeSkill.GetCosts());
        activeSkill.Use(dmg);
        CancelSkill();
    }

    public void CancelSkill()
    {
        targets = new List<Aurea>();
        _activeSkill = null;
        SkillCancled?.Invoke();
        Debug.Log("Skill Cancled");
    }
    public void TakeDamage(Damage _dmg)
    {
        Debug.Log("Got hit " + this.name);
        BeforeGettingHit?.Invoke(_dmg);
        Debug.Log("Before Getting Hit");

        switch (_dmg.skill.GetSkillType())
        {
            case SkillType.MAGICAL:
                RemoveLifePoints(_dmg.magicalDamage * data.levels[level - 1].magicalDefense);
                break;
            case SkillType.PHYSICAL:
            default:
                RemoveLifePoints(_dmg.physicalDamage * data.levels[level - 1].physicalDefense);
                break;
        }

        if (lifePointsLeft <= 0)
        {
            Die();
            return;
        }

        foreach (string modifier in _dmg.modifier)
        {
            System.Type modifierScript = System.Type.GetType(modifier);
            Component component = GetComponent(modifierScript);

            if (component)
                component.SendMessage("Kill");

            gameObject.AddComponent(modifierScript);
        }

        AfterGettingHit?.Invoke(_dmg);
        Debug.Log("After Getting Hit");
    }

    public void Die()
    {
        Died?.Invoke(this);
    }

    IEnumerator WaitTillApplyDamage(float _attackDelay)
    {
        Debug.Log("Im here");
        yield return new WaitForSeconds(_attackDelay);
        Debug.Log("Now im here");
    }

    #region Getter, Setter
    public string GetName() { return data.NAME; }
    public string GetID() { return data.ID; }
    public string GetDescription() { return data.DESCRIPTION; }
    public List<Skill> GetSkills() { return data.levels[level - 1].skills; }
    public PlayerController GetPlayer() { return player; }
    public Damage GetDamage(List<Aurea> _targets, Skill _skill)
    {
        Damage newDamage = new Damage
        {
            sender = this,
            targets = _targets,
            skill = _skill,
            physicalDamage = data.levels[level - 1].physicalDamage,
            magicalDamage = data.levels[level - 1].magicalDamage,
            modifier = _skill.GetModifier()
        };

        return newDamage;
    }
    public int GetLevel() { return level; }
    public void SetLevel(int level) { this.level = level; }
    public float GetLifePointsMax() { return data.levels[level - 1].lifePoints; }
    public float GetLifePointsLeft() { return lifePointsLeft; }
    public void SetLifePointsLeft(float amount)
    {
        this.lifePointsLeft = amount;
        ChangedLifepoints?.Invoke();
    }
    public void RemoveLifePoints(float amount)
    {
        this.lifePointsLeft -= amount;
        TookDamage?.Invoke(Mathf.FloorToInt(amount));
        ChangedLifepoints?.Invoke();
    }
    public float GetMagicalDamage() { return data.levels[level - 1].magicalDamage; }
    public float GetMagicalDefence() { return data.levels[level - 1].magicalDefense; }
    public float GetPhysicalDamage() { return data.levels[level - 1].physicalDamage; }
    public float GetPhysicalDefence() { return data.levels[level - 1].physicalDefense; }
    public AureaData GetAureaData() { return data; }
    #endregion
}
