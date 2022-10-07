using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveChecker : MonoBehaviour
{
    GridGenerator gen;
    bool noFish;
    [SerializeField] UnityEvent gameOverEvent;
    [SerializeField] UnityEvent levelCompleteEvent;

    private void Awake()
    {
        gen = FindObjectOfType<GridGenerator>();
    }

    private void Start()
    {
        noFish = FindObjectsOfType<FriendlyFish>().Length == 0;
        levelCompleteEvent.AddListener(FindObjectOfType<TransitionLevels>().transitionAction);
    }

    public void CheckFriendlyFishAmount()
    {
        if (noFish)
            return;

        if (FindObjectsOfType<FriendlyFish>().Length <= 0)
        {
            FindObjectOfType<TransitionLevels>().SetClearType(LevelClearType.Lose);
            levelCompleteEvent.Invoke();
        }
    }

    public bool CheckLevelCompletion()
    {
        bool complete = gen.CheckCompletion();

        if (complete)
        {
            FindObjectOfType<TransitionLevels>().SetClearType(LevelClearType.Win);
            levelCompleteEvent.Invoke();
        }

        return complete;
    }
}
