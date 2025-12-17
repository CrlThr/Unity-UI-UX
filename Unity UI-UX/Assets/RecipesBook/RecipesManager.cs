using System;
using UnityEngine;

/// <summary>
/// Conteneur de données pour une recette.
/// </summary>
[Serializable]
public class Recipe
{
    // --- Champs de données ---
    [Header("Détails de la Recette")]
    [SerializeField] private string recipeName = "Nouvelle Recette";

    [SerializeField][TextArea] private string ingredients = "Liste des ingrédients ici...";
    [SerializeField][TextArea] private string instructions = "Étapes de préparation ici...";

    // --- Propriétés publiques (Lecture seule) ---
    // Elles permettent au BookManager et à la Page d'accéder aux données
    public string Name => recipeName;
    public string Ingredients => ingredients;
    public string Instructions => instructions;
}
