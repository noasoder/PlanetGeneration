using UnityEngine;

public class MouseHover : MonoBehaviour
{
    GameObject cam;
    GameObject player;
    GameObject planetBuilder;

    public bool canStepTo = true;

    public Vector3 originalPos = new Vector3(0, 0, 0);
    public Vector3 extendedPos = new Vector3(0, 0, 0);
    public float extendAmount;
    public float time;
    bool haveMoved = false;

    float timeCounter;
    bool wantToRetract = false;
    bool wantToExtend = false;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("CamHandler");
        player = GameObject.FindGameObjectWithTag("Player");
        planetBuilder = GameObject.FindGameObjectWithTag("PlanetBuilder");

        originalPos = transform.position;
        extendedPos = transform.position + transform.forward * extendAmount;
    }
    private void OnMouseOver()
    {
        if (canStepTo)
        {
            if (haveMoved == false)
            {
                timeCounter = time;
                wantToExtend = true;
                wantToRetract = false;

                haveMoved = true;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //move here
                cam.GetComponent<CameraMove>().moveTo = gameObject;
                player.GetComponent<PlayerMove>().currentTile = gameObject;
            }
        }
        
        //for changing and building tiles
        planetBuilder.GetComponent<PlanetBuilder>().selectedTile = gameObject;
    }
    private void OnMouseExit()
    {
        timeCounter = time;
        wantToRetract = true;
        wantToExtend = false;
        haveMoved = false;
        planetBuilder.GetComponent<PlanetBuilder>().selectedTile = null;
    }
    private void Update()
    {
        if (timeCounter > 0)
        {
            if (wantToRetract == true)
            {
                transform.position = Vector3.Lerp(transform.position, originalPos, timeCounter - Time.deltaTime);
                //transform.Translate(new Vector3(0, 0, -Mathf.Lerp(0, extendAmount, time - Time.deltaTime)));
            }
            if (wantToExtend == true)
            {
                transform.position = Vector3.Lerp(transform.position, extendedPos, timeCounter - Time.deltaTime);
                //transform.Translate(new Vector3(0, 0, Mathf.Lerp(0, extendAmount, time - Time.deltaTime)));
            }
            timeCounter -= Time.deltaTime;
        }
    }
}