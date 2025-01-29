using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    private Vector3 ballSpeed;

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag.Contains("X"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.transform.position - gameObject.transform.position, ForceMode.Force);
        }

        if (gameObject.tag.Contains("Y"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.transform.position - gameObject.transform.position, ForceMode.Force);
        }
    }
}
