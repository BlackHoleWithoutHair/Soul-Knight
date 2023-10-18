using SoulKnightProtocol;
using UnityEngine;

public class ToBattleRoom : MonoBehaviour
{
    private MemoryModel m_MemoryModel;
    private void Start()
    {
        m_MemoryModel = ModelContainer.Instance.GetModel<MemoryModel>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_MemoryModel.isOnlineMode)
        {
            if (collision.tag == "Player")
            {
                m_MemoryModel.PlayerAttr = collision.transform.parent.GetComponent<Symbol>().GetCharacter().m_Attr as PlayerAttribute;

                SceneModelCommand.Instance.LoadScene(SceneName.OnlineStartScene).completed += (op) =>
                {
                    (ClientFacade.Instance.GetRequest(ActionCode.EnterOnlineStartRoom) as RequestEnterOnlineStartRoom).SendRequest(m_MemoryModel.PlayerAttr);
                };
            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                m_MemoryModel.PlayerAttr = collision.GetComponent<Symbol>().GetCharacter().m_Attr as PlayerAttribute;
                SceneModelCommand.Instance.LoadScene(SceneName.BattleScene);
            }
        }
    }
}
