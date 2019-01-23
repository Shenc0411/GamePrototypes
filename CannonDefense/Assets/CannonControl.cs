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

    private void Awake()
    {
        fireTimer = 0.0f;
        bCanFire = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        

        //Rotate Barrel
        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * 0.5f, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);

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

            EZCameraShake.CameraShaker.Instance.ShakeOnce(3, 4, 0.1f, 1);

            bCanFire = false;
            
        }
    }
}
