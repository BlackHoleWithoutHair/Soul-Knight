using Edgar.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BattleScene
{
    public class GameLoopBattle : MonoBehaviour
    {
        // Start is called before the first frame update
        public GungeonCustomInput input;
        private GameFacade m_Facade;
        public GameFacade Mediator => m_Facade;
        private DungeonGeneratorGrid2D m_Generator;
        private AstarPath finder;
        private bool m_isFinishGenerate;
        public bool isFinishGenerate => m_isFinishGenerate;
        void Start()
        {
            m_Generator = GameObject.Find("Generator").GetComponent<DungeonGeneratorGrid2D>();
            finder = GameObject.Find("AStarPath").GetComponent<AstarPath>();
            m_isFinishGenerate = false;
            m_Generator.CustomInputTask = input;
            CoroutinePool.Instance.StartCoroutine(Generate());
            m_Facade = new GameFacade();
        }
        void Update()
        {
            m_Facade.GameUpdate();
        }
        private IEnumerator Generate()
        {
            yield return m_Generator.GenerateCoroutine();
            m_isFinishGenerate = true;
            SetTilemaps();
            finder.Scan();
            EventCenter.Instance.NotisfyObserver(EventType.OnFinishRoomGenerate);
            yield return new WaitForSeconds(1);
            EventCenter.Instance.NotisfyObserver(EventType.OnCameraArriveAtPlayer);
        }
        private void SetTilemaps()
        {
            GameObject Tilemaps = GameObject.Find("Generated Level").transform.Find("Tilemaps").gameObject;
            GameObject Wall = Tilemaps.transform.Find("Walls").gameObject;
            //Tilemaps.transform.Find("Floor").gameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
            Wall.layer = LayerMask.NameToLayer("Obstacle");
            Wall.tag = "Obstacles";
            Wall.GetComponent<CompositeCollider2D>().geometryType = CompositeCollider2D.GeometryType.Polygons;
            Wall.GetComponent<CompositeCollider2D>().offsetDistance = 0.3f;
            Wall.GetComponent<CompositeCollider2D>().vertexDistance = 0.01f;
            Tilemaps.transform.Find("Collideable").GetComponent<TilemapRenderer>().sortOrder = TilemapRenderer.SortOrder.TopLeft;
            Tilemaps.transform.Find("Collideable").GetComponent<TilemapRenderer>().mode = TilemapRenderer.Mode.Individual;
            Tilemaps.transform.Find("Collideable").gameObject.layer = LayerMask.NameToLayer("Obstacle");
            Tilemaps.transform.Find("Collideable").gameObject.tag = "Obstacles";
        }
    }
}
