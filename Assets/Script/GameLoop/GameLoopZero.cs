using UnityEngine;
namespace MainMenuScene
{
    public class GameLoopZero : MonoBehaviour
    {
        private GameFacade mediator;
        void Start()
        {
            Time.timeScale = 1;
            mediator = new GameFacade();
        }

        void Update()
        {
            mediator.GameUpdate();
        }
    }
}

