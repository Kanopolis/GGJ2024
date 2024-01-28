using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct EnemyTypeData
{
    public string EnemyTypeIdentifier;
    public EnemyTypeDataIntern Data;
}

[Serializable]
public struct EnemyTypeDataIntern
{
    public int SpawnNumber;
}

[Serializable]
public struct LocationData
{
    public string LocationIdentifier;
    public LocationDataIntern Data;
}

[Serializable]
public struct LocationDataIntern
{
    public int SpawnNumber;
}

[Serializable]
public struct MusicData
{
    public string MusicIdentifier;
    public MusicDataIntern Data;
}

[Serializable]
public struct MusicDataIntern
{
    public int SpawnNumber;
}

[Serializable]
public struct ShaderData
{
    public string ShaderIdentifier;
    public ShaderDataIntern Data;
}

[Serializable]
public struct ShaderDataIntern
{
    public int SpawnNumber;
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Color[] m_PlayerColors;
    [SerializeField]
    private EnemyTypeData[] m_EnemyTypeData;
    [SerializeField]
    private LocationData[] m_LocationData;
    [SerializeField]
    private MusicData[] m_MusicData;
    [SerializeField]
    private ShaderData[] m_ShaderData;

    public static Color[] PlayerColors;
    private static readonly Dictionary<string, EnemyTypeDataIntern> EnemyTypes = new();
    private static readonly Dictionary<string, LocationDataIntern> Locations = new();
    private static readonly Dictionary<string, MusicDataIntern> Musics = new();
    private static readonly Dictionary<string, ShaderDataIntern> Shaders = new();

    public static EnemyTypeDataIntern SelectedEnemyTypeData;
    public static LocationDataIntern SelectedLocationData;
    public static MusicDataIntern SelectedMusicData;
    public static ShaderDataIntern SelectedShaderData;

    private void Awake()
    {
        PlayerColors = m_PlayerColors;

        foreach (var item in m_EnemyTypeData)
        {
            EnemyTypes[item.EnemyTypeIdentifier] = item.Data;
        }
        foreach (var item in m_LocationData)
        {
            Locations[item.LocationIdentifier] = item.Data;
        }
        foreach (var item in m_MusicData)
        {
            Musics[item.MusicIdentifier] = item.Data;
        }
        foreach (var item in m_ShaderData)
        {
            Shaders[item.ShaderIdentifier] = item.Data;
        }
    }

    public static void SelectEnemy(string _enemyIdentifier)
    {
        if (EnemyTypes.ContainsKey(_enemyIdentifier))
            SelectedEnemyTypeData = EnemyTypes[_enemyIdentifier];
        else
            SelectedEnemyTypeData = EnemyTypes.Values.ToArray()[0];
    }

    public static void SelectLocation(string _locationIdentifier)
    {
        if (Locations.ContainsKey(_locationIdentifier))
            SelectedLocationData = Locations[_locationIdentifier];
        else
            SelectedLocationData = Locations.Values.ToArray()[0];
    }

    public static void SelectMusic(string _musicIdentifier)
    {
        if (Musics.ContainsKey(_musicIdentifier))
            SelectedMusicData = Musics[_musicIdentifier];
        else
            SelectedMusicData = Musics.Values.ToArray()[0];
    }

    public static void SelectShader(string _shaderIdentifier)
    {
        if (Shaders.ContainsKey(_shaderIdentifier))
            SelectedShaderData = Shaders[_shaderIdentifier];
        else
            SelectedShaderData = Shaders.Values.ToArray()[0];
    }
}
