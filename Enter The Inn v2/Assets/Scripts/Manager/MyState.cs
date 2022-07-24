using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyState
{
	//状态名称
	private string stateName = "SceneState";

	/// <summary>
	/// 状态名称读写
	/// </summary>
	public string StateName
	{
		get { return stateName; }       //读取字段值。只读属性。无法对只读属性赋值
		set { stateName = value; }      //只写属性。只写属性除作为赋值的目标外，无法对其进行引用
	}

	// 状态拥有者
	protected MyStateController stateController = null;

	/// <summary>
	/// 构造场景状态
	/// </summary>
	/// <param name="controller"></param>
	public MyState(MyStateController controller)
	{
		stateController = controller;
	}

	/// <summary>
	/// 在状态跳转成功后，通知类对象，执行该场景中需要加载的资源和游戏参数等设置
	/// </summary>
	public virtual void StateStart() { }

	/// <summary>
	/// 更新状态，即执行循环逻辑
	/// </summary>
	public virtual void StateUpdate() { }

	/// <summary>
	/// 在状态被释放时，通知类对象，卸载不再使用的资源等操作
	/// </summary>
	public virtual void StateEnd() { }

	/// <summary>
	/// 激活或关闭目标UI
	/// </summary>
	/// <param name="list"></param>
	/// <param name="targetName"></param>
	/// <param name="setActive"></param>
	public virtual void ActiveTargetUi(List<GameObject> list, string targetName, bool setActive)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].name == targetName)
			{
				list[i].SetActive(setActive);
				break;
			}
		}
	}

	public GameObject GetTarget(List<GameObject> list, string targetName)
	{
		int t = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].name == targetName)
			{
				t = i;
				break;
			}
		}

		return list[t];
	}

	/// <summary>
	/// 用于打印当前场景状态名称，便于Debug
	/// </summary>
	/// <returns></returns>
	public override string ToString()
	{
		return string.Format("[SceneState: StateName={0}]", StateName);
	}

}
