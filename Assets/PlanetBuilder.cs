using UnityEngine;

public class PlanetBuilder : MonoBehaviour
{
    GameObject planet;
    SpawnIcosahedron sp;
    public GameObject selectedTile;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;

    public GameObject selectedObj;

    public SaveClass[] save;

    [System.Serializable]
    public class SaveClass
    {
        public int index;
        public string objectName;
        public GameObject prefab;

        public SaveClass(int i, string o, GameObject pref)
        {
            index = i;
            objectName = o;
            prefab = pref;
        }
        public SaveClass(int i, GameObject pref)
        {
            index = i;
            prefab = pref;
        }
    }
    private void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet");
        sp = planet.GetComponent<SpawnIcosahedron>();
        for (int i = 0; i < save.Length; i++)
        {
            if (save[i].prefab == null)
            {
                save[i].prefab = obj1;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetList();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(selectedTile != null)
            {
                selectedObj = obj1;
                CreateTile();
                UpdatePlanet();
                selectedObj = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (selectedTile != null)
            {
                selectedObj = obj2;
                CreateTile();
                UpdatePlanet();
                selectedObj = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (selectedTile != null)
            {
                selectedObj = obj3;
                CreateTile();
                UpdatePlanet();
                selectedObj = null;
            }
        }
    }

    void CreateTile()
    {
        SaveClass newTileSave = new SaveClass(selectedTile.GetComponent<SpawnPoly>().index, selectedObj);
        save[newTileSave.index] = newTileSave;
    }
    void UpdatePlanet()
    {
        sp.CalculateParameters();

        sp.ClearPentagons();
        sp.SpawnPentagons();
        sp.UpdatePentagons();
        sp.RotatePentagons();

        sp.ClearHexagons();
        sp.SpawnHexagons();
        sp.UpdateHexagons();
        sp.RotateHexagons();

        sp.ParentPolygons();
    }

    void ResetList()
    {
        for (int i = 0; i < save.Length; i++)
        {
            save[i].prefab = obj1;
        }
        UpdatePlanet();
    }
}
