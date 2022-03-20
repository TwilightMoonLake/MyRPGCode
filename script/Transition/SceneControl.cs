using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneControl : Singleton<SceneControl>,IEndGameObserver
{
    bool fadeFinished;
    public GameObject playerPrefab;
    GameObject player;
    NavMeshAgent playerAgent;
    public SceneFade sceneFade;
    protected override void Awake() //保护数据
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        GameManager.Instance.AddOberser(this);
        fadeFinished = true;
        
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SamScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName,transitionPoint.destinationTag));
                break;
        }
    }
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
       SaveManager.Instance.SavePlayerData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName); //在这一帧在等待什么事件的完成，在完成之后执行下面的所有命令

            yield return Instantiate(playerPrefab,GetDestination(destinationTag).transform.position,GetDestination(destinationTag).transform.rotation);
            Debug.Log("生成了Player");
            SaveManager.Instance.LoadPlayerData();
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStat.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
    }
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }
        return null;
    }
    public void TransitionToMainScene()
    {
        StartCoroutine(loadMain());
    }
    public void TransitionLoadGame()
    {
        StartCoroutine(Loadlevel(SaveManager.Instance.SceneName));

    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(Loadlevel("rpg"));
    }
    IEnumerator Loadlevel(string scene)
    {
        SceneFade fade = Instantiate(sceneFade);
        if (scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2.5f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab,GameManager.Instance.GetEntrance().position,GameManager.Instance.GetEntrance().rotation);
            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeIn(2.5f));
            yield break;
        }
    }
    IEnumerator loadMain()
    {
        SceneFade fade = Instantiate(sceneFade);
        yield return StartCoroutine(fade.FadeOut(2.5f));
        yield return SceneManager.LoadSceneAsync("MainScene");
        yield return StartCoroutine(fade.FadeIn(2.5f));
        yield break;
    }

    public void EndNotify()
    {
        if (fadeFinished)
        {
            StartCoroutine(loadMain());
            fadeFinished = false;
        }
    }
}
