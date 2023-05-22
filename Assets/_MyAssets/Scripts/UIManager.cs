using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class UIManager : MonoBehaviour  {
    
    [SerializeField] private int _score =  default;
    [SerializeField] private TextMeshProUGUI _txtScore = default;
    [SerializeField] private TextMeshProUGUI _txtTempsPartie = default;
    [SerializeField] private TextMeshProUGUI _txtTemps = default;
    [SerializeField] private TextMeshProUGUI _txtGameOver = default;
    [SerializeField] private TextMeshProUGUI _txtRestart = default;
    [SerializeField] private TextMeshProUGUI _txtQuit = default;
    [SerializeField] private Image _livesDisplayImage = default;
    [SerializeField] private Sprite[] _liveSprites = default;
    [SerializeField] private GameObject _pausePanel = default;

    private bool _pauseOn = false;
    public bool _isSoundOn = true;
    // Start is called before the first frame update

    private void Start() {
        _score = 0;       
        Time.timeScale = 1;
        UpdateScore();
    }

    private void Update()
    {

        _txtTemps.text = "Temps : " + Time.timeSinceLevelLoad.ToString("f2");
        UpdateScore();

        ////Récupère l'index de la scène en cours
        //int noScene = SceneManager.GetActiveScene().buildIndex;
        //if (noScene == (SceneManager.sceneCountInBuildSettings - 2))
        //{
        //    _txtTempsPartie.text = "Temps : " + Time.timeSinceLevelLoad.ToString("f2");
        //}

        if (_pauseOn && Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
        }
        if (_pauseOn && Input.GetKeyDown(KeyCode.S))
        {
            ToggleSound();
        }            

        // Permet la gestion du panneau de pause (marche/arrêt)
        if ((Input.GetKeyDown(KeyCode.Escape) && !_pauseOn))
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0;
            _pauseOn = true;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && _pauseOn))
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1;
            _pauseOn = false;
        }

        //if (_txtRestart.gameObject.activeSelf && Input.GetKeyDown(KeyCode.R)) {
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
        //else if (_txtRestart.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
        //    SceneManager.LoadScene(0);
        //}
    }

    public int getScore()
    {
        return _score;
    }
    public void AjouterScore(int points) {
        _score += points;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _txtScore.text = "Score : " + _score.ToString();
    }

    private void GameOverSequence() {
        _txtGameOver.gameObject.SetActive(true);
        _txtRestart.gameObject.SetActive(true);
        _txtQuit.gameObject.SetActive(true);
        StartCoroutine(GameOverBlinkRoutine());
    }

    IEnumerator GameOverBlinkRoutine() {
        while (true) {
            _txtGameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            _txtGameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.7f);
        }
    }

    public void ResumeGame() {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
        _pauseOn = false;
    }

    public void ToggleSound()
    {
        _isSoundOn = !_isSoundOn;
        AudioListener.volume =_isSoundOn ? 1 : 0;
    }
}
