using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Turns : MonoBehaviour
{
    GridBuilder builder;
    ObjectiveChecker checker;

    private void Awake()
    {
        checker = FindObjectOfType<ObjectiveChecker>();
        builder = FindObjectOfType<GridBuilder>();
    }

    public void InfectionPhase()
    {
        if (checker.CheckLevelCompletion())
            return;

        SetPlayerState(false);

        foreach (TileInfecter infecter in FindObjectsOfType<TileInfecter>())
        {
            infecter.InfectSurroundingTiles();
        }

        FishPhase();
    }

    public void FishPhase()
    {
        StartCoroutine(FishTurns());
    }

    private IEnumerator FishTurns()
    {
        yield return null;

        foreach (FriendlyFish fish in FindObjectsOfType<FriendlyFish>())
        {
            if (fish != null)
                yield return fish.Move();
        }

        foreach (EnemyFish fish in FindObjectsOfType<EnemyFish>())
        {
            if (fish != null)
                yield return fish.Move();
        }

        yield return null;
        checker.CheckFriendlyFishAmount();


        SetPlayerState(true);
        FindObjectOfType<TileKeeper>().GeneratePreview();
    }

    private void SetPlayerState(bool state)
    {
        builder.canBuild = state;
    }
}
