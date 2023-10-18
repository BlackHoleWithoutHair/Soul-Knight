using System.Collections.Generic;
using UnityEngine;
public class EnemyController : AbstractController
{
    private Room CurrentRoom;
    private List<IEnemy> enemies;
    public EnemyController()
    {
        enemies = new List<IEnemy>();
    }
    protected override void Init()
    {
        base.Init();
        if (GameObject.Find("Stake"))
        {
            IEnemy dummy = EnemyFactory.Instance.GetEnemy(EnemyType.Stake);
            dummy.gameObject.transform.position = GameObject.Find("Stake").transform.position;
            enemies.Add(dummy);
        }
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        if (CurrentRoom != null)
        {
            for (int i = 0; i < CurrentRoom.enemies.Count; i++)
            {
                if (CurrentRoom.enemies[i].ShouldBeRemove == true)
                {
                    CurrentRoom.CurrentEnemyNum -= 1;
                    CurrentRoom.enemies.RemoveAt(i);
                    continue;
                }
                CurrentRoom.enemies[i].GameUpdate();
            }
        }
        foreach (IEnemy enemy in enemies)
        {
            enemy.GameUpdate();
        }
    }
    public void SetRoom(Room room)
    {
        CurrentRoom = room;
    }

}
