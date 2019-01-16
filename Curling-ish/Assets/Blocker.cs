using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreLayerCollision(collision.gameObject.layer, (int)Mathf.Log(GameManager.instance.blockerLayer, 2), false);
    }

}
