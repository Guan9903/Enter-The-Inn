using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform player;

    public Camera miniCam;

    public static bool restart = false;

    private float defaultSize;

    // Start is called before the first frame update
    void Start()
    {
        miniCam = GetComponent<Camera>();
        defaultSize = miniCam.orthographicSize;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            miniCam.orthographicSize -= 10;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            miniCam.orthographicSize += 10;
        }
        if (restart == true)
        {
            miniCam.orthographicSize = defaultSize;
            restart = false;
        }
    }

    void FollowPlayer()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
