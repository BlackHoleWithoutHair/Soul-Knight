using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DecHp:Item
{
    private CanvasGroup group;
    private Text text;
    public DecHp(GameObject obj) : base(obj) 
    {
        text = transform.Find("Text").GetComponent<Text>();
        group = transform.GetComponent<CanvasGroup>();
    }
    public void SetTextValue(int damage)
    {
        text.text=damage.ToString();
    }
    public void DoAnimation()
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