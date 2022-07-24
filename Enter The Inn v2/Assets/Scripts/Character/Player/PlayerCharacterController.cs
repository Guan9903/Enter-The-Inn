using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("角色控制器")]
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = 20f;
    public AudioClip walkAudio;


    [Header("玩家摄像机")]
    public float mouseSensitivity = 100f;
    public float rotateMinRange = -60f;
    public float rotateMaxRange = 60f;
    public Transform player;
    public Transform fpsCamera;

    private AudioSource source;
    private Vector3 move = Vector3.zero;
    private float xRotation = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        fpsCamera = transform.Find("Main Camera");
        source = GetComponent<AudioSource>();
        //Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
            PlayerMovement();
    }

    void LateUpdate()
    {   
        CameraMovement();
        //Vector3 lineOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        //Debug.DrawRay(lineOrigin, fpsCamera.transform.forward * 100f, Color.green);
    }

    void PlayerMovement()
    {
        if (controller.isGrounded)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //解决斜向速度变快问题
            Vector3 output = Vector3.zero;
            output.x = x * Mathf.Sqrt(1 - (z * z) / 2.0f);
            output.z = z * Mathf.Sqrt(1 - (x * x) / 2.0f);

            move = transform.right * output.x + transform.forward * output.z;

            move *= speed;

            if (output != Vector3.zero && !source.isPlaying)
            {
                source.clip = walkAudio;
                source.Play();
            }
            else if(output == Vector3.zero)
            {
                source.Stop();
            }
        }

        move.y -= gravity * Time.deltaTime;
        controller.Move(move * Time.deltaTime);

    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, rotateMinRange, rotateMaxRange);

        fpsCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

}
