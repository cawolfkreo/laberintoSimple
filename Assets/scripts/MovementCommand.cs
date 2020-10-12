/// <summary>
/// Esta clase se usa para almacenar los comandos de movimiento
/// del jugador basado en el patrón de diseño Command.
/// </summary>
public abstract class MovementCommand
{
    /// <summary>
    /// Ejecuta el comando a realizar en el personaje que se 
    /// pasa por parámetro
    /// </summary>
    /// <param name="character">El personaje en el cual se debe 
    /// ejecutar el commando.</param>
    public abstract void Execute(Character character);
}
