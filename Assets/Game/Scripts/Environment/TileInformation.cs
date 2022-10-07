using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInformation : MonoBehaviour
{
    [SerializeField] public TileType tileType;
    [SerializeField] public Vector2Int[] shapeOffsets;
    public ParticleSystem particles;
}
