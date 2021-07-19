using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConstructorManager : MonoBehaviour
{
    public GameObject selectedTower; // prefab de torre seleccionada, si tenemos una lista podemos modificar su asignación.
    public GameObject prefabColCamino; // prefab collider para el camino.
    public LineRenderer linea; // Recorrido de enemigos.
    Ray r; // Rayo desde cámara para comprobaciones
    RaycastHit rHitPerm; // Hit del rayo, donde golpea
    public bool canPosObject; // Bool para saber si podemos poner objeto
    public GameObject boxCheck, rangeCheck, boxCheckPrefab; // checkers y prefab de checker.
    public Material matChecker; // Material para el checker.
    public LayerMask l; // LayerMask, por si quisiesemos evitar algún collider concreto, cuidado con los trigger, no se tienen en cuenta.

    public bool constructMode; // bool para saber si estamos construyendo o simulando.
    public int constructPoints; // puntos de construcción, para mejorar la base.

    //Variables para check de UI
    PointerEventData m_PointerEventData; 
    EventSystem m_EventSystem; 
    GraphicRaycaster m_Raycaster;
    

    // Start is called before the first frame update
    void Start()
    {
        m_Raycaster = GameObject.FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = GameObject.FindObjectOfType<EventSystem>();
        m_PointerEventData = new PointerEventData(m_EventSystem);
        
        boxCheck = Instantiate(boxCheckPrefab); //Instanciamos el checker.
                                                //RangeChecker ya creado, asignado en Editor.
        GenerateLineColliders(); //Generar collider a lo largo de la linea.
    }
    void GenerateLineColliders() //Función para generar colliders mediante un prefab, radio definido en el prefab.
    {
        for (int i = 0; i < linea.positionCount; i++) //Bucle for por cada punto de la linea.
        {
            if (i + 1 >= linea.positionCount) { return; } //Comprobación para no pasarnos de cuenta.
            Vector3 pos1 = new Vector3(linea.GetPosition(i).x + linea.transform.position.x, 0, linea.GetPosition(i).z + linea.transform.position.z);
            Vector3 pos2 = new Vector3(linea.GetPosition(i + 1).x + linea.transform.position.x, 0, linea.GetPosition(i + 1).z + linea.transform.position.z);
            Vector3 vDir = pos2 - pos1;
            Vector3 vPos = (pos1 + pos2) / 2;
            Transform t = Instantiate(prefabColCamino, vPos, Quaternion.identity, linea.transform).transform;
            t.localScale = new Vector3(vDir.magnitude, 1, 1);
            t.right = vDir.normalized;
        }
    }
    
    // Update is called once per frame
     void Update()
     {
        if (constructMode) //Si estamos en modo de construcción.
        {
            if (!boxCheck.activeSelf) //Si los checker no están activos los activamos.
            {
                boxCheck.SetActive(true);
                rangeCheck.SetActive(true);
            }

            List<RaycastResult> results = new List<RaycastResult>(); //Resultado de impacto de UI.
            m_PointerEventData.position = Input.mousePosition; //posición de ratón para el EventSystem(UI).
            m_Raycaster.Raycast(m_PointerEventData,results); //Rayo comprobación, desde el GraphicRaycaster(UI).

            if (results.Count > 0) //Si hay resultados hay interfaz de por medio.
            {
                Debug.Log("Over UI");
                if (matChecker.GetColor("_Color") != Color.red)
                {
                    matChecker.SetColor("_Color", Color.red);
                }
                canPosObject = false; return;
            }
            else
            { //Si no hay resultados comprobamos si podemos poner torretas.

                r = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); //Rayo desde cámara y posición de ratón
                //  Debug.DrawRay(r.origin,r.direction*100);

                if (canPosObject) //Si podemos poner el objeto, gestionado en FixedUpdate
                {    
                    if (Input.GetMouseButtonDown(0)) //Al pulsar click izq.
                    {  
                        RaycastHit rHit; //Variable local para el resultado del rayo.

                        if (Physics.Raycast(r, out rHit)) //Disparo de rayo
                        {
                            if (rHit.collider.tag == "Suelo") //Si golpea en el suelo.
                            {
                                if ((constructPoints - selectedTower.GetComponent<TorretaBasic>().cost >= 0)) //Si tenemos puntos de construcción
                                {
                                    Instantiate(selectedTower, rHit.point, Quaternion.identity); //Instanciamos torreta.
                                    constructPoints -= selectedTower.GetComponent<TorretaBasic>().cost; //Quitamos los puntos usados.
                                    GameManager.main.tConstP.text = "Puntos de construcción: " + constructPoints; //Actualizamos texto.
                                }
                                else
                                {
                                    Debug.Log("No tienes suficientes puntos de construcción.");
                                }

                            }
                        }

                    }
                }
            }            
        }
        else //Si no estamos en modo de construcción
        {
            if (boxCheck.activeSelf) //Desactivamos checkers
            {
                boxCheck.SetActive(false);
                rangeCheck.SetActive(false);
            }
        }
    }
   
    private void FixedUpdate() //Ejecución de físicas.
    {
        if (constructMode) //Si estamos en modo construcción
        {
            if (Physics.Raycast(r, out rHitPerm, l)) //Rayo continuo
            {           
                //Ajustamos tamaño y posición de los checker, de rango y de hueco de tablero.
                boxCheck.transform.localScale = selectedTower.transform.GetChild(0).transform.localScale;               
                boxCheck.transform.position = rHitPerm.point + new Vector3(0, boxCheck.transform.localScale.y / 2, 0);

                rangeCheck.transform.localScale = Vector3.one * selectedTower.GetComponent<SphereCollider>().radius *2;                               
                rangeCheck.transform.position = rHitPerm.point;



                if (rHitPerm.collider.tag == "Suelo") //si lo golpeado es suelo
                {
                    //Proyectamos una caja con el tamaño que ocupa la torreta y obtenemos los colliders que haya dentro.
                    Collider[] checkColliders = Physics.OverlapBox(rHitPerm.point, selectedTower.transform.GetChild(0).transform.localScale, Quaternion.identity, l);
                    
                    foreach (Collider c in checkColliders) //Recorremos la lista de colliders
                    {
                        if (c.tag == "Torre" || c.tag == "Camino") //Si hay alguna otra torreta o está el camino no podemos poner torreta.
                        {
                            //   Debug.Log("No puedes posicionar el objeto aquí.");
                            if (matChecker.GetColor("_Color") != Color.red)
                            {
                                matChecker.SetColor("_Color", Color.red);
                            }
                            canPosObject = false; return;

                        }
                        else
                        {
                            if (matChecker.GetColor("_Color") != Color.green)
                            {
                                matChecker.SetColor("_Color", Color.green);
                            }
                            canPosObject = true;
                        }
                    }
                }
                else //Si no está en el suelo no podemos poner torreta.
                {
                    if (matChecker.GetColor("_Color") != Color.red)
                    {
                        matChecker.SetColor("_Color", Color.red);
                    }
                    canPosObject = false;
                }
            }
        }
    }
}
