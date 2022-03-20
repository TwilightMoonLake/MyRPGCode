using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPoint : MonoBehaviour
{
    public GameObject enterLog;
    public enum TransitionType
    {
        SamScene,DifferentScene
    }
    [Header("Transition Info")]
    public string sceneName;

    public TransitionType transitionType;

    public TransitionDestination.DestinationTag destinationTag;
    private bool canTrans;
    public void OnTriggerStay(Collider other)
    {
        Debug.Log("进入了传送触发区域");
        if (other.gameObject.tag == ("Player"))
        {
            canTrans = true;
            enterLog.SetActive(true);
        }

    }
    public void OnTriggerExit(Collider other)
    {
        Debug.Log("离开了传送触发区域");
        if (other.gameObject.tag == "Player")
        {
            canTrans = false;
            enterLog.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&canTrans)
        {
            SceneControl.Instance.TransitionToDestination(this);
            Debug.Log("执行了传送");
            Debug.Log("是否可以传送"+canTrans);
        }
    }
}
