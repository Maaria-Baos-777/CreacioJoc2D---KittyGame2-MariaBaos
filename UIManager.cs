using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	//Boton intro/ retry para empezar juego
	public void StartGame()
	{
		SceneManager.LoadScene("GameScreen1");
	}

	//Boton return carga el menú principal
	public void GoToMenu()
	{
		SceneManager.LoadScene("Intro");
	}

}
