using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
	public enum INDEX
	{
		C0,
		C1,
		C2,
		C3
	}

	[Header("连接口ID")]
	public INDEX connectorID;

	[Header("门")]
	public GameObject doorFrame;
	public GameObject door;
	public Animator doorAnimator;

	[Header("填充块")]
	public GameObject block;

	[Header("连接口属性设置")]
	public bool isConnected = false;
	public bool isOpen = true;

	public void CloseConnector(bool r)
	{
		if (r)
		{
			isOpen = false;
			block.SetActive(true);
			if(doorFrame!=null)
				doorFrame.SetActive(false);
			door.SetActive(false);
		}
		else
		{
			isOpen = true;
			block.SetActive(false);
			if (doorFrame != null)
				doorFrame.SetActive(true);
			door.SetActive(true);
		}

	}

	public Bounds CreatBounds(float width, float length)
	{
		var meshBounds = gameObject.AddComponent<BoxCollider>();
		meshBounds.size = new Vector3(width, 0f, length);
		meshBounds.center = new Vector3(0f, 0f, length / 2f + 1f);

		return meshBounds.bounds;
	}

	/// <summary>
	/// 上锁
	/// </summary>
	public void DoorLock()
	{
		DoorClose();
		GetComponent<BoxCollider>().enabled = false;
	}

	/// <summary>
	/// 解锁
	/// </summary>
	public void DoorUnLock()
	{
		DoorOpen();
	}

	public void DoorOpen()
	{
		doorAnimator.SetInteger("Door_IO", 1);
		GetComponent<BoxCollider>().enabled = false;
	}

	public void DoorClose()
	{
		doorAnimator.SetInteger("Door_IO", 2);
	}

	private void OnTriggerEnter(Collider other)
	{	
		if (other.tag == "Player")
		{
			//Debug.Log("Open?");
			DoorOpen();
		}
	}


	void OnDrawGizmos()
	{
		var scale = 1.0f;

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * scale);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position - transform.right * scale);
		Gizmos.DrawLine(transform.position, transform.position + transform.right * scale);

		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.up * scale);

		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 0.125f);
	}
}
