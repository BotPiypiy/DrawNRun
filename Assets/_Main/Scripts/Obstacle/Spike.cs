using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float yOffset;
    [SerializeField] private float hideTime;
    [SerializeField] private float moveDuration;
    [SerializeField] private float unhideTime;
    private Sequence anim;

    private void Start()
    {
        anim = DOTween.Sequence();
        anim.SetAutoKill(false);
        Tweener temp;
        temp = transform.DOLocalMoveY(0, hideTime);
        anim.Append(temp);
        temp = transform.DOLocalMoveY(yOffset, moveDuration);
        anim.Append(temp);
        temp = transform.DOLocalMoveY(0, unhideTime);
        anim.Append(temp);
        temp = transform.DOLocalMoveY(-yOffset, moveDuration);
        anim.Append(temp);
        anim.SetRelative(true);
        anim.SetLoops(-1);
    }
}
