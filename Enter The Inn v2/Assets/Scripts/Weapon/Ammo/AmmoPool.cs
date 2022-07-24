using System.Collections.Generic;
using UnityEngine;

public class AmmoPool
{
    #region 单例

    private static AmmoPool instance;

    private AmmoPool()
    {
        pool = new Dictionary<string, List<GameObject>>();
        prefabs = new Dictionary<string, GameObject>();
    }

    public static AmmoPool GetInstance()
    {
        if (instance == null)
        {
            instance = new AmmoPool();
        }
        return instance;
    }

    #endregion

    /// <summary>
    /// 对象池
    /// </summary>
    private Dictionary<string, List<GameObject>> pool;

    /// <summary>
    /// 预设体
    /// </summary>
    private Dictionary<string, GameObject> prefabs;

    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <param name="objName"></param>
    /// <returns></returns>
    public GameObject GetObj(string objName)
    {
        GameObject resultObj = null;        // 结果对象

        if (pool.ContainsKey(objName))      // 判断是否有该名字的对象池
        {
            if (pool[objName].Count > 0)
            {
                resultObj = pool[objName][0];       // 获取对象
                resultObj.SetActive(true);          // 激活对象
                pool[objName].Remove(resultObj);    // 从池中移除该对象
                return resultObj;
            }
        }

        GameObject prefab = null;           // 如果没有该名字的对象池或者该名字对象池没有对象

        if (prefabs.ContainsKey(objName))   // 如果已经加载过预设体
        {
            prefab = prefabs[objName];
        }
        else
        {
            prefab = Resources.Load<GameObject>("Prefabs/Ammo/" + objName);
            prefabs.Add(objName, prefab);   // 更新字典
        }

        resultObj = Object.Instantiate(prefab);
        resultObj.name = objName;

        return resultObj;
    }

    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    /// <param name="obj"></param>
    public void RecycleObj(GameObject obj)
    {
        obj.SetActive(false);

        if (pool.ContainsKey(obj.name))
        {
            pool[obj.name].Add(obj);
        }
        else
        {
            pool.Add(obj.name, new List<GameObject>() { obj });     // 创建该类型的池，并将对象放入
        }
    }

    public void Clear(string key)
    {
        if (pool.ContainsKey(key))
        {
            for (int i = 0; i < pool[key].Count; i++)
            {
                Object.Destroy(pool[key][i]);
            }
        }
        pool.Remove(key);

        //if (prefabs.ContainsKey(key))
        //{
        //    Object.Destroy(prefabs[key]);
        //}
        //prefabs.Remove(key);

    }

    public void ClearAll()
    {
        var poolList = new List<string>(pool.Keys);
        for (int i = 0; i < poolList.Count; i++)
        {
            Clear(poolList[i]);
        }

        //var prefabsList = new List<string>(prefabs.Keys);
        //for (int i = 0; i < prefabsList.Count; i++)
        //{
        //    Clear(prefabsList[i]);
        //}
    }

}
