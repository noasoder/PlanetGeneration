using UnityEngine;

public class SpawnIcosahedron : MonoBehaviour
{
    GameObject planetBuilder;
    PlanetBuilder.SaveClass[] save;

    public GameObject pentagon;
    public GameObject[] pentagons;
    public float pentagonSide;
    public float pentagonThickness;

    public GameObject hexagon;
    public GameObject[] hexagons;
    public float hexagonSide;
    public float hexagonThickness;

    public float theta;
    public float phi;
    public Vector3 referenceCirMid;
    private float savedTheta;
    private float savedPhi;
    private Vector3 savedReferenceCirMid;

    public float sideLength;
    public float surfaceArea;
    public float volume;
    public float cRadius;
    public float mRadius;
    public float iRadius;

    private float savedSideLength = 0f;
    private float savedPentaSide = 0f;
    private float savedPentaThic = 0f;
    private float savedHexaSide = 0f;
    private float savedHexaThic = 0f;

    private void Start()
    {
        pentagons = new GameObject[12];
        hexagons = new GameObject[150];

        planetBuilder = GameObject.FindGameObjectWithTag("PlanetBuilder");
        save = planetBuilder.GetComponent<PlanetBuilder>().save;

        CalculateParameters();

        SpawnPentagons();
        UpdatePentagons();
        RotatePentagons();

        SpawnHexagons();
        UpdateHexagons();
        RotateHexagons();

        ParentPolygons();
    }

    public void CalculateParameters()
    {
        //calculate stuff

        surfaceArea = 5f * (Mathf.Pow(sideLength, 2f)) * Mathf.Sqrt(3);
        volume = 5f / 12f * (Mathf.Pow(sideLength, 3f)) * (3f + Mathf.Sqrt(5f));
        cRadius = sideLength / 4f * Mathf.Sqrt(10f + 2f * Mathf.Sqrt(5f));
        mRadius = sideLength / 4f * (1f + Mathf.Sqrt(5));
        iRadius = sideLength / 12f * Mathf.Sqrt(3f) * (3f + Mathf.Sqrt(5));
    }

    public void SpawnPentagons()
    {
        savedSideLength = sideLength;

        //instantiate two circles of pentagons on sphere
        for (int i = 0; i < 2; i++)
        {
            float theta;
            if (i == 0) theta = 63.5f;
            else theta = 180 - 63.5f;

            for (int j = 0; j < 5; j++)
            {
                float phi = 360f / 5f * j + 36f * i;

                //sphere stuff
                float x = cRadius * Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad);
                float y = cRadius * Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
                float z = cRadius * Mathf.Cos(theta * Mathf.Deg2Rad);

                Vector3 pos = new Vector3(x, y, z);

                //spawn pentagon
                pentagons[j + i * 5] = Instantiate(save[j + i * 5 + hexagons.Length].prefab, transform.position + (pos), Quaternion.Euler(new Vector3(0, 0, 0)));
                pentagons[j + i * 5].name = "Pentagon " + (j + i * 5);
                pentagons[j + i * 5].GetComponent<SpawnPoly>().index = j + i * 5 + 150;
            }
        }
        
        //end caps of icosahedron
        for (int j = 0; j < 2; j++)
        {
            float theta = 180f * j;
            float phi = 0;

            //sphere stuff
            float x = cRadius * Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad);
            float y = cRadius * Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
            float z = cRadius * Mathf.Cos(theta * Mathf.Deg2Rad);

            Vector3 pos = new Vector3(x, y, z);

            //spawn pentagon
            pentagons[j + 10] = Instantiate(save[j + 10 + hexagons.Length].prefab, transform.position + (pos), Quaternion.Euler(new Vector3(0, 0, 0)));
            pentagons[j + 10].name = "Pentagon " + (j + 10);
            pentagons[j + 10].GetComponent<SpawnPoly>().index = j + 10 + 150;
        }
    }

    public void RotatePentagons()
    {
        float angleX = 180 - Mathf.Acos(-Mathf.Sqrt(5) / 3);
        for (int i = 0; i < pentagons.Length; i++)
        {
            Vector3 rot = pentagons[i].transform.position - transform.position;
            pentagons[i].transform.forward = rot;
            pentagons[i].transform.Rotate(new Vector3(0, 0, 0));
        }

        //rotate stuff plz make for loop or something
        pentagons[0].transform.Rotate(new Vector3(0, 0, 0));
        pentagons[1].transform.Rotate(new Vector3(0, 0, -18));
        pentagons[2].transform.Rotate(new Vector3(0, 0, 18));
        pentagons[3].transform.Rotate(new Vector3(0, 0, -18));
        pentagons[4].transform.Rotate(new Vector3(0, 0, 18));
        pentagons[5].transform.Rotate(new Vector3(0, 0, 18));
        pentagons[6].transform.Rotate(new Vector3(0, 0, -18));
        pentagons[7].transform.Rotate(new Vector3(0, 0, 0));
        pentagons[8].transform.Rotate(new Vector3(0, 0, 18));
        pentagons[9].transform.Rotate(new Vector3(0, 0, -18));
        pentagons[10].transform.Rotate(new Vector3(0, 0, 36));
        pentagons[11].transform.Rotate(new Vector3(0, 0, 36));
    }

    public void UpdatePentagons()
    {
        savedPentaSide = pentagonSide;
        savedPentaThic = pentagonThickness;

        for (int i = 0; i < pentagons.Length; i++)
        {
            if (pentagons[i].GetComponent<SpawnPoly>() != null)
            {
                pentagons[i].GetComponent<SpawnPoly>().tileSide = pentagonSide;
                pentagons[i].GetComponent<SpawnPoly>().corners = 5;
                pentagons[i].GetComponent<SpawnPoly>().tileThickness = pentagonThickness;
            }
        }
    }

    public void ClearPentagons()
    {
        for (int i = 0; i < pentagons.Length; i++)
        {
            Destroy(pentagons[i]);
        }
    }

    public void SpawnHexagons()
    {
        for (int i = 0; i < 10; i++)
        {
            //instantiates row of 3 hexagons between 2 pentagons
            Vector3 pos;
            Vector3 pos2;
            Vector3 pos3;
            if (i == 4)
            {
                pos = (pentagons[i].transform.position + pentagons[0].transform.position) / 2;
                pos3 = (pentagons[0].transform.position + pos) / 2;

                pos3 += (pentagons[0].transform.position - pos) * 0.87f * 0.01f;
            }
            else if (i == 9)
            {
                pos = (pentagons[i].transform.position + pentagons[5].transform.position) / 2;
                pos3 = (pentagons[5].transform.position + pos) / 2;

                pos3 += (pentagons[5].transform.position - pos) * 0.87f * 0.01f;
            }
            else
            {
                pos = (pentagons[i].transform.position + pentagons[i + 1].transform.position) / 2;
                pos3 = (pentagons[i + 1].transform.position + pos) / 2;

                pos3 += (pentagons[i + 1].transform.position - pos) * 0.87f * 0.01f;
            }
            pos2 = (pentagons[i].transform.position + pos) / 2;
            pos2 += (pentagons[i].transform.position - pos) * 0.87f * 0.01f;

            pos *= phi;
            pos2 *= phi;
            pos3 *= phi;
            hexagons[i] = Instantiate(save[i].prefab, transform.position + pos, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[30 + i] = Instantiate(save[i + 30].prefab, transform.position + pos2, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[40 + i] = Instantiate(save[i + 40].prefab, transform.position + pos3, Quaternion.Euler(new Vector3(0, 0, 0)));

            hexagons[i].name = "Hexagon " + i;
            hexagons[30 + i].name = "Hexagon " + (30 + i);
            hexagons[40 + i].name = "Hexagon " + (40 + i);

            //unnecessary???
            hexagons[i].GetComponent<SpawnPoly>().index = i;
            hexagons[30 + i].GetComponent<SpawnPoly>().index = 30 + i;
            hexagons[40 + i].GetComponent<SpawnPoly>().index = 40 + i;
        }
        for (int i = 0; i < 5; i++)
        {
            Vector3 pos;
            Vector3 pos1;
            Vector3 pos2;
            Vector3 pos3;
            Vector3 pos4;
            Vector3 pos5;

            if (i == 0)
            {
                pos = (pentagons[i].transform.position + pentagons[i + 9].transform.position) / 2;
                pos3 = (pentagons[i + 9].transform.position + pos) / 2;

                pos3 += (pentagons[i + 9].transform.position - pos) * 0.87f * 0.01f;
            }
            else
            {
                pos = (pentagons[i].transform.position + pentagons[i + 4].transform.position) / 2;
                pos3 = (pentagons[i + 4].transform.position + pos) / 2;

                pos3 += (pentagons[i + 4].transform.position - pos) * 0.87f * 0.01f;
            }
            pos1 = (pentagons[i].transform.position + pentagons[i + 5].transform.position) / 2;

            pos2 = (pentagons[i].transform.position + pos) / 2;
            pos4 = (pentagons[i].transform.position + pos1) / 2;
            pos5 = (pentagons[i + 5].transform.position + pos1) / 2;

            pos2 += (pentagons[i].transform.position - pos) * 0.87f * 0.01f;
            pos4 += (pentagons[i].transform.position - pos1) * 0.87f * 0.01f;
            pos5 += (pentagons[i + 5].transform.position - pos1) * 0.87f * 0.01f;

            pos *= phi;
            pos1 *= phi;
            pos2 *= phi;
            pos3 *= phi;
            pos4 *= phi;
            pos5 *= phi;

            hexagons[10 + i] = Instantiate(save[i + 10].prefab, transform.position + pos, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[15 + i] = Instantiate(save[i + 15].prefab, transform.position + pos1, Quaternion.Euler(new Vector3(0, 0, 0)));

            hexagons[50 + i] = Instantiate(save[i + 50].prefab, transform.position + pos2, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[55 + i] = Instantiate(save[i + 55].prefab, transform.position + pos3, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[60 + i] = Instantiate(save[i + 60].prefab, transform.position + pos4, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[65 + i] = Instantiate(save[i + 65].prefab, transform.position + pos5, Quaternion.Euler(new Vector3(0, 0, 0)));

            hexagons[10 + i].name = "Hexagon " + (10 + i);
            hexagons[15 + i].name = "Hexagon " + (15 + i);
            hexagons[50 + i].name = "Hexagon " + (50 + i);
            hexagons[55 + i].name = "Hexagon " + (55 + i);
            hexagons[60 + i].name = "Hexagon " + (60 + i);
            hexagons[65 + i].name = "Hexagon " + (65 + i);

            hexagons[10 + i].GetComponent<SpawnPoly>().index = 10 + i;
            hexagons[15 + i].GetComponent<SpawnPoly>().index = 15 + i;
            hexagons[50 + i].GetComponent<SpawnPoly>().index = 50 + i;
            hexagons[55 + i].GetComponent<SpawnPoly>().index = 55 + i;
            hexagons[60 + i].GetComponent<SpawnPoly>().index = 60 + i;
            hexagons[65 + i].GetComponent<SpawnPoly>().index = 65 + i;
        }
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos;
            Vector3 pos2;
            Vector3 pos3;
            if (i < 5)
            {
                pos = (pentagons[i].transform.position + pentagons[10].transform.position) / 2;
                pos3 = (pentagons[10].transform.position + pos) / 2;

                pos3 += (pentagons[10].transform.position - pos) * 0.87f * 0.01f;
            }
            else
            {
                pos = (pentagons[i].transform.position + pentagons[11].transform.position) / 2;
                pos3 = (pentagons[11].transform.position + pos) / 2;

                pos3 += (pentagons[11].transform.position - pos) * 0.87f * 0.01f;
            }
            pos2 = (pentagons[i].transform.position + pos) / 2;
            pos2 += (pentagons[i].transform.position - pos) * 0.87f * 0.01f;

            pos *= phi;
            pos2 *= phi;
            pos3 *= phi;

            hexagons[20 + i] = Instantiate(save[i + 20].prefab, transform.position + pos, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[70 + i] = Instantiate(save[i + 70].prefab, transform.position + pos2, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[80 + i] = Instantiate(save[i + 80].prefab, transform.position + pos3, Quaternion.Euler(new Vector3(0, 0, 0)));

            hexagons[20 + i].name = "Hexagon " + (20 + i);
            hexagons[70 + i].name = "Hexagon " + (70 + i);
            hexagons[80 + i].name = "Hexagon " + (80 + i);

            hexagons[20 + i].GetComponent<SpawnPoly>().index = 20 + i;
            hexagons[70 + i].GetComponent<SpawnPoly>().index = 70 + i;
            hexagons[80 + i].GetComponent<SpawnPoly>().index = 80 + i;
        }

        //hexagons between rows of hexagons. fills 1 hole per loop
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos;
            Vector3 pos2;
            Vector3 pos3;
            if (i == 4)
            {
                pos = (hexagons[i].transform.position + hexagons[i + 20].transform.position) / 2;
                pos2 = (hexagons[i].transform.position + hexagons[20].transform.position) / 2;
                pos3 = (hexagons[i + 20].transform.position + hexagons[20].transform.position) / 2;
            }
            else if (i == 9)
            {
                pos = (hexagons[i].transform.position + hexagons[i + 20].transform.position) / 2;
                pos2 = (hexagons[i].transform.position + hexagons[25].transform.position) / 2;
                pos3 = (hexagons[i + 20].transform.position + hexagons[25].transform.position) / 2;
            }
            else
            {
                pos = (hexagons[i].transform.position + hexagons[i + 20].transform.position) / 2;
                pos2 = (hexagons[i].transform.position + hexagons[i + 21].transform.position) / 2;
                pos3 = (hexagons[i + 20].transform.position + hexagons[i + 21].transform.position) / 2;
            }

            Vector3 angle = (pos + ((pos2 + pos3) / 2f)) / 2f;

            pos += angle * theta * 0.7f;
            pos2 += angle * theta;
            pos3 += angle * theta;

            hexagons[90 + i] = Instantiate(save[i + 90].prefab, transform.position + pos, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[100 + i] = Instantiate(save[i + 100].prefab, transform.position + pos2, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[110 + i] = Instantiate(save[i + 110].prefab, transform.position + pos3, Quaternion.Euler(new Vector3(0, 0, 0)));

            hexagons[90 + i].transform.forward = angle;
            hexagons[100 + i].transform.forward = angle;
            hexagons[110 + i].transform.forward = angle;

            hexagons[90 + i].name = "Hexagon " + (90 + i);
            hexagons[100 + i].name = "Hexagon " + (100 + i);
            hexagons[110 + i].name = "Hexagon " + (120 + i);

            hexagons[90 + i].GetComponent<SpawnPoly>().index = 90 + i;
            hexagons[100 + i].GetComponent<SpawnPoly>().index = 100 + i;
            hexagons[110 + i].GetComponent<SpawnPoly>().index = 110 + i;
        }
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos;
            Vector3 pos2;
            Vector3 pos3;
            if (i == 4)
            {
                pos = (hexagons[i].transform.position + hexagons[10].transform.position) / 2;
                pos2 = (hexagons[i].transform.position + hexagons[i + 15].transform.position) / 2;
                pos3 = (hexagons[10].transform.position + hexagons[i + 15].transform.position) / 2;
            }
            else if (i == 9)
            {
                pos = (hexagons[i].transform.position + hexagons[10].transform.position) / 2;
                pos2 = (hexagons[i].transform.position + hexagons[15].transform.position) / 2;
                pos3 = (hexagons[10].transform.position + hexagons[15].transform.position) / 2;
            }
            else if (i < 4)
            {
                pos = (hexagons[i].transform.position + hexagons[i + 11].transform.position) / 2;
                pos2 = (hexagons[i].transform.position + hexagons[i + 15].transform.position) / 2;
                pos3 = (hexagons[i + 11].transform.position + hexagons[i + 15].transform.position) / 2;
            }
            else
            {
                pos = (hexagons[i].transform.position + hexagons[i + 6].transform.position) / 2;
                pos2 = (hexagons[i].transform.position + hexagons[i + 11].transform.position) / 2;
                pos3 = (hexagons[i + 6].transform.position + hexagons[i + 11].transform.position) / 2;
            }

            Vector3 angle = (pos + ((pos2 + pos3) / 2f)) / 2f;

            pos += angle * theta * 0.7f;
            pos2 += angle * theta;
            pos3 += angle * theta;

            hexagons[120 + i] = Instantiate(save[i + 120].prefab, transform.position + pos, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[130 + i] = Instantiate(save[i + 130].prefab, transform.position + pos2, Quaternion.Euler(new Vector3(0, 0, 0)));
            hexagons[140 + i] = Instantiate(save[i + 140].prefab, transform.position + pos3, Quaternion.Euler(new Vector3(0, 0, 0)));

            hexagons[120 + i].transform.forward = angle;
            hexagons[130 + i].transform.forward = angle;
            hexagons[140 + i].transform.forward = angle;

            hexagons[120 + i].name = "Hexagon " + (120 + i);
            hexagons[130 + i].name = "Hexagon " + (130 + i);
            hexagons[140 + i].name = "Hexagon " + (140 + i);

            hexagons[120 + i].GetComponent<SpawnPoly>().index = 120 + i;
            hexagons[130 + i].GetComponent<SpawnPoly>().index = 130 + i;
            hexagons[140 + i].GetComponent<SpawnPoly>().index = 140 + i;
        }
    }

    public void RotateHexagons()
    {
        for (int i = 0; i < hexagons.Length; i++)
        {
            if (i < 90)
            {
                Vector3 rot = hexagons[i].transform.position - transform.position;
                hexagons[i].transform.forward = rot;
                hexagons[i].transform.Rotate(new Vector3(0, 0, 0));
            }
        }
        hexagons[0].transform.Rotate(new Vector3(0, 0, 21));
        hexagons[1].transform.Rotate(new Vector3(0, 0, 2));
        hexagons[2].transform.Rotate(new Vector3(0, 0, 0));
        hexagons[3].transform.Rotate(new Vector3(0, 0, -2));
        hexagons[4].transform.Rotate(new Vector3(0, 0, -21));
        hexagons[5].transform.Rotate(new Vector3(0, 0, 2));
        hexagons[6].transform.Rotate(new Vector3(0, 0, 21));
        hexagons[7].transform.Rotate(new Vector3(0, 0, -21));
        hexagons[8].transform.Rotate(new Vector3(0, 0, -2));
        hexagons[9].transform.Rotate(new Vector3(0, 0, 0));

        hexagons[10 + 0].transform.Rotate(new Vector3(0, 0, -2));
        hexagons[10 + 1].transform.Rotate(new Vector3(0, 0, -2));
        hexagons[10 + 2].transform.Rotate(new Vector3(0, 0, -2));
        hexagons[10 + 3].transform.Rotate(new Vector3(0, 0, -2));
        hexagons[10 + 4].transform.Rotate(new Vector3(0, 0, 28));
        hexagons[10 + 5].transform.Rotate(new Vector3(0, 0, 2));
        hexagons[10 + 6].transform.Rotate(new Vector3(0, 0, 32));
        hexagons[10 + 7].transform.Rotate(new Vector3(0, 0, 2));
        hexagons[10 + 8].transform.Rotate(new Vector3(0, 0, 2));
        hexagons[10 + 9].transform.Rotate(new Vector3(0, 0, 2));

        hexagons[20 + 0].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[20 + 1].transform.Rotate(new Vector3(0, 0, -21));
        hexagons[20 + 2].transform.Rotate(new Vector3(0, 0, -2));
        hexagons[20 + 3].transform.Rotate(new Vector3(0, 0, 2));
        hexagons[20 + 4].transform.Rotate(new Vector3(0, 0, 21));
        hexagons[20 + 5].transform.Rotate(new Vector3(0, 0, -2));
        hexagons[20 + 6].transform.Rotate(new Vector3(0, 0, -21));
        hexagons[20 + 7].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[20 + 8].transform.Rotate(new Vector3(0, 0, 21));
        hexagons[20 + 9].transform.Rotate(new Vector3(0, 0, 2));

        for (int i = 0; i < 10; i++)
        {
            hexagons[30 + i].transform.rotation = hexagons[i].transform.rotation;
        }
        for (int i = 0; i < 10; i++)
        {
            hexagons[40 + i].transform.rotation = hexagons[i].transform.rotation;
        }
        for (int i = 0; i < 5; i++)
        {
            hexagons[50 + i].transform.rotation = hexagons[i + 10].transform.rotation;
        }
        for (int i = 0; i < 5; i++)
        {
            hexagons[55 + i].transform.rotation = hexagons[i + 10].transform.rotation;
        }
        for (int i = 0; i < 5; i++)
        {
            hexagons[60 + i].transform.rotation = hexagons[i + 15].transform.rotation;
        }
        for (int i = 0; i < 5; i++)
        {
            hexagons[65 + i].transform.rotation = hexagons[i + 15].transform.rotation;
        }
        for (int i = 0; i < 10; i++)
        {
            hexagons[70 + i].transform.rotation = hexagons[i + 20].transform.rotation;
        }
        for (int i = 0; i < 10; i++)
        {
            hexagons[80 + i].transform.rotation = hexagons[i + 20].transform.rotation;
        }
        hexagons[90 + 0].transform.Rotate(new Vector3(0, 0, 29));
        hexagons[90 + 1].transform.Rotate(new Vector3(0, 0, 51));
        hexagons[90 + 2].transform.Rotate(new Vector3(0, 0, 0));
        hexagons[90 + 3].transform.Rotate(new Vector3(0, 0, 6));
        hexagons[90 + 4].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[90 + 5].transform.Rotate(new Vector3(0, 0, 54));
        hexagons[90 + 6].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[90 + 7].transform.Rotate(new Vector3(0, 0, 31));
        hexagons[90 + 8].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[90 + 9].transform.Rotate(new Vector3(0, 0, 0));

        hexagons[100 + 0].transform.Rotate(new Vector3(0, 0, 29));
        hexagons[100 + 1].transform.Rotate(new Vector3(0, 0, 51));
        hexagons[100 + 2].transform.Rotate(new Vector3(0, 0, 0));
        hexagons[100 + 3].transform.Rotate(new Vector3(0, 0, 6));
        hexagons[100 + 4].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[100 + 5].transform.Rotate(new Vector3(0, 0, 54));
        hexagons[100 + 6].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[100 + 7].transform.Rotate(new Vector3(0, 0, 31));
        hexagons[100 + 8].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[100 + 9].transform.Rotate(new Vector3(0, 0, 0));

        hexagons[110 + 0].transform.Rotate(new Vector3(0, 0, 29));
        hexagons[110 + 1].transform.Rotate(new Vector3(0, 0, 51));
        hexagons[110 + 2].transform.Rotate(new Vector3(0, 0, 0));
        hexagons[110 + 3].transform.Rotate(new Vector3(0, 0, 6));
        hexagons[110 + 4].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[110 + 5].transform.Rotate(new Vector3(0, 0, 54));
        hexagons[110 + 6].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[110 + 7].transform.Rotate(new Vector3(0, 0, 31));
        hexagons[110 + 8].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[110 + 9].transform.Rotate(new Vector3(0, 0, 0));


        hexagons[120 + 0].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[120 + 1].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[120 + 2].transform.Rotate(new Vector3(0, 0, 0));
        hexagons[120 + 3].transform.Rotate(new Vector3(0, 0, 37));
        hexagons[120 + 4].transform.Rotate(new Vector3(0, 0, 52));
        hexagons[120 + 5].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[120 + 6].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[120 + 7].transform.Rotate(new Vector3(0, 0, 52));
        hexagons[120 + 8].transform.Rotate(new Vector3(0, 0, 37));
        hexagons[120 + 9].transform.Rotate(new Vector3(0, 0, 0));

        hexagons[130 + 0].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[130 + 1].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[130 + 2].transform.Rotate(new Vector3(0, 0, 0));
        hexagons[130 + 3].transform.Rotate(new Vector3(0, 0, 37));
        hexagons[130 + 4].transform.Rotate(new Vector3(0, 0, 52));
        hexagons[130 + 5].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[130 + 6].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[130 + 7].transform.Rotate(new Vector3(0, 0, 52));
        hexagons[130 + 8].transform.Rotate(new Vector3(0, 0, 37));
        hexagons[130 + 9].transform.Rotate(new Vector3(0, 0, 0));

        hexagons[140 + 0].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[140 + 1].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[140 + 2].transform.Rotate(new Vector3(0, 0, 0));
        hexagons[140 + 3].transform.Rotate(new Vector3(0, 0, 37));
        hexagons[140 + 4].transform.Rotate(new Vector3(0, 0, 52));
        hexagons[140 + 5].transform.Rotate(new Vector3(0, 0, 30));
        hexagons[140 + 6].transform.Rotate(new Vector3(0, 0, 9));
        hexagons[140 + 7].transform.Rotate(new Vector3(0, 0, 52));
        hexagons[140 + 8].transform.Rotate(new Vector3(0, 0, 37));
        hexagons[140 + 9].transform.Rotate(new Vector3(0, 0, 0));
    }

    public void UpdateHexagons()
    {
        savedTheta = theta;
        savedPhi = phi;
        savedReferenceCirMid = referenceCirMid;
        savedHexaSide = hexagonSide;
        savedHexaThic = hexagonThickness;

        for (int i = 0; i < hexagons.Length; i++)
        {
            if (hexagons[i].GetComponent<SpawnPoly>() != null)
            {
                hexagons[i].GetComponent<SpawnPoly>().tileSide = hexagonSide;
                hexagons[i].GetComponent<SpawnPoly>().corners = 6;
                hexagons[i].GetComponent<SpawnPoly>().tileThickness = hexagonThickness;
            }
        }
    }

    public void ClearHexagons()
    {
        for (int i = 0; i < hexagons.Length; i++)
        {
            Destroy(hexagons[i]);
        }
    }

    public void ParentPolygons()
    {
        for (int i = 0; i < pentagons.Length; i++)
        {
            pentagons[i].transform.parent = transform;
        }
        for (int i = 0; i < hexagons.Length; i++)
        {
            hexagons[i].transform.parent = transform;
        }
    }

    private void Update()
    {
        if (sideLength != savedSideLength || pentagonSide != savedPentaSide ||
            pentagonThickness != savedPentaThic || hexagonSide != savedHexaSide ||
            hexagonThickness != savedHexaThic || theta != savedTheta || phi != savedPhi || 
            referenceCirMid != savedReferenceCirMid)
        {
            CalculateParameters();

            ClearPentagons();
            SpawnPentagons();
            UpdatePentagons();
            RotatePentagons();

            ClearHexagons();
            SpawnHexagons();
            UpdateHexagons();
            RotateHexagons();

            ParentPolygons();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotatePentagons();
        }
    }
}