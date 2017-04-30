using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BnBClient : MonoBehaviour
{
    NetworkSocketInfo NetworkInfo; // Informations sur la place dans le réseau de ce client.

    // Propriétés du client
    public string Username; // Nom du client.

    // ---------

    // Propriétés de connexion
    bool Connected; // Le client est-il connecté au serveur ?
    bool InAMatch; // Le client est-il actuellement dans un match ?
    string IP;
    int Port = 25000;
    // ---------

    // Utilitaires de jeu CLIENT-SIDE
    public ClientUIManager UIManager;

        /// <summary>
        /// Lance une tentative de connexion au Master Server et lance la recherche d'un match.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
    public void StartMatchSearch()
    {
        IP = UIManager.GetTextInputValue("IPInputField");
        if (IP == "")
        {
            IP = "127.0.0.1";
        }
        Username = UIManager.GetTextInputValue("UsernameInputField");
        if (Username == "")
        {
            Debug.Log("Nom d'utilisateur non spécifié !");
            return;
        }
        byte error;
        NetworkTransport.Connect(NetworkInfo.HostID, IP, Port, 0, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.Log("Erreur lors de la connexion au Master Server ! Type d'erreur : " + (NetworkError)error);
        }
        else
        {
            Connected = true;
        }
    }

    /// <summary>
    /// Retour au menu principal et déconnection.
    /// </summary>
    private void Reset()
    {
        Connected = false;
        InAMatch = false;
        Username = "";
        UIManager.SwitchToUI("MainMenuUI");
        UIManager.BindButtonToFunction("StartMatchSearchButton", StartMatchSearch);
    }

    private void Start()
    {
        NetworkInfo = new NetworkSocketInfo(1);
        UIManager = GetComponent<ClientUIManager>();
        Reset();
    }

    private void Update()
    {

    }

}