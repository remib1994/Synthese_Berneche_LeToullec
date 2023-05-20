using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _Vitesse = 6.0f;
    [SerializeField] private int _Point = 100;
    [SerializeField] private GameObject _ExplosionPrefab = default;
    private UIManager _uiManager;

    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MouvementsEnemy();
    }

    private void MouvementsEnemy()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _Vitesse);
        if (transform.position.y < -7f)
        {
            float randomX = Random.Range(-8.5f, 8.5f);
            transform.position = new Vector3(randomX, 7f, 0f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            _uiManager.AjouterScore(_Point);
            Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }else if (collision.tag == "Player" )
        {
            Player player = collision.GetComponent<Player>();
            player.Damage();
            Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
