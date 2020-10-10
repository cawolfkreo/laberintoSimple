using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este escript se encarga de manejar
/// el tamaño y objetos presentes en el tablero.
/// </summary>
public class BoardManager : MonoBehaviour
{
    /// <summary>
    /// este es el espacio que hay entre los diferentes
    /// cuadros de la grilla que representa al
    /// laberinto
    /// </summary>
    [Tooltip("Este es el espacio que hay entre los diferentes cuadros de la grilla que representa al laberinto.")]
    public float espacioEntreCuadros = 1f;

    /// <summary>
    /// El prefab que contiene el sprite
    /// de suelo del mapa.
    /// </summary>
    [Tooltip("Este es el prefab que contiene el suelo del mapa.")]
    public GameObject Suelo;

    /// <summary>
    /// El prefab que contiene el sprite
    /// de las paredes del mapa.
    /// </summary>
    [Tooltip("Este es el prefab que contiene las paredes del mapa.")]
    public GameObject Pared;

    /// <summary>
    /// El prefab que contiene el sprite
    /// de la Meta del mapa.
    /// </summary>
    [Tooltip("Este es el prefab que contiene la meta para el jugador.")]
    public GameObject Meta;

    /// <summary>
    /// El prefab que contiene el sprite
    /// del objeto que remueve comandos.
    /// </summary>
    [Tooltip("Este es el prefab que contiene objeto que remueve los comandos del jugador")]
    public GameObject remover;

    /// <summary>
    /// El prefab que contiene el sprite
    /// del jugador.
    /// </summary>
    [Tooltip("Este es el prefab que contiene el jugador.")]
    public GameObject Jugador;

    /// <summary>
    /// La cantidad de filas que tiene el tablero.
    /// </summary>
    private const int _NumFilas = 3;

    /// <summary>
    /// La cantidad de columnas que tiene el tablero.
    /// </summary>
    private const int _NumCols = 3;

    /// <summary>
    /// Guarda la posición donde estará el tablero.
    /// </summary>
    private Transform _PosTablero;

    /// <summary>
    /// El tablero que se utilizará para representar
    /// los objetos en el mapa.
    /// </summary>
    private GameObject[,] _Tablero;

    /// <summary>
    /// Cuando el componente despierta, toma las
    /// referencias necesarias para su funcionamiento.
    /// </summary>
    public void Awake()
    {
        _PosTablero = GetComponent<Transform>();

        // Se crea el tablero con los prefabs escogidos.
        // Esto es hecho para temas de demostración pero
        // es posible realizar este proceso cargando
        // el tablero de un JSON o de otras formas
        // que permitan un mejor control nivel por nivel.
        _Tablero = new GameObject[_NumFilas, _NumCols] {
            { Pared, Pared, Pared},
            { Meta, null, Pared },
            { Pared, Jugador, Pared},
        };
    }

    /// <summary>
    /// Cuando el juego comienza, se crea el tablero y se instancia
    /// </summary>
    public void Start()
    {
        CentrarTablero();
        AgregarObjetosAlTablero();
    }

    /// <summary>
    /// Se ajusta la posición del tablero en base a la cantidad
    /// y tamaño de los elementos que tendrá el tablero.
    /// </summary>
    private void CentrarTablero()
    {
        // Se posiciona el tablero en el centro de la pantalla.
        Camera principal = Camera.main;
        _PosTablero.position = principal.ScreenToWorldPoint(
            new Vector3(Screen.width / 2, Screen.height / 2, principal.nearClipPlane));

        // Esta es la distancia extra que debe desplazarse el punto inicial
        // del tablero para que al dibujarse, este se vea centrado.
        float deltaAlto = -0.5f * ((_NumFilas -1) * espacioEntreCuadros);
        float deltaAncho = 0.5f * ((_NumCols -1) * espacioEntreCuadros);

        // Se ajustan las distancias a la mitad para correr el 
        // tablero al centro de su posición.
        _PosTablero.position -= new Vector3(deltaAncho, deltaAlto, _PosTablero.position.z);
    }

    /// <summary>
    /// Agrega los prefabs de la matriz del tablero
    /// en las posiciones que les corresponde a cada
    /// objeto.
    /// </summary>
    private void AgregarObjetosAlTablero()
    {
        for (int y = 0; y < _NumFilas; ++y)
        {
            for (int x = 0; x < _NumCols; ++x)
            {
                // Se crea el suelo respectivo a la posición actual del tablero
                GameObject nuevoSuelo = Instantiate(Suelo, new Vector3(0, 0, 0), Quaternion.identity);
                nuevoSuelo.transform.parent = _PosTablero;
                nuevoSuelo.transform.localPosition = new Vector3(x * espacioEntreCuadros, -y * espacioEntreCuadros, 0f);

                // Se crea el objeto que debe ir sobre el suelo en esta posición.
                GameObject objetoAInstanciar = _Tablero[y, x];
                // El objeto solo se crea si en la matriz este no es un objeto nulo.
                if (objetoAInstanciar != null)
                {
                    GameObject NuevaInstancia = Instantiate(objetoAInstanciar, new Vector3(0, 0, 0f), Quaternion.identity);
                    NuevaInstancia.transform.position = nuevoSuelo.transform.position;
                    NuevaInstancia.transform.parent = _PosTablero;

                    // Para que este objeto se encuentre sobre el suelo, se le agrega 1f a su "altura"
                    // del eje Z.
                    NuevaInstancia.transform.localPosition -= new Vector3(0, 0, 1f);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
