using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NodeResult : MonoBehaviour
{
    public void ResultShow(HitResult result, Sprite[] resultSprites, float duration)
    {
        Image resultImage = this.GetComponent<Image>();
        // вибір спрайта
        switch (result)
        {
            case HitResult.Miss:
                resultImage.sprite = resultSprites[0];
                break;

            case HitResult.Ok:
                resultImage.sprite = resultSprites[1];
                break;

            case HitResult.Good:
                resultImage.sprite = resultSprites[2];
                break;

            case HitResult.Perfect:
                resultImage.sprite = resultSprites[3];
                break;
        }

        var rectTransformResult = transform as RectTransform;

        Sequence seq = DOTween.Sequence();

        seq.Join(
            rectTransformResult.DOAnchorPosY(rectTransformResult.anchoredPosition.y + 20f, duration)
            .SetEase(Ease.OutQuad)
        );

        seq.Join(
            resultImage.DOFade(0, duration)
        );

        seq.Join(
            rectTransformResult.DOScale(1.2f , duration)
        );

        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
