using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyFish : FishMovement
{
    public override bool EnemyPresent()
    {
        return FindObjectOfType<EnemyFish>();
    }

    public override GridPoint GetPath(List<GridPoint> options)
    {
        Vector3 clEnemy = GetClosestEnemy();

        Vector3 fleeDirection = (clEnemy - transform.position).normalized * -1f;

        GridPoint clGV = GetClosestVertex(options, fleeDirection + transform.position);

        return clGV;
    }

    private GridPoint GetClosestVertex(List<GridPoint> options, Vector3 pos)
    {
        float clGVDis = Mathf.Infinity;
        GridPoint clGV = null;

        foreach (GridPoint gPoint in options)
        {
            float dis = Vector3.Distance(pos, gPoint.transform.position);
            if (dis < clGVDis)
            {
                clGV = gPoint;
                clGVDis = dis;
            }
        }

        return clGV;
    }

    private Vector3 GetClosestEnemy()
    {
        Vector3 clEnemy = Vector3.zero;
        float clDis = Mathf.Infinity;

        foreach (EnemyFish eFish in FindObjectsOfType<EnemyFish>())
        {
            float v = Vector3.Distance(transform.position, eFish.transform.position);
            if (v < clDis)
            {
                clDis = v;
                clEnemy = eFish.transform.position;
            }
        }

        return clEnemy;
    }
}
