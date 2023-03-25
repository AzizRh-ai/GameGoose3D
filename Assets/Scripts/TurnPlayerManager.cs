using UnityEngine;

public class TurnPlayerManager : MonoBehaviour
{
    public enum IsPlayer
    {
        Human,
        AI
    }

    public delegate void ActivePlayerChangedEventHandler(IsPlayer currentPlayerType);
    public event ActivePlayerChangedEventHandler OnActivePlayerChanged;

    [SerializeField] private IsPlayer[] _playerOrder;
    [SerializeField] private Player[] players;
    private int _currentPlayerIndex;

    private void Start()
    {
        _currentPlayerIndex = 0;
        StartNewTurn();

        foreach (var player in players)
        {
            player.OnPlayerMoveFinished += StartNewTurn;
        }
    }

    private void OnDestroy()
    {
        foreach (var player in players)
        {
            player.OnPlayerMoveFinished -= StartNewTurn;
        }
    }

    public void StartNewTurn()
    {
        IsPlayer currentPlayerType = _playerOrder[_currentPlayerIndex];
        Debug.Log("Tour du player: " + currentPlayerType);
        OnActivePlayerChanged?.Invoke(currentPlayerType);

        _currentPlayerIndex = (_currentPlayerIndex + 1) % _playerOrder.Length;
    }
}
