
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerPopupNum : Item
{
    private GameObject Canvas;
    private TextMeshProUGUI text;
    private string s;
    private Color color;
    public PlayerPopupNum(GameObject obj) : base(obj) { }
    protected override void Init()
    {
        base.Init();
        Canvas = gameObject.transform.GetChild(0).gameObject;
        text = Canvas.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        text.color = color;
        text.text = s;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Canvas.transform.DOMoveY(position.y + 1f, 0.3f));
        sequence.Append(Canvas.GetComponent<CanvasGroup>().DOFade(0, 0.2f));
        sequence.OnComplete(() => { Remove(); });
        sequence.Play();
    }
    public void SetText(string s)
    {
        this.s = s;
    }
    public void SetColor(Color color)
    {
        this.color = color;
    }
}
