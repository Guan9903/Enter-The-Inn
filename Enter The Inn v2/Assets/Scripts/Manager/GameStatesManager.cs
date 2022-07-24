using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatesManager : Singleton<GameStatesManager>
{
    // 场景状态持有者
    public static MyStateController gameStateController = new MyStateController();


    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            gameStateController.SetState(new SceneMainMenu(gameStateController));
        }

        if (SceneManager.GetActiveScene().name == "Demo")
        {
            gameStateController.SetState(new GameReady(gameStateController));
        }

    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        gameStateController.StateUpdate();
    }

    public void ButtonResume()
    {
        gameStateController.SetState(new GamePlay(gameStateController));
    }

    public void ButtonMainMenu()
    {
        gameStateController.SetState(new SceneMainMenu(gameStateController), "Start");
    }

    public void ButtonRestart()
    {
        gameStateController.SetState(new GameReady(gameStateController), "Demo");
    }

    public void ButtonQuitGame()
    {
        Application.Quit();
    }

    public void ButtonSetting()
    {
        
    }

    public void ButtonStartGame()
    {
        
    }
}
