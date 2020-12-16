﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action StartedTurn;
    public Action EndedTurn;
    public Action<int> ChangedAP;
    public Action<Aurea> SelectedAurea;
    public Action<Aurea> SelectedTarget;
    public Action ResetTarget;
    public Action ResetedSelection;
    public Action<PlayerController> GameOver;
    public Action<Aurea> AureaHasDied;

    [SerializeField]
    public bool isPlayer = false;

    [SerializeField]
    private PlayerData data = null;

    [SerializeField]
    private int actionPoints = 15;

    [SerializeField]
    private bool isOnTurn = false;

    // TO-DO Hier kommt die ULTI hin

    [SerializeField]
    private List<Aurea> aureaInstances = new List<Aurea>();

    [SerializeField]
    private GameController gameController = null;

    [SerializeField]
    private int actionPointsLeft = 0;

    [SerializeField]
    private int actionPointsPerRound = 3;


    private Aurea selected = null;
    private Aurea target = null;
    private Skill selectedSkill = null;

    void Start()
    {
        if(!gameController)
        {
            Debug.LogError("No Game Controller available");
            return;
        }
    }

    public void StartGame(List<GameObject> spawnPoints)
    {
        InstantiateSquad(spawnPoints);

        AddAP(actionPoints);
    }

    void InstantiateSquad(List<GameObject> spawnPoints)
    {
        int i = 0;
        List<string> squad = data.GetSquad();
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (squad[i] == "" || squad[i] == null)
            {
                Debug.Log("More Spawnpoints than Aureas in Squad");
                break;
            }

            int aureaLevel = data.GetAureaLevel(squad[i]);
            GameObject aureaPrefab = gameController.GetAureaData(squad[i]).levels[aureaLevel - 1].prefab;
            Aurea aurea = Instantiate(aureaPrefab, spawnPoint.transform).GetComponent<Aurea>();
            if(aurea)
                Debug.Log("Found Aurea");
            Debug.Log(aureaLevel);
            aurea.Init(aureaLevel, this);
            aureaInstances.Add(aurea);
            aurea.Died += AureaDied;

            i++;
        }
    }
    public void StartTurn()
    {
        StartedTurn?.Invoke();
        isOnTurn = true;
        AddAP(actionPointsPerRound);
    }

    public void EndTurn()
    {
        EndedTurn?.Invoke();
        isOnTurn = false;
    }

    public void ManuallyEndTurn()
    {
        if (!isOnTurn || !gameController.CanInteract())
            return;

        gameController.EndTurn();
    }

    public void AureaDied(Aurea _aurea)
    {
        AureaHasDied?.Invoke(_aurea);
        foreach(Aurea aurea in aureaInstances)
        {
            if (aurea.IsAlive())
                return;
        }
        GameOver?.Invoke(this);
    }

    public void Won()
    {
        foreach(Aurea aurea in aureaInstances)
        {
            if (aurea.IsAlive())
                aurea.GetComponent<Animator>().SetTrigger("Victory");
        }
    }

    public void Select(Aurea aurea)
    {
        if (!aurea && gameController.CanInteract())
        {
            selected = null;
            target = null;
            selectedSkill = null;
            ResetedSelection?.Invoke();
            return;
        }

        if (!isOnTurn || !gameController.CanInteract() || !aurea.IsAlive()) { return; }


        if (!selected && IsOwnAurea(aurea))
        {
            selected = aurea;
            SelectedAurea?.Invoke(selected);
        }
        else
        {
            target = aurea;
            SelectedTarget?.Invoke(target);
        }

        if (selectedSkill && target)
            UseSkill();
    }

    public void SelectSkill(Skill skill)
    {
        selectedSkill = skill;

        if (target)
            UseSkill();
    }

    private void UseSkill()
    {
        if (selectedSkill.GetCosts() >= actionPointsLeft)
            return;

        selected.UseSkill(selectedSkill, target);
        RemoveAP(selectedSkill.GetCosts());

        selectedSkill = null;
    }
    void AddAP(int amount)
    {
        actionPointsLeft = Mathf.Clamp(actionPointsLeft + amount, 0, actionPoints);
        ChangedAP?.Invoke(actionPointsLeft);
    }
    
    public void RemoveAP(int amount)
    {
        actionPointsLeft = Mathf.Clamp(actionPointsLeft - amount, 0, actionPoints);
        ChangedAP?.Invoke(actionPointsLeft);
    }

    public int HeroesLeft()
    {
        int heros = 0;
        foreach(Aurea hero in aureaInstances)
        {
            heros += hero.IsAlive() ? 1 : 0;
        }
        return heros;
    }

    bool IsOwnAurea(Aurea aureaToCheck)
    {
        foreach (Aurea aurea in aureaInstances)
        {
            if (aurea == aureaToCheck)
                return true;
        }

        return false;
    }
    public void ResetSelectedTarget()
    {
        target = null;
        ResetTarget?.Invoke();
    }
    public Aurea GetSelected() { return selected; }
    public bool IsOnTurn() { return isOnTurn; }
    public void SetData(PlayerData _data) { data = _data; }
    public PlayerData GetData() { return data; }
    public GameController GetGameController() { return gameController; }
}
