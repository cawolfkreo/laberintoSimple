using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Este script se encarga de manejar la interfaz y sus componentes.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Enum creado para manejar la direccion de
    /// las flechas al momento en que se crean en
    /// la interfaz.
    /// </summary>
    public enum Direccion
    {
        arriba,
        derecha,
        abajo,
        izquierda
    }

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
    /// El objeto encargado de almacenar las flechas que se irán mostrando en la
    /// interfáz de usuario.
    /// </summary>
    private List<GameObject> Flechas;

    /// <summary>
    /// El recurso que contiene la imagen de la flecha
    /// </summary>
    private Texture2D ImgFlecha;

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

        Flechas = new List<GameObject>();

        // Se carga el recurso de la flecha para evitar cargarlo constantemente
        // más adelante.
        ImgFlecha = Resources.Load<Texture2D>("Images/flecha-izq");
    }

    /// <summary>
    /// Se hacen los ajustes necesarios en los componentes de la interfáz antes de
    /// que el juego comience.
    /// </summary>
    void Start()
    {
        Panel.SetActive(false);

        // Prueba de que agregar flechas de forma programática funciona
        // Esto se va a remover.
        AgregarFlechaUI(Direccion.izquierda);
        AgregarFlechaUI(Direccion.abajo);
        AgregarFlechaUI(Direccion.derecha);
        AgregarFlechaUI(Direccion.arriba);
        AgregarFlechaUI(Direccion.izquierda);
        AgregarFlechaUI(Direccion.abajo);
        AgregarFlechaUI(Direccion.derecha);
        AgregarFlechaUI(Direccion.arriba);
        AgregarFlechaUI(Direccion.izquierda);
        AgregarFlechaUI(Direccion.abajo);
        AgregarFlechaUI(Direccion.derecha);
        AgregarFlechaUI(Direccion.arriba);
        AgregarFlechaUI(Direccion.izquierda);
        AgregarFlechaUI(Direccion.abajo);
        AgregarFlechaUI(Direccion.derecha);
        AgregarFlechaUI(Direccion.arriba);
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
        LevelManager.Instance.MostrarTablero();
    }

    /// <summary>
    /// Agrega una flecha en la interfaz con la rotación
    /// entregada por parámetro.
    /// </summary>
    /// <param name="direc">la direccion de la flecha, puede ser arriba, abajo, izquierda o derecha</param>
    public void AgregarFlechaUI(Direccion direc)
    {
        float rotation = 0f;
        switch (direc)
        {
            case Direccion.arriba:
                rotation = 270f;
                break;
            case Direccion.derecha:
                rotation = 180f;
                break;
            case Direccion.abajo:
                rotation = 90f;
                break;
            case Direccion.izquierda:
            default:
                break;
        }
        CrearFlecha(rotation);
    }

    /// <summary>
    /// Crea una flecha con la rotación entregada por parámetro y la
    /// agrega al panel de la interfáz.
    /// </summary>
    /// <param name="rotacion"></param>
    private void CrearFlecha(float rotacion)
    {
        // Se guarda el total actual de flechas para procesos posteriores
        int totalflechas = Flechas.Count;

        // Se crea la flecha a partir de un objeto vacio
        GameObject vacio = new GameObject("Flecha" + totalflechas);
        GameObject Flecha = Instantiate(vacio, Panel.transform);

        // Se agrega al objeto vacio la imagen de la flecha
        RectTransform transfor = Flecha.AddComponent<RectTransform>();
        transfor.transform.SetParent(Flecha.transform.parent);
        transfor.localScale = Vector3.one;

        // Al agregar una nueva flecha, esta no debe cubrir las anteriores. En esta parte
        // se calcula el "padding" que cada flecha va a tener en base a la columna y fila en
        // la que le corresponde estar.
        int columna = totalflechas % 7;
        int paddingX = columna * ImgFlecha.width * 5;
        int fila = totalflechas / 7;
        int paddingY = fila * ImgFlecha.height * 5;

        // Se ajusta la posicion y rotación donde irá la imagen.
        transfor.anchoredPosition = new Vector2(-300f + paddingX, 150f - paddingY);
        transfor.Rotate(new Vector3(0f, 0f, rotacion));

        // Se carga el recurso 
        Image img = Flecha.AddComponent<Image>();
        img.sprite = Sprite.Create(ImgFlecha, new Rect(0, 0, ImgFlecha.width, ImgFlecha.height), new Vector2(0.5f, 0.5f));

        // Se agrega el objeto a la lista de flechas.
        Flechas.Add(Flecha);
    }
}
