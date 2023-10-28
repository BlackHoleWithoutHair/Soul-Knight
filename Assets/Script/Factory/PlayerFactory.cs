using UnityEngine;

public class PlayerFactory : Singleton<PlayerFactory>
{
    private PlayerFactory() { }
    public IPlayerPet GetPlayerPet(PetType type, IPlayer player)
    {
        IPlayerPet pet = null;
        GameObject obj = GameObject.Find("LittleCool");
        if (obj == null)
        {
            obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer("LittleCool"), (Vector2)player.transform.position + Random.insideUnitCircle * 4, Quaternion.identity);
        }
        pet = new LittleCool(obj, player);
        pet.transform.GetComponent<Animator>().runtimeAnimatorController = ProxyResourceFactory.Instance.Factory.GetPetAnimatorController(type.ToString());
        pet.gameObject.GetComponent<Symbol>().SetCharacter(pet);
        return pet;
    }
    public IPlayer GetPlayer(PlayerType type, PlayerAttribute attr)
    {
        IPlayer player = null;
        GameObject obj = null;
        switch (type)
        {
            case PlayerType.Knight:
                obj = GameObject.Find("Knight");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Knight(obj, attr);
                break;
            case PlayerType.Rogue:
                obj = GameObject.Find("Rogue");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Rogue(obj, attr);
                break;
            case PlayerType.Engineer:
                obj = GameObject.Find("Engineer");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Engineer(obj, attr);
                break;
            case PlayerType.Elf:
                obj = GameObject.Find("Elf");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Elf(obj, attr);
                break;
            case PlayerType.Assassin:
                obj = GameObject.Find("Assassin");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Assassin(obj, attr);
                break;

            case PlayerType.Alchemist:
                obj = GameObject.Find("Alchemist");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Alchemist(obj, attr);
                break;
            case PlayerType.Wizard:
                obj = GameObject.Find("Wizard");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Wizard(obj, attr);
                break;
            case PlayerType.Vampire:
                obj = GameObject.Find("Vampire");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Vampire(obj, attr);
                break;
            case PlayerType.Werewolf:
                obj = GameObject.Find("Werewolf");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Werewolf(obj, attr);
                break;
            case PlayerType.Pastor:
                obj = GameObject.Find("Pastor");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Pastor(obj, attr);
                break;
            case PlayerType.Druid:
                obj = GameObject.Find("Druid");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Druid(obj, attr);
                break;
            case PlayerType.Berserker:
                obj = GameObject.Find("Berserker");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Berserker(obj, attr);
                break;
            case PlayerType.Paladin:
                obj = GameObject.Find("Paladin");
                if (obj == null)
                {
                    obj = Object.Instantiate(ProxyResourceFactory.Instance.Factory.GetPlayer(type.ToString()));
                }
                player = new Paladin(obj, attr);
                break;

        }
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        UnityTool.Instance.GetComponentFromChild<CheckPlayerClick>(obj, "BulletCheckBox").enabled = false;
        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        if (player == null)
        {
            Debug.Log("PlayerFactory GetPlayer " + type + " return null");
        }
        if (GameObject.Find("BirthPoint") != null)
        {
            player.gameObject.transform.position = GameObject.Find("BirthPoint").transform.position;
        }
        if (UnityTool.Instance.GetComponentFromChild<Symbol>(player.gameObject, "BulletCheckBox") == null)
        {
            UnityTool.Instance.GetGameObjectFromChildren(player.gameObject, "BulletCheckBox").AddComponent<Symbol>();
        }

        UnityTool.Instance.GetComponentFromChild<Symbol>(player.gameObject, "BulletCheckBox").SetCharacter(player);

        //set player's skin by the attribute
        UnityTool.Instance.GetComponentFromChild<Animator>(player.gameObject, "Sprite").runtimeAnimatorController = ProxyResourceFactory.Instance.Factory.GetCharacterAnimatorController(attr.CurrentSkinType.ToString());
        return player;
    }
}
