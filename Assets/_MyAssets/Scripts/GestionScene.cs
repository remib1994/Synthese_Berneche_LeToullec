using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class GestionScene : MonoBehaviour
{    
    //Méthodes publiques

    public void ChangerSceneSuivante()
    {
        int noScene = SceneManager.GetActiveScene().buildIndex; // Récupère l'index de la scène en cours
        SceneManager.LoadScene(noScene + 1);
    }

    //Méthode pour quitter le jeu
    public void Quitter()
    {
        Application.Quit();
    }

    //Méthode pour retour au menu principal (Scène Start)
    public void ChargerSceneDepart()
    {
        SceneManager.LoadScene(0);
    }

}
