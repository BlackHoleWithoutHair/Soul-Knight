using Edgar.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Room
{
    public RoomInstanceGrid2D roomInstanceGrid2D;
    public int CurrentEnemyNum;//由于enemys延迟加入,需要这个变量获取加入后的数量
    public int WaveNum = Random.Range(2, 4);
    private int m_SpawnEnemyNum = Random.Range(3, 6);
    public int SpawnEnemyNum => m_SpawnEnemyNum + WaveNum;
}
public class RoomController : AbstractController
{
    private List<RoomInstanceGrid2D> RoomInstances;
    private EnemyController enemyController;
    private Room enterRoom;
    private IPlayer player;
    private Animator m_FinishAnim;
    private bool isStart;
    private bool isEnterEnemyFloor;
    private bool isEnterEnemyFloorStart;
    private bool isClearEnemyStart;
    public RoomController()
    {
        m_FinishAnim = UnityTool.Instance.GetGameObjectFromCanvas("Finish").GetComponent<Animator>();
    }
    protected override void Init()
    {
        base.Init();
        enemyController = GameMediator.Instance.GetController<EnemyController>();
    }
    protected override void OnAfterRunInit()
    {
        base.OnAfterRunInit();
    }
    protected override void OnAfterRunUpdate()
    {
        base.OnAfterRunUpdate();
        if (GameMediator.Instance.GetController<PlayerController>().Player != null && !isStart)
        {
            isStart = true;
            player = GameMediator.Instance.GetController<PlayerController>().Player;
            RoomInstances = GameObject.Find("Generator").GetComponent<RoomPostProcessing>().GetRoomInstances();
            foreach (RoomInstanceGrid2D roomInstance in RoomInstances)
            {
                Room room = new Room();
                room.roomInstanceGrid2D = roomInstance;
                if ((roomInstance.Room as CustomRoom).RoomType == RoomType.BirthRoom)
                {
                    player.gameObject.transform.position = UnityTool.Instance.GetComponentFromChild<CompositeCollider2D>(roomInstance.RoomTemplateInstance, "Floor").bounds.center;
                }
                else if ((roomInstance.Room as CustomRoom).RoomType == RoomType.EnemyRoom)
                {
                    SpawnEnemies(room, false);
                }
                else if ((roomInstance.Room as CustomRoom).RoomType == RoomType.EliteEnemyRoom)
                {
                    SpawnEnemies(room, true);
                }
                else if ((roomInstance.Room as CustomRoom).RoomType == RoomType.TreasureRoom)
                {
                    CreateOtherTreasureBox(room);
                }
                else if ((roomInstance.Room as CustomRoom).RoomType == RoomType.BossRoom)
                {
                    room.WaveNum = 0;
                    enemyController.AddBoss(room, GetFloorCenter(room));
                }
                TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerEnter, player.gameObject, GetFloorCollider(room).gameObject, obj =>
                {
                    RoomType roomType = (roomInstance.Room as CustomRoom).RoomType;
                    if (roomType == RoomType.EnemyRoom || roomType == RoomType.EliteEnemyRoom || roomType == RoomType.BossRoom)
                    {
                        enterRoom = room;
                        isEnterEnemyFloor = true;
                        isEnterEnemyFloorStart = false;
                        isClearEnemyStart = false;
                    }
                });
                TriggerCenter.Instance.RegisterObserver(TriggerType.OnTriggerExit, player.gameObject, GetFloorCollider(room).gameObject, obj =>
                {
                    RoomType roomType = (roomInstance.Room as CustomRoom).RoomType;
                    if (roomType == RoomType.EnemyRoom || roomType == RoomType.EliteEnemyRoom || roomType == RoomType.BossRoom)
                    {
                        isEnterEnemyFloor = false;
                    }
                });
            }
        }
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        if (isEnterEnemyFloor)
        {
            RoomType roomType = (enterRoom.roomInstanceGrid2D.Room as CustomRoom).RoomType;
            if (IsPlayerInFloor(GetFloorCollider(enterRoom).bounds, player.gameObject.transform.Find("BulletCheckBox").GetComponent<CapsuleCollider2D>().bounds))
            {
                if (!isEnterEnemyFloorStart)
                {
                    isEnterEnemyFloorStart = true;

                    if (roomType != RoomType.BossRoom)
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnPlayerEnterBattleRoom, enterRoom);
                    }
                    else
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnPlayerEnterBossRoom, enterRoom);
                    }
                    CloseDoor(enterRoom.roomInstanceGrid2D);
                }
            }
            if (enterRoom.CurrentEnemyNum == 0 && enterRoom.WaveNum > 0)
            {
                if ((enterRoom.roomInstanceGrid2D.Room as CustomRoom).RoomType == RoomType.EliteEnemyRoom)
                {
                    SpawnEnemies(enterRoom, true);
                }
                else
                {
                    SpawnEnemies(enterRoom, false);
                }
            }
            else if (enterRoom.CurrentEnemyNum == 0 && !isClearEnemyStart)
            {
                isClearEnemyStart = true;
                CreateWhiteTreasureBox(enterRoom);
                ShowBattleFinishAnim();
                OpenDoor(enterRoom.roomInstanceGrid2D);
                TriggerCenter.Instance.RemoveObserver(TriggerType.OnTriggerEnter, player.gameObject, GetFloorCollider(enterRoom).gameObject);
            }

        }
    }
    private void CloseDoor(RoomInstanceGrid2D roomInstance)
    {
        foreach (DoorInstanceGrid2D door in roomInstance.Doors)
        {
            GameObject roomObj = door.ConnectedRoomInstance.RoomTemplateInstance;
            SetDoorAnimator(roomObj, true);
        }
        player.m_Attr.isBattle = true;
    }
    private void ShowBattleFinishAnim()
    {
        m_FinishAnim.gameObject.SetActive(true);
        m_FinishAnim.Play("Finish", 0, 0);
        CoroutinePool.Instance.StartAnimatorCallback(m_FinishAnim, "Finish", () =>
        {
            m_FinishAnim.gameObject.SetActive(false);
        });
    }
    private void OpenDoor(RoomInstanceGrid2D roomInstance)
    {
        player.m_Attr.isBattle = false;
        foreach (DoorInstanceGrid2D door in roomInstance.Doors)
        {
            GameObject roomObj = door.ConnectedRoomInstance.RoomTemplateInstance;
            SetDoorAnimator(roomObj, false);
        }
    }
    private void CreateWhiteTreasureBox(Room room)
    {
        ItemFactory.Instance.GetTreasureBox(TreasureBoxType.White, RandomPointInBounds(GetFloorCollider(room).bounds, 3));
    }
    private void CreateOtherTreasureBox(Room room)
    {
        if (ModelContainer.Instance.GetModel<MemoryModel>().Stage < 6)
        {
            ItemFactory.Instance.GetTreasureBox(TreasureBoxType.Brown, GetFloorCenter(room));
        }
        else
        {
            ItemFactory.Instance.GetTreasureBox(TreasureBoxType.Blue, GetFloorCenter(room));
        }
    }
    private void SetDoorAnimator(GameObject roomObj, bool isUp)
    {
        GameObject d = null;

        if ((d = UnityTool.Instance.GetGameObjectFromChildren(roomObj, "LeftVerDownDoor")) != null)
        {
            d.GetComponent<Animator>().SetBool("isUp", isUp);
        }
        if ((d = UnityTool.Instance.GetGameObjectFromChildren(roomObj, "RightVerDownDoor")) != null)
        {
            d.GetComponent<Animator>().SetBool("isUp", isUp);
        }
        if ((d = UnityTool.Instance.GetGameObjectFromChildren(roomObj, "TopHorDownDoor")) != null)
        {
            d.GetComponent<Animator>().SetBool("isUp", isUp);
        }
        if ((d = UnityTool.Instance.GetGameObjectFromChildren(roomObj, "BottomHorDownDoor")) != null)
        {
            d.GetComponent<Animator>().SetBool("isUp", isUp);
        }
    }
    private bool IsPlayerInFloor(Bounds floorBounds, Bounds playerBounds)
    {
        Vector2 dir = playerBounds.center - floorBounds.center;
        if (floorBounds.Contains(playerBounds.center))
        {

            if (dir.y < 0 && Mathf.Abs(dir.x) < floorBounds.extents.x - 2)//down
            {
                if (dir.y > -floorBounds.extents.y + 1.5)
                {
                    return true;
                }
            }
            else if (dir.x < 0 && Mathf.Abs(dir.y) < floorBounds.extents.y - 2)//left
            {
                if (dir.x > -floorBounds.extents.x)
                {
                    return true;
                }
            }
            else if (dir.x > 0 && Mathf.Abs(dir.y) < floorBounds.extents.y - 2)//right
            {
                if (dir.x < floorBounds.extents.x)
                {
                    return true;
                }
            }
            else
            {
                if (dir.y < floorBounds.extents.y)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void SpawnEnemies(Room room, bool isElite)
    {
        CompositeCollider2D FloorCollider = GetFloorCollider(room);
        var totalEnemiesCount = room.SpawnEnemyNum;
        while (room.CurrentEnemyNum < totalEnemiesCount)
        {
            // Find random position inside floor collider bounds
            var position = RandomPointInBounds(FloorCollider.bounds, 2f);

            // Check if the point is actually inside the collider as there may be holes in the floor, and the point is not in the wall.
            if (!FloorCollider.OverlapPoint(position))
            {
                continue;
            }

            // We want to make sure that there is no other collider in the radius of 1
            if (Physics2D.OverlapCircleAll(position, 1f).Any(x => !x.isTrigger))
            {
                continue;
            }
            // Pick random enemy prefab

            // Create an instance of the enemy and set position and parent
            enemyController.SpawnEnemy(room, position, isEnterEnemyFloorStart, isElite);
        }
        room.WaveNum--;
    }

    private Vector3 RandomPointInBounds(Bounds bounds, float margin = 0)
    {
        return new Vector3(
            RandomRange(bounds.min.x + margin, bounds.max.x - margin),
            RandomRange(bounds.min.y + margin, bounds.max.y - margin),
            RandomRange(bounds.min.z + margin, bounds.max.z - margin)
        );
    }

    private static float RandomRange(float min, float max)
    {
        return (float)(Random.Range(0f, 1f) * (max - min) + min);
    }
    private Vector2 GetFloorCenter(Room room)
    {
        return UnityTool.Instance.GetComponentFromChild<CompositeCollider2D>(room.roomInstanceGrid2D.RoomTemplateInstance, "Floor").bounds.center;
    }
    public Vector3 RandomPointInFloor(Room room, float margin = 0)
    {
        CompositeCollider2D collider = GetFloorCollider(room);
        while (true)
        {
            Vector2 position = RandomPointInBounds(collider.bounds, margin);
            if (!collider.OverlapPoint(position))
            {
                continue;
            }
            // We want to make sure that there is no other collider in the radius of 1
            if (Physics2D.OverlapCircleAll(position, 1f).Any(x => !x.isTrigger))
            {
                continue;
            }
            return position;
        }
    }
    public CompositeCollider2D GetFloorCollider(Room room)
    {
        return UnityTool.Instance.GetComponentFromChild<CompositeCollider2D>(room.roomInstanceGrid2D.RoomTemplateInstance, "Floor");
    }
}

