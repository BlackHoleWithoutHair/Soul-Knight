using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelectPet : IPanel
{
    private int CurrentSkinIndex;
    private Animator PetAnimator;
    private MemoryModel model;
    private RectTransform Title;
    private RectTransform ImageLeftSkin;
    private RectTransform ImageRightSkin;
    private RectTransform DivBottom;
    public PanelSelectPet(IPanel parent) : base(parent)
    {
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
        isShowPanelAfterExit = true;
    }
    protected override void OnInit()
    {
        base.OnInit();

        model = ModelContainer.Instance.GetModel<MemoryModel>();
        PetAnimator = GameObject.Find("LittleCool").GetComponent<Animator>();
        Title = UnityTool.Instance.GetComponentFromChild<RectTransform>(gameObject, "Title");
        ImageLeftSkin = UnityTool.Instance.GetComponentFromChild<RectTransform>(gameObject, "ImageLeftSkin");
        ImageRightSkin = UnityTool.Instance.GetComponentFromChild<RectTransform>(gameObject, "ImageRightSkin");
        DivBottom = UnityTool.Instance.GetComponentFromChild<RectTransform>(gameObject, "DivBottom");

        Title.anchoredPosition = new Vector2(0, 55);
        DivBottom.anchoredPosition = new Vector2(0, -130);
        ImageLeftSkin.anchoredPosition = new Vector2(-350, 0);
        ImageRightSkin.anchoredPosition = new Vector2(350, 0);

        UnityTool.Instance.GetComponentFromChild<Button>(gameObject, "ButtonBack").onClick.AddListener(() =>
        {
            OnExit();
            DisappearAnim();
        });
        UnityTool.Instance.GetComponentFromChild<Button>(gameObject, "ButtonStart").onClick.AddListener(() =>
        {
            OnExit();
            DisappearAnim();
        });
        UnityTool.Instance.GetComponentFromChild<Button>(gameObject, "ButtonLeftSkin").onClick.AddListener(() =>
        {
            CurrentSkinIndex -= 1;
            model.PetType = GetSkinType(CurrentSkinIndex);
            SetPetAnimator(model.PetType);
        });
        UnityTool.Instance.GetComponentFromChild<Button>(gameObject, "ButtonRightSkin").onClick.AddListener(() =>
        {
            CurrentSkinIndex += 1;
            model.PetType = GetSkinType(CurrentSkinIndex);
            SetPetAnimator(model.PetType);
        });
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        CameraUtility.Instance.ChangeActiveCamera(CameraType.SelectCamera);
        CameraUtility.Instance.SetSelect(PetAnimator.transform);

        CurrentSkinIndex = 0;
        AppearAnim();
    }
    public override void OnExit()
    {
        base.OnExit();
        CameraUtility.Instance.ChangeActiveCamera(CameraType.StaticCamera);
    }
    private PetType GetSkinType(int index)
    {
        if (index < 0)
        {
            index = -index;
        }
        int i = index % System.Enum.GetNames(typeof(PetType)).Length;
        return (PetType)i;
    }
    private void SetPetAnimator(PetType type)
    {
        PetAnimator.runtimeAnimatorController = ProxyResourceFactory.Instance.Factory.GetPetAnimatorController(type.ToString());
    }
    private void AppearAnim()
    {
        ImageLeftSkin.DOAnchorPosX(0, 0.3f);
        ImageRightSkin.DOAnchorPosX(0, 0.3f);
        Title.DOAnchorPosY(-55, 0.3f);
        DivBottom.DOAnchorPosY(130, 0.3f);
    }
    private void DisappearAnim()
    {
        ImageLeftSkin.DOAnchorPosX(-350, 0.3f);
        ImageRightSkin.DOAnchorPosX(350, 0.3f);
        Title.DOAnchorPosY(55, 0.3f);
        DivBottom.DOAnchorPosY(-130, 0.3f);
    }
}
