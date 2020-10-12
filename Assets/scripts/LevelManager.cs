using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este script se encarga de manejar las secuencia del nivel,
/// los estados y eventos de ganar o perder.
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
    /// <summary>
    /// El evento de ganar al cual se suscriben los demás objetos en el juego.
    /// </summary>
    public event Action OnWin;

    /// <summary>
    /// El evento de perder al cual se suscriben los demás objetos en el juego.
    /// </summary>
    public event Action OnLose;

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
    private float _tiempoInicial;

    /// <summary>
    /// Momento en el cual el tablero debe desaparecer.
    /// </summary>
    private float _tiempoFinal;

    /// <summary>
    /// La velocidad a la que se aplicará
    /// la opacidad que cubre al tablero.
    /// </summary>
    private float _veloOpacidad;

    /// <summary>
    /// Bandera para indicar si se aceptan
    /// o no comandos por el usuario en el
    /// juego. True si se aceptan, False
    /// en caso contrario.
    /// </summary>
    private bool _AceptarComandos = false;

    /// <summary>
    /// Bandera para indicar si el juego esta
    /// reproduciendo o no los comandos del
    /// jugador en el tablero.
    /// </summary>
    private bool _EstaReproduciendo = false;

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
        _tiempoInicial = Time.time + TiempoInicTransi;
        _tiempoFinal = Time.time + TiempoMostrarTablero;
        _veloOpacidad = (_tiempoFinal - _tiempoInicial) * 0.016f;
    }

    /// <summary>
    /// Se hacen las revisiones del juego para conocer en que momento se
    /// encuentra y se hacen los cambios y ajustes necesarios al tablero
    /// o la interfaz.
    /// </summary>
    void Update()
    {
        OrdenTransicionesUI();

        if (_AceptarComandos)
        {
            CapturarComando();
        }
    }

    /// <summary>
    /// Ejecuta los cambios en orden que va a tener la escena. Esto se 
    /// refiere a que se encarga de las transiciones de la interfáz
    /// </summary>
    private void OrdenTransicionesUI()
    {
        float tiempoActual = Time.time;
        // Entre el inicio y final de la transición, se ajusta la transparencia de la imagen
        if (tiempoActual < _tiempoFinal && tiempoActual >= _tiempoInicial)
        {
            AjustarAlpha();
        }
        else if(tiempoActual > _tiempoFinal)
        {
            UIManag.AlphaActual = _AlphaObjetivo;
            // Cuando se oculta el tablero, se activa la interfaz y se ajusta la transparencia final de la imagen
            if (!_EstaReproduciendo)
            {
                if (!UIManag.IsUIActive)
                {
                    UIManag.IsUIActive = true;

                    _AceptarComandos = true;
                }
            }
            else
            {
                // Cuando se muestra otra vez el tablero, se deben ejecutar los comandos del personaje.
                Tablero.Character.RealizarComandos();
            }
        }
        
    }

    /// <summary>
    /// Ajusta el nivel de transparencia de la imagen
    /// en el frame actual del juego. Este metodo solo
    /// debe ser llamado cuando el juego se encuentra
    /// realizando una transicion como mostrar el
    /// tablero o dejar de mostrar el tablero.
    /// </summary>
    private void AjustarAlpha()
    {
        float transpareActual = UIManag.AlphaActual;

        float diferenciaTransp = Mathf.Abs(transpareActual - _AlphaObjetivo);

        if (diferenciaTransp > 0.00001f)
        {
            transpareActual = Mathf.Lerp(transpareActual, _AlphaObjetivo, _veloOpacidad);
            UIManag.AlphaActual = transpareActual;
        }
    }

    /// <summary>
    /// revisa si el usuario está presionando alguna
    /// de las flechas de movimiento en el teclado,
    /// crea el comando correspondiente y lo envia
    /// a el personaje del jugador.
    /// </summary>
    private void CapturarComando()
    {
        // Se crea una referencia nula al commando a ejecutar
        MovementCommand newCommand = null;

        // Se crea una referencia a la direccion
        // que tendra el comando a crear para
        // enviarlo a la interfaz.
        Direccion direccion = Direccion.derecha;

        // Si el jugador presiona alguna tecla,
        // entonces se crea el comando a ejecutar
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            newCommand = new UpCommand();
            direccion = Direccion.arriba;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            newCommand = new DownCommand();
            direccion = Direccion.abajo;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newCommand = new LeftCommand();
            direccion = Direccion.izquierda;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            newCommand = new RightCommand();
            // no es necesario modificar el valor de la variable "dirección".
        }

        // Si alguno de las flechas en el teclado
        // fue presionada, entonces se le envia
        // el comando al personaje del jugador.
        if(newCommand != null)
        {
            Tablero.Character.RegistrarComandos(newCommand);

            // Solo se agrega la flecha en la dirección del movimiento
            // del jugador cuando se recibe un comando de este.
            UIManag.AgregarFlechaUI(direccion);
        }
    }

    /// <summary>
    /// Se encarga de hacer los cambios necesarios para mostrar
    /// una vez más el tablero del juego
    /// </summary>
    public void MostrarTablero()
    {
        // Se marca que ya no se aceptan comandos.
        _AceptarComandos = false;

        // Se marca que el juego ya está 
        // reproduciendo los comandos
        _EstaReproduciendo = true;

        // Se procede a calcular los tiempos
        // en donde se volverá a mostrar el tablero
        _tiempoInicial = Time.time;
        _tiempoFinal = _tiempoInicial + (TiempoMostrarTablero - TiempoInicTransi);

        // Se cambia el nivel de transparencia objetivo a 0
        _AlphaObjetivo = 0f;

        // Se oculta la interfaz
        UIManag.IsUIActive = false;
    }

    /// <summary>
    /// Método creado para iniciar el broadcast del evento de perdida
    /// para los componentes del juego que se encuentran suscritos.
    /// </summary>
    public void TriggerWin()
    {
        OnWin?.Invoke();
    }

    /// <summary>
    /// Método creado para iniciar el broadcast del evento de perdida
    /// para los componentes del juego que se encuentran suscritos.
    /// </summary>
    public void TriggerLose()
    {
        OnLose?.Invoke();
    }
}
