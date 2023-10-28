using SoulKnightProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AbstractController
{
    private IPlayer m_Player;
    public IPlayer Player => m_Player;
    public List<IPlayer> players = new List<IPlayer>();
    public List<IPlayerPet> pets = new List<IPlayerPet>();
    private MainPack pack;
    private MainPack upPack;
    private bool isEnterOnlineStartRoomResponse;
    private bool isUpdatePlayerStateResponse;
    private bool isHostPlayerInit;
    public PlayerController()
    {
        EventCenter.Instance.RegisterObserver<MainPack>(EventType.OnEnterOnlineStartRoomResponse, (pack) =>
        {
            isEnterOnlineStartRoomResponse = true;
            this.pack = pack;
        });
    }
    protected override void AlwaysUpdate()
    {
        base.AlwaysUpdate();
        if (isEnterOnlineStartRoomResponse)
        {
            isEnterOnlineStartRoomResponse = false;
            if (pack.IsBroadcastMessage)
            {
                AddPlayer(AttributeFactory.Instance.GetPlayerAttr(System.Enum.Parse<PlayerType>(pack.CharacterPacks[0].PlayerType)), pack.CharacterPacks[0]);
            }
            else
            {
                foreach (CharacterPack p in pack.CharacterPacks)
                {
                    if (p.CharacterName == ModelContainer.Instance.GetModel<MemoryModel>().UserName)
                    {
                        SetPlayer(AttributeFactory.Instance.GetPlayerAttr(System.Enum.Parse<PlayerType>(p.PlayerType)));
                        TurnOnController();
                    }
                    else
                    {
                        AddPlayer(AttributeFactory.Instance.GetPlayerAttr(System.Enum.Parse<PlayerType>(p.PlayerType)), p);
                    }
                }

                Debug.Log("设置角色");
            }
        }
        if (isUpdatePlayerStateResponse)
        {
            isUpdatePlayerStateResponse = false;
            foreach (IPlayer player in players)
            {
                if (player.UserName.CompareTo(upPack.CharacterPacks[0].CharacterName) == 0)
                {
                    player.m_Input = Message.ConvertToPlayerInput(upPack.CharacterPacks[0].InputPack);
                    player.gameObject.transform.position = player.m_Input.CharacterPos;
                }
            }
        }
    }
    protected override void Init()
    {
        base.Init();
        if (!ModelContainer.Instance.GetModel<MemoryModel>().isOnlineMode)
        {
            if (ModelContainer.Instance.GetModel<SceneModel>().sceneType == SceneType.Battle)
            {
                if (ModelContainer.Instance.GetModel<MemoryModel>().PlayerAttr != null)
                {
                    SetPlayer(ModelContainer.Instance.GetModel<MemoryModel>().PlayerAttr);
                }
                else
                {
                    SetPlayer(AttributeFactory.Instance.GetPlayerAttr(PlayerType.Knight));
                }
            }
        }
    }
    protected override void OnAfterRunInit()
    {
        base.OnAfterRunInit();
    }
    protected override void OnAfterRunUpdate()
    {
        base.OnAfterRunUpdate();
        if (!isHostPlayerInit && m_Player != null)
        {
            isHostPlayerInit = true;
            if (!ModelContainer.Instance.GetModel<MemoryModel>().isOnlineMode)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (obj.transform.parent.gameObject != m_Player.gameObject && obj.transform.parent.name != "Templar" && obj.transform.parent.name != "Knight")
                    {

                        obj.transform.parent.transform.Find("Collider").GetComponent<CapsuleCollider2D>().isTrigger = false;
                        if (obj.transform.parent.GetComponent<Rigidbody2D>() != null)
                        {
                            obj.transform.parent.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                        }
                    }
                }
            }
            else
            {
                if (GameObject.Find("BirthPoint"))//联机房间测试用
                {
                    m_Player.gameObject.transform.position = GameObject.Find("BirthPoint").transform.position;
                }
            }
            CameraUtility.Instance.SetFollow(m_Player.gameObject.transform);
            UnityTool.Instance.GetComponentFromChild<CapsuleCollider2D>(m_Player.gameObject, "Collider").isTrigger = false;
        }
        if (m_Player != null)
        {
            m_Player.m_Input = ModelContainer.Instance.GetModel<PlayerInputModel>().m_InputPack;
            m_Player.GameUpdate();
            foreach (IPlayer player in players)
            {
                player.GameUpdate();
            }
        }
        foreach (IPlayerPet pet in pets)
        {
            pet.GameUpdate();
        }
    }
    public void SetPlayer(PlayerAttribute attr)
    {
        m_Player = PlayerFactory.Instance.GetPlayer(attr.m_ShareAttr.Type, attr);
        if (ModelContainer.Instance.GetModel<MemoryModel>().isOnlineMode)
        {
            CoroutinePool.Instance.StartCoroutine(SendUpdatePack());
        }
        if (ModelContainer.Instance.GetModel<SceneModel>().sceneName == SceneName.MiddleScene)
        {
            m_Player.gameObject.SetActive(true);
        }
        if (ModelContainer.Instance.GetModel<SceneModel>().sceneName == SceneName.OnlineStartScene)
        {
            m_Player.EnterBattleScene();
        }

        if (ModelContainer.Instance.GetModel<SceneModel>().sceneName == SceneName.BattleScene)
        {
            m_Player.m_Attr.isRun = false;
        }
    }
    public void AddPlayer(PlayerAttribute attr, CharacterPack p)
    {
        players.Add(PlayerFactory.Instance.GetPlayer(attr.m_ShareAttr.Type, attr));
        players[players.Count - 1].gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        players[players.Count - 1].UserName = p.CharacterName;
        if (ModelContainer.Instance.GetModel<SceneModel>().sceneName == SceneName.OnlineStartScene)
        {
            players[players.Count - 1].EnterBattleScene();
        }
    }
    public void AddPet(PetType type, IPlayer player)
    {
        pets.Add(PlayerFactory.Instance.GetPlayerPet(type, player));
        if (SceneModelCommand.Instance.GetActiveSceneName() == SceneName.MiddleScene)
        {
            pets.ForEach(x => x.gameObject.SetActive(true));
        }
    }
    public void AddPet(IPlayerPet pet)
    {
        pets.Add(pet);
        if (SceneModelCommand.Instance.GetActiveSceneName() == SceneName.MiddleScene)
        {
            pets.ForEach(x => x.gameObject.SetActive(true));
        }
    }
    public IEnumerator SendUpdatePack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / 30f);
            ModelContainer.Instance.GetModel<PlayerInputModel>().m_InputPack.CharacterPos = m_Player.gameObject.transform.position;
            (ClientFacade.Instance.GetRequest(ActionCode.UpdatePlayerState) as RequestUpdatePlayerState).SendRequest(ModelContainer.Instance.GetModel<PlayerInputModel>().m_InputPack, OnUpdateStateResponse);
        }
    }
    private void OnUpdateStateResponse(MainPack pack)
    {
        isUpdatePlayerStateResponse = true;
        upPack = pack;
    }
    public IEnemy GetCloseEnemy(GameObject origin)
    {
        float min = 15;
        GameObject o = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector2.Distance(origin.transform.position, obj.transform.position) < min)
            {
                min = Vector2.Distance(origin.transform.position, obj.transform.position);
                o = obj;
            }
        }
        if (o == null)
        {
            return null;
        }
        return o.GetComponent<Symbol>().GetCharacter() as IEnemy;
    }
}
