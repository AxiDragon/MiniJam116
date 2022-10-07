using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlasphemyAttack : MonoBehaviour
{
    GridpointGetter getter;
    GridGenerator gen;
    Transform par;
    Vector3 parStart;

    [SerializeField] GameObject explosion;
    [SerializeField] Transform ikTarget;
    [SerializeField] Transform ikHint;
    [SerializeField] float shakeTime = 8f;
    [SerializeField] float recoverTime = 8f;
    [SerializeField] float attackTime = 5f;
    [SerializeField] AudioSource smash;

    Vector3 neutralPos;
    bool attacking = false;

    void Awake()
    {
        par = transform.parent;
        parStart = par.position;
        getter = FindObjectOfType<GridpointGetter>();    
        gen = FindObjectOfType<GridGenerator>();
    }

    private void Start()
    {
        neutralPos = ikTarget.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            StartAttack();
    }

    [ContextMenu("StartAttack")]
    public void StartAttack()
    {
        if (attacking)
            return;

        GridPoint attackPoint = getter.GetClosestPoint(true);
        if (attackPoint != null)
        {
            attacking = true;
            StopAllCoroutines();
            StartCoroutine(LaunchAttack(attackPoint));
            FindObjectOfType<TransitionLevels>().SetClearType(LevelClearType.Blasphemous);
        }
    }

    IEnumerator LaunchAttack(GridPoint point)
    {
        Vector3 target = point.transform.position;
        ikTarget.position = target + Vector3.up * 10f;

        float timer = 0f;
        while (timer < attackTime)
        {
            ikTarget.position = Vector3.Lerp(target + Vector3.up * 10f, target, Mathf.Pow(timer, 2f) / Mathf.Pow(attackTime, 2f));
            ikHint.position = ikTarget.position + Vector3.up * 100f;
            timer += Time.deltaTime;
            yield return null;
        }

        Instantiate(explosion, target, Quaternion.identity);

        smash.pitch = UnityEngine.Random.Range(.4f, .7f);
        smash.Play();
        
        ExecuteAttack(point);

        if (FindObjectOfType<TransitionLevels>())
            FindObjectOfType<TransitionLevels>().UpdateCorruption(.1f);

        FindObjectOfType<ObjectiveChecker>().CheckFriendlyFishAmount();

        timer = 0f;

        while (timer < shakeTime)
        {
            Vector3 randomVector = GetRandomVector(.2f);
            ikTarget.position = target + randomVector;
            Vector3 randomParVector = GetRandomVector(8f);
            par.position = parStart + randomParVector;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        attacking = false;

        par.position = parStart;

        timer = 0f;
        
        while (timer < recoverTime)
        {
            ikTarget.position = Vector3.Lerp(target, neutralPos, Mathf.Pow(timer, 2f) / Mathf.Pow(attackTime, 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        ikTarget.position = neutralPos;
        ikHint.position = ikTarget.position + Vector3.up * 100f;

    }

    private Vector3 GetRandomVector(float magnitude)
    {
        float x = UnityEngine.Random.Range(-magnitude, magnitude);
        float y = UnityEngine.Random.Range(-magnitude, magnitude);
        float z = UnityEngine.Random.Range(-magnitude, magnitude);
        return new Vector3(x, y, z);
    }

    private void ExecuteAttack(GridPoint point)
    {
        List<GridPoint> affected = new List<GridPoint>();
        List<Vector2> sides = new List<Vector2>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j < 1; j++)
            {
                sides.Add(new Vector2(i, j));
            }
        }

        foreach (Vector2 side in sides)
        {
            int x = Mathf.RoundToInt(side.x + point.transform.position.x);
            int y = Mathf.RoundToInt(side.y + point.transform.position.z);
            if (gen.CheckOccupied(x, y))
            {
                affected.Add(gen.grid[x][y]);
            }
        }

        foreach(GridPoint affTile in affected)
        {
            gen.Infect(affTile);
        }

        for (int i = 0; i < point.transform.childCount; i++)
        {
            Destroy(point.transform.GetChild(i).gameObject);
        }
    }
}
