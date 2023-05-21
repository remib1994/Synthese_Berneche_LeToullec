using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;

public class GestionSon : MonoBehaviour
{
    //Attributs
    private AudioSource _audioSource;
    public AudioMixer _audioMixer;

    private void Start()
    {
        _audioSource = FindObjectOfType<MusiqueFond>().GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("Muted") == 0)
        {
            _audioSource.Stop();
        }
    }

    //Méthode publique

    //Méthode pour mettre le son en sourdine ou non 
    public void MusiqueOnOff()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            _audioSource.Play();
            PlayerPrefs.SetInt("Muted", 1);
            PlayerPrefs.Save();
        }
        else
        {
            _audioSource.Pause();
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

