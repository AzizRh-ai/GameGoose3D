using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 360, 0f)*Time.deltaTime);
    }
}
