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

    //M�thodes publiques

    public void ChangerSceneSuivante()
    {
        int noScene = SceneManager.GetActiveScene().buildIndex; // R�cup�re l'index de la sc�ne en cours
        SceneManager.LoadScene(noScene + 1);
    }

    //M�thode pour afficher le panel des instructions
    public void Instruction()
    {        
        if (!_menuOuvert)
        {
            _menuInstruction.SetActive(true);
            _btOption.SetActive(false);
            _menuOuvert = true;
        }
    }

    //M�thode pour afficher le menu des options
    public void Option()
    {
        if (!_menuOptionOuvert)
        {
            _menuOption.SetActive(true);
            _menuOuvert = true;
        }
    }

    //M�thode pour quitter le jeu
    public void Quitter()
    {
        Application.Quit();
    }

    //M�thode pour recommencer une partie (sc�ne de jeu)    
    public void Recommencer()
    {
        //Il faut s'assurer que le temps et le pointage sont remis � zero
        SceneManager.LoadScene(1);        
    }

    //M�thode pour retour au menu principal (Sc�ne Start)
    public void ChargerSceneDepart()
    {
        SceneManager.LoadScene(0);
        _btOption.SetActive(true);
    }

}
