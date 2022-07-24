using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MyState
{
	public GamePlay(MyStateController Controller) : base(Controller)
	{
		StateName = "Game Play";
	}

	public override void StateStart()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		ActiveTargetUi(UIManager.uis, "Weapon UI", true);
		ActiveTargetUi(UIManager.uis, "Player Info UI", true);
	}

	public override void StateUpdate() 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameStatesManager.gameStateController.SetState(new GameStop(GameStatesManager.gameStateController));
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			GameStatesManager.gameStateController.SetState(new GameMap(GameStatesManager.gameStateController));
		}
	}

	public override void StateEnd() { }
}
