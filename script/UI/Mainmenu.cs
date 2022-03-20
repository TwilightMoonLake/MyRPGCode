using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class Mainmenu : MonoBehaviour
{
    // Start is called before the first frame update
    Button newGameButton;

    Button continurButton;

    Button quitButton;

    PlayableDirector director; //使用Timeline
    void Awake()
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continurButton = transform.GetChild(2).GetComponent<Button>();
        quitButton = transform.GetChild(3).GetComponent<Button>();
        newGameButton.onClick.AddListener(PlayTimeline);
        continurButton.onClick.AddListener(ContinurGame);
        quitButton.onClick.AddListener(QuitGame);
        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }
    void PlayTimeline()
    {
        director.Play();
    }
    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();

        SceneControl.Instance.TransitionToFirstLevel();
    }
    void ContinurGame()
    {
        SceneControl.Instance.TransitionLoadGame();
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("结束游戏");
    }
   
    
}
