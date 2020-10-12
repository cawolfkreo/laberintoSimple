using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /// <summary>
    /// El tablero de juego donde se encuentra
    /// este personaje.
    /// </summary>
    public BoardManager BoardManag;

    /// <summary>
    /// El tiempo, en segundos, que el personaje espera
    /// para ejecutar el siguiente movimiento.
    /// </summary>
    public float TiempoEntreMovimientos = 5f;

    /// <summary>
    /// La lista de comandos que el personaje debe ejecutar.
    /// </summary>
    private Queue<MovementCommand> _ComandosAEjecutar;

    /// <summary>
    /// El momento en el que el siguiente comando debe ser
    /// ejecutado por el personaje.
    /// </summary>
    private float _TiempoProximoCom = 0f;

    /// <summary>
    /// Bandera que almacena si el juego finalizó o no.
    /// </summary>
    private bool _FinalizoElJuego;

    /// <summary>
    /// Cuando el personaje se despierta obtiene las referencias
    /// necesarias para su funcionamiento.
    /// </summary>
    private void Awake()
    {
        _ComandosAEjecutar = new Queue<MovementCommand>();

        // Se suscribe el metodo para manejar el estado de perder
        // o ganar al personaje
        LevelManager.Instance.OnWin += HandleWinOrLose;
        LevelManager.Instance.OnLose += HandleWinOrLose;

        _FinalizoElJuego = false;
    }

    /// <summary>
    /// Mueve el personaje a la siguiente posición en el tablero
    /// basado en la dirección entregada por parámetro.
    /// </summary>
    /// <param name="direction">La dirección a la que se debe
    /// mover el personaje.</param>
    public void MoverseHacia(Direccion direction)
    {
        Vector2Int deltaPos = Vector2Int.zero;
        switch(direction)
        {
            case Direccion.arriba:
                deltaPos.y = -1;
                break;
            case Direccion.abajo:
                deltaPos.y = 1;
                break;
            case Direccion.derecha:
                deltaPos.x = 1;
                break;
            case Direccion.izquierda:
                deltaPos.x = -1;
                break;
            default:
                break;
        }

        // Se trata de realizar el movimiento y se revisa si el jugador
        // se sale del tablero si ejecuta este movimiento.
        bool sePudoMover = BoardManag.MoverPersonaje(deltaPos);
        bool haChocado = BoardManag.JugadorChocoPared();
        if (!sePudoMover || haChocado)
        {
            // Si el jugador salio del tablero, se procede a decir que este perdió
            LevelManager.Instance.TriggerLose();
        }

        bool haGanado = BoardManag.JugadorenMeta();
        if (haGanado)
        {
            LevelManager.Instance.TriggerWin();
        }
    }

    /// <summary>
    /// Agrega un nuevo comando a la cola de comandos que el personaje
    /// debe realizar.
    /// </summary>
    /// <param name="mvCommand">el nuevo comando a encolar</param>
    public void RegistrarComandos(MovementCommand mvCommand)
    {
        _ComandosAEjecutar.Enqueue(mvCommand);
    }

    /// <summary>
    /// Hace que el personaje ejecute uno a uno los comandos en la
    /// cola de comandos. Un comando solo es ejecutado luego de que
    /// pase un tiempo menor o igual a "TiempoEntreMovimientos", por
    /// lo que se espera que el tiempo entre cada comando sea el 
    /// ajustado en dicha variable.
    /// </summary>
    public void RealizarComandos()
    {
        // Se captura el tiempo actual
        // para calculos futuros
        float tiempoAct = Time.time;

        if (!_FinalizoElJuego &&_TiempoProximoCom <= tiempoAct && _ComandosAEjecutar.Count > 0)
        {
            MovementCommand comandoAEjecutar = _ComandosAEjecutar.Peek();

            comandoAEjecutar.Execute(this);

            // Se remueve el comando solo cuando este ya fue ejecutado
            _ = _ComandosAEjecutar.Dequeue();

            _TiempoProximoCom = tiempoAct + TiempoEntreMovimientos;
        }
    }

    /// <summary>
    /// Método que modifica el comportamiento del personaje cuando
    /// el juego entra en el estado de ganar o de perder.
    /// </summary>
    private void HandleWinOrLose()
    {
        _FinalizoElJuego = false;
    }
}
