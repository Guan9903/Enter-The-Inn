using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReady : MyState
{
	public GameReady(MyStateController Controller) : base(Controller)
	{
		StateName = "Game Ready";
	}

	// 可以在此进行游戏数据的加载和初始化等
	public override void StateStart()
	{
		AmmoPool.GetInstance().ClearAll();
		Time.timeScale = 1;
	}

	public override void StateUpdate()
	{
		stateController.SetState(new GamePlay(stateController));
	}

	public override void StateEnd()
	{

	}
}
