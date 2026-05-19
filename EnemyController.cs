using UnityEngine;

public class EnemyController : MonoBehaviour
{
	// Componentes lógicos y físicos internos del enemigo
	private Rigidbody2D rb;
	private Animator animator;
	private bool isDead = false;

	// Configuración de estadísticas internas de la criatura
	private float speed;
	private int health = 1;

	void Start()
	{
		// Vinculación de los componentes del motor al inicializarse el objeto
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		// Si se ha procesado su muerte, detenemos cualquier actualización de volteo o estados
		if (isDead) return;

		// Modificación de la escala del transform para invertir visualmente la dirección del recorrido
		if (speed < 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}
		else if (speed > 0)
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
	}

	private void FixedUpdate()
	{
		// Detiene inmediatamente el empuje físico horizontal del cuerpo si ha muerto
		if (isDead)
		{
			rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
			return;
		}

		// Desplazamiento físico constante aplicando velocidad horizontal controlada
		rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Si entra en contacto con un objeto delimitador invisible, invierte el sentido de la marcha
		if (!isDead)
		{
			if (collision.gameObject.CompareTag("WalkingInverted"))
			{
				speed *= -1f;
			}
		}
	}

	// Interfaz pública para recibir daño externo desde el sistema de ataque del jugador
	public void TakeDamage()
	{
		if (isDead) return;

		health -= 1;

		if (health <= 0)
		{
			Muriendo();
		}
	}

	// Ejecuta el cese de funciones motrices, deshabilita colisiones físicas e inicia animación de muerte
	private void Muriendo()
	{
		isDead = true;

		// Desactivamos el colisionador para que deje de interactuar con el jugador de forma inmediata
		Collider2D col = GetComponent<Collider2D>();
		if (col != null) col.enabled = false;

		// Actualización del parámetro lógico en el gestor de animaciones
		if (animator != null)
		{
			animator.SetBool("isDead", true);
		}

		// Eliminación diferida del GameObject de la memoria para dar margen al desarrollo del clip visual
		Destroy(gameObject, 1f);
	}
}