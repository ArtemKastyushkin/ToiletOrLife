using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Path _pathPrefab;
    [SerializeField] private PathColor _pathColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Path _path;

    private Vector2 _currentPathPoint = Vector2.zero;

    private float _pathLength = 0.0f;
    private float _speed = 3.0f;

    private bool _isDrawing = false;
    private bool _isMoving = false;

    public static event EventHandler OnPlayerCreated;

    public static event EventHandler<OnPathCreatedEventArgs> OnPathCreated;
    public class OnPathCreatedEventArgs : EventArgs
    {
        public float PathLength;
    }

    private void Start()
    {
        GameInput.Instance.OnDrawingStarted += GameInput_OnDrawingStarted;
        GameInput.Instance.OnDrawingFinished += GameInput_OnDrawingFinished;

        OnPlayerCreated?.Invoke(this, EventArgs.Empty);

        GameManager.Instance.OnAllPathsCreated += GameManager_OnAllPathsCreated; ;
    }

    private void GameInput_OnDrawingStarted(object sender, System.EventArgs e)
    {
        if (_path == null && IsChoosen())
            _isDrawing = true;
    }

    private void GameInput_OnDrawingFinished(object sender, System.EventArgs e)
    {
        if (_isDrawing)
        {
            _isDrawing = false;

            _pathLength = _path.CalculatePathLength(transform.position);

            OnPathCreated?.Invoke(this, new OnPathCreatedEventArgs
            {
                PathLength = _pathLength
            });
        }
    }

    private void GameManager_OnAllPathsCreated(object sender, GameManager.OnAllPathsCreatedEventArgs e)
    {
        _isMoving = true;

        SetMovingSpeed(e.MinPathLength);
        SetNextPathPoint();
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

    private void SetNextPathPoint()
    {
        if (_path.GetPathPoint(ref _currentPathPoint) == false)
            _isMoving = false;
    }

    private void Move()
    {
        float speed = _speed * Time.deltaTime;
        float distance = Vector2.Distance(transform.position, _currentPathPoint);

        transform.position = Vector2.MoveTowards(transform.position, _currentPathPoint, speed);

        if ((Vector2)transform.position == _currentPathPoint)
        {
            while (speed > distance && _isMoving)
            {
                SetNextPathPoint();

                speed -= distance;
                distance = Vector2.Distance(transform.position, _currentPathPoint);

                transform.position = Vector2.MoveTowards(transform.position, _currentPathPoint, speed);
            }
        }
    }

    private void SetMovingSpeed(float minPathLength)
    {
        _speed *= _pathLength / minPathLength;
    }
}