using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;

public class GestionSon : MonoBehaviour
{
    //Attributs
    private AudioSource _audioSource;
    public AudioMixer _audioMixer;

    private bool _isPlaying;

    private void Start()
    {        
        _audioSource = FindObjectOfType<MusiqueFond>().GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("Muted") == 0)
        {
            _audioSource.Stop();
            _isPlaying= false;
        }        
        else
        {
            _audioSource.Play();
            _isPlaying = true;
            PlayerPrefs.SetInt("Muted", 1);
            PlayerPrefs.Save();
        }
    }   

    private void Awake()
    {
        _audioSource = FindObjectOfType<MusiqueFond>().GetComponent<AudioSource>();

        //Récupère l'index de la scène en cours
        int noScene = SceneManager.GetActiveScene().buildIndex;
        if (noScene == (SceneManager.sceneCountInBuildSettings - 1))
        {
            _audioSource.Play();
            _isPlaying = true;
            PlayerPrefs.SetInt("Muted", 1);
            PlayerPrefs.Save();
        }
    }


    //Méthodes publiques

    //Méthode pour mettre le son en sourdine ou non 
    public void MusiqueOnOff()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0 && _isPlaying == false)
        {
            _audioSource.Play();
            _isPlaying= true;
            PlayerPrefs.SetInt("Muted", 1);
            PlayerPrefs.Save();            
        }
        else if(PlayerPrefs.GetInt("Muted", 1) == 1 && _isPlaying == true)
        {
            _audioSource.Pause();
            _isPlaying = false;
            PlayerPrefs.SetInt("Muted", 0);
            PlayerPrefs.Save();
        }
        else
        {
            _audioSource.Pause();
            _isPlaying = false;
            PlayerPrefs.SetInt("Muted", 0);
            PlayerPrefs.Save();
        }
    }

    //Méthode pour gérer le volume avec un slider
    public void GererVolume (float volume)
    {
        _audioMixer.SetFloat("volume", volume);        
    }
}

