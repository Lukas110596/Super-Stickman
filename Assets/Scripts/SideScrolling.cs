using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

    

    // make the camera to follow the player and centralize player while running right
    private void LateUpdate()
    {
        player = GameObject.FindWithTag("Player").transform;
        Vector3 cameraPosition = transform.position;
        //cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
        cameraPosition.x = player.position.x;
        transform.position = cameraPosition;
    }
}
