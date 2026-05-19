using UnityEngine;

public class Persistance : MonoBehaviour
{
	// Almacenamiento de la referencia estática global para el control de duplicados (Patrón Singleton)
	private static Persistance instancia;

	void Start()
	{
		// Comprobación de existencia previa: Si ya se registró una instancia en la escena anterior, autodestruye esta copia
		if (instancia != null && instancia != this)
		{
			Destroy(gameObject);
			return;
		}

		// Consolidación de la instancia y orden de inmunidad ante cargas de escenas sucesivas
		instancia = this;
		DontDestroyOnLoad(gameObject);
	}
}