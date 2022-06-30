using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDrawer : MonoBehaviour
{
    public event System.Action<Vector3[]> OnNewFormation;

    [SerializeField] private KeyCode _drawingKey = KeyCode.Mouse0;

    [SerializeField] private LayerMask _drawingAreaLayerMask;

    [SerializeField] private float _deltaThreshold = 0.01f;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _pointsLimit;

    private float _deltaThresholdSqr;
    private bool _drawing;
    private Vector3 _prevP;
    private Camera _cam;

    private bool _pointChecked;
    private Vector3? _drawingAreaPoint;

    private readonly List<Vector3> _linePoints = new();

    public int GuysCount { get; set; }


    private void Start()
    {
        SetDeltaThreshold(_deltaThreshold);
        OnNewFormation += PotatoDrawer_OnNewFormation;
        GuysCount = 10;
        _cam = Camera.main;
    }

    private void PotatoDrawer_OnNewFormation(Vector3[] points)
    {
        CleanLine();
    }

    private void CleanLine()
    {
        _lineRenderer.positionCount = 0;
        _lineRenderer.SetPositions(new Vector3[0]);
    }

    private void SetDeltaThreshold(float deltaThreshold)
    {
        _deltaThreshold = deltaThreshold;
        _deltaThresholdSqr = deltaThreshold * deltaThreshold;
    }

    private void EnterDrawMode()
    {
        _drawing = true;
        if (TryGetDrawingAreaPoint(out var p))
        {
            _prevP = p;
        }
    }

    private void EndDrawing()
    {
        _drawing = false;

        CalculateFormationPoints();

    }

    private void CalculateFormationPoints()
    {
        if (GuysCount == 0)
        {
            _linePoints.Clear();
            return;
        }

        var pts = _linePoints;
        var ptsC = pts.Count;
        var arr = new Vector3[GuysCount];

        if (ptsC < GuysCount)
        {
            Vector3 fillElement = ptsC == 0 ? _prevP : pts[0];
            arr = new Vector3[GuysCount];
            Array.Fill(arr, fillElement);
            pts.Clear();
            OnNewFormation?.Invoke(arr);
            return;
        }
        ptsC = pts.Count;
        int deltaPoints = (int)(ptsC / (float)GuysCount);

        var c = GuysCount;

        for (int i = 0, n = 0; i < c; i++)
        {
            arr[i] = pts[n];
            n += deltaPoints;
        }
        pts.Clear();
        OnNewFormation?.Invoke(arr);

    }

    private void Update()
    {
        UpdateDrawingMode();

        if (_drawing)
        {
            EvaluateDraw();
        }
    }

    private void LateUpdate()
    {
        _pointChecked = false;

        if (_drawingAreaPoint.HasValue)
            _drawingAreaPoint = null;
    }

    private void EvaluateDraw()
    {
        TryGetDrawingAreaPoint(out var p);

        if ((p - _prevP).sqrMagnitude < _deltaThresholdSqr)
            return;

        var lps = _linePoints;

        Vector3 dir = p - _prevP;
        float length = (p - _prevP).magnitude;
        int pCount = (int)(length / _deltaThreshold);
        //Debug.Log(pCount);
        for (int i = 1; i < pCount; i++ )
        {
            p = _prevP + dir / pCount * i;

            lps.Add(p);
        }

        //Debug.Log(lps.Count);

        var lr = _lineRenderer;
        lr.positionCount = lps.Count;
        lr.SetPositions(lps.ToArray());

        _prevP = p;

        if(lps.Count > _pointsLimit)
        {
            EndDrawing();
        }
    }

    private bool TryGetDrawingAreaPoint(out Vector3 point)
    {
        if (_pointChecked)
        {
            point = _drawingAreaPoint ?? default;
            return _drawingAreaPoint.HasValue;
        }

        _pointChecked = true;

        Vector3 from = Input.mousePosition;
        from.z = 0;
        var ray = _cam.ScreenPointToRay(from);

        if (Physics.Raycast(ray, out var hitInfo, 10000, _drawingAreaLayerMask))
        {
            var tr = hitInfo.collider.transform;
            var p = tr.InverseTransformPoint(hitInfo.point);
            _drawingAreaPoint = point = new Vector3(p.x * tr.localScale.x, p.y, p.z * tr.localScale.z);
            return true;
        }

        _drawingAreaPoint = null;
        point = default;
        return false;
    }

    private void UpdateDrawingMode()
    {
        if (_drawing)
        {
            if (Input.GetKeyUp(_drawingKey) || !TryGetDrawingAreaPoint(out _))
            {
                EndDrawing();
            }
        }
        else
        {
            if (Input.GetKeyDown(_drawingKey) && TryGetDrawingAreaPoint(out _))
            {
                EnterDrawMode();
            }
        }
    }


}