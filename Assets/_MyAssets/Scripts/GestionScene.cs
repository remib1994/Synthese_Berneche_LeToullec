using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class GestionScene : MonoBehaviour
{    
    //M�thodes publiques

    public void ChangerSceneSuivante()
    {
        int noScene = SceneManager.GetActiveScene().buildIndex; // R�cup�re l'index de la sc�ne en cours
        SceneManager.LoadScene(noScene + 1);
    }

    //M�thode pour quitter le jeu
    public void Quitter()
    {
        Application.Quit();
    }

    //M�thode pour retour au menu principal (Sc�ne Start)
    public void ChargerSceneDepart()
    {
        SceneManager.LoadScene(0);
    }

}
