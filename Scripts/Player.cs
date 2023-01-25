using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb; // la hitbox du Player (21.01.2023)
    public float speed; // variable pour le déplacement (21.01.2023)
    public float JumpForce; // variable (21.01.2023)
    private float moveInput; // variable (21.01.2023)

    private bool isGrounded; // boucle permettant de savoir si le joueur est au sol (21.01.2023)
    public Transform CheckPos; // position de la zone pour voir si il est grounded (21.01.2023)
    public float RadiusCheck; // zone pour savoir si le joueur est au sol (21.01.2023)
    public LayerMask wahtIsGround; // Detecter quel object sont considérer comme du sol (21.01.2023)

    private float JumpTimeCounter; // temps pendant le quel il peut continuer de sauter (21.01.2023)
    public float JumpTime; // temps maximal pendant le quel il peut continuer de sauter (21.01.2023)
    private bool isJumping; // boucle pour savoir si il saute (21.01.2023) 

    private Animator anim;  // initialasation du componenent animator (23.01.2023)

    private float timeAttack; //le temps restant pour attaquer (25.01.2023)
    public float startTimeAttack; // le temps total pour attaquer (25.01.2023)

    public Transform attackPos; // position du cercle qui va infliger des dégats au enemies (25.01.2023)
    public LayerMask whatIsEnemies; // detecter c'est quoi un enemie (25.01.2023)
    public float attackRange; // ryon du cercle qui permet de tuer les enemies (25.01.2023)
    public int damage; // les demage infliger (25.01.2023)

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // va identifier le script rigidbody pour l'utiliser  (21.01.2023)
        anim = GetComponent<Animator>(); // va identifier le script animator pour l'utiliser (23.01.2023)
    }

    // Update is called once per frame
    void Update()
    {
        Move(); // fonction (22.01.2023)
        Jump(); // fonction (21.01.2023)
        Attack(); // fonction (25.01.2023)
    }

    private void Move()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); // initialiser le moveInput, il est égal au fait que lorsque on appuie sur une flèche à droite ou à gauche l'on pose une valeur entre -1 et 1 (21.01.2023)
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); // crée un vecteur en 2d sur le rigidbody qui multiplie le moveInput par la variable speed  sur l'axe x et garde la vitesse de base sur l'axe y (21.01.2023)

        if (moveInput < 0f) // si il le Player se délplace vers la gauche (22.01.2023)
        {
            Vector3 localScale = transform.localScale; //(22.01.2023)
            localScale.x = -1f; //(22.01.2023)
            transform.localScale = localScale; //(22.01.2023) 
        }

        if (moveInput > 0f) // si il le Player se délplace vers la droite (22.01.2023)
        {
            Vector3 localScale = transform.localScale; //(22.01.2023)
            localScale.x = 1f; //(22.01.2023)
            transform.localScale = localScale; //(22.01.2023)
        }

        if (moveInput == 0) // si l'on se déplace (23.01.2023)
        {
            anim.SetBool("IsRunning", false); // lance une boucle d'animation de position imobile (23.01.2023)
        }
        else //sinon (23.01.2023)
        {
            anim.SetBool("IsRunning", true); // lance une boucle d'animation de cours (23.01.2023)
        }
    }

    private void Jump() // fonction pour sauter  (21.01.2023)
    {
        isGrounded = Physics2D.OverlapCapsule(CheckPos.position, new Vector2(0.58f, 0.2f), CapsuleDirection2D.Horizontal, 0, wahtIsGround); // définition de la zone pour détécter le sol (21.01.2023)
        if(isGrounded == true && Input.GetKeyDown(KeyCode.Space)) // si le player detect le sol et appuie sur espace (21.01.2023) 
        {
            isJumping = true; 
            JumpTimeCounter = JumpTime; // reset le chronomètre à notre variable
            rb.velocity = Vector2.up * JumpForce; // vecteur en 2d pour faire sauter le Player (21.01.2023)
            anim.SetTrigger("StartJump"); // animation pour faire sauter le Player (24.01.2023) 
        }

        if(Input.GetKey(KeyCode.Space) && isJumping == true) // si la touche espace est maintenue enfoncé (21.01.2023) 
        {
            if (JumpTimeCounter > 0) // si le tempse de saut et au dessu de 0 (21.01.2023) 
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce); // vecteur en 2d pour faire sauter le Player (21.01.2023)
                JumpTimeCounter -= Time.deltaTime; // enlève une valeur toute les frames à la variable JumpTimeCounter (21.01.2023) 
            }
            else 
            {
                isJumping = false; 
            }
        }

        if(Input.GetKeyUp(KeyCode.Space)) // si la touche espace est relevé (21.01.2023) 
        {
            isJumping = false; 
        }

        if(isGrounded == true) // si il trouche le sol  (24.01.2023) 
        {
            anim.SetBool("EndJump", true); // active la boucle qui arrete l'animation de saut (24.01.2023) 
        }
        else
        {
            anim.SetBool("EndJump", false); // continue de lancer l'animation de saut car il n'est pas au sol (24.01.2023) 
        }
    }

    private void Attack() // fonction pour attaquer les enemie  (25.01.2023)
    {
        if(timeAttack <= 0) // si le temps pour attaquer est passé (25.01.2023)
        {
            if (Input.GetKey("y")) // lorsque on appuie sur y (25.01.2023)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies); // va detecter les collision avec la Layer choisi donc Enemie (25.01.2023)
                for (int i = 0; i < enemiesToDamage.Length; i++) // une boucle qui nous permet d'infliger des dégats au enemie (25.01.2023)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage); // identifier le script Enemy pour enclencher sa fonction TakeDamage (25.01.2023)
                }
                anim.SetTrigger("Attack"); // animation d'attaque (25.01.2023)
                timeAttack = startTimeAttack; // reinstaller le temps réstant au temps définie de base (25.01.2023)
            }

        }
        else
        {
            timeAttack -= Time.deltaTime; // si le temps n'est toujour pas passer on réduit la variable jusqu'à qu'elle sois en dessous de 0 (25.01.2023)
        }
    }

    void OnDrawGizmosSelected() // permet de voir le cercle rouge pour notre fonction Attack (25.01.2023)
    {
        Gizmos.color = Color.red; // couleur du cercle = rouge
        Gizmos.DrawWireSphere(attackPos.position, attackRange); // le cercle qu'il faut dessiner qui est donc egal au proportion de celui de la fonction Attack (25.01.2023)
    }

}
