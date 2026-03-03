using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Game.Core;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject hitCircle;
    public RectTransform hitCircleRectTransform;
    public Image hitCircleImage;

    public GameObject approachCircle;
    public Image approachCircleColor;
    public Sprite[] resultSprites;

    public GameObject resultPrefab;

    [HideInInspector]public float spawnTime;
    [HideInInspector]public float duration = 2;

    private Tween approachTween;

    public void Initialize(float approachTime, float size, Vector2 position, float spawnTime, Color color)
    {
        hitCircle.transform.localScale = new Vector3(size, size, 1f);
        hitCircleRectTransform.anchoredPosition = position;
        this.spawnTime = spawnTime;
        
        SetColor(color);

        
        PlaySpawnAnimation();
        PlayApproachAnimation(approachTime);
    }

    public void SetColor(Color color)
    {
        hitCircleImage.color = color;
        approachCircleColor.color = color;
    }

    public void OnHit(HitResult hitResult)
    {
        CreateResult(hitResult);

        approachTween.Kill();
        PlayHitPulse();
    }

    public void OnMiss()
    {
        PlayMissAnimation();
    }

    public void OnFail()
    {
        CreateResult(HitResult.Miss);

        approachTween?.Kill();
        PlayFailAnimation();
    }

    private void PlaySpawnAnimation(float duration = 0.15f)
    {
        //hitCircleImage.color = new Color(0, 0, 0, 0);

        Sequence seq = DOTween.Sequence();
        seq.SetId(TweenCategory.Gameplay);

        seq.Join(hitCircle.transform.DOScale(1f, duration).SetEase(Ease.OutBack));
        seq.Join(hitCircleImage.DOFade(1f, duration));
    }

    private void PlayApproachAnimation(float approachTime,float startScale = 3f)
    {
        approachCircle.transform.localScale = Vector3.one * startScale;

        Tween tween = approachCircle.transform
            .DOScale(1f, approachTime)
            .SetEase(Ease.Linear)
            .SetId(TweenCategory.Gameplay);

        approachTween = tween;
    }

    private void PlayHitPulse(float duration = 0.15f)
    {
        Sequence seq = DOTween.Sequence();
        seq.SetId(TweenCategory.Gameplay);

        seq.Join(hitCircle.transform.DOScale(0f, duration).SetEase(Ease.InBack));
        seq.Join(hitCircleImage.DOFade(0f, duration));
        seq.OnComplete(() =>
        {
            hitCircle.gameObject.SetActive(false);
        });

    }

    private void PlayMissAnimation(float duration = 0.2f)
    {
        Sequence seq = DOTween.Sequence();
        seq.SetId(TweenCategory.Gameplay);

        seq.Append(hitCircle.transform.DOShakeScale(duration, strength: 0.2f, vibrato: 10, randomness: 90));
    }


    private void PlayFailAnimation(float duration = 0.1f)
    {
        Sequence seq = DOTween.Sequence();
        seq.SetId(TweenCategory.Gameplay);

        // Тремтіння + зменшення та зникнення
        seq.Append(hitCircle.transform.DOShakeScale(duration, strength: 0.3f, vibrato: 10, randomness: 90));
        seq.Append(hitCircle.transform.DOScale(0f, 0.1f).SetEase(Ease.InBack));
        seq.Join(hitCircleImage.DOFade(0f, 0.1f));
        seq.OnComplete(() =>
        {
            hitCircle.gameObject.SetActive(false);
        });
    }


    private void CreateResult(HitResult hitResult)
    {
        var resultPos = new Vector3(transform.position.x, transform.position.y + 20f, transform.position.z);

        GameObject result = Instantiate(resultPrefab, resultPos, transform.rotation, transform.parent);

        result.GetComponent<NodeResult>().ResultShow(hitResult, resultSprites, duration);
    }
    
}
