using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Un Model est une association entre un maillage (Mesh) et un ensemble d'animations qui peuvent être appliquées au modèle.
/// </summary>
public class Model  {

    private int ID; // Identifiant de ce modèle.
    public Mesh ModelMesh { get; private set; } // Mesh associé à ce modèle.
    public Material ModelMaterial { get; private set; } // Material associé à ce modèle.
    /// <summary>
    /// Animations associées à ce modèle.
    /// Les animations doivent être classées dans un ordre bien précis pour qu'elles soient cohérentes avec l'action qu'effectue l'entité associée.
    /// 0 = Idle
    /// 1 = Marche en avant
    /// 2 = Marche en arrière
    /// 3 = Marche à droite
    /// 4 = Marche à gauche
    /// 5 = Prendre des dégats
    /// 6 = Mort
    /// Au delà, l'ordre dépend de l'entité.
    /// </summary>
    public AnimationClip[] Animations { get; private set; } // 

    static Model[] Models; // Tous les Model du jeu.
    public static Model[] GetModels()
    {
        return Models;
    }

    public static void LoadModels() // TODO : Charger les Models depuis le disque dur. Pour l'instant ils sont crées au lancement et conservés en mémoire.
    {
        Models = new Model[]
        {
            // Tous les objets "Models" sont crées ici.
            CreateModel(0, "Model_Test","Tex_Test", new string[0]),
        };
    }

    /// <summary>
    /// Création d'un Model simplifiée par l'utilisation de chaines de caractères servants à retrouver les données dans les Assets
    /// </summary>
    /// <param name="modelMesh"> Nom du modèle dans les Assets </param>
    /// <param name="animations"> Nom des animations dans les Assets </param>
    private static Model CreateModel(int ID, string modelMesh, string material, string[] animations)
    {
        GameObject go = GameObject.Instantiate(Resources.Load("Models/" + modelMesh + "/" + modelMesh) as GameObject);
        Mesh mesh = go.GetComponent<MeshFilter>().mesh;
        GameObject.Destroy(go);
        List<AnimationClip> modelAnimations = new List<AnimationClip>();
        foreach(string name in animations)
        {
            modelAnimations.Add(Resources.Load("Animations/" + name) as AnimationClip);
        }
        Material mat = Resources.Load("Models/" + modelMesh + "/Materials/" + material) as Material;

        return new Model(ID, mesh, mat, modelAnimations.ToArray());
    }

    private Model(int id, Mesh modelMesh, Material mat, AnimationClip[] animations)
    {
        this.ID = id;
        this.ModelMesh = modelMesh;
        this.Animations = animations;
        this.ModelMaterial = mat;
    }

}
