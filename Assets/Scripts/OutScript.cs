using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutScript : MonoBehaviour
{
    [SerializeField] MatchScript matchScript;

    private static bool isOut = false;
    public bool IsOut { get { return isOut; } set { isOut = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Ball") && !isOut)
        {
            isOut = true;
            matchScript.OutStart();
        }
    }
}
