using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RoomType
{
	BossRoom,
	StartRoom,
	NormalRoom,
	ShopRoom,
	Corridor
}

public class TileSet : MonoBehaviour
{
    #region 连接地图

    public Bounds GetBounds()
	{
		return GetComponentInChildren<RoomBounds>().floorBounds();
	}

	public Connector[] GetConnectors()
	{
		return GetComponentsInChildren<Connector>();
	}

	public bool CollideOtherTile(TileSet other)
	{
		bool val;

		val = GetBounds().Intersects(other.GetBounds());

		return val;
	}

	/// <summary>
	/// 生成检测器，用于检测玩家进入房间
	/// </summary>
	/// <returns></returns>
	public Collider CheckCollider()
	{
		var size = GetBounds().size;
		var collider = gameObject.AddComponent<BoxCollider>();
		collider.size = new Vector3(size.x - 6f, 1, size.z - 6f);
		collider.isTrigger = true;

		return collider;
	}

	public Collider CheckCollider(float x, float z)
	{
		var size = new Vector3(x, 0, z);
		var collider = gameObject.AddComponent<BoxCollider>();
		collider.size = new Vector3(size.x, 1, size.z);
		collider.isTrigger = true;

		return collider;
	}

	#endregion

	#region 关卡管理

	public RoomType roomType;
	public GameObject map;

	[HideInInspector]
	public bool isNew = true;

	[HideInInspector]
	public NavMeshSurface navMesh;

	private List<Connector> doors;

	private LevelManager levelManager;


	/// <summary>
	/// 获取室内的门
	/// </summary>
	/// <returns></returns>
	public List<Connector> GetOpenDoors()
	{
		var doors = new List<Connector>();
		var connectors = GetComponentsInChildren<Connector>();

		foreach (var c in connectors)
		{
			if (c.isOpen)
				doors.Add(c);
		}
		return doors;
	}

    #region 随机获取NavMeshSurface上的一个顶点

    /// <summary>
    /// 从NavMesh中随机取一个点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomLocation()
	{
		NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

		int t = Random.Range(0, navMeshData.indices.Length - 10);

		Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
		point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

		Debug.Log(point);
		return point;
	}

	/// <summary>
	/// 获取远离tartgetPoint的点
	/// </summary>
	/// <param name="tartpoint"></param>
	/// <returns></returns>
	public Vector3 GetRandomEscapPoint(Vector3 targetPoint, float distance, out bool success)
	{
		NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
		List<int> list = new List<int>();
		//获取所有三角的第一个顶点的索引值
		for (int i = 0; i < navMeshData.indices.Length; i += 3)
			list.Add(i);
		int count = list.Count; 
		//Debug.Log("顶点数量:" + navMeshData.indices.Length);
		List<Vector3> triangle = new List<Vector3>();
		for (int i = 0; i < count; i++)
		{
			int index = Random.Range(0, list.Count);
			if (Vector3.Distance(targetPoint, navMeshData.vertices[navMeshData.indices[list[index]]]) > distance) triangle.Add(navMeshData.vertices[navMeshData.indices[list[index]]]);
			if (Vector3.Distance(targetPoint, navMeshData.vertices[navMeshData.indices[list[index] + 1]]) > distance) triangle.Add(navMeshData.vertices[navMeshData.indices[list[index] + 1]]);
			if (Vector3.Distance(targetPoint, navMeshData.vertices[navMeshData.indices[list[index] + 2]]) > distance) triangle.Add(navMeshData.vertices[navMeshData.indices[list[index] + 2]]);
			if (triangle.Count > 0) break;
			list.RemoveAt(index);
		}

		Vector3 point = Vector3.zero;
		success = false;
		if (triangle.Count > 0)
		{
			point = GerLerpPoint(triangle);
			success = true;
		}
		return point;
	}

	/// <summary>
	/// 获取多个顶点之间的一个随机点
	/// </summary>
	/// <param name="list"></param>
	/// <returns></returns>
	Vector3 GerLerpPoint(List<Vector3> list)
	{
		Vector3 point = list[0];
		foreach (Vector3 v in list)
		{
			point = Vector3.Lerp(point, v, Random.value);
		}
		return point;
	}

    #endregion

    /// <summary>
    /// 锁上所有门
    /// </summary>
    public void LockDoors()
	{
		foreach (var d in doors)
		{
			d.DoorLock();
		}
	}

	/// <summary>
	/// 解锁所有门
	/// </summary>
	public void UnLockDoors()
	{
		foreach (var d in doors)
		{
			d.DoorUnLock();
		}

		navMesh.enabled = false;
	}

	void Start()
	{
		doors = GetOpenDoors();

		if (map != null)
		{
			if (roomType == RoomType.StartRoom)
			{
				map.SetActive(true);
			}
			else
			{
				map.SetActive(false);
			}
		}

		if (levelManager == null)
		{
			levelManager = FindObjectOfType<LevelManager>();
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && isNew)
		{
			//Debug.Log("Player In!");
			if (roomType != RoomType.Corridor)
			{
				LockDoors();
				NavMeshBake();
			}

			levelManager.SetTargetRoom(this);

			isNew = false;
		}
	}

	public void NavMeshBake()
	{
		navMesh = gameObject.AddComponent<NavMeshSurface>();
		navMesh.collectObjects = CollectObjects.Children;
		navMesh.layerMask = ~(1 << 11 | 1 << 14);
		navMesh.BuildNavMesh();
	}

	#endregion
}
