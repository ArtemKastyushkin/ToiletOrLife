using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Path _pathPrefab;
    [SerializeField] private PathColor _pathColor;

    private Path _path;
    private Vector3[] _pathPoints;
    private int _pointer = 0;
    private float _speed = 2.0f;

    public bool IsDrawingFinished { get; private set; }

    private bool _startMoving = false;

    private void Start()
    {
        IsDrawingFinished = true;

        GameInput.Instance.OnDrawingStarted += GameInput_OnDrawingStarted;
        GameInput.Instance.OnDrawingFinished += GameInput_OnDrawingFinished;
    }

    private void GameInput_OnDrawingStarted(object sender, System.EventArgs e)
    {
        if (_path == null)
            IsDrawingFinished = false;
    }

    private void GameInput_OnDrawingFinished(object sender, System.EventArgs e)
    {
        IsDrawingFinished = true;

        _pathPoints = _path.GetPath();
    }

    private void Update()
    {
        if (IsDrawingFinished == false)
            DrawPath();

        if (Input.GetMouseButtonDown(1))
            StartMoving();

        if (_startMoving)
            Move();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnDrawingStarted -= GameInput_OnDrawingStarted;
        GameInput.Instance.OnDrawingFinished -= GameInput_OnDrawingFinished;
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void DrawPath()
    {
        Vector2 pathPointPosition = GetMousePosition();

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

    private void StartMoving()
    {
        _startMoving = true;
    }

    private void Move()
    {
        if (_pointer >= _pathPoints.Length)
        {
            _startMoving = false;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, _pathPoints[_pointer], Time.deltaTime * _speed);

        if (transform.position == _pathPoints[_pointer])
            _pointer++;
    }
}