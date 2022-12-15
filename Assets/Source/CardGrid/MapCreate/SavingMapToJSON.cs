using System;
using System.Collections;
using System.Collections.Generic;
using CardGrid;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
public class SavingMapToJSON : MonoBehaviour
{
    public CommonGameSettings GameSetings;
    public Transform FogsParent;
    public Transform CardsParent;
    public BlockDataObj[] BlocksData;

    private const int qiantityCells = 64;
    private List<SpriteRenderer> Fogs = new List<SpriteRenderer>(qiantityCells);
    private List<SpriteRenderer> Cards = new List<SpriteRenderer>(qiantityCells);
    
    public string MapName;
    public TextAsset MapLoad;
    private MapData MapData = new MapData();
    
    [Button]
    private void UpdateArrays()
    {
        Fogs.Clear();
        if(FogsParent)
            foreach (Transform fog in FogsParent)
            {
                Fogs.Add(fog.GetComponent<SpriteRenderer>());
            }

        Cards.Clear();
        foreach (Transform card in CardsParent)
        {
            Cards.Add(card.GetComponent<SpriteRenderer>());
        }

        BlocksData = new BlockDataObj[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            BlocksData[i] = transform.GetChild(i).GetComponent<BlockDataObj>();
            BlocksData[i].Validate += UpdateMap;
        }
    }

    [Button]
    void UpdateMap()
    {
        MapData.Cells = new int[qiantityCells];
        MapData.Cards = new CT[qiantityCells];
        for (int i = 0; i < BlocksData.Length; i++)
        {
            var blockData = BlocksData[i];

            if(FogsParent)
                MapData.Cells[i] = blockData.FogQuantity;
            MapData.Cards[i] = blockData.CardType;
            
            if(FogsParent) 
                Fogs[i].gameObject.SetActive(blockData.FogQuantity > 0);
            if (TryGetCardSO(blockData.CardType, out var card))
            {
                Cards[i].sprite = card.Sprite;
            }
            else
            {
                Cards[i].sprite = null;
            }
        }
    }

    void Load()
    {
        for (int i = 0; i < BlocksData.Length; i++)
        {
            var blockData = BlocksData[i];

            if(FogsParent)
                blockData.FogQuantity = MapData.Cells[i];
            blockData.CardType = MapData.Cards[i];
            
            if(FogsParent)
                Fogs[i].gameObject.SetActive(blockData.FogQuantity > 0);
            if (TryGetCardSO(blockData.CardType, out var card))
            {
                Cards[i].sprite = card.Sprite;
            }
            else
            {
                Cards[i].sprite = null;
            }
        }
    }

    [Button]
    public void SaveMap()
    {
        var map = JsonUtility.ToJson(MapData);
        System.IO.File.WriteAllText(Application.dataPath+"/Resources/Maps/"+ MapName +".json" , map);
    }

    [Button]
    public void LoadMap()
    {
        MapData = JsonUtility.FromJson<MapData>(MapLoad.text);
        Load();
    }
    
    public bool TryGetCardSO(CT type, out CardSO card)
    {
        card = null;
        if (type == CT.Empty)
            return false;
            
        foreach (var enemy in  GameSetings.Enemies)
        {
            if (enemy.CardType == type)
            {
                card = enemy;
                return true;
            }
        }

        foreach (var item in GameSetings.Items)
        {
            if (item.CardType == type)
            {
                card = item;
                return true;
            }
        }

        return false;
    }
}

public class MapData
{
    public int[] Cells;
    public CT[] Cards;
}
