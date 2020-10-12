﻿using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Clase encargada de almacenar y ejecutar el comando para mover
/// el personaje hacia arriba en el tablero.
/// </summary>
public class UpCommand : MovementCommand
{
    /// <summary>
    /// La direccion hacia la que se moverá el comando
    /// el cual es hacia arriba.
    /// </summary>
    private const Direccion _direc = Direccion.arriba;

    /// <summary>
    /// Ejecuta el comando respectivo en el jugador que
    /// se entrega por parámetro.
    /// </summary>
    /// <param name="character">El componente character
    /// que tiene el personaje del jugador</param>
    public override void Execute(Character character)
    {
        character.MoverseHacia(_direc);
    }
}