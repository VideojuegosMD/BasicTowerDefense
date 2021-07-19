using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempo : MonoBehaviour
{
    [SerializeField]
    float tiempo;
    public bool contar;
    public static Tempo tManager;

    public void SetTime(float t) { tiempo = Time.unscaledTime + t; }

    private void Awake()
    {
        if (tManager == null)
        {
            tManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (contar)
        {
          if(tiempo <= Time.unscaledTime)
          {
                Debug.Log("He terminado de contar");

                Time.timeScale = 1f;
          }
          else
          {               
                Time.timeScale = 0.1f;
          }
           
        }  
    }
}
