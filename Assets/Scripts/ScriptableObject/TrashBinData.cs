using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrashBin", menuName = "ScriptableObjects/TrashBinData")]
public class TrashBinData : ScriptableObject
{
    public string binName;         // Name of the bin (e.g., Organic Bin)
    public string acceptedType;    // "Organic", "Inorganic", or "B3"
    public int pointsForCorrectTrash = 5; // Points for depositing correct trash
    public int penaltyForWrongTrash = -2; // Penalty for depositing incorrect trash
}