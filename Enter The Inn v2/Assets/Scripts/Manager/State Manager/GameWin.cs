using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : MyState
{
    public GameWin(MyStateController controller) : base(controller)
    {
        StateName = "Game Win";
    }

	public override void StateStart()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0f;

		ActiveTargetUi(UIManager.uis, "Game Win UI", true);
	}

	public override void StateUpdate()
	{

	}

	public override void StateEnd()
	{
		Time.timeScale = 1f;
		ActiveTargetUi(UIManager.uis, "Game Win UI", false);
	}
}
