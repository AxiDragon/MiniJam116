using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TransitionLevels : MonoBehaviour
{
    bool firstScene = true;
    bool freeplay = false;
    float corruption = 0f;
    float calmBaseVolume;
    float angerBaseVolume;
    LevelClearType clearType = LevelClearType.None;

    [SerializeField] AudioSource calmMusic;
    [SerializeField] AudioSource angerMusic;
    [SerializeField] Color calmColor;
    [SerializeField] Color angerColor;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] Image canvasColor;
    [SerializeField] ShafiResponse response;
    [HideInInspector] public UnityAction transitionAction;

    private void Start()
    {
        if (firstScene)
        {
            DontDestroyOnLoad(gameObject);
            firstScene = false;
            //SceneManager.sceneLoaded += LoadedScene;
            transitionAction = new UnityAction(Transition);
            calmBaseVolume = calmMusic.volume;
            angerBaseVolume = angerMusic.volume;
            angerMusic.volume = 0f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetClearType(LevelClearType.Skip);
            Transition();
        }
    }

    public void ActivateFreeplayMode()
    {
        freeplay = true;
        response.angerThreshold = -1;
    }

    private void LoadedScene(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(TransitionRoutine(true));
    }

    public void SetClearType(LevelClearType clear)
    {
        if (freeplay && clear == LevelClearType.Blasphemous)
            return;

        if (clearType != LevelClearType.Blasphemous)
            clearType = clear;
    }

    public void Transition()
    {
        //StartCoroutine(TransitionRoutine(start));
        StartCoroutine(BasicTransitionRoutine());
    }

    IEnumerator TransitionRoutine(bool start)
    {
        print(start);

        CanvasGroup[] groups = FindObjectsOfType<CanvasGroup>();
        GameObject moveable = GameObject.FindWithTag("Target");

        if (moveable == null)
            moveable = Camera.main.gameObject;

        foreach (CanvasGroup group in groups)
        {
            float targetAlpha = start ? 1f : 0f;
            group.LeanAlpha(targetAlpha, 2f).setEaseInOutSine();
        }

        if (start)
        {
            Vector3 targetPosition = GameObject.Find("Point 0 0").transform.position;
            moveable.LeanMove(targetPosition, 3f).setEaseOutQuint();
        }
        else
        {
            Vector3 targetPosition = transform.position + Vector3.right * 100f;
            moveable.LeanMove(targetPosition, 3f).setEaseInQuint();
        }

        if (!start)
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void UpdateCorruption(float increase)
    {
        corruption += increase;
        Color c = Color.Lerp(calmColor, angerColor, corruption);

        foreach(Camera cam in FindObjectsOfType<Camera>())
        {
            cam.backgroundColor = c;
        }
        canvasColor.color = c;

        calmMusic.volume = Mathf.Lerp(calmBaseVolume, 0f, corruption);
        angerMusic.volume = Mathf.Lerp(0f, angerBaseVolume, corruption);
    }

    IEnumerator BasicTransitionRoutine()
    {
        response.UpdateText(clearType);
        int scene = GetScene();
        clearType = LevelClearType.None;

        canvas.LeanAlpha(1f, 2f).setEaseInOutSine();

        yield return new WaitForSeconds(4f);


        SceneManager.LoadScene(scene);

        yield return null;

        UpdateCorruption(0f);

        if (!response.CheckAnger())
        {
            if (FindObjectOfType<BlasphemyAttack>())
                FindObjectOfType<BlasphemyAttack>().enabled = false;
        }

        canvas.LeanAlpha(0f, 2f).setEaseInOutSine();
    }

    private int GetScene()
    {
        if (freeplay && (clearType == LevelClearType.Lose || clearType == LevelClearType.Skip))
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
        
        return SceneManager.GetActiveScene().buildIndex + 1;
    }
}
