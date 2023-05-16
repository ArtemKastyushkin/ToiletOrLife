using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _playerCount = 0;
    private int _createdPathCount = 0;
    private float _minPathLength = 0.0f;

    public static GameManager Instance;

    public event EventHandler<OnAllPathsCreatedEventArgs> OnAllPathsCreated;
    public class OnAllPathsCreatedEventArgs : EventArgs
    {
        public float MinPathLength;
    }

    private void Awake()
    {
        Instance = this;

        Player.OnPlayerCreated += Player_OnPlayerCreated;
        Player.OnPathCreated += Player_OnPathCreated;
    }

    private void OnDestroy()
    {
        Player.OnPlayerCreated -= Player_OnPlayerCreated;
        Player.OnPathCreated -= Player_OnPathCreated;
    }

    private void Player_OnPlayerCreated(object sender, System.EventArgs e)
    {
        _playerCount++;
    }

    private void Player_OnPathCreated(object sender, Player.OnPathCreatedEventArgs e)
    {
        _createdPathCount++;

        SetMinPathLength(e.PathLength);

        if (_createdPathCount == _playerCount)
        {
            OnAllPathsCreated?.Invoke(this, new OnAllPathsCreatedEventArgs
            {
                MinPathLength = _minPathLength
            });
        }
    }

    private void SetMinPathLength(float pathLength)
    {
        if (_createdPathCount == 1)
            _minPathLength = pathLength;
        else if (_minPathLength > pathLength)
            _minPathLength = pathLength;
    }
}