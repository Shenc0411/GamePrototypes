using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform cannonBarrelTransform;

    public float distance;
    public float height;

    public float lerpRate;

    void LateUpdate()
    {
        Vector3 realPosition = cannonBarrelTransform.position;

        Vector3 forward = cannonBarrelTransform.forward - Vector3.Dot(cannonBarrelTransform.forward, Vector3.up) * Vector3.up;

        realPosition -= forward * distance;

        realPosition.y += height;

        Quaternion realRotation = Quaternion.LookRotation(cannonBarrelTransform.position + forward * 10f - realPosition);


        if (GameManager.instance.enableSmoothCameraFollow)
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, lerpRate * Time.deltaTime);
        }
        else
        {
            transform.position = realPosition;
        }

        if (GameManager.instance.enableSmoothCameraFollow)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, lerpRate * Time.deltaTime);
        }
        else
        {
            transform.rotation = realRotation;
        }

    }


}
