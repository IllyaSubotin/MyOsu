using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Game.Core;
using UnityEngine;

public class EditNodeView : MonoBehaviour
{
    public GameObject hitCircle;
    public RectTransform hitCircleRectTransform;
    public Image hitCircleImage;

    public GameObject approachCircle;

    [HideInInspector]public float spawnTime;
    [HideInInspector]public float destroyTime;
    
    private float approachTime;
    private float size;
    

    private Sequence masterSequence;


    public void Initialize(float approachTime, float size, Vector2 position, float spawnTime)
    {
        transform.position = position;
        this.spawnTime = spawnTime;
        this.approachTime = approachTime;
        this.size = size;
        this.destroyTime = spawnTime + approachTime;

        // Початкові налаштування
        hitCircle.transform.localScale = Vector3.one * size;
        hitCircleImage.color = new Color(hitCircleImage.color.r, hitCircleImage.color.g, hitCircleImage.color.b, 0f);
        approachCircle.transform.localScale = Vector3.one * 2f;

        BuildMasterSequence();
    }

    private void BuildMasterSequence(float spawnDuration = 0.15f, float hitPulseDuration = 0.15f)
    {
        masterSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Pause();

        // 1. Spawn animation
        masterSequence.Append(
            hitCircle.transform.DOScale(1f, spawnDuration).SetEase(Ease.OutBack)
        );
        masterSequence.Join(
            hitCircleImage.DOFade(1f, spawnDuration)
        );

        // 2. Approach animation
        masterSequence.Append(
            approachCircle.transform.DOScale(1f, approachTime).SetEase(Ease.Linear)
        );

        // 3. Hit pulse animation
        masterSequence.Append(
            hitCircle.transform.DOScale(0f, hitPulseDuration).SetEase(Ease.InBack)
        );
        masterSequence.Join(
            hitCircleImage.DOFade(0f, hitPulseDuration)
        );

        masterSequence.OnComplete(() =>
        {
            hitCircle.gameObject.SetActive(false);
        });

        masterSequence.OnRewind(() =>
        {
            gameObject.SetActive(false);
        });
    }

    // Запуск вперед
    public void PlayForward()
    {
        hitCircle.gameObject.SetActive(true);
        hitCircle.transform.localScale = new Vector3(size, size, 1f); 
        hitCircleImage.color = new Color(hitCircleImage.color.r, hitCircleImage.color.g , hitCircleImage.color.b, 1f);

        masterSequence.Restart();
    }

    // Запуск назад
    public void PlayBackwards()
    {
        hitCircle.gameObject.SetActive(true);
        hitCircle.transform.localScale = new Vector3(size, size, 1f); 
        hitCircleImage.color = new Color(hitCircleImage.color.r, hitCircleImage.color.g , hitCircleImage.color.b, 1f);

        masterSequence.Goto(masterSequence.Duration(), true);
        masterSequence.PlayBackwards();
    }
}   
