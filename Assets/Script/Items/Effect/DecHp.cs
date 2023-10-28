using DG.Tweening;
using TMPro;
using UnityEngine;

public class DecHp : Item
{
    private CanvasGroup group;
    private TextMeshProUGUI text;
    public DecHp(GameObject obj) : base(obj)
    {
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        group = transform.GetComponent<CanvasGroup>();
    }
    public void SetTextValue(int damage)
    {
        text.text = damage.ToString();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        text.color = Color.HSVToRGB(Random.Range(0f, 360f)/360f, 1, 1);
        DoAnimation();
    }
    private void DoAnimation()
    {
        group.alpha = 0;
        group.transform.localScale = Vector3.one * 0.5f;
        transform.position = transform.position + Vector3.left * Random.Range(-1f, 1f);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(group.DOFade(1, 0.2f));
        sequence.Join(group.transform.DOScale(Vector3.one, 0.2f));
        sequence.Join(group.transform.DOMoveY(group.transform.position.y + Random.Range(1f, 1.5f), 0.2f));
        sequence.AppendInterval(0.2f);
        sequence.Append(group.transform.DOMoveY(group.transform.position.y + Random.Range(2f, 2f), 0.2f));
        sequence.Join(group.DOFade(0, 0.2f));
        sequence.OnComplete(() =>
        {
            ItemPool.Instance.ReturnItem(EffectType.DecHp, this);
        });
        sequence.Play();
    }
}