using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "ScriptableObjects/LeveDataSO")]
public class LevelDataSO : ScriptableObject
{
    [Range(5, 12)] public int blockCount = 5;
    [Range(4, 6)] public int gridSize = 4;
    public Color[] colors;
}
