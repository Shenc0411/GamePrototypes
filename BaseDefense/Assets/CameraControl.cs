using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float moveSpeed;
    public float zoomSpeed;

    public float cameraDistance;
    public float maxDistance = 20;

    private void Awake()
    {
        cameraDistance = maxDistance / 2.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");


        Vector3 movement = new Vector3(horizontalAxis * moveSpeed * Time.deltaTime, 0, verticalAxis * moveSpeed * Time.deltaTime);

        transform.position += movement;


        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        float distanceMovement = mouseScroll * zoomSpeed;

        if (cameraDistance + distanceMovement > maxDistance)
        {
            distanceMovement = maxDistance - cameraDistance;
            cameraDistance = maxDistance;
        }
        else if (cameraDistance + distanceMovement < 0)
        {
            distanceMovement = cameraDistance;
            cameraDistance = 0;
        }
        else
        {
            cameraDistance += distanceMovement;
        }


        transform.position += transform.forward * distanceMovement;

    }
}
