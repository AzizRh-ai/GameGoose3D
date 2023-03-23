using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    public event Action<int> OnDiceLaunched;

    [SerializeField] private GameObject[] faces = new GameObject[6];
    [SerializeField] private Rigidbody diceRigidbody;
    [SerializeField] private Vector3 force;
    private Rigidbody _dice;
    private bool _diceRolling;
    private Coroutine _checkDiceStoppedCoroutine;
    [SerializeField] private Player player;



    private void Start()
    {
        _dice = GetComponent<Rigidbody>();
        _diceRolling = true;
        Roll();
        _checkDiceStoppedCoroutine = StartCoroutine(CheckDiceStopped());
        player.ActivateDiceCamera(); // Ajoutez cette ligne

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_diceRolling)
            {
                Roll();
                if (_checkDiceStoppedCoroutine != null)
                {
                    StopCoroutine(_checkDiceStoppedCoroutine);
                }
                _checkDiceStoppedCoroutine = StartCoroutine(CheckDiceStopped());
                player.ActivateDiceCamera(); // Ajoutez cette ligne

            }
        }
    }

    private IEnumerator CheckDiceStopped()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() => !IsStopped());
        _diceRolling = false;

        int topFace = GetTopFace();
        Debug.Log("Resultu: " + topFace);

        OnDiceLaunched?.Invoke(topFace);


    }

    private bool IsStopped()
    {
        return _dice.velocity.sqrMagnitude < 0.01f && _dice.angularVelocity.sqrMagnitude < 0.01f;
    }

    void Roll()
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
}
