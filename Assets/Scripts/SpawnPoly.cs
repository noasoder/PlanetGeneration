using UnityEngine;

public class SpawnPoly : MonoBehaviour
{
    //public Vector3 planetSpawnLocation;
    //public float planetRadius;
    public int index;

    public float tileThickness;
    public float tileRadius;
    public float tileSide;

    private float savedTileThickness = 0f;
    private float savedTileSide = 0f;

    [Range(3, 24)]
    public int corners = 3;

    Mesh mesh;

    public Vector3[] verticies;
    public int[] triangles;

    private void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape(corners);
        UpdateMesh();
        transform.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void Update()
    {
        if (corners != triangles.Length / 3 || tileSide != savedTileSide || tileThickness != savedTileThickness)
        {
            CreateShape(corners);
            UpdateMesh();
        }
    }

    void CalculateSide()
    {
        float alphaAngle = ((corners - 2) * 180) / (2 * corners);
        float betaAngle = 360f / corners;

        tileRadius = tileSide * (Mathf.Sin(alphaAngle * Mathf.Deg2Rad) / Mathf.Sin(betaAngle * Mathf.Deg2Rad));
    }

    void CreateShape(int corners)
    {
        savedTileThickness = tileThickness;
        savedTileSide = tileSide;

        CalculateSide();

        verticies = new Vector3[(corners + 1) * 2];


        verticies[0] = new Vector3(0, 0, tileThickness / 2); //center of shape
        int angle = 0;
        for (int i = 1; i < verticies.Length / 2; i++)
        {
            //converts from angle and length to vector
            Vector3 vector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * tileRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * tileRadius, tileThickness / 2f);
            verticies[i] = vector;
            angle += 360 / corners;
        }

        verticies[verticies.Length / 2] = new Vector3(0, 0, -tileThickness / 2); //center of shape
        angle = 0;
        for (int i = 1; i < verticies.Length / 2; i++)
        {
            //converts from angle and length to vector
            Vector3 vector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * tileRadius, Mathf.Sin(angle * Mathf.Deg2Rad) * tileRadius, -tileThickness / 2f);
            verticies[verticies.Length / 2 + i] = vector;
            angle += 360 / corners;
        }

        triangles = new int[corners * 2 * 3 * 2];

        //triangles for front
        for (int i = 0; i < corners; i++)
        {
            //adds 1 triangle (3 points)
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            if (i == corners - 1) //last triangle
            {
                triangles[i * 3 + 2] = 1;
            }
            else
            {
                triangles[i * 3 + 2] = i + 2;
            }
        }
        //output for corners = 3
        // 0, 1, 2
        // 0, 2, 3
        // 0, 3, 1

        //tris for back
        for (int i = 0; i < corners; i++)
        {
            //adds 1 triangle (3 points)
            int index = triangles.Length / 4 + (i * 3);
            triangles[index] = triangles.Length / 12 + 1;
            if (i == corners - 1) //last triangle
            {
                triangles[index + 1] = triangles.Length / 12 + 2;
            }
            else
            {
                triangles[index + 1] = triangles.Length / 12 + i + 3;
            }
            triangles[index + 2] = triangles.Length / 12 + i + 2;
        }
        // 4, 6, 5
        // 4, 7, 6
        // 4, 5, 7

        //tris for 1/2 sides
        for (int i = 0; i < corners; i++)
        {
            int index = (triangles.Length / 2) + i * 3;

            if (i == corners - 1) triangles[index] = 1;
            else triangles[index] = i + 2;
            triangles[index + 1] = i + 1;
            triangles[index + 2] = corners + i + 2;
        }
        // 2, 1, 5
        // 3, 2, 6
        // 1, 3, 7

        for (int i = 0; i < corners; i++)
        {
            int index = (triangles.Length / 2) + triangles.Length / 4 + i * 3;

            if (i == 0) triangles[index] = corners * 2 + 1;
            else triangles[index] = corners + i + 1;
            triangles[index + 1] = corners + i + 2;
            triangles[index + 2] = i + 1;
        }
        // 7, 5, 1
        // 5, 6, 2
        // 6, 7, 3
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}

