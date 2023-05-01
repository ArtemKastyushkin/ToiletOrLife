using UnityEngine;

public class Line : MonoBehaviour
{
    private const float DRAWING_STEP = 0.1f;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private bool CanAppend(Vector2 pathPointPosition)
    {
        if (_lineRenderer.positionCount == 0) 
            return true;

        return Vector2.Distance(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1), pathPointPosition) > DRAWING_STEP;
    } 

    public void SetColor(Color color)
    {
        _lineRenderer.startColor = _lineRenderer.endColor = color;
    }

    public void SetPathPointPosition(Vector2 pathPointPosition)
    {
        if (CanAppend(pathPointPosition) == false)
            return;

        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pathPointPosition);
    }
}