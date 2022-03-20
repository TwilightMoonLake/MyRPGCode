using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTAGAIN : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("TRIGGER WORKING");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("No Problrm");
        }
    }
}
