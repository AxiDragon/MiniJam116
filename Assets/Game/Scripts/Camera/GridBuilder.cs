using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] UnityEvent buildEndEvent;
    [HideInInspector] public bool canBuild = true;
    [SerializeField] AudioSource placeSFX;
    GridpointGetter gridpointGetter;
    GridGenerator gridGenerator;
    TileKeeper tileKeeper;

    void Start()
    {
        tileKeeper = GetComponent<TileKeeper>();
        gridpointGetter = FindObjectOfType<GridpointGetter>();
        gridGenerator = FindObjectOfType<GridGenerator>();
    }


    public void Build(InputAction.CallbackContext callback)
    {
        if (!callback.performed)
            return;

        if (!canBuild)
            return;

        GridPoint point = gridpointGetter.GetClosestPoint(false);

        if (point == null)
            return;

        Build(point);
    }

    public void Build(GridPoint point)
    {
        tileKeeper.GetTile(out TileInformation tile);

        if (tile != null)
            if (gridGenerator.Place(point, tile))
            {
                tileKeeper.currentTiles.RemoveAt(tileKeeper.currentTiles.Count - 1);
                buildEndEvent.Invoke();
                    placeSFX.pitch = UnityEngine.Random.Range(.6f, .9f);
                    placeSFX.Play();

                canBuild = false;
            }
    }
}
