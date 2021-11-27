using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garlic : MonoBehaviour
{
    private float _damage=10;
    public float damage { get { return _damage; } set { _damage = value; } }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
