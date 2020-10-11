using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// El tiempo en segundos en el que se dejará de mostrar el tablero.
    /// </summary>
    [Tooltip("El tiempo en segundos en el que se dejará de mostrar el tablero.")]
    public float TiempoMostrarTablero = 10f;

    /// <summary>
    /// Momento en el cual debe empezar la transición para ocultar el
    /// tablero. Como debe ocurrir antes de que se oculte el tablero,
    /// debe ser menor a TiempoMostrarTablero
    /// </summary>
    [Tooltip("Momento en el cual debe empezar la transición para ocultar el tablero. Como debe ocurrir antes de que se oculte el tablero, debe ser menor a Tiempo Mostrar Tablero")]
    public float TiempoInicTransi = 9f;

    //public event Action OnWin;
    //public event Action OnLose;

    /// <summary>
    /// El componente encargado de controlar la interfáz.
    /// </summary>
    [Tooltip("El componente encargado de controlar la interfáz.")]
    public UIManager UIManag;

    /// <summary>
    /// El tablero donde ocurre el juego
    /// </summary>
    [Tooltip("El tablero donde ocurre el juego")]
    public BoardManager Tablero;

    /// <summary>
    /// El nivel de transparencia que tendrá la imagen una vez esta se active en el canvas.
    /// </summary>
    private float _AlphaObjetivo;

    /// <summary>
    /// Momento en el cual el tablero debe empezar a desaparecer.
    /// </summary>
    private float tiempoInicial;

    /// <summary>
    /// Momento en el cual el tablero debe desaparecer.
    /// </summary>
    private float tiempoFinal;

    /// <summary>
    /// La velocidad a la que se aplicará
    /// la opacidad que cubre al tablero.
    /// </summary>
    private float veloOpacidad;

    /// <summary>
    /// Cuando este componente despierta se obtienen las referencias o valores necesarios
    /// </summary>
    void Awake()
    {
        _AlphaObjetivo = UIManag.AlphaOriginal;        
    }

    /// <summary>
    /// se efectuan los cambios iniciales del juego y se calculan los tiempos
    /// de la transición para ocultar el tablero.
    /// </summary>
    void Start()
    {
        UIManag.AlphaActual = 0f;
        tiempoInicial = Time.time + TiempoInicTransi;
        tiempoFinal = Time.time + TiempoMostrarTablero;
        veloOpacidad = (tiempoFinal - tiempoInicial) * 0.016f;
    }

    /// <summary>
    /// Se hacen las revisiones del juego para conocer en que momento se
    /// encuentra y se hacen los cambios y ajustes necesarios al tablero
    /// o la interfaz.
    /// </summary>
    void Update()
    {
        if(Time.time < tiempoFinal)
        {
            if(Time.time >= tiempoInicial)
            {
                float transpareActual = UIManag.AlphaActual;

                float diferenciaTransp = Mathf.Abs(transpareActual - _AlphaObjetivo);

                if(diferenciaTransp > 0.00001f)
                {
                    transpareActual = Mathf.Lerp(transpareActual, _AlphaObjetivo, veloOpacidad);
                    UIManag.AlphaActual = transpareActual;
                }
            }
        }
        else
        {
            UIManag.AlphaActual = _AlphaObjetivo;
        }
    }
}
