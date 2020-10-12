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
    /// del objeto que deshace los comandos.
    /// </summary>
    [Tooltip("Este es el prefab que contiene objeto que deshace los comandos del jugador")]
    public GameObject Revertir;

    /// <summary>
    /// El prefab que contiene el sprite
    /// del jugador.
    /// </summary>
    [Tooltip("Este es el prefab que contiene el jugador.")]
    public GameObject Jugador;

    /// <summary>
    /// El script que se encarga de manejar
    /// al jugador y su movimiento.
    /// </summary>
    public Character Character { get; private set; }

    /// <summary>
    /// Vector 2d utilizado para representar la posición
    /// del jugador en el tablero.
    /// </summary>
    public Vector2Int PosJugador { get; private set; }

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
    /// Referencia para guardar la casilla que está
    /// debajo del jugador en el tablero.
    /// </summary>
    private GameObject _CasillaBajoJugador;

    /// <summary>
    /// Cuando el componente despierta, toma las
    /// referencias necesarias para su funcionamiento.
    /// </summary>
    void Awake()
    {
        _PosTablero = GetComponent<Transform>();

        PosJugador = new Vector2Int();

        // Se crea el tablero con los prefabs escogidos.
        // Esto es hecho para temas de demostración pero
        // es posible realizar este proceso cargando
        // el tablero de un JSON o de otras formas
        // que permitan un mejor control nivel por nivel.
        _Tablero = new GameObject[_NumFilas, _NumCols] {
            { Pared, Pared, Pared},
            { Meta, Revertir, Pared },
            { Pared, Jugador, Pared},
        };
    }

    /// <summary>
    /// Cuando el juego comienza, se crea el tablero y se instancia
    /// </summary>
    void Start()
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
                nuevoSuelo.transform.localPosition = PosTableroALocalPos(new Vector2Int(x, y), 0f);

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
                    float alturaObjeto = 1f;

                    // al momento de agregar al jugador en el tablero, se almacena su
                    // posición en el tablero y se le entrega la referencia a este
                    // componente del tablero.
                    if (objetoAInstanciar == Jugador)
                    {
                        PosJugador = new Vector2Int(x, y);

                        Character = NuevaInstancia.GetComponent<Character>();
                        Character.BoardManag = this;

                        // Como la casilla bajo el jugador siempre es el piso al
                        // iniciar la partida, se almacena null en esta variable.
                        _CasillaBajoJugador = null;

                        // Si el objeto es el jugador, la altura a agregarle debe
                        // ser mayor para que este se posicione sobre los demás
                        // objetos en el tablero al moverse sobre este.
                        alturaObjeto = 2f;
                    }
                    
                    // Se ajusta la altura del objeto de acuerdo a la altura calculada en "alturaObjeto".
                    NuevaInstancia.transform.localPosition -= new Vector3(0, 0, alturaObjeto);

                    // Se actualiza el tablero para que incluya la nueva instancia creada.
                    _Tablero[y, x] = NuevaInstancia;
                }
            }
        }
    }

    /// <summary>
    /// Mueve el jugador hacia la direccion que se entrega por parámetro.
    /// La dirección que se espera es el "cambio" o la diferencia que hay
    /// hacía la posición donde se espera al jugador
    /// </summary>
    /// <param name="movimiento">Vector que indica cuantas filas y 
    /// columnas debe moverse el personaje en el tablero.</param>
    /// <returns>True si el movimiento fue válido, false en caso
    /// de que algo ocurriera.</returns>
    public bool MoverPersonaje(Vector2Int movimiento)
    {
        Vector2Int nuevaPos = PosJugador + movimiento;

        // Si la nueva posicion está por fuera del tablero
        if(PosicionAFueraDelTablero(nuevaPos))
        {
            // Se retorna falso inmediatamente y no se efectua ningun cambio.
            return false;
        }

        GameObject ObjetoAMoverse = _Tablero[nuevaPos.y, nuevaPos.x];
        GameObject PersojJugador = _Tablero[PosJugador.y, PosJugador.x];

        // Se mueve el objeto del jugador en el tablero y se actualizan las
        // casillas y posiciones.
        PersojJugador.transform.localPosition = PosTableroALocalPos(nuevaPos, -2f);
        _Tablero[nuevaPos.y, nuevaPos.x] = PersojJugador;
        _Tablero[PosJugador.y, PosJugador.x] = _CasillaBajoJugador;
        PosJugador = nuevaPos;

        // Se actualiza la casilla sobre la que se va a mover el personaje
        _CasillaBajoJugador = ObjetoAMoverse;

        return true;
    }

    /// <summary>
    /// Convierte la posicion de la matriz que representa el tablero
    /// a una posicion relativa de unity, esta posicion se utiliza con
    /// "localposition" en los "GameObjects" que son hijos del GameObject
    /// del tablero.
    /// </summary>
    /// <param name="posicion">Las coordenadas dentro de la matriz</param>
    /// <returns>un Vector2 con la posicion local de Unity.</returns>
    private Vector3 PosTableroALocalPos(Vector2Int posicion, float posZ)
    {
        return new Vector3(posicion.x * espacioEntreCuadros, -posicion.y * espacioEntreCuadros, posZ);
    }

    /// <summary>
    /// Revisa si la posición que se entrega por parámetro se encuentra a
    /// fuera del tablero de juego
    /// </summary>
    /// <param name="posicion">La posición que se requiere ver si está afuera
    /// del tablero</param>
    /// <returns>True si se encuentra por fuera del tablero, False en caso 
    /// contrario</returns>
    private bool PosicionAFueraDelTablero(Vector2Int posicion)
    {
        bool afueraSuperior = posicion.x >= _NumCols || posicion.y >= _NumFilas;
        bool afueraInferior = posicion.x < 0 || posicion.y < 0;
        return afueraSuperior || afueraInferior;
    }

    /// <summary>
    /// Revisa si hay o no una pared en la posicion entregada por parámetro
    /// </summary>
    /// <param name="posicion">un vector2 de enteros con la coordenada en X y Y del
    /// tablero donde se quiere conocer si hay o no una pared</param>
    /// <returns>True en caso de que si lo este, Falso en caso contrario</returns>
    public bool HayParedEnLaPos(Vector2Int posicion)
    {
        return posicion.x == 0;
    }

    /// <summary>
    /// Este método revisa si un GameObject instanciado es o no
    /// una instancia de un prefab comparando sus nombres.
    /// </summary>
    /// <param name="instancia">El GameObject ya instanciado</param>
    /// <param name="prefab">El prefab que se sospecha fue clonado</param>
    /// <returns>True si instancia es una instancia del prefab, false en
    /// el caso contrario</returns>
    private bool EsClonDelPrefab(GameObject instancia, GameObject prefab)
    {
        if(instancia == null)
        {
            return false;
        }

        return instancia.name == string.Format("{0}(Clone)", prefab.name);
    }

    /// <summary>
    /// Revisa si el jugador se encuentra o no sobre la meta
    /// </summary>
    /// <returns>True si el jugador esta sobre la meta, false
    /// en caso contrario</returns>
    public bool JugadorenMeta()
    {
        return EsClonDelPrefab(_CasillaBajoJugador, Meta);
    }

    /// <summary>
    /// Revisa si el jugador ha chocado o no contra una pared.
    /// Esto lo hace revisando si el jugador se encuentra sobre
    /// una pared o no.
    /// </summary>
    /// <returns>True su el jugador está sobre una pared. False
    /// en caso contrario</returns>
    public bool JugadorChocoPared()
    {
        return EsClonDelPrefab(_CasillaBajoJugador, Pared);
    }
}
