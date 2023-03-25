using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TurnPlayerManager;

public class Player : MonoBehaviour
{
    [SerializeField] private Dice _diceResult;
    [SerializeField] private Transform[] boardPositions;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private TurnPlayerManager turnManager;
    [SerializeField] private float jumpPower = 2f;

    [SerializeField] private int currentBoardPosition = 0;
    [SerializeField] private float moveSpeed = 5.0f;
    public IsPlayer playerType;
    public Dice dice;

    public event Action OnPlayerMoveFinished;

    private void Start()
    {
        _diceResult.OnDiceLaunched += Move;
        turnManager.OnActivePlayerChanged += HandleActivePlayerChanged;
    }

    private void OnDestroy()
    {
        _diceResult.OnDiceLaunched -= Move;
        turnManager.OnActivePlayerChanged -= HandleActivePlayerChanged;
    }

    private void Update()
    {
        if (playerType == TurnPlayerManager.IsPlayer.Human && Input.GetKeyDown(KeyCode.Space))
        {
            dice.RequestRoll();
        }
    }

    private void HandleActivePlayerChanged(TurnPlayerManager.IsPlayer currentPlayerType)
    {
        if (playerType == currentPlayerType)
        {
            if (currentPlayerType == TurnPlayerManager.IsPlayer.Human)
            {
                dice.ActivateDiceCamera();
            }
            else
            {
                dice.ActivateDiceCamera();
                StartCoroutine(AiPlayDelay());
            }
        }
    }

    public void Move(int move)
    {
        Debug.Log("Move: " + playerType + " to : " + move + " plateforme");

        StartCoroutine(MovePlayer(move));
        dice.DeactivateDiceCamera();
    }

    private IEnumerator AiPlayDelay()
    {
        dice.ActivateDiceCamera();

        dice.RequestRoll();
        yield return new WaitForSeconds(5f);

        //TODO: A corriger
        dice.DeactivateDiceCamera();
        dice.ActivateAiCamera();
    }

    private IEnumerator MovePlayer(int steps)
    {
        while (steps > 0)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = boardPositions[currentBoardPosition + 1].position;
            Debug.Log("CurrentPosition = " + currentBoardPosition);
            Debug.Log("steps = " + steps);
            float distanceToTarget = Vector3.Distance(startPosition, targetPosition);

            float jumpDuration = distanceToTarget / moveSpeed;

            float jumpProgress = 0f;
            float currentJumpHeight = 0f;
            float previousJumpHeight = 0f;

            while (jumpProgress < 1f)
            {
                jumpProgress += Time.deltaTime / jumpDuration;

                currentJumpHeight = Mathf.Lerp(0, jumpPower, jumpProgress) - Mathf.Lerp(0, jumpPower, jumpProgress * jumpProgress);

                float verticalMovement = currentJumpHeight - previousJumpHeight;
                previousJumpHeight = currentJumpHeight;

                characterController.Move((targetPosition - startPosition) * (Time.deltaTime / jumpDuration) + new Vector3(0, verticalMovement, 0));

                yield return null;
            }

            currentBoardPosition++;
            steps--;
        }
        OnPlayerMoveFinished?.Invoke();
        CheckWinOrLoose();

    }

    public void DisableDiceRolling()
    {
        _diceResult.DisableDiceRolling();
    }

    private void CheckWinOrLoose()
    {
        if (currentBoardPosition >= boardPositions.Length - 1)
        {
            if (playerType == TurnPlayerManager.IsPlayer.Human)
            {
                SceneManager.LoadScene("EndScreen");
            }
            else
            {
                //IA gagne..
                SceneManager.LoadScene("EndScreen");
            }
        }
    }
}
