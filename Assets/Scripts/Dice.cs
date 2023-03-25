using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    public event Action<int> OnDiceLaunched;
    [SerializeField] private CinemachineVirtualCamera diceCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera aiCamera;

    [SerializeField] private GameObject[] faces = new GameObject[6];
    [SerializeField] private Rigidbody diceRigidbody;
    [SerializeField] private Vector3 force;
    [SerializeField] private Player _player;
    [SerializeField] private TurnPlayerManager turnManager;

    private Rigidbody _dice;
    private bool _diceRolling;
    private Coroutine _checkDiceStoppedCoroutine;
    private bool canRoll = true;
    private TurnPlayerManager.IsPlayer _activePlayerType = TurnPlayerManager.IsPlayer.Human;

    private void Start()
    {
        _dice = GetComponent<Rigidbody>();
        _diceRolling = false;
        turnManager.OnActivePlayerChanged += HandleActivePlayerChanged;
    }

    private void OnDestroy()
    {
        turnManager.OnActivePlayerChanged -= HandleActivePlayerChanged;
    }

    public void RequestRoll()
    {
        diceCamera.enabled = true;
        if (!_diceRolling && canRoll)
        {
            Roll();
            if (_checkDiceStoppedCoroutine != null)
            {
                StopCoroutine(_checkDiceStoppedCoroutine);
            }
            _checkDiceStoppedCoroutine = StartCoroutine(CheckDiceStopped());
        }
    }

    private void HandleActivePlayerChanged(TurnPlayerManager.IsPlayer currentPlayerType)
    {
        _activePlayerType = currentPlayerType;
    }

    public void EnableDiceRolling()
    {
        canRoll = true;
    }

    public void DisableDiceRolling()
    {
        canRoll = false;
    }

    private IEnumerator CheckDiceStopped()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitWhile(() => !IsStopped());
        _diceRolling = false;

        int topFace = GetTopFace();
        OnDiceLaunched?.Invoke(topFace);

        if (_activePlayerType == TurnPlayerManager.IsPlayer.Human)
        {
            // Idem besoin d'une optimisiation et refacto
            DeactivateDiceCamera();
            ActivatePlayerCamera();
        }
        else
        {
            DeactivateDiceCamera();
            ActivateAiCamera();
        }
    }

    private bool IsStopped()
    {
        return _dice.velocity.sqrMagnitude < 0.01f && _dice.angularVelocity.sqrMagnitude < 0.01f;
    }

    public void Roll()
    {
        _diceRolling = true;
        diceRigidbody.AddForce(force, ForceMode.Impulse);
        diceRigidbody.AddTorque(Random.insideUnitSphere * 10, ForceMode.Impulse);
    }

    public int GetTopFace()
    {
        float maxDot = -Mathf.Infinity;
        int resultFace = -1;

        for (int i = 0; i < 6; i++)
        {
            GameObject currentFace = faces[i];
            Vector3 faceDirection = currentFace.transform.up;
            float dot = Vector3.Dot(faceDirection, Vector3.up);

            if (dot > maxDot)
            {
                maxDot = dot;
                resultFace = i + 1;
            }
        }
        return resultFace;
    }

    public void ActivateDiceCamera()
    {
        diceCamera.enabled = true;
        playerCamera.enabled = false;
        aiCamera.enabled = false;
    }
    public void DeactivateDiceCamera()
    {
        diceCamera.enabled = false;
    }

    public void ActivatePlayerCamera()
    {
        playerCamera.enabled = true;
        aiCamera.enabled = false;
    }

    public void ActivateAiCamera()
    {
        aiCamera.enabled = true;
        playerCamera.enabled = false;
    }
}



