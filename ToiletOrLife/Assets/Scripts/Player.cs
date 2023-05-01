using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Path _pathPrefab;
    [SerializeField] private PathColor _pathColor;

    private Path _path;

    public bool IsDrawingFinished { get; private set; }

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
    }

    private void Update()
    {
        if (IsDrawingFinished == false)
            DrawPath();
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

    private void OnDestroy()
    {
        GameInput.Instance.OnDrawingStarted -= GameInput_OnDrawingStarted;
        GameInput.Instance.OnDrawingFinished -= GameInput_OnDrawingFinished;
    }
}