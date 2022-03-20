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
    //生成敌人的时候加入列表，当敌人死亡的时候从列表中移除
    public void AddOberser(IEndGameObserver observer)//添加到列表中
    {
        endGameObservers.Add(observer);
    }
    public void RemoveOberser(IEndGameObserver observer)//添加到列表中
    {
        endGameObservers.Remove(observer);
    }
    public void NotifyObservers()//广播
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();//这样写可以保证每个调用的接口。所有的函数都会执行这个方法
       
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
