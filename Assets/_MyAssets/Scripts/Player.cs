using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine.Serialization;
using Pathfinding;

public class Player : MonoBehaviour
{
    public int Strength => _strength;
    // Start is called before the first frame update

    //Stats
    [SerializeField] protected Character4D _character;
    [SerializeField] protected AnimationManager _animation;
    [SerializeField] protected int _strength = 6;    
    [SerializeField] protected int _Dexterity = 6;
    [SerializeField] protected int _Intelligence = 6;
    
    
    [SerializeField] protected float _Speed = 10;
    [SerializeField] protected int _health;
    
    [SerializeField] protected BarreDeVie _barreDeVie;
    [SerializeField] protected float _AttackRate = 1.0f;

    [SerializeField] protected GameObject _Attack1Prefab = default;
    [SerializeField] protected GameObject _Attack2Prefab = default;
    [SerializeField] protected GameObject _Attack3Prefab = default;
    [SerializeField] protected AudioClip[] _Attack1Sound = default;
    [SerializeField] protected AudioClip[] _Attack2Sound = default;
    [SerializeField] protected AudioClip[] _Attack3Sound = default;
    [SerializeField] protected AudioClip[] _Attack4Sound = default;
    [SerializeField] protected AudioClip[] _DamageSound = default;

    [SerializeField] protected float _volume = default;
    
    private bool _isInvincible = false;

    private EnemySpawner _enemySpawner;
    private float _canAttack1 = -1f;
    private float _canAttack2 = -1f;
    private float _canAttack3 = -1f;
    private float _canAttack4 = -1f;

    private float _cdAttack3 = 15f;
    private float _cdAttack4 = 20f;
    private float _initialAttackRate;
    private int _healthMax;
    private float _speedMax;

    private GameObject _shield;
    private ParticleSystem _buffParticuleFX;
    private Animator _shieldAnimator;

    [SerializeField] private CanvasGroup flashMort;

    void Start()
    {
        _healthMax = _strength * 10;
        _health= _strength * 10;
        _character.SetDirection(Vector2.down);
        _animation.SetState(CharacterState.Idle);
        _enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        _initialAttackRate = _AttackRate;
        _shield = transform.GetChild(0).gameObject;
        _buffParticuleFX = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        _shieldAnimator = _shield.GetComponent<Animator>();
        _buffParticuleFX.Stop();
        _shield.SetActive(false);


        _barreDeVie.SetMaxHealth(_healthMax);
    }

    // Update is called once per frame
    void Update()
    {
        MouvementsJoueur();
        Attack1();
        Attack2();
        Attack3();
        Attack4();
    }
  

    protected void Attack1()
    {
        if(Input.GetButtonDown("Attack1") && Time.time > _canAttack1)
        {
            int randomSound = Random.Range(0, _Attack1Sound.Length);
            _animation.Attack();
            _canAttack1 = Time.time + _AttackRate;
            if(_character.Front.isActiveAndEnabled)
            {
                 Instantiate(_Attack1Prefab, transform.position+new Vector3(0,-.5f,0), Quaternion.Euler(0,0,0)); 
            }
            else if(_character.Back.isActiveAndEnabled)
            {
                Instantiate(_Attack1Prefab, transform.position+new Vector3(0,1f,0), Quaternion.Euler(0,0,180));
            }
            else if(_character.Left.isActiveAndEnabled)
            {
                Instantiate(_Attack1Prefab, transform.position+new Vector3(-1,0,0), Quaternion.Euler(0,0,270));
            }
            else if(_character.Right.isActiveAndEnabled)
            {
                Instantiate(_Attack1Prefab, transform.position+new Vector3(1f,0,0), Quaternion.Euler(0,0,90));
            }

            AudioSource.PlayClipAtPoint(_Attack1Sound[randomSound], transform.position, _volume);
        }
    }

    protected void Attack2()
    {
        if(Input.GetButtonDown("Attack2") && Time.time > _canAttack2)
        {
            int randomSound = Random.Range(0, _Attack2Sound.Length);
            _animation.Jab();
            _canAttack2 = Time.time + _AttackRate;
            if(_character.Front.isActiveAndEnabled)
            {
                Instantiate(_Attack2Prefab, transform.position+new Vector3(0f,-2f,0), Quaternion.Euler(0,0,180+45)); 
            }
            else if(_character.Back.isActiveAndEnabled)
            {
                Instantiate(_Attack2Prefab, transform.position+new Vector3(0f,2.5f,0), Quaternion.Euler(0,0,45));
            }
            else if(_character.Left.isActiveAndEnabled)
            {
                Instantiate(_Attack2Prefab, transform.position+new Vector3(-3,0,0), Quaternion.Euler(0,0,90+45));
            }
            else if(_character.Right.isActiveAndEnabled)
            {
                Instantiate(_Attack2Prefab, transform.position+new Vector3(3f,.5f,0), Quaternion.Euler(0,0,270+45));
            }
            
            AudioSource.PlayClipAtPoint(_Attack2Sound[randomSound], transform.position, _volume);
        }
    }
    protected void Attack3()
    {
        if(Input.GetButtonDown("Attack3") && Time.time > _canAttack3)
        {
            _buffParticuleFX.Play();
            //int randomSound = Random.Range(0, _Attack2Sound.Length);
            _canAttack3 = Time.time + _cdAttack3;
            _AttackRate = _initialAttackRate / 2;
            AudioSource.PlayClipAtPoint(_Attack3Sound[0], transform.position, _volume);
            StartCoroutine(BuffRoutine());
        }
    }
    IEnumerator BuffRoutine()
    {
        yield return new WaitForSeconds(5f);
        _buffParticuleFX.Stop();
        _AttackRate = _initialAttackRate;
    }
    protected void Attack4()
    {
        if(Input.GetButtonDown("Attack4") && Time.time > _canAttack4)
        {
            //int randomSound = Random.Range(0, _Attack2Sound.Length);
            _shieldAnimator.SetBool("ShieldActif", true);
            _shield.SetActive(true);
            _canAttack4 = Time.time + _cdAttack4;
            AudioSource.PlayClipAtPoint(_Attack4Sound[0], transform.position, _volume);
            StartCoroutine(ShieldRoutine());
            
        }
    }

    IEnumerator ShieldRoutine()
    {
        yield return new WaitForSeconds(5f);
        _shieldAnimator.SetBool("ShieldActif", false);
    }

    protected void MouvementsJoueur()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float VertInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizInput, VertInput, 0f);

        //Gestion des animations
        if (horizInput > 0)
        {
            _character.SetDirection(Vector2.right);
        }
        else if (horizInput < 0)
        {
            _character.SetDirection(Vector2.left);
        }
        else if (VertInput > 0)
        {
            _character.SetDirection(Vector2.up);
        }
        else if (VertInput < 0)
        {
            _character.SetDirection(Vector2.down);
        }
        
        if (horizInput == 0 && VertInput == 0)
        {
            _animation.SetState(CharacterState.Idle);
        }
        else
        {
            _animation.SetState(CharacterState.Run); 
        }

        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = direction * _Speed;
    }

    //Methode public
    public void Damage(int damage)
    {
        if (_isInvincible) // Si le joueur est invincible, ne subissez pas de d�g�ts
        {
            return;
        }

        if (_shield.activeSelf == true)
        {
            _shieldAnimator.SetBool("ShieldActif", false);
            _shield.SetActive(false);
        }
        else
        {
            int randomSound = Random.Range(0, _DamageSound.Length-1);
            AudioSource.PlayClipAtPoint(_DamageSound[randomSound], transform.position, _volume);
            _animation.Hit();
            _health -= damage;
            _barreDeVie.SetHealth(_health);
        }

        if (_health < 1)
        {
            StartCoroutine(FlashRoutine(0.5f));

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            this.enabled = false;
            _animation.Die();

            _enemySpawner.OnPlayerDeath();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(0.5f); // D�lai d'invincibilit� d'une seconde
        _isInvincible = false;
    }

    private IEnumerator FlashRoutine(float duration)
    {
        float timer = 0f;
        float startAlpha = flashMort.alpha;
        float targetAlpha = 1f;

        // Animation de l'augmentation de la transparence (de 0 à 1)
        while (timer < duration / 2f)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / (duration / 2f));
            flashMort.alpha = alpha;
            yield return null;
        }

        // Attendre pendant la seconde moitié de la durée spécifiée
        yield return new WaitForSeconds(duration / 2f);

        timer = 0f;
        startAlpha = flashMort.alpha;
        targetAlpha = 0f;

        // Animation de la diminution de la transparence (de 1 à 0)
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            flashMort.alpha = alpha;
            yield return null;
        }
    }

}
