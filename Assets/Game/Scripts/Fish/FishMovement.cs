using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float yOffset = 3f;
    AudioSource fishMoveSFX;

    GridpointGetter getter;
    internal GridGenerator gen;

    public float moveTime = 2f;
    Vector2 pos;

    IEnumerator moveCoroutine;

    internal bool dead = false;

    private void Awake()
    {
        getter = FindObjectOfType<GridpointGetter>();
        gen = FindObjectOfType<GridGenerator>();
        fishMoveSFX = GetComponent<AudioSource>();
    }

    public void Start()
    {
        GridPoint closest = gen.GetClosestSnapPoint(transform.position, true);
        transform.parent = closest.transform;

        pos = new Vector2(closest.transform.position.x, closest.transform.position.z);
        transform.position = new Vector3(pos.x * gen.offset, yOffset, pos.y * gen.offset);
    }

    private GridPoint GetDestination()
    {
        List<GridPoint> options = GetOptionSpaces();

        if (options.Count == 0)
        {
            if (CheckForInfection())
                Die();
            return gen.GetClosestSnapPoint(transform.position, true);
        }

        if (EnemyPresent())
        {
            return GetPath(options);
        }
        else
        {
            int randomOption = UnityEngine.Random.Range(0, options.Count);
            return options[randomOption];
        }
    }

    public virtual GridPoint GetPath(List<GridPoint> options)
    {
        throw new NotImplementedException();
    }

    private List<GridPoint> GetOptionSpaces()
    {
        List<GridPoint> possible = new List<GridPoint>();
        Vector2[] sides = new Vector2[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

        foreach(Vector2 side in sides)
        {
            int x = Mathf.RoundToInt(side.x + pos.x);
            int y = Mathf.RoundToInt(side.y + pos.y);
            if (gen.CheckOccupied(x, y))
            {
                if (gen.grid[x][y].GetComponentInChildren<TileInformation>().tileType != TileType.Infected)
                    possible.Add(gen.grid[x][y]);
            }
        }

        return possible;
    }

    public virtual bool EnemyPresent()
    {
        //check for enemy
        return false;
    }

    public void Die()
    {
        StopAllCoroutines();
        dead = true;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator Move()
    {
        if (!dead)
        {
            GridPoint destination = GetDestination();
            Vector3 desPos = destination.transform.position;

            pos = new Vector2(desPos.x, desPos.z);
            Vector3 modDest = desPos + (Vector3.up * yOffset);

            transform.LookAt(modDest);

            if (!dead)
            {
                fishMoveSFX.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                fishMoveSFX.Play();
            }

            float timer = 0f;
            while (!dead && timer < moveTime && Vector3.Distance(transform.position, modDest) > .1f)
            {
                transform.position = Vector3.Slerp(transform.position, modDest, timer / moveTime);
                timer += Time.deltaTime;
                yield return null;
            }

            if (!dead)
            {
                transform.parent = destination.transform;
                transform.position = modDest;
                if (CheckForInfection())
                    Die();
            }
        }
    }

    private bool CheckForInfection()
    {
        TileInformation tI = transform.parent.GetComponentInChildren<TileInformation>();
        return tI.tileType == TileType.Infected;
    }
}
