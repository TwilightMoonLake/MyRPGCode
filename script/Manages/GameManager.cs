using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{

    private CinemachineFreeLook followCinema;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public CharacterStats playerStat;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    public void RigisterPlayer(CharacterStats player)
    {
        playerStat = player;

        followCinema = FindObjectOfType<CinemachineFreeLook>();

        if (followCinema != null)
        {
            followCinema.Follow = playerStat.transform.GetChild(2);
            followCinema.LookAt = playerStat.transform.GetChild(2);
        }
    }
    //���ɵ��˵�ʱ������б�������������ʱ����б����Ƴ�
    public void AddOberser(IEndGameObserver observer)//��ӵ��б���
    {
        endGameObservers.Add(observer);
    }
    public void RemoveOberser(IEndGameObserver observer)//��ӵ��б���
    {
        endGameObservers.Remove(observer);
    }
    public void NotifyObservers()//�㲥
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();//����д���Ա�֤ÿ�����õĽӿڡ����еĺ�������ִ���������
       
        }
    }
    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
            {
                return item.transform;
            }
         
        }
        return null; 
    }
}
