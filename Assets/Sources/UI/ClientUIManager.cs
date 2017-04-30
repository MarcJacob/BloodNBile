using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClientUIManager : MonoBehaviour
{
    GameObject CurrentUICanvas; // GameObject du Canvas actuellement utilisé.

    public void SwitchToUI(string UIName)
    {
        if (CurrentUICanvas != null)
        {
            GameObject.Destroy(CurrentUICanvas);
        }
        GameObject CanvasSearcher = Resources.Load("UI/"+UIName) as GameObject;

        if (CanvasSearcher == null)
        {
            Debug.Log("L'interface de nom '" + UIName + "' n'existe pas !");
        }
        else
        {
            CurrentUICanvas = Instantiate(CanvasSearcher) as GameObject;
        }
    }

    /// <summary>
    /// Lie la pression d'un bouton de l'interface à une procédure.
    /// Le lien est supprimé lors du changement d'interface.
    /// </summary>
    /// <param name="buttonName"> Nom du bouton concerné </param>
    /// <param name="function"> Nom de la fonction concernée </param>
    public void BindButtonToFunction(string buttonName, UnityEngine.Events.UnityAction function)
    {
        Transform ButtonGO = CurrentUICanvas.transform.Find(buttonName);
        if (ButtonGO != null)
        {
            Button ButtonComponent = ButtonGO.gameObject.GetComponent<Button>();
            if (ButtonComponent != null)
            {
                ButtonComponent.onClick.AddListener(function);
            }
        }
    }
    
    public string GetTextInputValue(string textInputFieldName)
    {
        Transform field = CurrentUICanvas.transform.Find(textInputFieldName);
        if (field == null)
        {
            Debug.Log("Il n'existe pas de champs de texte nommé : '" + textInputFieldName + "'");
            return "";
        }

        InputField inputFieldComponent = field.gameObject.GetComponent<InputField>();
        if (inputFieldComponent == null)
        {
            Debug.Log("Le champs de texte spécifié ne possède pas le component Input Field.");
            return "";
        }

        return inputFieldComponent.text;
    }
}