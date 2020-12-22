using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MazeData", menuName = "ScriptableObjects/StartData", order = 1)]
public class MazeData : ScriptableObject
{
    [HideInInspector]
    public int startIndex;
    [HideInInspector]
    public int finishIndex;
    [HideInInspector]
    public List<int> busyIndices = new List<int>();
}