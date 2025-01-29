using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    [SerializeField] private GameObject playerBlue,playerRed;
    [SerializeField] private MatchScript matchScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Ball"))
        {
            if (gameObject.name.Contains("Red"))
            {
                matchScript.GoalCoroutine("Blue");
            }

            if (gameObject.name.Contains("Blue"))
            {
                matchScript.GoalCoroutine("Red");
            }
        } 
    }
}