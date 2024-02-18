using System.Collections.Generic;
using UnityEngine;

public class WaveProfile
{
    const string BackgroundPath = "Stage/Backgrounds";

    public readonly int WaveFrom;
    public readonly int WaveUntil;
    public readonly int BigWaveNumOfCatches;
    public readonly List<int> MonsterIDs;
    public readonly string FieldImageName;

    public WaveProfile(int waveFrom, int waveUntil, int bigWaveNumOfCatches, List<int> monsterIDs, string fieldImageName)
    {
        WaveFrom = waveFrom;
        WaveUntil = waveUntil;
        BigWaveNumOfCatches = bigWaveNumOfCatches;
        MonsterIDs = new List<int>(monsterIDs);
        FieldImageName = fieldImageName;
    }
}