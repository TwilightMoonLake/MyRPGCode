using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//[System.Serializable]

//public class Eventvectop3 : UnityEvent<Vector3> { };
public class Mousemove : Singleton<Mousemove>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public Texture2D point, attack, doorway, target, arrow;// �������ͼƬ
   // public static Mousemove Instance;
    RaycastHit hitInfo;
    // Start is called before the first frame update
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;
    public void Update()
    {
        SetcorsueTexture();
        MouseControl();
    }
    //protected override void Awake()
    //{
    //    base.Awake();
    //}
    //private void Awake()
    //{
    //    if (Instance != null)
    //        Destroy(gameObject);
    //    Instance = this;

    //}
    void SetcorsueTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out hitInfo))
        {
            //�л�����ͼ
            switch (hitInfo.collider.gameObject.tag)
            {
                
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);
                   // Debug.Log("ִ���˵�����ͼ");
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack,new Vector2(16,16),CursorMode.Auto);
                   // Debug.Log("ִ����Ŀ����ͼ");
                    break;
                case "Attackable":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                   // Debug.Log("ִ����Ŀ����ͼ");
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
      
                    break;
                default:
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }
    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0)&&hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground")) 
            OnMouseClicked?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Enemy")) 
            OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);//���Թ���ʯͷ��
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
                OnMouseClicked?.Invoke(hitInfo.point);
        }
    }
}
