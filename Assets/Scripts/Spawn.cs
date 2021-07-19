using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{    

    float t;
    public float tALlegar = 1f;
    Transform tPlayer;
    // Start is called before the first frame update
    void Start()
    {      
        //tALlegar = Random.Range(0.01f, 0.5f);
        tPlayer = GameObject.FindGameObjectWithTag("Player").transform;       
    }

    // Update is called once per frame
    void Update()
    {
        if(t >= tALlegar)
        {

           // Tempo.tManager.SetTime(0.5f);
            //tALlegar = Random.Range(5, 10);
            t = 0;

            GameObject g = InstanceManager.main.GetItem();
            g.transform.position = transform.position;
            g.transform.forward = transform.forward;
            g.SetActive(true);
            g.GetComponent<Rigidbody>().AddForce(g.transform.forward * 200, ForceMode.Impulse);
            StartCoroutine(InstanceManager.main.IDesactivarObj(g));
           

        }
        else
        {
            t += Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
       transform.forward = Vector3.Lerp(transform.forward, (tPlayer.position + Vector3.up*(tPlayer.localScale.y/2)) - transform.position, 0.5f * Time.deltaTime);
    }
}
