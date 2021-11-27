using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Garlic" || other.tag == "EnemyBat")
        {
            Destroy(other.gameObject);
        }
    }
}
