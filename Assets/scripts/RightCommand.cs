using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Clase encargada de almacenar y ejecutar el comando para mover
/// el personaje hacía la derecha en el tablero.
/// </summary>
public class RightCommand : MovementCommand
{
    /// <summary>
    /// Ejecuta el comando respectivo en el jugador que
    /// se entrega por parámetro.
    /// </summary>
    /// <param name="character">El componente character
    /// que tiene el personaje del jugador</param>
    public override void Execute(Character character)
    {
        // La direccion hacia la que se moverá el comando
        // el cual es hacia la derecha.
        Direccion direc = Direccion.derecha;
        character.MoverseHacia(direc);
    }

    /// <summary>
    /// Deshace el comando respectivo en el jugador
    /// que se entrega por parámetro.
    /// </summary>
    /// <param name="character">El componente character
    /// que tiene el personaje del jugador</param>
    public override void Undo(Character character)
    {
        // La direccion hacia la que se moverá el personaje
        // para deshacer el comando, en este caso es la
        // dirección izquierda.
        Direccion direc = Direccion.izquierda;
        character.MoverseHacia(direc);
    }
}
