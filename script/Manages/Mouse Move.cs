using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class Eventvector3 :UnityEvent<Vector2> { }
public class MouseMove : MonoBehaviour
{
    public Eventvector3 OnMouseClicked;
    // Start is called before the first frame update

    
}
