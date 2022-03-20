using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testtigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("TEST TIGGER");
        if(other.gameObject.tag == "Players")
        {
            Debug.Log("TEST TIGGER AGAEN");
        }

    }

}
