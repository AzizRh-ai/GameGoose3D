/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int ScoreValue;
    private float _speed = 0.5f;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScenarioData _scenario;
    [SerializeField] private GameObject _wallPrefab;
    public delegate void MessageEvent();
    public static event MessageEvent ObjetToucher;
    public delegate void Particule();
    public static event Particule PlayParticule;
    [SerializeField]private ParticleSystem _particle;
    [SerializeField] private GameObject followPlayer;
    private float movementX;
    private float movementY;
    private int FirstStart;


    //private bool _reset = false;
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Level - 1")
        {
            PlayerPrefs.DeleteKey("Score");
            PlayerPrefs.DeleteKey("ScoreValue");
        }


        _rigidbody = GetComponent<Rigidbody>();
        _scoreText.text = PlayerPrefs.GetString("Score");
        ScoreValue = PlayerPrefs.GetInt("ScoreValue");
    }

    private void Update()
    {
    }

    public void OnMove(InputValue movementvalue)
    {
        Vector2 movementVector = movementvalue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

    }

    private void FixedUpdate()
    {

        if (_rigidbody.velocity == Vector3.zero)
        {
            if (FirstStart < 1)
            {
                PlayParticule.Invoke();
                FirstStart++;
            }


        }
        else
        { 
            FirstStart = 0;
        }
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        _rigidbody.AddForce(movement * _speed);
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target_Trigger"))
        {
            Debug.Log(other.gameObject.transform.position);
            Destroy(other.gameObject);
            UpdateScore();
            ObjetToucher.Invoke();
            PlayParticule.Invoke();


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log(collision.gameObject.transform.position);
            Destroy(collision.gameObject);
            UpdateScore();
            ObjetToucher.Invoke();
            PlayParticule.Invoke();

        }
    }


    private void UpdateScore()
    {
        int i = 0;
        if (i < _scenario.FirstWalls.Length)
        {
            Instantiate(_wallPrefab, _scenario.FirstWalls[ScoreValue].position,Quaternion.identity);
            i++;
        }
       
        ScoreValue++;
        PlayerPrefs.SetString("Score", "Score : " + ScoreValue.ToString());
        _scoreText.text = PlayerPrefs.GetString("Score");




        if (ScoreValue == 8)
        {
            PlayerPrefs.SetInt("ScoreValue", ScoreValue);
            SceneManager.LoadScene("Level - 2");


        }

        if (ScoreValue == 16)
        {
            SceneManager.LoadScene("EndScreen");
        }

    }

}*/