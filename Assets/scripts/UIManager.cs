using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// El nivel de transparencia original de la imagen.
    /// </summary>
    public float AlphaOriginal { get; private set; }

    /// <summary>
    /// Esta es la imagen que se utilizará para cubrir al escenario en el juego.
    /// </summary>
    private Image ImgCubreEscenario;

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
    private void Awake()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        ImgCubreEscenario = canvas.GetComponentInChildren<Image>();
        AlphaOriginal = ImgCubreEscenario.color.a;
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
}
