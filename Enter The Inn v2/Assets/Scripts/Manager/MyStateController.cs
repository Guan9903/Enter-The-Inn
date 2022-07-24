using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyStateController
{
    private MyState myState;

	private bool stateRunning = false;

	public MyStateController() { }

	/// <summary>
	/// 设定状态
	/// </summary>
	/// <param name="sceneState">场景状态</param>
	/// <param name="loadSceneName"></param>
	public void SetState(MyState sceneState, string loadSceneName)
	{
		Debug.Log("SetState:" + sceneState.ToString());
		stateRunning = false;

		// 载入场景
		LoadScene(loadSceneName);

		// 通知前一个State結束
		if (myState != null)
			myState.StateEnd();

		myState = sceneState;
	}

	public void SetState(MyState gameState)
	{
		//Debug.Log("SetState:" + gameState.ToString());
		stateRunning = false;

		// 通知前一个State結束
		if (myState != null)
			myState.StateEnd();

		myState = gameState;
	}

	/// <summary>
	/// 载入场景
	/// </summary>
	/// <param name="loadSceneName"></param>
	private void LoadScene(string loadSceneName)
	{
		if (loadSceneName == null || loadSceneName.Length == 0)
			return;

		// 异步加载
		SceneManager.LoadSceneAsync(loadSceneName);
	}

	/// <summary>
	/// 状态更新
	/// </summary>
	[System.Obsolete]
	public void StateUpdate()
	{
		// 场景是否还在载入
		if (Application.isLoadingLevel)
			return;

		// 通知新的State开始
		if (myState != null && stateRunning == false)
		{
			myState.StateStart();
			stateRunning = true;
		}

		if (myState != null)
			myState.StateUpdate();
	}
}
