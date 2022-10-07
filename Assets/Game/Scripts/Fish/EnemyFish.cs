using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFish : FishMovement
{
    public override bool EnemyPresent()
    {
        return FindObjectOfType<FriendlyFish>();
    }

    public override GridPoint GetPath(List<GridPoint> options)
    {
        Vector3 clEnemy = GetClosestEnemy().transform.position;

        GridPoint clGV = GetClosestVertex(options, clEnemy);

        return clGV;
    }

    private void Update()
    {
        if (!EnemyPresent())
            return;

        if (Vector3.Distance(transform.position, GetClosestEnemy().transform.position) < .2f)
        {
            Kill();
        }
    }

    private void Kill()
    {
        GetClosestEnemy().Die();
        FindObjectOfType<ObjectiveChecker>().CheckFriendlyFishAmount();
    }

    private GridPoint GetClosestVertex(List<GridPoint> options, Vector3 pos)
    {
        float clGVDis = Mathf.Infinity;
        GridPoint clGV = null;

        foreach (GridPoint v in options)
        {
            float dis = Vector3.Distance(pos, v.transform.position);
            if (dis < clGVDis)
            {
                clGV = v;
                clGVDis = dis;
            }
        }

        return clGV;
    }

    private FriendlyFish GetClosestEnemy()
    {
        FriendlyFish clEnemy = null;
        float clDis = Mathf.Infinity;

        foreach (FriendlyFish fFish in FindObjectsOfType<FriendlyFish>())
        {
            float v = Vector3.Distance(transform.position, fFish.transform.position);
            if (v < clDis)
            {
                clDis = v;
                clEnemy = fFish;
            }
        }

        return clEnemy;
    }
}
