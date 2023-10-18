using DG.Tweening;
using SoulKnightProtocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelLogin : IPanel
{
    private Button ButtonClose;
    private Button ButtonLogin;
    private Button ButtonRegister;
    private Toggle ToggleLogin;
    private Toggle ToggleRegister;
    private GameObject DivLogin;
    private GameObject DivRegister;
    private TMP_InputField InputLoginName;
    private TMP_InputField InputLoginPassword;
    private TMP_InputField InputRegisterName;
    private TMP_InputField InputRegisterPassword;
    private TMP_InputField InputRegisterPasswordRepeat;
    public PanelLogin(IPanel parent) : base(parent)
    {
        isShowPanelAfterExit = true;
        m_GameObject = UnityTool.Instance.GetGameObjectFromCanvas(GetType().Name);
    }
    protected override void OnInit()
    {
        base.OnInit();
        DivLogin = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivLogin");
        DivRegister = UnityTool.Instance.GetGameObjectFromChildren(m_GameObject, "DivRegister");
        ButtonClose = UnityTool.Instance.GetComponentFromChild<Button>(m_GameObject, "ButtonClose");
        ButtonLogin = UnityTool.Instance.GetComponentFromChild<Button>(DivLogin, "ButtonOk");
        ButtonRegister = UnityTool.Instance.GetComponentFromChild<Button>(DivRegister, "ButtonOk");
        ToggleLogin = UnityTool.Instance.GetComponentFromChild<Toggle>(m_GameObject, "ToggleLogin");
        ToggleRegister = UnityTool.Instance.GetComponentFromChild<Toggle>(m_GameObject, "ToggleRegister");
        InputLoginName = UnityTool.Instance.GetComponentFromChild<TMP_InputField>(DivLogin, "InputFieldName");
        InputLoginPassword = UnityTool.Instance.GetComponentFromChild<TMP_InputField>(DivLogin, "InputFieldPassword");
        InputRegisterName = UnityTool.Instance.GetComponentFromChild<TMP_InputField>(DivRegister, "InputFieldName");
        InputRegisterPassword = UnityTool.Instance.GetComponentFromChild<TMP_InputField>(DivRegister, "InputFieldPassword");
        InputRegisterPasswordRepeat = UnityTool.Instance.GetComponentFromChild<TMP_InputField>(DivRegister, "InputFieldPasswordRepeat");
        ButtonClose.onClick.AddListener(() =>
        {
            OnExit();
        });
        ButtonLogin.onClick.AddListener(() =>
        {
            if (InputLoginName.text == null || InputLoginName.text.Length == 0)
            {
                InputLoginName.placeholder.transform.GetComponent<TextMeshProUGUI>().text = "” œ‰≤ªƒ‹Œ™ø’";
            }
            if (InputLoginPassword.text == null || InputLoginPassword.text.Length == 0)
            {
                InputLoginPassword.placeholder.transform.GetComponent<TextMeshProUGUI>().text = "√‹¬Î≤ªƒ‹Œ™ø’";
            }
            if (InputLoginName.text.Length != 0 && InputLoginPassword.text.Length != 0)
            {
                (ClientFacade.Instance.GetRequest(ActionCode.Login) as RequestLogin).SendRequest(InputLoginName.text, InputLoginPassword.text, (pack) =>
                {
                    if (pack.ReturnCode == ReturnCode.Success)
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "µ«¬º≥…π¶");
                    }
                    if (pack.ReturnCode == ReturnCode.Fail)
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "µ«¬º ß∞‹");
                    }
                });
                Debug.Log("Client send login request to server");
            }
        });
        ButtonRegister.onClick.AddListener(() =>
        {
            if (InputRegisterName.text.Length == 0)
            {
                InputLoginName.placeholder.transform.GetComponent<TextMeshProUGUI>().text = "” œ‰≤ªƒ‹Œ™ø’";
            }
            if (InputRegisterPassword.text.Length == 0)
            {
                InputLoginPassword.placeholder.transform.GetComponent<TextMeshProUGUI>().text = "√‹¬Î≤ªƒ‹Œ™ø’";
            }
            if (InputRegisterPasswordRepeat.text.Length == 0)
            {
                EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "«Î‘Ÿ¥Œ ‰»Î√‹¬Î");
            }
            if (InputRegisterPassword.text.CompareTo(InputRegisterPasswordRepeat.text) != 0)
            {
                EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, " ‰»Îµƒ√‹¬Î≤ª“ª÷¬");
            }
            if (InputRegisterName.text.Length != 0 && InputRegisterPassword.text.Length != 0 &&
            InputRegisterPassword.text.CompareTo(InputRegisterPasswordRepeat.text) == 0)
            {
                (ClientFacade.Instance.GetRequest(ActionCode.Register) as RequestRegister).SendRequest(InputRegisterName.text, InputRegisterPassword.text, (pack) =>
                {
                    if (pack.ReturnCode == ReturnCode.Success)
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "◊¢≤·≥…π¶");
                    }
                    if (pack.ReturnCode == ReturnCode.Fail)
                    {
                        EventCenter.Instance.NotisfyObserver(EventType.OnWantShowNotice, "◊¢≤· ß∞‹");
                    }
                });
            }
        });
        InputLoginName.onValueChanged.AddListener((text) =>
        {
            if (text.Length != 0)
            {
                InputLoginName.placeholder.transform.GetComponent<TextMeshProUGUI>().text = "QQ” œ‰";
            }
        });
        InputLoginPassword.onValueChanged.AddListener((text) =>
        {
            if (text.Length != 0)
            {
                InputLoginPassword.placeholder.transform.GetComponent<TextMeshProUGUI>().text = "«Î ‰»Î√‹¬Î";
            }
        });
        InputRegisterName.onValueChanged.AddListener((text) =>
        {
            if (text.Length != 0)
            {
                InputRegisterName.placeholder.transform.GetComponent<TextMeshProUGUI>().text = "QQ” œ‰";
            }
        });
        InputRegisterPassword.onValueChanged.AddListener((text) =>
        {
            if (text.Length != 0)
            {
                InputRegisterPassword.placeholder.transform.GetComponent<TextMeshProUGUI>().text = "«Î ‰»Î√‹¬Î";
            }
        });
        ToggleLogin.onValueChanged.AddListener((isOn) =>
        {
            DivLogin.SetActive(isOn);
        });
        ToggleRegister.onValueChanged.AddListener((isOn) =>
        {
            DivRegister.SetActive(isOn);
        });

    }
    protected override void OnEnter()
    {
        base.OnEnter();
        m_GameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1080);
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 0.3f);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        m_GameObject.GetComponent<RectTransform>().DOAnchorPosY(1080, 0.3f);
    }
}
