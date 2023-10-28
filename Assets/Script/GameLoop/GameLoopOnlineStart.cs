using UnityEngine;

public class GameLoopOnlineStart : MonoBehaviour
{
    private MediatorOnlineStart m_Mediator;
    private void Start()
    {
        Time.timeScale = 1;
        m_Mediator = new MediatorOnlineStart();
    }
    private void Update()
    {
        m_Mediator.GameUpdate();
    }
}
