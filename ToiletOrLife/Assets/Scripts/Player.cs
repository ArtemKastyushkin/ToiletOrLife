using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Path _pathPrefab;
    [SerializeField] private PathColor _pathColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Path _path;
    private float _speed = 2.0f;

    private bool _isDrawing = false;
    private bool _isMoving = false;

    private void Start()
    {
        GameInput.Instance.OnDrawingStarted += GameInput_OnDrawingStarted;
        GameInput.Instance.OnDrawingFinished += GameInput_OnDrawingFinished;

        GameInput.Instance.OnTestMoved += GameInput_OnTestMoved;
    }

    private void GameInput_OnTestMoved(object sender, System.EventArgs e)
    {
        if (_path != null)
            ChangeMovingState();
    }

    private void GameInput_OnDrawingStarted(object sender, System.EventArgs e)
    {
        if (_path == null && IsChoosen())
            ChangeDrawingState();
    }

    private void GameInput_OnDrawingFinished(object sender, System.EventArgs e)
    {
        _isDrawing = false;
    }

    private void Update()
    {
        if (_isDrawing)
            DrawPath();

        if (_isMoving)
            Move();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnDrawingStarted -= GameInput_OnDrawingStarted;
        GameInput.Instance.OnDrawingFinished -= GameInput_OnDrawingFinished;
    }

    private bool IsChoosen()
    {
        Vector2 pointerPosition = Pointer.GetPosition();

        return pointerPosition.x >= _spriteRenderer.bounds.min.x && pointerPosition.x <= _spriteRenderer.bounds.max.x 
            && pointerPosition.y >= _spriteRenderer.bounds.min.y && pointerPosition.y <= _spriteRenderer.bounds.max.y;
    }

    private void DrawPath()
    {
        Vector2 pathPointPosition = Pointer.GetPosition();

        if (_path == null)
        {
            _path = Instantiate(_pathPrefab, pathPointPosition, Quaternion.identity);
            _path.SetColor(_pathColor);
        }
        else
        {
            _path.SetPathPointPosition(pathPointPosition);
        }
    }

    private void Move()
    {
        Vector3 pathPoint = Vector3.zero;

        if (_path.GetPathPoint(ref pathPoint))
        {
            transform.position = Vector2.MoveTowards(transform.position, pathPoint, Time.deltaTime * _speed);

            if (transform.position == pathPoint)
                _path.GetNextPathPoint();
        }
        else
        {
            ChangeMovingState();
        }
    }

    private void ChangeDrawingState()
    {
        _isDrawing = !_isDrawing;
    }

    private void ChangeMovingState()
    {
        _isMoving = !_isMoving;
    }
}