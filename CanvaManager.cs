using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvaManager : MonoBehaviour
{
	// Elementos de texto e interfaz gráfica configurables desde el editor de Unity
	[Header("Textos de la Interfaz")]
	public TMP_Text textCoins;
	public TMP_Text textPotion;

	[Header("Imágenes de los Corazones")]
	public Image[] heartImages;
	public Sprite emptyHeartSprite;

	// Actualiza el marcador numérico de la interfaz con la cantidad de monedas recolectadas
	public void ActualizarMonedas(int cantidadMonedas)
	{
		if (textCoins != null) textCoins.text = cantidadMonedas.ToString();
	}

	// Actualiza el formato del indicador de texto con las pociones conseguidas respecto al total
	public void ActualizarPociones(int cantidadPociones)
	{
		if (textPotion != null) textPotion.text = cantidadPociones.ToString() + "/2";
	}

	// Sustituye el contenedor gráfico del corazón por el sprite de contenedor vacío según el índice
	public void ReducirCorazon(int indiceVida)
	{
		if (indiceVida >= 0 && indiceVida < heartImages.Length)
		{
			if (heartImages[indiceVida] != null && emptyHeartSprite != null)
			{
				heartImages[indiceVida].sprite = emptyHeartSprite;
			}
		}
	}
}