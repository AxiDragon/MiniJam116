using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShafiResponse : MonoBehaviour
{
    [SerializeField] private List<Responses> responses = new List<Responses>();
    [SerializeField] GameObject blasphemyHint;
    [SerializeField] public int angerThreshold = 4;
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    [Serializable]
    public class Responses
    {
        public LevelClearType clearType;
        [TextArea]
        public List<string> messages;
        public int timesTriggered;
    }

    public void UpdateText(LevelClearType clear)
    {
        for (int i = 0; i < responses.Count; i++)
        {
            if (responses[i].clearType == clear)
            {
                DisplayText(i);
                return;
            }
        }
    }

    public bool CheckAnger()
    {
        int anger = 0;

        for (int i = 0; i < responses.Count; i++)
        {
            if (responses[i].clearType == LevelClearType.Skip || responses[i].clearType == LevelClearType.Lose)
            {
                anger += responses[i].timesTriggered;
            }
        }

        bool thresReached = anger > angerThreshold;

        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            thresReached = false;

        blasphemyHint.SetActive(thresReached);
        if (GameObject.Find("BlasUI"))
        {
            GameObject.Find("BlasUI").SetActive(thresReached);
        }

        return thresReached;
    }

    private void DisplayText(int i)
    {
        textMesh.text = responses[i].messages[responses[i].timesTriggered];
        responses[i].timesTriggered++;
        responses[i].timesTriggered %= responses[i].messages.Count;
    }
}
