using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoProgress : MonoBehaviour
{
    [SerializeField] private float timeProgress = 5f;

    void Start()
    {
        Invoke("NextScene", timeProgress);
    }

    void NextScene()
    {
        FindObjectOfType<TransitionLevels>().Transition();
    }
}
