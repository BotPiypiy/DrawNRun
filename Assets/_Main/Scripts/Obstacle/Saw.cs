using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private int direction;
    [SerializeField] private int duration;
    [SerializeField] private float xPos;
    private Tweener tweener;

    private void Start()
    {
        DoMove();
    }

    private void DoMove()
    {
        tweener = transform.DOMoveX(xPos * Mathf.Sign(direction), duration);
        direction *= -1;
        transform.DORotate(new Vector3(0, 0, 360) * direction, duration, RotateMode.FastBeyond360);
        tweener.onComplete += DoMove;
    }
}
