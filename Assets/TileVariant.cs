using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVariant : MonoBehaviour
{
    public Tile[] tiles;

    [System.Serializable]
    public class Tile
    {
        public string tileName;
        public GameObject model;
    }
    
}
