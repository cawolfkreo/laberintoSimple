using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject piso;

    public GameObject instancia;

    // Start is called before the first frame update
    void Start()
    {
        instancia = Instantiate(piso, Vector3.zero, Quaternion.identity);
        bool esUnClon = instancia.name == string.Format("{0}(Clone)", piso.name);
        if(esUnClon)
        {
            Debug.Log("Si es un clon!!!");
        }
        else
        {
            Debug.Log("No es un clon!! :C");
        }
    }
}
