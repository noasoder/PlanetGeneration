using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject currentTile;
    public float speed;

    private void Update()
    {
        if (currentTile != null)
        {
            //lerp player position and rotation to current tile
            transform.position = Vector3.Lerp(transform.position, currentTile.transform.position + currentTile.transform.forward * 
                                 currentTile.GetComponent<TileStates>().offset, speed * Time.deltaTime);
            transform.up = Vector3.Lerp(transform.up, currentTile.transform.forward, speed * Time.deltaTime);
        }
    }
}
