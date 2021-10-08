using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MejoraTorreta : MonoBehaviour
{

    public AnimationCurve aC;
    public ConstructorManager constructorManager;
    public TorretaBasic torretaSelec;
    public Text tNombre, tDmg, tCadencia, tVel, tRango, tNivel, tValor, tCoste;
    public Image img;

    public int costeMejora;

    public float iDmg,iLvl,iLvlMax=100;
    public float fCadencia, fVel, fRango; //variables para incrementar los stats

    // Start is called before the first frame update
    void Start()
    {
        iDmg = 0;
        fCadencia = 0;
        fVel = 0;
        fRango = 0;
        iLvl = 0;
        iLvlMax = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CalcMejoraDmg(int i)
    {
        if (iLvl + i <= iLvlMax)
        {
            iDmg += i;
            tDmg.text = (torretaSelec.dmg + iDmg).ToString();
            iLvl += i;
            tNivel.text = "Nivel de torreta:" + "\n" + (torretaSelec.nivel + iLvl).ToString();
            costeMejora = Mathf.RoundToInt(aC.Evaluate(iLvl / iLvlMax)* iDmg);           

            Debug.Log(Mathf.RoundToInt(aC.Evaluate(iLvl / iLvlMax)));

            tCoste.text = "Coste de cambios:" + "\n" + costeMejora.ToString();
        }
        else
        {
            Debug.Log("No se puede mejorar más esta torreta.");
        }
    }

    public int CalcCoste(TorretaBasic t)
    {
        int coste = 0;


        return coste;
    }

    public void UpdateContent()
    {
        //Actualización de contenido con la torreta seleccionada
        tNombre.text = torretaSelec.name;
        tDmg.text = torretaSelec.dmg.ToString();
        tCadencia.text = torretaSelec.fireRate.ToString();
        tVel.text = torretaSelec.bulletSpeed.ToString();
        tRango.text = torretaSelec.rango.ToString();
        tNivel.text = "Nivel de torreta:" + "\n" + torretaSelec.nivel.ToString();
        tValor.text = "Valor de torreta:" + "\n" + torretaSelec.cost.ToString();
        tCoste.text = "Coste de cambios:" + "\n" + CalcCoste(torretaSelec);
        img.sprite = torretaSelec.icon;
    }
}
