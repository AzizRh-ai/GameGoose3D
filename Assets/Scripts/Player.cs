using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Dice _diceResult;
    [SerializeField] private Transform[] boardPositions;
    [SerializeField] private Camera _diceCamera;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController characterController;

    private int currentBoardPosition = 0;
    public float moveSpeed = 5.0f;


    void Start()
    {
        _diceResult.OnDiceLaunched += Move;

    }
    private void OnDestroy()
    {
        _diceResult.OnDiceLaunched -= Move;
    }

    public void Move(int steps)
    {
        Debug.Log("step::" + steps);
        StartCoroutine(MovePlayer(steps));
        ActivatePlayerCamera(); // Ajoutez cette ligne

    }

    public float jumpPower = 2f; // Hauteur du saut
    private IEnumerator MovePlayer(int steps)
    {
        while (steps > 0)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = boardPositions[currentBoardPosition + 1].position;
            float distanceToTarget = Vector3.Distance(startPosition, targetPosition);

            // Calculez la durée du saut en fonction de la vitesse de déplacement
            float jumpDuration = distanceToTarget / moveSpeed;

            float jumpProgress = 0f;
            float currentJumpHeight = 0f;
            float previousJumpHeight = 0f;

            while (jumpProgress < 1f)
            {
                jumpProgress += Time.deltaTime / jumpDuration;

                // Utilisez une fonction d'interpolation pour déterminer la hauteur actuelle du saut
                currentJumpHeight = Mathf.Lerp(0, jumpPower, jumpProgress) - Mathf.Lerp(0, jumpPower, jumpProgress * jumpProgress);

                // Calculez le déplacement vertical pour cette itération
                float verticalMovement = currentJumpHeight - previousJumpHeight;
                previousJumpHeight = currentJumpHeight;

                // Déplacez le personnage vers la position cible tout en suivant la trajectoire de saut
                characterController.Move((targetPosition - startPosition) * (Time.deltaTime / jumpDuration) + new Vector3(0, verticalMovement, 0));

                yield return null;
            }

            currentBoardPosition++;
            steps--;
        }
    }


    public void ActivateDiceCamera()
    {
        _diceCamera.enabled = true;
        playerCamera.enabled = false;
    }

    public void ActivatePlayerCamera()
    {
        playerCamera.enabled = true;
        _diceCamera.enabled = false;
    }
}
