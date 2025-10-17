using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallManager : MonoBehaviour
{

    private enum BallType
    {
        Standard,
        Bowling,
        Piano,
        Metronome,
        Featherfall,
        Shackles,
        RedBoots,
        BlueBoots,
        Wings,
        StunGrenade,
        FuseBall,
        HolyBall,
        Ecstasphere,
        ThrowingArm,
        CryoBomb,
        Hammer,
        C4,
        Dynamite

    }

    private Stack<Ball>[] pool;

    public Action recall;

    private bool spawnOn;
    private bool ended;

    // Use this for initialization
    private OnCourtPlayerManager players;
    public void Init()
    {
        ended = false;
        InitPools();

    }

    public void Init(OnCourtPlayerManager thePlayers)
    {

        players = thePlayers;

        InitPools();

    }

    private void InitPools()
    {
        pool = new Stack<Ball>[12];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = new Stack<Ball>();

            for (int j = 0; j < 15; j++)
            {
                pool[i].Push(CreateBallFromEnum((BallType)i));
                pool[i].Peek().gameObject.name += (j + 1);
                pool[i].Peek().Init(new Vector3(300, 300) + (i * 2 * Vector3.up) + (pool[i].Count * 2 * Vector3.right));
                pool[i].Peek().Pause();
                pool[i].Peek().recalled += RecallBall;
            }
        }

    }

    private Ball CreateBallFromEnum(BallType ballType)
    {

        return ballType switch
        {
            BallType.Standard => new GameObject("Volley Ball").AddComponent<Ball_StandardBall>(),
            BallType.Piano => new GameObject("Piano").AddComponent<Ball_Piano>(),
            BallType.Bowling => new GameObject("Bowling Ball").AddComponent<Ball_BowlingBall>(),
            BallType.Metronome => new GameObject("Metronome").AddComponent<Ball_Metronome>(),
            BallType.Featherfall => new GameObject("Grace Wing").AddComponent<Ball_GraceFeather>(),
            BallType.Shackles => new GameObject("Shackles").AddComponent<Ball_Shackles>(),
            BallType.RedBoots => new GameObject("Red Boots").AddComponent<Ball_RedBoots>(),
            BallType.BlueBoots => new GameObject("Blue Boots").AddComponent<Ball_BlueBoots>(),
            BallType.Wings => new GameObject("Heaven Wings").AddComponent<Ball_AngelWings>(),
            BallType.StunGrenade => new GameObject("Stun Grenade").AddComponent<Ball_StunGrenade>(),
            BallType.FuseBall => new GameObject("Fuse").AddComponent<Ball_FuseBomb>(),
            BallType.HolyBall => new GameObject("Deep Blessing").AddComponent<Ball_HolyBall>(),
            BallType.Ecstasphere => new GameObject("Ecstasphere").AddComponent<Ball_StandardBall>(),
            _ => new GameObject("Volley Ball").AddComponent<Ball_StandardBall>(),
        };
    }

    public Ball NextFromPool(float value)
    {
        int typeID;

        if (value < 0.4f)
        {
            typeID = 0;
        }
        else if (value < 0.525f)
        {
            typeID = 1;
        }
        else if (value < 0.575f)
        {
            typeID = 2;
        }
        else if (value < 0.6f)
        {
            typeID = 3;
        }
        else if (value < 0.635f)
        {
            typeID = 4;
        }
        else if (value < 0.67f)
        {
            typeID = 5;
        }
        else if (value < 0.69f)
        {
            typeID = 6;
        }
        else if (value < 0.71f)
        {
            typeID = 7;
        }
        else if (value < 0.735f)
        {
            typeID = 8;
        }
        else if (value < 0.77f)
        {
            typeID = 9;
        }
        else if (value < 0.8f)
        {
            typeID = 10;
        }
        else
        {
            typeID = 0;
        }

        if (pool[typeID].Count > 0)
        {
            return pool[typeID].Pop();
        }
        else
        {
            return CreateBallFromEnum((BallType)typeID);
        }
    }

    public void ReplaceInPool(Ball ballToReplace, int typeID)
    {

        ballToReplace.transform.position = new Vector3(300, 300) + (typeID * 2 * Vector3.up) + (pool[typeID].Count * 2 * Vector3.right);
        ballToReplace.despawned -= OnBallDespawn;
        pool[typeID].Push(ballToReplace);


    }

    public void StartSpawning()
    {
        spawnOn = true;
        StartCoroutine(SpawnTimer());
        StartCoroutine(SpawnTimer());
        StartCoroutine(SpawnTimer());

    }

    private IEnumerator SpawnTimer()
    {
        float randy = UnityEngine.Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(randy);

        if (spawnOn)
        {
            Ball ball = NextFromPool(UnityEngine.Random.value);

            ball.transform.position = new Vector3(30f * (UnityEngine.Random.value - 0.5f), 10, 0);

            ball.despawned += OnBallDespawn;

            ball.Play();
            ball.ZeroVelocity();

            recall += ball.Despawn;
        }

        StartCoroutine(SpawnTimer());

    }

    public void DespawnAll()
    {
        recall?.Invoke();
    }
    public void RecallBall(Ball ball, int typeID)
    {
        ReplaceInPool(ball, typeID);
    }


    public void ToggleSpawning()
    {
        spawnOn = !spawnOn;
    }

    private void OnBallDespawn(Ball ball, int typeID)
    {
        if (players != null)
        {
            if (!ended)
            {
                players.PointScore(ball.transform.position.x, ball.GetPoints());
            }
        }
        ReplaceInPool(ball, typeID);

    }


}
