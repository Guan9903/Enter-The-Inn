using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("地图初始化")]
    public MapGenerator mapGenerator;
    public GameObject player;

    [Header("敌人生成设置")]
    [Range(2,6)]
    public int maxIteration;
    public float spawnDistance = 10f;
    public EnemyWeight[] enemyPrefabs;
    public GameObject[] bossPrefabs;

    [Header("其他设置")]
    public AudioClip bgmAudio;
    public AudioClip battleAudio;

    //private bool has
    private AudioSource source;
    private List<TileSet> rooms = new List<TileSet>();


    /// <summary>
    /// 动态难度参数
    /// </summary>
    private TileSet targetRoom;
    private int roomDiffcult = 0;
    private int nextDiffcult = 0;
    private bool startCount;
    private bool enemiesClear = true;
    private float passTime = 0f;


    void Awake()
    {
        //mapGenerator = FindObjectOfType<MapGenerator>();
        Instantiate(player, new Vector3(0, 4, 0), Quaternion.identity);
        mapGenerator.MapGanerate();
        
        //Instantiate(playerPrefab);
        //playerManager = playerPrefab.GetComponent<PlayerManager>();
        rooms = mapGenerator.roomTilesList;
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = bgmAudio;
        source.Play();
        //FindObjectOfType<AudioManager>().BGMPlay("music_1");
        //AudioManager.PlayBackground("music_2");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            AmmoPool.GetInstance().ClearAll();
            SceneManager.LoadScene("Demo");
        }

        if (startCount)
        {
            RoomProcess();
        }

    }

    #region 动态难度监控

    /// <summary>
    /// 设置玩家所处目标房间
    /// </summary>
    /// <param name="room"></param>
    public void SetTargetRoom(TileSet room)
    {
        if (room == null)
            return;

        targetRoom = room;
        startCount = true;
        enemiesClear = true;

        targetRoom.map.SetActive(true);
    }

    /// <summary>
    /// 对当前玩家所处房间进行处理
    /// </summary>
    void RoomProcess()
    {
        if (targetRoom.roomType != RoomType.Corridor)
        {
            passTime += Time.deltaTime;
            //Debug.Log("Time Pass : " + passTime);

            if (enemiesClear)
            {
                switch (targetRoom.roomType)
                {
                    case RoomType.NormalRoom:
                        StartCoroutine(DelayedGenerate(0));
                        //EnemyGenerate();
                        break;
                    case RoomType.BossRoom:
                        StartCoroutine(DelayedGenerate(1));
                        //BossGenerate();
                        break;
                }

                enemiesClear = false;
            }

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                targetRoom.UnLockDoors();

                //source.clip = bgmAudio;
                //source.Play();
            }
        }

    }

    /// <summary>
    /// 依据难度调整敌人生成
    /// </summary>
    void EnemyGenerate()
    {
        if (nextDiffcult == 0)
        {
            //roomDiffcult = maxIteration;
            for (int i = 0; i < maxIteration; i++)
            {
                var ep = GetRandom(enemyPrefabs);
                Instantiate(ep.enemyPrefab, targetRoom.GetRandomEscapPoint(player.transform.position, spawnDistance, out _), Quaternion.identity);
            }
        }
        else
        {
            roomDiffcult = nextDiffcult;
            while (roomDiffcult >= 0)
            {
                var ep = GetRandom(enemyPrefabs);
                var dv = ep.diffcultValue;
                Instantiate(ep.enemyPrefab, targetRoom.GetRandomEscapPoint(player.transform.position, spawnDistance, out _), Quaternion.identity);

                roomDiffcult -= dv;
            }
        }
    }

    void BossGenerate()
    {
        var bp = GetRandom(bossPrefabs);
        Instantiate(bp, new Vector3(targetRoom.transform.position.x,
                    bp.transform.position.y,
                    targetRoom.transform.position.z), Quaternion.identity);
    }

    private static T GetRandom<T>(T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    IEnumerator DelayedGenerate(int n)
    {
        Time.timeScale = 0.2f;

        //source.clip = battleAudio;
        //source.Play();

        switch (n)
        {
            case 0:
                EnemyGenerate();
                break;
            case 1:
                BossGenerate();
                break;
        }

        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
    }

    #endregion
}

[Serializable]
public class EnemyWeight
{
    public GameObject enemyPrefab;
    public int diffcultValue;
}