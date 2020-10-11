using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase creada para hacer el patrón de arquitectura Singleton con 
/// la que cualquier objeto que lo necesite puede heredarla y así
/// tener el patrón Singleton.
/// </summary>
/// <typeparam name="T">El script de tipo MonoBehaviour que va a heredar esta clase</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// Si la instancia está por ser eliminada o no.
    /// </summary>
    private static bool _porEliminarse;

    /// <summary>
    /// Objeto utilizado para asegurar que no existan problemas al 
    /// momento de eliminar o revisar la existencia de una instancia.
    /// </summary>
    private static object _Lock = new object();

    /// <summary>
    /// La, supues, única instancia de esta clase.
    /// </summary>
    private static T _Instance;

    /// <summary>
    /// La instancia pública del objeto. Se hacen revisiones importantes al
    /// momento en que se se pide el objeto para asegurar que la instancia
    /// está creada y si no lo está, crearla.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_porEliminarse)
            {
                Debug.LogWarning(string.Format("[Singleton] La instancia de {0} ya fue destruida, retornando null.", typeof(T)));
                return null;
            }
            lock(_Lock)
            {
                //sino hay una instancia, se debe revisar que no exista ya una
                //o se debe crear una nueva
                if(_Instance == null)
                {
                    _Instance = (T)FindObjectOfType(typeof(T));

                    if(_Instance == null)
                    {
                        //Sino hay una instancia, se crea.
                        var singletonObj = new GameObject();
                        _Instance = singletonObj.AddComponent<T>();
                        singletonObj.name = typeof(T).ToString() + "(Singleton)";

                        DontDestroyOnLoad(singletonObj);
                    }
                }

                return _Instance;
            }
        }
    }

    /// <summary>
    /// Si la aplicacion se cierra, se activa la bandera
    /// de pronta eliminación.
    /// </summary>
    private void OnApplicationQuit()
    {
        _porEliminarse = true;
    }

    /// <summary>
    /// Si el objeto se va a destruir, se activa la bandera
    /// de pronta eliminación.
    /// </summary>
    private void OnDestroy()
    {
        _porEliminarse = true;
    }
}
