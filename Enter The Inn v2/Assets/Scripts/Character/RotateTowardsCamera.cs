using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{

    public float angleY = 90f;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate2Camera();
    }

    void Rotate2Camera()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.y = 0f;
        Vector3 pos = transform.position;
        pos.y = 0f;

        Vector3 dir2Camera = (cameraPos - pos).normalized;
        float angle2Camera = GetAngleFromVectorFloat(dir2Camera);
        transform.eulerAngles = new Vector3(0f, -angle2Camera + angleY, 0f);
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
