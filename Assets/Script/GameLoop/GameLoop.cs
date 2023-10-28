using UnityEngine;
namespace MiddleScene
{
    public class GameLoop : MonoBehaviour
    {
        // Start is called before the first frame update
        private GameFacade mediator;
        public GameFacade m_Facade => mediator;
        void Start()
        {
            Time.timeScale = 1;
            mediator = new GameFacade();
            Debug.Log(Application.persistentDataPath);
        }

        // Update is called once per frame
        void Update()
        {
            mediator.GameUpdate();
        }
    }
}

