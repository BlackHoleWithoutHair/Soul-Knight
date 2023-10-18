using UnityEngine;

namespace Edgar.Unity.Examples.Metroidvania
{
    public class MetroidvaniaCameraController : MonoBehaviour
    {
        public Camera MainCamera;
        public Camera LevelMapCamera;
        public GameObject Follow;

        public int ZoomDefault = 1;
        public int ZoomLevelMap = 20;

        private bool disabledLevelMap;

        public void Start()
        {
            if (MainCamera == null)
            {
                MainCamera = Camera.main;
            }

            SetOrthographicSize(MainCamera, ZoomDefault);
            SetOrthographicSize(LevelMapCamera, ZoomLevelMap);

            if (MetroidvaniaGameManager.Instance.LevelMapSupported())
            {
                var levelMapLayer = LayerMask.NameToLayer(MetroidvaniaGameManager.LevelMapLayer);
                MainCamera.cullingMask &= ~(1 << levelMapLayer);
                LevelMapCamera.cullingMask = 1 << levelMapLayer;
            }
        }

        public void Update()
        {
            if (InputHelper.GetKeyDown(KeyCode.LeftControl))
            {
                SetOrthographicSize(MainCamera, ZoomDefault);
                SetOrthographicSize(LevelMapCamera, ZoomLevelMap);

                if (MainCamera.gameObject.activeSelf)
                {
                    MainCamera.gameObject.SetActive(false);
                    LevelMapCamera.gameObject.SetActive(true);
                }
                else
                {
                    MainCamera.gameObject.SetActive(true);
                    LevelMapCamera.gameObject.SetActive(false);
                }
            }

            if (InputHelper.GetKeyDown(KeyCode.L))
            {
                SetOrthographicSize(MainCamera, ZoomDefault);
                SetOrthographicSize(LevelMapCamera, ZoomLevelMap);

                if (disabledLevelMap)
                {
                    disabledLevelMap = false;
                    var levelMapLayer = LayerMask.NameToLayer(MetroidvaniaGameManager.LevelMapLayer);
                    LevelMapCamera.cullingMask = 1 << levelMapLayer;
                }
                else
                {
                    disabledLevelMap = true;
                    LevelMapCamera.cullingMask = MainCamera.cullingMask;
                }
            }
        }

        private void SetOrthographicSize(Camera camera, int zoomLevel)
        {
            const int verticalUnitsOnScreen = 16;
            const int pixelsPerUnit = 16;

            var tempUnitSize = Screen.height / verticalUnitsOnScreen;
            var finalUnitSize = GetNearestMultiple(tempUnitSize, pixelsPerUnit);
            camera.orthographicSize = Screen.height / (finalUnitSize * 2.0f / zoomLevel);
        }

        private int GetNearestMultiple(int value, int multiple)
        {
            var rem = value % multiple;
            var result = value - rem;
            if (rem > (multiple / 2))
                result += multiple;

            return result;
        }

        public void LateUpdate()
        {
            MainCamera.transform.position = new Vector3(Follow.transform.position.x, Follow.transform.position.y, MainCamera.transform.position.z);
        }
    }
}