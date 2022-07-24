using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStop : MyState
{
	public GameStop(MyStateController Controller) : base(Controller)
	{
		StateName = "Game Stop";
	}

	public override void StateStart()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0f;

		//AudioManager.

		ActiveTargetUi(UIManager.uis, "Pause UI", true);
	}

	public override void StateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameStatesManager.gameStateController.SetState(new GamePlay(GameStatesManager.gameStateController));
		}
	}

	public override void StateEnd()
	{
		Time.timeScale = 1f;

		ActiveTargetUi(UIManager.uis, "Pause UI", false);
	}
}
