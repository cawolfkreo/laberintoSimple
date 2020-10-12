using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Clase encargada de almacenar y ejecutar el comando para mover
/// el personaje hacia abajo en el tablero.
/// </summary>
public class DownCommand : MovementCommand
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
        // el cual es hacia abajo.
        Direccion direc = Direccion.abajo;
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
        // dirección arriba.
        Direccion direc = Direccion.arriba;
        character.MoverseHacia(direc);
    }
}
