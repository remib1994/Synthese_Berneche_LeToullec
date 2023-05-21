using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class GestionScene : MonoBehaviour
{
    //Attributs
    [SerializeField] private GameObject _menuInstruction = default;
    [SerializeField] private GameObject _menuOption = default;

    [SerializeField] private GameObject _btOption = default;

    private bool _menuOuvert = false;
    private bool _menuOptionOuvert = false;

    //Méthodes publiques

    public void ChangerSceneSuivante()
    {
        int noScene = SceneManager.GetActiveScene().buildIndex; // Récupère l'index de la scène en cours
        SceneManager.LoadScene(noScene + 1);
    }

    //Méthode pour afficher le panel des instructions
    public void Instruction()
    {        
        if (!_menuOuvert)
        {
            _menuInstruction.SetActive(true);
            _btOption.SetActive(false);
            _menuOuvert = true;
        }
    }

    //Méthode pour afficher le menu des options
    public void Option()
    {
        if (!_menuOptionOuvert)
        {
            _menuOption.SetActive(true);
            _menuOuvert = true;
        }
    }

    //Méthode pour quitter le jeu
    public void Quitter()
    {
        Application.Quit();
    }

    //Méthode pour recommencer une partie (scène de jeu)    
    public void Recommencer()
    {
        //Il faut s'assurer que le temps et le pointage sont remis à zero
        SceneManager.LoadScene(1);        
    }

    //Méthode pour retour au menu principal (Scène Start)
    public void ChargerSceneDepart()
    {
        SceneManager.LoadScene(0);
        _btOption.SetActive(true);
    }

}
