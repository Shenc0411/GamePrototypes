using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.layer = LayerMask.NameToLayer("Unselectable");
    }

}
