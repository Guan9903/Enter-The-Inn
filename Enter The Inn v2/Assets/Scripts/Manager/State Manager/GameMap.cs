using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MyState
{
	GameObject t;
	Animator tA;

	public GameMap(MyStateController Controller) : base(Controller)
	{
		StateName = "Minimap Open";
	}

	// 可以在此进行游戏数据的加载和初始化等
	public override void StateStart()
	{
		WeaponHolder.enableUse = false;

		t = GetTarget(UIManager.uis, "Minimap UI");
		tA = t.GetComponentInChildren<Animator>();
		tA.SetInteger("Minimap_IO", 1);
	}

	public override void StateUpdate()
	{
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			tA.SetInteger("Minimap_IO", 2);
			GameStatesManager.gameStateController.SetState(new GamePlay(GameStatesManager.gameStateController));

		}
	}


	public override void StateEnd() 
	{
		MinimapCameraFollow.restart = true;
		WeaponHolder.enableUse = true;
	}
}
