using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    [Header("路径房间")]
    public TileSet[] roomTiles;
    public TileSet[] corridorTiles;
    public TileSet StartTile;
    public TileSet BossTile;
   
    [Header("特殊房间")]
    public TileSet rewardTile;
    public TileSet shopTile;

    [Header("参数设置")]
    public int mainRoomNums = 4;
    public int sideRoomMaxIteration = 4;

    [HideInInspector]
    public List<TileSet> mainRoomsList = new List<TileSet>();

    [HideInInspector]
    public List<TileSet> roomTilesList = new List<TileSet>();

    private Connector mainPathExit;
    private Connector mainPathEntrance;
    private bool isTurn = false;
    private bool specialRoomSet = false;
    private int[] dirWeight = new int[3];   // 方向权重，用于判断主路径曲折， 下标0、1、2分别表示直向、右向、左向

    public void MapGanerate()
    {
        mainPathGenerate();
        SidePathGenerate();
    }

    #region 主要路径生成

    /// <summary>
    /// 主路经生成
    /// </summary>
    void mainPathGenerate()
    {
        StartRoomGenerate();
        CorridorGenerate(ref mainPathExit, ref mainPathEntrance);

        for (int i = 0; i < mainRoomNums; i++)
        {
            mainRoomGenerate();
            CorridorGenerate(ref mainPathExit, ref mainPathEntrance);
        }

        BossRoomGenerate();

    }

    /// <summary>
    /// 初始房间生成
    /// </summary>
    void StartRoomGenerate()
    {
        TileSet startTile = Instantiate(StartTile, transform.position, transform.rotation);
        var startExits = new List<Connector>(startTile.GetConnectors());

        // 保证开始房间只有一个门
        int rand = Random.Range(0, startExits.Count);

        mainPathExit = startExits[rand];
        mainPathExit.isConnected = true;

        foreach (var exit in startExits)
        {
            if (exit != startExits[rand])
                exit.CloseConnector(true);
            else
                exit.CloseConnector(false);
        }

        //startExits.RemoveAll(c => c != startExits[rand]);

        roomTilesList.Add(startTile);
    }

    /// <summary>
    /// boss房间生成
    /// </summary>
    void BossRoomGenerate()
    {
        TileSet bossTile = Instantiate(BossTile);
        var bossEntrances = new List<Connector>(bossTile.GetConnectors());
        bossTile.CheckCollider();
        //bossTile.GetBounds();

        int rand = Random.Range(0, bossEntrances.Count);
        foreach (var bossEntrance in bossEntrances)
        {
            if (bossEntrance != bossEntrances[rand])
                bossEntrance.CloseConnector(true);
            else
                bossEntrance.CloseConnector(false);
        }

        MatchConnector(mainPathExit, bossEntrances[rand]);


        foreach (var room in roomTilesList)
        {
            if (bossTile.CollideOtherTile(room))
            {
                //Debug.Log("Reload");
                SceneManager.LoadScene("Demo");
                Debug.Log("Level Reload");
            }
        }

        bossEntrances.RemoveAll(c => c != bossEntrances[rand]);

        roomTilesList.Add(bossTile);

    }

    /// <summary>
    /// 主路径房间生成
    /// </summary>
    void mainRoomGenerate()
    { 
        isTurn = false;
        var roomTile = SetTile(roomTiles);
        roomTile.CheckCollider();
        var newConnectors = roomTile.GetConnectors();
        mainPathEntrance = GetRandom(newConnectors);
        MatchConnector(mainPathExit, mainPathEntrance);
        int id = SetExitDirection(mainPathEntrance);
        foreach (var e in newConnectors)
        {
            if ((int)e.connectorID == id && isTurn)
                mainPathExit = e;
            if (e != mainPathEntrance && !isTurn)
                mainPathExit = e;
        }
        
        SetConnector(mainPathExit, mainPathEntrance);

        mainPathExit.CloseConnector(false);
        mainPathEntrance.CloseConnector(false);

        roomTilesList.Add(roomTile);
        mainRoomsList.Add(roomTile);
    }

    /// <summary>
    /// 通道生成，注意：所有通道都只有两个连接点
    /// </summary>
    void CorridorGenerate(ref Connector exit, ref Connector entrance)
    {
        var corridor = SetTile(corridorTiles);
        corridor.CheckCollider(19, 5);
        var newConnectors = corridor.GetConnectors();
        entrance = GetRandom(newConnectors);
        MatchConnector(exit, entrance);
        foreach (var e in newConnectors)
        {
            if (e != entrance)
                exit = e;
        }

        SetConnector(exit, entrance);
    }

    //void TileGenerate(TileSet[] tiles)
    //{
    //    //entrances.Clear();
    //    entrances = new List<Connector>();
    //    foreach (var exit in exits)
    //    {
    //        entrances.Clear();

    //        var newPrefab = GetRandom(tiles);
    //        TileSet tile = Instantiate(newPrefab);
    //        tile.GetBounds();
    //        var newConnectors = tile.GetConnectors();
    //        Connector entrance = GetRandom(newConnectors);
    //        MatchConnector(exit, entrance);
    //        entrances.AddRange(newConnectors.Where(e => e != entrance));
    //    }

    //    exits = entrances;
    //}

    /// <summary>
    /// 获取并实例化目标Tile
    /// </summary>
    /// <param name="tiles"></param>
    private TileSet SetTile(TileSet[] tiles)
    {
        var newPrefab = GetRandom(tiles);
        TileSet tile = Instantiate(newPrefab);

        return tile;
    }

    /// <summary>
    /// 设置连接口的属性
    /// </summary>
    private void SetConnector(Connector exit, Connector entrance)
    {
        exit.isConnected = true;
        entrance.isConnected = true;
        int ex = (int)exit.connectorID;
        int en = (int)entrance.connectorID;

        // 路径直向：0
        if (Mathf.Abs(ex - en) == 2)
            dirWeight[0]++;

        // 路径右向：1
        if ((en == 0 & ex == 3) || (en == 1 & ex == 0) || (en == 2 & ex == 1) || (en == 3 & ex == 2))
            dirWeight[1]++;

        // 路径左向：2
        if ((en == 0 & ex == 1) || (en == 1 & ex == 2) || (en == 2 & ex == 3) || (en == 3 & ex == 0))
            dirWeight[2]++;

    }

    /// <summary>
    /// 设置路径转向
    /// </summary>
    private int SetExitDirection(Connector entrance)
    {  
        int ex = 0;

        int length = dirWeight.Length;
        int turnDir;
        for (int i = 1; i < length; i++)
        {
            if (dirWeight[i] >= 2)
            {
                isTurn = true;
                dirWeight[i] = 0;
                turnDir = length - i;
                int en = (int)entrance.connectorID;
                switch (en)
                {
                    case 0:
                        if (turnDir == 1)   // 右向
                            ex = 3;
                        if (turnDir == 2)   // 左向
                            ex = 1;
                        break;
                    case 1:
                        if (turnDir == 1)
                            ex = 0;
                        if (turnDir == 2)
                            ex = 2;
                        break;
                    case 2:
                        if (turnDir == 1)
                            ex = 1;
                        if (turnDir == 2)
                            ex = 3;
                        break;
                    case 3:
                        if (turnDir == 1)
                            ex = 2;
                        if (turnDir == 2)
                            ex = 0;
                        break;
                }
                break;
            }
        }

        return ex;

    }

    /// <summary>
    /// 匹配连接点
    /// </summary>
    /// <param name="exit"></param>
    /// <param name="entrance"></param>
    void MatchConnector(Connector exit, Connector entrance)
    {
        var newTile = entrance.transform.parent;
        var forwardVectorToMatch = -exit.transform.forward;
        var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(entrance.transform.forward);
        newTile.RotateAround(entrance.transform.position, Vector3.up, correctiveRotation);
        var correctiveTranslation = exit.transform.position - entrance.transform.position;
        newTile.transform.position += correctiveTranslation;
    }

    /// <summary>
    /// 泛型函数，减少函数重载，该函数用于随机获取数组中的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    private static T GetRandom<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    /// <summary>
    /// 方位角获取
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private static float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }

    #endregion

    #region 支线路径生成

    private Connector sidePathExit;
    private Connector sidePathEntrance;

    private bool getSidePath = false;

    /// <summary>
    /// 支路生成
    /// </summary>
    void SidePathGenerate()
    {
        int index = mainRoomsList.Count;

        foreach (var mainRoom in mainRoomsList)
        {
            SidePathSet(index, mainRoom);
            index--;
        }
    }

    /// <summary>
    /// 支路房间生成
    /// </summary>
    /// <param name="index"></param>
    /// <param name="mainRoom"></param>
    void SidePathSet(int index,TileSet mainRoom)
    {
        SetSidePathStart(mainRoom);
        if (getSidePath)
        {
            CorridorGenerate(ref sidePathExit, ref sidePathEntrance);
            for (int i = 0; i < sideRoomMaxIteration; i++)
            {
                if (index <= mainRoomNums / 2 && !specialRoomSet)
                {
                    SpecialRoomGenerate();
                    specialRoomSet = true;
                }
                else
                    SideRoomGenerate();
                if (getSidePath)
                {
                    if (i == sideRoomMaxIteration - 1)
                        break;
                    CorridorGenerate(ref sidePathExit, ref sidePathEntrance);
                }
            }
            sidePathExit.CloseConnector(true);
        }
    }

    /// <summary>
    /// 遍历主路径节点设置支路起点
    /// </summary>
    void SetSidePathStart(TileSet room)
    {
        getSidePath = false;
        var startExits = GetRemainConnectors(room);

        foreach (var e in startExits)
        {
            if (!ConnectorJudege(e, 75, 160) && !getSidePath)
            {
                sidePathExit = e;
                sidePathExit.isConnected = true;
                sidePathExit.CloseConnector(false);
                getSidePath = true;
            }
            else
                e.CloseConnector(true);
        }
    }

    void SideRoomGenerate()
    {
        var roomTile = SetTile(roomTiles);
        roomTile.CheckCollider();
        var newConnectors = roomTile.GetConnectors();
        sidePathEntrance = GetRandom(newConnectors);
        MatchConnector(sidePathExit, sidePathEntrance);
        int id = SetExitDirection(sidePathEntrance);
        foreach (var e in newConnectors)
        {
            if (e != sidePathEntrance)
                sidePathExit = e;
        }

        //sidePathExit.isConnected = true;
        sidePathEntrance.isConnected = true;
        sidePathEntrance.CloseConnector(false);

        roomTilesList.Add(roomTile);

        SetSidePathStart(roomTile);
    }

     
    void SpecialRoomGenerate()
    {
        var roomTile = Instantiate(shopTile);
        var newConnectors = roomTile.GetConnectors();
        sidePathEntrance = GetRandom(newConnectors);
        MatchConnector(sidePathExit, sidePathEntrance);
        foreach (var e in newConnectors)
        {
            if (e != sidePathEntrance)
                sidePathExit = e;
        }

        sidePathEntrance.isConnected = true;
        sidePathEntrance.CloseConnector(false);

        roomTilesList.Add(roomTile);
        SetSidePathStart(roomTile);

    }

    /// <summary>
    /// 获取主路径房间中剩余的连接点
    /// </summary>
    /// <param name="mainRoom"></param>
    /// <returns></returns>
    private List<Connector> GetRemainConnectors(TileSet mainRoom)
    {
        List<Connector> remainConnectors = new List<Connector>();
        var connectors = mainRoom.GetConnectors();
        foreach (var remain in connectors)
        {
            if (!remain.isConnected)
            {
                var remainConnector = remain;
                remainConnectors.AddRange(connectors.Where(e => e == remainConnector));
            }
        }

        return remainConnectors;
    }

    /// <summary>
    /// 检测连接口前一定范围内是否有房间阻碍
    /// </summary>
    /// <param name="connector"></param> 连接口
    /// <param name="room"></param> 房间
    /// <param name="length"></param> 矩形前方距离
    /// <param name="width"></param> 矩形宽度
    /// <returns></returns>
    private bool ConnectorJudege(Connector connector, float width, float length)
    {
        bool val = false;

        var bounds = connector.CreatBounds(width, length);

        foreach (var room in roomTilesList)
        {
            if (bounds.Intersects(room.GetBounds()))
            {
                val = true;
                //Debug.Log("Bounds!!!");
                break;      
            }
        }

        return val;
    }

    #endregion

}

