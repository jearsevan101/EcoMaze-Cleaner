using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrash", menuName = "ScriptableObjects/TrashData")]
public class TrashData : ScriptableObject
{
    public string trashName; // Name of the trash
    public string trashType; // "Organic", "Inorganic", or "B3"
    public float weight;     // Weight of the trash (affects player speed)
}