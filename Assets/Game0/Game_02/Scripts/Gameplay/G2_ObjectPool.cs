using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

[System.Serializable]
public class G2_Pool
{
    public string name;
    public GameObject go;
    public int count;
    public List<GameObject> actives;
    public Queue<GameObject> deactives;

    public G2_Pool(string name, GameObject go, int count)
    {
        this.name = name;
        this.go = go;
        this.count = count;
    }

    public void InitaPool(Transform container, Dictionary<int, int> dicClones)
    {
        actives = new List<GameObject>();
        deactives = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            spawnAClone(container, dicClones);
        }
    }

    void spawnAClone(Transform container, Dictionary<int, int> dicClones)
    {
        var clone = Object.Instantiate(go, container);
       // clone.transform.localScale = Vector3.one;
        clone.name += (actives.Count + deactives.Count);
        deactives.Enqueue(clone);
        dicClones.Add(clone.GetHashCode(), GetHashCode());
    }

    public GameObject Get(Transform container, Dictionary<int, int> dicClones)
    {
        if (deactives.Count == 0)
            spawnAClone(container, dicClones);
        var clone = deactives.Dequeue();
        actives.Add(clone);
        return clone;
    }

    public void Return(GameObject go, bool deactive)
    {
        go.transform.rotation = Quaternion.identity;

        if (deactive)
        {
            go.SetActive(false);
        }
        if (actives.Contains(go))
            actives.Remove(go);
        if (!deactives.Contains(go))
            deactives.Enqueue(go);
    }

    public void OnDestroy()
    {
        foreach (var active in actives)
        {
            if (active != null)
            {
                active.transform.DOKill();
            }

        }

        foreach (var deactive in deactives)
        {
            if (deactive != null)
            {
                deactive.transform.DOKill();
            }
        }
    }
}

public class G2_ObjectPool : MonoBehaviour
{
    public static G2_ObjectPool Instance;

    //khai bao pool
    public G2_Pool node;
    public G2_Pool obstacle;
    public G2_Pool fire;
    public G2_Pool enemy;
    public List<G2_Pool> wayCards;
    
    public Dictionary<int, int> dicClones = new Dictionary<int, int>();
    public List<G2_Pool> pools = new List<G2_Pool>();
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //add pool
        pools.Add(node);
        pools.Add(obstacle);
        pools.Add(fire);
        pools.Add(enemy);
        pools.AddRange(wayCards);

        foreach (var pool in pools)
        {
            pool.InitaPool(transform, dicClones);
        }
    }

    public void ReturnAllPool()
    {
        foreach (var p in pools)
        {
            while (p.actives.Count > 0)
            {
                p.actives[p.actives.Count - 1].transform.parent = transform;
                p.Return(p.actives[p.actives.Count - 1], true);
            }
        }
    }

    public G2_Pool TryAddPoolByScript(G2_Pool p)
    {
        var existedPool = pools.Find(x => x.go == p.go);
        if (existedPool != null)
        {
            Debug.LogWarning($"existed pool: {p.go.name}", p.go.transform);
            return existedPool;
        }
        pools.Add(p);
        p.InitaPool(transform, dicClones);
        return p;
    }

    public GameObject Get(G2_Pool p, bool active = true)
    {
        var obj = p.Get(transform, dicClones);
        //obj.transform.localScale = Vector3.one;
        obj.SetActive(active);
        return obj;
    }

    public G2_Pool CheckAddPool(GameObject obj)
    {
        foreach (var p in pools)
        {
            if (p.go == obj)
            {
                return p;
            }
        }

        var pool = new G2_Pool(obj.name, obj, 0);
        TryAddPoolByScript(pool);
        return pool;

    }

    public void Return(GameObject clone, bool deactive)
    {
        clone.transform.DOKill();
        clone.transform.parent = transform;
        var hash = clone.GetHashCode();
        if (dicClones.ContainsKey(hash))
        {
            var p = getPool(dicClones[hash]);
            p.Return(clone, deactive);
        }
        else
        {
            Destroy(clone);
            //Debug.LogError(clone.transform.name, clone.transform);
        }
    }
    G2_Pool getPool(int hash)
    {
        foreach (var pool in pools)
        {
            if (pool.GetHashCode() == hash)
                return pool;
        }

        return null;
    }

    private void OnDestroy()
    {
        foreach (var pool in pools)
        {
            pool.OnDestroy();
        }
    }
}