using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    public GameObject planet;
    public GameObject cam;
    public float cameraZoom;
    public float speed;
    public float rotSpeed;

    public float lastPhi = 0;
    public float lastTheta = 0;

    public GameObject moveTo;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        planet = GameObject.FindGameObjectWithTag("Planet");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            MoveCamera();
        }
        if (moveTo != null && Input.GetKey(KeyCode.Alpha0))
        {
            //move camera
            cam.transform.position = Vector3.Lerp(cam.transform.position, moveTo.transform.position + moveTo.transform.forward * cameraZoom, speed * Time.deltaTime);
            cam.transform.LookAt(Vector3.Lerp(cam.transform.forward, moveTo.transform.position, rotSpeed * Time.deltaTime), cam.transform.up);
        }
    }

    void MoveCamera()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, player.transform.position + player.transform.up * cameraZoom, speed * Time.deltaTime);
        cam.transform.LookAt(Vector3.Lerp(cam.transform.forward, player.transform.position, rotSpeed * Time.deltaTime), cam.transform.up);
    }
}
