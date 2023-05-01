using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [SerializeField] private Line _linePrefab;

    private Line _line;

    public bool IsDrawingStarted { get; private set; }
    public bool IsDrawingFinished { get; private set; }

    private void Start()
    {
        IsDrawingStarted = false;
        IsDrawingFinished = false;

        GameInput.Instance.OnDrawingStarted += GameInput_OnDrawingStarted;
        GameInput.Instance.OnDrawingFinished += GameInput_OnDrawingFinished;
    }

    private void GameInput_OnDrawingStarted(object sender, System.EventArgs e)
    {
        if (IsDrawingFinished)
            return;

        IsDrawingStarted = true;
    }

    private void GameInput_OnDrawingFinished(object sender, System.EventArgs e)
    {
        IsDrawingFinished = true;
        IsDrawingStarted = false;
    }

    private void Update()
    {
        if (IsDrawingStarted)
        {
            DrawPath();
        }
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnDrawingStarted -= GameInput_OnDrawingStarted;
        GameInput.Instance.OnDrawingFinished -= GameInput_OnDrawingFinished;
    }

    private void DrawPath()
    {
        Vector2 pathPointPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_line == null)
        {
            _line = Instantiate(_linePrefab, pathPointPosition, Quaternion.identity);
        }
        else
        {
            _line.SetPathPointPosition(pathPointPosition);
        }
    }
}