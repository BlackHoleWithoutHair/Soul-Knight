using UnityEngine;
namespace MainMenuScene
{
    public class GameLoopZero : MonoBehaviour
    {
        private GameFacade mediator;
        void Start()
        {
            mediator = new GameFacade();
        }

        void Update()
        {
            mediator.GameUpdate();
        }
    }
}

