using System.Collections.Generic;
using UnityEngine;

public class TMPEManager : MonoBehaviour
{

    Stack<TMPEmission> pool;
    private int all;

    public void Init()
    {
        all = 0;
        InitPool();

    }

    private void InitPool()
    {
        pool = new Stack<TMPEmission>();

        for (int i = 0; i < 50; i++)
        {
            pool.Push(new GameObject("TMPE" + all).AddComponent<TMPEmission>());
            pool.Peek().Init();
            pool.Peek().transform.position = new Vector3(150, 150) + (pool.Count * 2 * Vector3.right);
            pool.Peek().transform.SetParent(transform);
            all++;
        }

    }

    public TMPEmission NextFromPool()
    {
        if (pool.Count > 0)
        {
            CreatePoolEntry();
        }
        pool.Peek().expire += ReturnToPool;
        return pool.Pop();

    }

    private void ReturnToPool(TMPEmission returnee)
    {
        pool.Push(returnee);
        returnee.transform.position = new Vector3(150, 150) + (pool.Count * 2 * Vector3.right);
    }

    private void CreatePoolEntry()
    {

        pool.Push(new GameObject("TMPE"+ all).AddComponent<TMPEmission>());
        pool.Peek().Init();
        pool.Peek().transform.position = new Vector3(150, 150) + (pool.Count * 2 * Vector3.right);
        all++;

    }


}
