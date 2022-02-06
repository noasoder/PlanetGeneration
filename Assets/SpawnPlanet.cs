using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanet : MonoBehaviour
{
    [Range(3, 16)]
    public int polygonCorners = 3;
    public int polygonCount = 3;
    public float polygonSide = 1;
    public Vector3 rotation = Vector3.zero;

    private int savedPolygonCorners;
    private int savedPolygonCount;
    private float savedPolygonSide;
    private Vector3 savedRotation;

    public GameObject spawnPolygon;
    public List<GameObject> polygons;

    void Start()
    {
        polygons = new List<GameObject>();
        //spawn some polygons
        //polygons.Add(Instantiate(spawnPolygon, new Vector3(1, 0, 0), transform.rotation));

        //polygons[0].GetComponent<SpawnPoly>().corners = 5;

        SpawnCoolPolyStuff01();
    }

    void SpawnCoolPolyStuff01()
    {
        savedPolygonCorners = polygonCorners;
        savedPolygonCount = polygonCount;
        savedPolygonSide = polygonSide;
        savedRotation = rotation;

        for (int i = 0; i < polygonCount; i++)
        {
            polygons.Add(Instantiate(spawnPolygon, new Vector3(0, 0, 0.5f * i), Quaternion.Euler(rotation * i)));
            polygons[i].GetComponent<SpawnPoly>().corners = polygonCorners;
            polygons[i].GetComponent<SpawnPoly>().tileSide = polygonSide;
        }
    }

    void Update()
    {
        if (polygonCorners != savedPolygonCorners || polygonCount != savedPolygonCount || polygonSide != savedPolygonSide || rotation != savedRotation)
        {
            for (int i = 0; i < polygons.Count; i++)
            {
                Destroy(polygons[i].gameObject);
            }
            polygons.Clear();
            
            SpawnCoolPolyStuff01();
        }
    }
}
