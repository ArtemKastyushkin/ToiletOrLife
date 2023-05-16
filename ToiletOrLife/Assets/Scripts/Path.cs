using UnityEngine;

public class Path : MonoBehaviour
{
    private const float DRAWING_STEP = 0.1f;

    private LineRenderer _lineRenderer;
    private int _pointer = 0;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private bool CanAppend(Vector2 pathPointPosition)
    {
        if (_lineRenderer.positionCount == 0) 
            return true;

        return  Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1), pathPointPosition) >= DRAWING_STEP;
    } 

    public void SetColor(PathColor pathColor)
    {
        Color color = pathColor switch
        {
            PathColor.Blue => Color.blue,
            PathColor.Red => Color.red,
            _ => Color.blue,
        };

        _lineRenderer.startColor = _lineRenderer.endColor = color;
    }

    public void SetPathPointPosition(Vector2 pathPointPosition)
    {
        if (CanAppend(pathPointPosition) == false)
            return;

        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pathPointPosition);
    }

    public bool GetPathPoint(ref Vector2 pathPoint)
    {
        if (_pointer == _lineRenderer.positionCount)
        {
            return false;
        }
        else
        {
            pathPoint = _lineRenderer.GetPosition(_pointer++);
            return true;
        }
    }

    public float CalculatePathLength(Vector2 startPosition)
    {
        float pathLength = Vector2.Distance(startPosition, _lineRenderer.GetPosition(0));

        for (int i = 0; i < _lineRenderer.positionCount - 1; i++)
        {
            pathLength += Vector2.Distance(_lineRenderer.GetPosition(i), _lineRenderer.GetPosition(i + 1));
        }

        return pathLength;
    }
}