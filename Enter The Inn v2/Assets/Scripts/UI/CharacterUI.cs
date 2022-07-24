using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public GameObject player;

    public GameObject healthIconYin;
    public GameObject healthIconYang;
    public Vector3 iconStartPos;
    public Vector3 iconOffset;

    public int healthNums;

    private GameObject tempPrefab;
    private Vector3 tempIconOffset = Vector3.zero;
    private int tempHealth;
    private List<GameObject> healthIcons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        GetTargetHealth();
        tempHealth = healthNums;

        for (int i = 0; i < healthNums; i++)
        {
            if (i % 2 == 0)
            {
                tempPrefab = healthIconYin;
                tempIconOffset = Vector3.zero;
            }
            else if (i % 2 != 0 || i == 1)
            {
                tempPrefab = healthIconYang;
                tempIconOffset = iconOffset;
            }

            var hIcon = Instantiate(tempPrefab, iconStartPos, Quaternion.identity, transform);

            iconStartPos += tempIconOffset;

            healthIcons.Add(hIcon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetTargetHealth();
        HealthCount();
        
    }

    /// <summary>
    /// 获取目标生命值
    /// </summary>
    void GetTargetHealth()
    {
        healthNums = player.GetComponent<PlayerManager>().playerHealth;
        //healthNums = PlayerManager.playerHealth;
    }

    /// <summary>
    /// 计算生命值
    /// </summary>
    void HealthCount()
    {
        if (healthNums >= 0)
        {
            if (tempHealth - healthNums > 0)
            {
                healthIcons[healthNums].SetActive(false);
            }
            if (tempHealth - healthNums < 0)
            {
                healthIcons[healthNums].SetActive(true);
            }

            tempHealth = healthNums;
        }
    }
}
