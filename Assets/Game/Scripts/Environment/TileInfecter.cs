using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfecter : MonoBehaviour
{
    GridGenerator gen;

    private void Awake()
    {
        gen = FindObjectOfType<GridGenerator>();
    }

    public void InfectSurroundingTiles()
    {
        List<GridPoint> infectable = GetInfectableSpaces();

        foreach(GridPoint p in infectable)
            gen.Infect(p);
    }

    private List<GridPoint> GetInfectableSpaces()
    {
        List<GridPoint> possible = new List<GridPoint>();
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        Vector2[] sides = new Vector2[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

        foreach (Vector2 side in sides)
        {
            int x = Mathf.RoundToInt(side.x + pos.x);
            int y = Mathf.RoundToInt(side.y + pos.y);
            if (gen.CheckOccupied(x, y))
            {
                possible.Add(gen.grid[x][y]);
            }
        }

        return possible;
    }
}
