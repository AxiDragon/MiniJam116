using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public List<List<GridPoint>> grid = new List<List<GridPoint>>();
    [SerializeField] int gridWidth = 5;
    [SerializeField] int gridLength = 5;
    [SerializeField] public float offset = 1f;
    [SerializeField] LevelSpawn[] spawns;
    [SerializeField] GridPoint basePoint;
    [SerializeField] TileInformation infectedTile;
    [SerializeField] GameObject gridPointIndicator;
    ObjectiveChecker checker;

    [Serializable]
    public struct LevelSpawn
    {
        public int x;
        public int y;
        public TileInformation tile;
    }

    private void Awake()
    {
        checker = FindObjectOfType<ObjectiveChecker>();
        GenerateGrid();
        PlaceStartingTiles();
    }

    private void PlaceStartingTiles()
    {
        foreach (LevelSpawn spawn in spawns)
        {
            Place(grid[spawn.x][spawn.y], spawn.tile);
        }
    }

    private void GenerateGrid()
    {
        grid.Clear();
        for (int i = 0; i < gridLength; i++)
        {
            List<GridPoint> list = new List<GridPoint>();
            for (int j = 0; j < gridWidth; j++)
            {
                Vector3 pos = new Vector3(i * offset, 0f, j * offset);
                GridPoint v = Instantiate(basePoint, pos, Quaternion.identity);
                Instantiate(gridPointIndicator, v.transform.position, Quaternion.identity);
                v.name = $"Point {i} {j}";
                list.Add(v);
            }
            grid.Add(list);
        }
    }

    public GridPoint GetClosestSnapPoint(Vector3 hitPoint, bool occ)
    {
        float closest = Mathf.Infinity;
        GridPoint closestGridVertex = null;

        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[i].Count; j++)
            {
                if (grid[i][j].transform.childCount == (occ ? 0 : 1))
                    continue;

                if (Vector3.Distance(hitPoint, grid[i][j].transform.position) < closest)
                {
                    closestGridVertex = grid[i][j];
                    closest = Vector3.Distance(hitPoint, grid[i][j].transform.position);
                }
            }
        }

        return closestGridVertex;
    }

    public bool Place(GridPoint point, TileInformation tile)
    {
        GetCoords(point, out int x, out int y);

        if (CheckOverlap(x, y, tile.shapeOffsets))
            return false;

        PlaceTiles(x, y, tile.shapeOffsets, tile);
        return true;
    }

    private void PlaceTiles(int x, int y, Vector2Int[] shapeOffsets, TileInformation tile)
    {
        Instantiate(tile, grid[x][y].transform);

        foreach (Vector2Int p in shapeOffsets)
        {
            if (x + p.x >= grid.Count || y + p.y >= grid[0].Count || x + p.x < 0 || y + p.y < 0)
            {
                Vector3 extraOffset = new Vector3((float)p.x * offset, 0f, (float)p.y);
                Instantiate(tile, grid[x][y].transform.position + extraOffset, Quaternion.identity);
                continue;
            }

            Instantiate(tile, grid[x + p.x][y + p.y].transform);
        }
    }

    public bool CheckCompletion()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[i].Count; j++)
            {
                if (grid[i][j].transform.childCount == 0)
                    return false;
            }
        }

        return true;
    }

    private bool CheckOverlap(int x, int y, Vector2Int[] shapeOffsets)
    {
        if (grid[x][y].transform.childCount > 0)
            return true;

        foreach (Vector2Int p in shapeOffsets)
        {
            if (x + p.x >= grid.Count || y + p.y >= grid[0].Count || x + p.x < 0 || y + p.y < 0)
                continue;

            if (grid[x + p.x][y + p.y].transform.childCount > 0)
                return true;
        }

        return false;
    }

    public bool CheckOccupied(GridPoint point)
    {
        GetCoords(point, out int x, out int y);

        if (x >= grid.Count || y >= grid[0].Count || x < 0 || y < 0)
            return false;

        return grid[x][y].transform.childCount > 0;
    }

    public bool CheckOccupied(int x, int y)
    {
        if (x >= grid.Count || y >= grid[0].Count || x < 0 || y < 0)
            return false;

        return grid[x][y].transform.childCount > 0;
    }

    private void GetCoords(GridPoint point, out int x, out int y)
    {
        Vector3 pointPos = point.transform.position;
        x = Mathf.RoundToInt(pointPos.x);
        y = Mathf.RoundToInt(pointPos.z);
    }

    public void Infect(GridPoint infected)
    {
        for (int i = 0; i < infected.transform.childCount; i++)
        {
            if (!infected.transform.GetChild(i).GetComponent<FishMovement>())
            Destroy(infected.transform.GetChild(i).gameObject);
        }

        Instantiate(infectedTile, infected.transform);
    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.green;

        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                Vector3 pos = new Vector3(i * offset, 0f, j * offset);
                Gizmos.DrawSphere(pos, .1f);
            }
        }

        Gizmos.color = Color.red;

        for (int i = 0; i < spawns.Length; i++)
        {
            Vector3 pos = new Vector3(spawns[i].x, 0f, spawns[i].y * offset);
            Gizmos.DrawSphere(pos, .25f);
        }
    }
}
