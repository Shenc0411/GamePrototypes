using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public GameObject shotFXPrefab;
    public GameObject explosionFXPrefab;

    public float explosionRadius;
    public float explosionPower;

    public int damage;

    private bool bCanExplode;

    private void Awake()
    {
        bCanExplode = true;

        if (GameManager.instance.enableParticleEffect)
        {
            GameObject shotFX = Instantiate(shotFXPrefab, transform.position, Quaternion.identity);
            shotFX.transform.localScale /= 2.0f;
            if (!GameManager.instance.enableSoundEffect)
            {
                shotFX.GetComponent<AudioSource>().enabled = false;
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!bCanExplode)
        {
            return;
        }

        bCanExplode = false;

        Vector3 explosionPos = transform.position;
        if (GameManager.instance.enableParticleEffect)
        {
            GameObject explosionFX = Instantiate(explosionFXPrefab, explosionPos, Quaternion.identity);

            explosionFX.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);

            explosionFX.transform.localScale /= 4.0f;

            if (!GameManager.instance.enableSoundEffect)
            {
                explosionFX.GetComponent<AudioSource>().enabled = false;
            }
        }

        if (GameManager.instance.enableCameraShake)
        {
            EZCameraShake.CameraShaker.Instance.ShakeOnce(3, 4, 0.1f, 1);
        }
        

        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {

            if(hit.gameObject.layer == GameManager.instance.enemyLayer)
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                if (enemy != null)
                {
                    Rigidbody[] partRBs = enemy.OnDamage(1);

                    if(partRBs != null)
                    {
                        foreach (Rigidbody partRB in partRBs)
                        {
                            if (partRB != null)
                            {
                                partRB.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
                            }
                        }
                    }
                }
            }

            AutoDestroy AD = hit.GetComponent<AutoDestroy>();
            if(AD != null)
            {
                AD.OnHit();
            }

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
            }
                
        }
    }

}
