using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;

public class Enemy : MonoBehaviour
{
    public AIPath aiPath;

    [SerializeField] protected int _health;
    [SerializeField] protected BarreDeVie _barreDeVie;
    [SerializeField] protected int _points = 100;
    [SerializeField] protected int _damage = 1;

    private UIManager _uiManager;

    [SerializeField] protected Character4D _character;
    [SerializeField] protected AnimationManager _animation;

    [SerializeField] private Animator _animator;

    private bool isAttacking = false;
    private float attackCooldown = 1f; // Délai entre chaque attaque en secondes
    private float currentCooldown = 0f;

    private bool isDead = false;
    [SerializeField] private GameObject _sang;

    private void Update()
    {
        Vector2 direction = aiPath.desiredVelocity.normalized;

        if (direction.magnitude > 0.01f)
        {
            if (direction.x >= 0.01f)
            {
                _character.SetDirection(Vector2.right);
            }
            else if (direction.x <= -0.01f)
            {
                _character.SetDirection(Vector2.left);
            }
            else if (direction.y >= 0.01f)
            {
                _character.SetDirection(Vector2.up);
            }
            else if (direction.y <= -0.01f)
            {
                _character.SetDirection(Vector2.down);
            }

            _animation.SetState(CharacterState.Run);
        }
        else
        {
            _animation.SetState(CharacterState.Idle);
        }

        if (isAttacking)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                isAttacking = false;
                currentCooldown = 0f;
            }
        }

        if (isDead)
        {
            // Si l'ennemi est mort, arrêtez de mettre à jour le script
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            player.Damage(_damage);

            Vector2 enemyToPlayer = other.transform.position - transform.position;
            float distanceToPlayer = enemyToPlayer.magnitude;

            if (distanceToPlayer < 2f && !isAttacking)
            {
                _animation.Attack();
                isAttacking = true;
                currentCooldown = attackCooldown;
            }
        }
    }

    // Appelée depuis l'animation pour réinitialiser l'état de l'attaque
    public void ResetAttackState()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead)
        {
            // Si l'ennemi est déjà mort, ne faites rien
            return;
        }

        if (other.tag == "Slash")
        {
            if (_health > 0)
            {
                _health -= 10;
                _barreDeVie.SetHealth(_health);
                if (_health <= 0)
                {
                    _uiManager = FindObjectOfType<UIManager>();
                    if (_uiManager != null)
                    {
                        _uiManager.AjouterScore(_points);
                    }

                    StartCoroutine(DieSequence());
                }
            }
        }
        if (other.tag == "Thrust")
        {
            if (_health > 0)
            {
                _health -= 5;
                _barreDeVie.SetHealth(_health);
                if (_health <= 0)
                {
                    _uiManager = FindObjectOfType<UIManager>();
                    if (_uiManager != null)
                    {
                        _uiManager.AjouterScore(_points);
                    }

                    StartCoroutine(DieSequence());
                }
            }
        }
    }
    private IEnumerator DieSequence()
    {
        GameObject instantiateSang = Instantiate(_sang, transform.position, Quaternion.identity);
        isDead = true;
        
        GetComponent<AIPath>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        this.enabled = false;
        _animation.Die();

        yield return new WaitForSeconds(1.5f);

        Destroy(instantiateSang);
        Destroy(gameObject);
    }
}
