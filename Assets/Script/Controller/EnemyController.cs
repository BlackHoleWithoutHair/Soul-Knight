using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AbstractController
{
    private List<IEnemy> enemies;
    private List<IBoss> bosses;
    public EnemyController()
    {
        enemies = new List<IEnemy>();
        bosses = new List<IBoss>();
    }
    protected override void Init()
    {
        base.Init();
        if (GameObject.Find("Stake"))
        {
            IEnemy dummy = EnemyFactory.Instance.GetEnemy(EnemyType.Stake,false);
            dummy.gameObject.transform.position = GameObject.Find("Stake").transform.position;
            enemies.Add(dummy);
        }
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        for(int i=0;i<enemies.Count;i++)
        {
            if (enemies[i].ShouldBeRemove)
            {
                enemies.RemoveAt(i);
            }
            else
            {
                enemies[i].GameUpdate();
            }
        }
        foreach(IBoss boss in bosses)
        {
            boss.GameUpdate();
        }
    }
    public void AddBoss(Vector2 pos)
    {
        IBoss boss = EnemyFactory.Instance.GetBoss(BossType.DevilSnare);
        boss.transform.position = pos;
        bosses.Add(boss);
    }
    public void SpawnEnemy(Room room,Vector2 pos,bool isRun,bool isElite)
    {
        CoroutinePool.Instance.StartCoroutine(WaitForSpawnEnemy(room,pos,isRun,isElite));
    }
    private IEnumerator WaitForSpawnEnemy(Room room, Vector2 pos,bool isRun,bool isElite)
    {
        EffectFactory.Instance.GetEffect(EffectType.Pane, pos).AddToController();
        yield return new WaitForSeconds(0.8f);
        IEnemy enemy = null;
        if(isElite)
        {
            enemy = EnemyFactory.Instance.GetEliteEnemy();
        }
        else
        {
            enemy = EnemyFactory.Instance.GetRandomEnemy();
        }
        enemy.m_Room = room;
        enemy.gameObject.transform.position = pos;
        enemy.m_Attr.isRun=isRun;
        enemies.Add(enemy);
    }
}
