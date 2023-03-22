using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Rigidbody diceRigidbody;
    [SerializeField] private Vector3 force;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();
        }
    }

    void Roll()
    {
        diceRigidbody.AddForce(force, ForceMode.Impulse);
        diceRigidbody.AddTorque(Random.insideUnitSphere * 10, ForceMode.Impulse);
    }
}
