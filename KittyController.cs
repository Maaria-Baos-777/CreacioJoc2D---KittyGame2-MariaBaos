using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class KittyController : MonoBehaviour
{
	// Propiedades de configuración física ajustables desde el inspector
	[SerializeField] private float speed = 1f;
	[SerializeField] private float force = 7f;
	[SerializeField] private float barrelKnockbackForce = 6f;
	[SerializeField] private GameObject attackZone;

	// Componentes de audio y clips de efectos de sonido
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip coinSound;
	[SerializeField] private AudioClip jumpSound;
	[SerializeField] private AudioClip attackSound;
	[SerializeField] private AudioClip hurtSound;
	[SerializeField] private AudioClip winSound;
	[SerializeField] private AudioClip gameOverSound;
	[SerializeField] private AudioClip potionSound;

	// Variables internas del juego para el estado y coleccionables del jugador
	private int coins;
	private int potions;
	private int lifes = 3;
	private bool canJump = false;
	private bool isAttacking = false;
	private float horizontalMovementCat;

	// Referencias a los componentes físicos y visuales del objeto
	private Rigidbody2D rbCat;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private CapsuleCollider2D capsuleCollider;
	private Vector3 initialPosition;
	private Vector3 attackForm;

	// Referencia directa al script que gestiona la interfaz gráfica
	[Header("UI Reference")]
	public CanvaManager canvaManager;

	void Start()
	{
		// Capturamos los componentes del objeto al iniciar la escena
		rbCat = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		initialPosition = transform.position;
		spriteRenderer = GetComponent<SpriteRenderer>();
		capsuleCollider = GetComponent<CapsuleCollider2D>();
	}

	void Update()
	{
		// Entrada de salto: Solo se permite si se pulsa la tecla y el gato toca el suelo
		if (Input.GetKeyDown(KeyCode.W) && canJump)
		{
			if (audioSource != null && jumpSound != null) audioSource.PlayOneShot(jumpSound);
			rbCat.AddForce(transform.up * force, ForceMode2D.Impulse);
		}

		// Control de la orientación del sprite según la dirección del movimiento
		if (horizontalMovementCat < 0)
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
		else if (horizontalMovementCat > 0)
		{
			transform.localScale = new Vector3(-1, 1, 1);
		}

		// Captura del eje horizontal mediante el teclado (A/D o flechas)
		horizontalMovementCat = Input.GetAxisRaw("Horizontal");

		// Sincronización de variables físicas con los parámetros de animación
		animator.SetFloat("Speed", Mathf.Abs(horizontalMovementCat));
		animator.SetFloat("VerticalVelocity", rbCat.linearVelocity.y);
		animator.SetBool("IsGrounded", canJump);

		// Entrada de ataque: Clic izquierdo del ratón
		if (Input.GetMouseButtonDown(0))
		{
			if (audioSource != null && attackSound != null) audioSource.PlayOneShot(attackSound);
			StartCoroutine(Attack());
		}

		// Asignación de velocidad horizontal manteniendo el movimiento vertical de la gravedad
		rbCat.linearVelocity = new Vector2(horizontalMovementCat * speed, rbCat.linearVelocity.y);

		// Adaptación del tamaño del collider para que coincida exactamente con el sprite actual
		if (spriteRenderer.sprite != null)
		{
			Vector2 boundsSprite = spriteRenderer.sprite.bounds.size;
			capsuleCollider.size = boundsSprite;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Detección de suelo para habilitar de nuevo el salto
		if (collision.gameObject.CompareTag("Floor")) canJump = true;

		// Colisión con obstáculos dañinos (Pinchos)
		if (collision.gameObject.CompareTag("Pinchos"))
		{
			if (audioSource != null && hurtSound != null) audioSource.PlayOneShot(hurtSound);
			lifes -= 1;

			if (canvaManager != null)
			{
				canvaManager.ReducirCorazon(lifes);
			}

			if (lifes > 0)
			{
				ResetPosition();
			}
			else
			{
				if (audioSource != null && gameOverSound != null) audioSource.PlayOneShot(gameOverSound);
				StartCoroutine(KittyDeath());
			}
		}

		// Colisión física con el enemigo (Rata)
		if (collision.gameObject.CompareTag("Enemy"))
		{
			EnemyController enemyScript = collision.gameObject.GetComponent<EnemyController>();

			// Cálculo del vector de contacto para determinar la dirección del impacto
			ContactPoint2D contact = collision.contacts[0];
			bool isFallingOnTop = (contact.normal.y > 0.5f || rbCat.linearVelocity.y < -0.1f);

			// Comprobación del estado actual de la animación de ataque
			isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Cat-Attack");

			// Si atacamos desde arriba o ejecutamos el ataque frontal, infligimos daño al enemigo
			if ((isFallingOnTop && enemyScript != null) || isAttacking)
			{
				enemyScript.TakeDamage();

				// Si el daño fue por caída, aplicamos una fuerza de rebote vertical
				if (!isAttacking && isFallingOnTop)
				{
					rbCat.linearVelocity = new Vector2(rbCat.linearVelocity.x, 0);
					rbCat.AddForce(transform.up * (force * 0.9f), ForceMode2D.Impulse);
					canJump = true;
				}
			}
			else
			{
				// Si no atacamos y el contacto es lateral, el gato recibe el daño
				if (audioSource != null && hurtSound != null) audioSource.PlayOneShot(hurtSound);
				lifes -= 1;

				if (canvaManager != null) canvaManager.ReducirCorazon(lifes);

				if (lifes > 0) ResetPosition();
				else SceneManager.LoadScene("ScreenGameOver");
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Recolección y procesamiento del objeto Moneda
		if (collision.transform.CompareTag("Coin"))
		{
			if (audioSource != null && coinSound != null) audioSource.PlayOneShot(coinSound);

			Destroy(collision.gameObject);
			coins++;

			if (canvaManager != null) canvaManager.ActualizarMonedas(coins);
		}

		// Recolección y procesamiento del objeto Poción
		if (collision.transform.CompareTag("Potion"))
		{
			if (audioSource != null && potionSound != null) audioSource.PlayOneShot(potionSound);

			Destroy(collision.gameObject);
			potions++;

			if (canvaManager != null) canvaManager.ActualizarPociones(potions);
		}

		// Interacción física con elementos destruibles (Barril)
		if (collision.transform.CompareTag("Barrel"))
		{
			// Cálculo del vector de dirección para empujar al gato tras el impacto
			Vector2 knockbackDir = (rbCat.position - (Vector2)collision.transform.position).normalized;
			knockbackDir.y += 0.5f;
			knockbackDir = knockbackDir.normalized;

			rbCat.linearVelocity = new Vector2(rbCat.linearVelocity.x * 0.5f, 0);
			rbCat.AddForce(knockbackDir * barrelKnockbackForce, ForceMode2D.Impulse);

			// Desactivación de colisionadores del barril para evitar impactos repetidos
			BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();
			foreach (BoxCollider2D col in colliders)
			{
				col.enabled = false;
			}

			// Activación de la animación de ruptura y destrucción diferida del objeto
			Animator barrelAnim = collision.GetComponent<Animator>();
			if (barrelAnim != null) barrelAnim.enabled = true;

			Destroy(collision.gameObject, 0.20f);
		}

		// Validación de la condición de victoria al tocar el objetivo final (Pescado)
		if (collision.transform.CompareTag("Fish"))
		{
			if (potions >= 2)
			{
				if (audioSource != null && winSound != null) audioSource.PlayOneShot(winSound);

				// Desactivamos el collider del pescado inmediatamente para evitar bucles de carga
				Collider2D fishCollider = collision.gameObject.GetComponent<Collider2D>();
				if (fishCollider != null) fishCollider.enabled = false;

				Destroy(collision.gameObject);
				SceneManager.LoadScene("ScreenWin");
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		// Deshabilitamos el salto al perder el contacto directo con el suelo
		if (collision.gameObject.CompareTag("Floor")) canJump = false;
	}

	// Reposicionamiento del personaje en su punto de control inicial restableciendo velocidades
	private void ResetPosition()
	{
		transform.position = initialPosition;
		rbCat.linearVelocity = Vector2.zero;
	}

	// Corrutina temporal para activar y desactivar el estado de animación del ataque
	private IEnumerator Attack()
	{
		isAttacking = true;
		animator.SetBool("IsAttack", true);
		yield return new WaitForSeconds(1f);
		animator.SetBool("IsAttack", false);
		isAttacking = false;
	}

	// Corrutina de muerte: Inhabilita colisiones físicas antes de proceder al cambio de escena
	private IEnumerator KittyDeath()
	{
		Collider2D col = GetComponent<Collider2D>();
		if (col != null) col.enabled = false;

		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("ScreenGameOver");
	}
}