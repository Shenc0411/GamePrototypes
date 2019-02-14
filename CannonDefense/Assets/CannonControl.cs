using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    public float rotationSpeed = 2.0f;


    public float fireInterval = 1.0f;
    private float fireTimer;
    private bool bCanFire;

    public int ammo = 10;

    public GameObject cannonBallPrefab;
    public Transform openningTransform;

    public RectTransform reloadBackground;
    public RectTransform reloadBar;

    private float xRot;
    private float yRot;

    private void Awake()
    {
        fireTimer = 0.0f;
        bCanFire = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        xRot = 0;
        yRot = 0;
    }

    // Update is called once per frame
    void Update()
    {



        //Rotate Barrel

        xRot -= Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed;
        yRot += Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        xRot = Mathf.Clamp(xRot, -15, 10);

        transform.localRotation = Quaternion.Euler(xRot, yRot, 0);


        //Reload
        if (!bCanFire)
        {
            fireTimer += Time.deltaTime;

            reloadBar.sizeDelta = new Vector2(fireTimer / fireInterval * reloadBackground.sizeDelta.x, reloadBackground.sizeDelta.y);

            if (fireTimer >= fireInterval)
            {
                fireTimer = 0.0f;
                bCanFire = true;
            }

            
        }

        //Check Input
        if (Input.GetKeyDown(KeyCode.Space) && bCanFire)
        {

            //Fire!!!
            GameObject cannonBall = Instantiate(cannonBallPrefab, openningTransform.position, Quaternion.identity);
            cannonBall.GetComponent<Rigidbody>().AddForce(transform.forward * 40f, ForceMode.Impulse);

            if (GameManager.instance.enableCameraShake)
            {
                EZCameraShake.CameraShaker.Instance.ShakeOnce(3, 4, 0.1f, 1);
            }

            bCanFire = false;
            
        }
    }
}
