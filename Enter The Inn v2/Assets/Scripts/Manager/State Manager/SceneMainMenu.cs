using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMainMenu : MyState
{
	public SceneMainMenu(MyStateController Controller) : base(Controller)
	{
		StateName = "Start Scene";
	}

	public override void StateStart()
	{
		AmmoPool.GetInstance().ClearAll();
	}

	public override void StateUpdate() { }

	public override void StateEnd() { }
}
