using Cinemachine;
using UnityEngine;


public class CameraUtility : Singleton<CameraUtility>
{
    private CinemachineVirtualCamera vc;
    private GameObject Cameras;
    private GameObject FollowCamera;
    private GameObject SelectCamera;
    private GameObject StaticCamera;
    private CameraUtility()
    {
        Cameras = GameObject.Find("Cameras");
        FollowCamera = Cameras.transform.Find("FollowCamera")?.gameObject;
        SelectCamera = Cameras.transform.Find("SelectCamera")?.gameObject;
        StaticCamera = Cameras.transform.Find("StaticCamera")?.gameObject;
        vc = FollowCamera?.GetComponent<CinemachineVirtualCamera>();
        EventCenter.Instance.RegisterObserver(EventType.OnSceneChangeComplete, () =>
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
            {
                Cameras = GameObject.Find("Cameras");
                FollowCamera = Cameras.transform.Find("FollowCamera").gameObject;
                SelectCamera = Cameras.transform.Find("SelectCamera").gameObject;
                StaticCamera = Cameras.transform.Find("StaticCamera").gameObject;
                vc = FollowCamera.GetComponent<CinemachineVirtualCamera>();
            }
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex > 1)
            {
                Cameras = GameObject.Find("Cameras");
                FollowCamera = Cameras.transform.Find("FollowCamera").gameObject;
                vc = FollowCamera.GetComponent<CinemachineVirtualCamera>();
            }
        });
    }
    public void SetFollow(Transform trans)
    {
        vc.Follow = trans;
    }
    public void SetSelect(Transform trans)
    {
        SelectCamera.GetComponent<CinemachineVirtualCamera>().Follow = trans;
    }
    public void SetStatic(Transform trans)
    {
        StaticCamera.GetComponent<CinemachineVirtualCamera>().Follow = trans;
    }
    public void ChangeActiveCamera(CameraType type)
    {
        switch (type)
        {
            case CameraType.StaticCamera:
                StaticCamera.SetActive(true);
                SelectCamera.SetActive(false);
                FollowCamera.SetActive(false);
                break;
            case CameraType.SelectCamera:
                SelectCamera.SetActive(true);
                StaticCamera.SetActive(false);
                FollowCamera.SetActive(false);
                break;
            case CameraType.FollowCamera:
                FollowCamera.SetActive(true);
                SelectCamera.SetActive(false);
                StaticCamera.SetActive(false);
                break;
        }
    }

}
