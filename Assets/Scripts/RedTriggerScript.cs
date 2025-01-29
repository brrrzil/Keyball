using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedTriggerScript : MonoBehaviour
{
    [SerializeField] private GameObject redPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Ball"))
        {
            if (gameObject.name.Equals("LeftTrigger"))
            {
                if (redPlayer.GetComponent<AIScript>()) redPlayer.GetComponent<AIScript>().IsLeft = true;
            }
            if (gameObject.name.Equals("RightTrigger")) 
            {
                if (redPlayer.GetComponent<AIScript>()) redPlayer.GetComponent<AIScript>().IsRight = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Ball"))
        {
            if (gameObject.name.Equals("LeftTrigger"))
            {
                if (redPlayer.GetComponent<AIScript>()) redPlayer.GetComponent<AIScript>().IsLeft = false;
            }
            if (gameObject.name.Equals("RightTrigger"))
            {
                if (redPlayer.GetComponent<AIScript>()) redPlayer.GetComponent<AIScript>().IsRight = false;
            }
        }
    }
}