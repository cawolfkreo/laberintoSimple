using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// El componente encargado de manejar el nivel.
    /// </summary>
    [Tooltip("El componente encargado de manejar el nivel.")]
    public LevelManager LvlManag;

    /// <summary>
    /// El estado actual de la interfaz. True si está activada,
    /// False en caso contrario.
    /// </summary>
    public bool IsUIActive
    {
        get => Panel.activeSelf;
        set
        {
            Panel.SetActive(value);
        }
    }

    /// <summary>
    /// El nivel de transparencia original de la imagen.
    /// </summary>
    public float AlphaOriginal { get; private set; }

    /// <summary>
    /// Esta es la imagen que se utilizará para cubrir al escenario en el juego.
    /// </summary>
    private Image ImgCubreEscenario;

    /// <summary>
    /// El panel donde la interfaz para entregar los comandos va a ocurrir.
    /// </summary>
    private GameObject Panel;

    /// <summary>
    /// El nivel de transparencia actual de la imagen.
    /// </summary>
    public float AlphaActual
    {
        get => ImgCubreEscenario.color.a;
        set
        {
            if (ImgCubreEscenario != null)
            {
                SetAlphaImagen(value);
            }
        }
    }    

    /// <summary>
    /// Se obtienen las referencias de la interfáz cuando este componente inicia.
    /// </summary>
    void Awake()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        ImgCubreEscenario = canvas.GetComponentInChildren<Image>();
        AlphaOriginal = ImgCubreEscenario.color.a;

        Panel = GameObject.FindWithTag("panel");
    }

    /// <summary>
    /// Se hacen los ajustes necesarios en los componentes de la interfáz antes de
    /// que el juego comience.
    /// </summary>
    void Start()
    {
        Panel.SetActive(false);
    }

    /// <summary>
    /// Cambia el valor del nivel de transparencia de la imagen al entregado
    /// por parámetro.
    /// </summary>
    /// <param name="nuevoAlpha">el valor </param>
    private void SetAlphaImagen(float nuevoAlpha)
    {
        Color colorImg = ImgCubreEscenario.color;
        colorImg.a = nuevoAlpha;
        ImgCubreEscenario.color = colorImg;
    }

    /// <summary>
    /// Es llamado al momento en que se hace click en el botón
    /// de "go" en la interfaz.
    /// </summary>
    public void OnClickGo()
    {
        LvlManag.MostrarTablero();
    }
}
