using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileKeeper : MonoBehaviour
{
    [SerializeField] public List<TileInformation> tiles = new List<TileInformation>();
    public List<TileInformation> currentTiles = new List<TileInformation>();
    [SerializeField] int previewCount = 3;

    [SerializeField] Transform previewHolder;

    private void Start()
    {
        currentTiles.Clear();
        currentTiles = new List<TileInformation>(tiles);

        GeneratePreview();
    }

    public void GetTile(out TileInformation tile)
    {
        if (currentTiles.Count == 0)
        {
            currentTiles.Clear();
            currentTiles = new List<TileInformation>(tiles);
        }

        tile = currentTiles[currentTiles.Count - 1];
    }

    public void GeneratePreview()
    {
        if (currentTiles.Count == 0)
        {
            currentTiles.Clear();
            currentTiles = new List<TileInformation>(tiles);
        }

        for (int i = 0; i < previewCount; i++)
        {
            if (currentTiles.Count <= i)
                continue;

            GameObject tileHold = new GameObject();
            tileHold.transform.parent = previewHolder;

            TileInformation info = currentTiles[currentTiles.Count - 1 - i];
            TileInformation originTile = Instantiate(info, tileHold.transform);
            originTile.particles.Stop();
            originTile.particles.Clear();


            foreach (Vector2 offset in info.shapeOffsets)
            {
                TileInformation newTile = Instantiate(info, tileHold.transform);
                newTile.transform.localPosition = new Vector3(offset.x, 0f, offset.y);
                newTile.particles.Stop();
                newTile.particles.Clear();
            }

            tileHold.transform.localPosition = new Vector3(i * -3f, 0f, i * 3f);
            if (i != 0)
            tileHold.transform.localScale = Vector3.one / 2f;
        }
    }

    public void DeletePreview()
    {
        for(int i = 0; i < previewHolder.childCount; i++)
        {
            Destroy(previewHolder.GetChild(i).gameObject);
        }
    }
}
