using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="CaraSet")]
public class CharacterData : ScriptableObject
{
    [Header("番号")]
    public int No;
    [Header("図鑑番号")]
    public int BookNo;

    [Header("名前")]
    public string CharacterName;

    [Header("艦種")]
    public ShipType Type;

    [Header("耐久")]
    public int Durable;

    [Header("初期火力")]
    public int Def_FirepowerP;
    [Header("最大火力")]
    public int Max_FirepowerP;

    [Header("初期装甲")]
    public int Def_ArmorP;
    [Header("最大装甲")]
    public int Max_ArmorP;

    [Header("初期雷装")]
    public int Def_ThunderstormP;
    [Header("最大雷装")]
    public int Max_ThunderstormP;

    [Header("初期回避")]
    public int Def_Evasion_rate;
    [Header("最大回避")]
    public int Max_Evasion_rate;

    [Header("初期対空")]
    public int Def_Anti_aircraftP;
    [Header("最大対空")]
    public int Max_Anti_aircraftP;

    [Header("初期対潜")]
    public int Def_Anti_submarine;
    [Header("最大対潜")]
    public int Max_Anti_submarine;

    [Header("速力")]
    public Ship_Speed Speed;

    [Header("初期索敵")]
    public int Def_SearchenemyP;
    [Header("最大索敵")]
    public int Max_SearchenemyP;

    [Header("射程")]
    public Ship_Range Range;

    [Header("初期運")]
    public int Def_luck;
    [Header("最大運")]
    public int Max_luck;

    [Header("最大消費燃料")]
    public int Max_Fuelconsumption;

    [Header("最大消費弾薬")]
    public int Max_Consumedammunition;

    [Header("初期装備")]
    public WeaponData[] Initial_equipment = new WeaponData[5];

    [Header("艦載機搭載数")]
    public int[] Numberofmounted = new int[5];

    [Header("例外装備可能装備")]
    public WeaponData[]Exceptionalequipmentavailableequipment = new WeaponData[0];

    [Header("改造レベル")]
    public int Remodeling_Level;

    [Header("改造必要鋼材")]
    public int Steel;
    [Header("改造必要弾薬")]
    public int Ammunition;
    [Header("改造必要開発資材")]
    public int Development_Materials;
    [Header("改造必要高速建造材")]
    public int HighSpeed_Construction_material;

    [Header("改造必要改装設計図")]
    public int Refurbished_Blueprint;
    [Header("改造必要戦闘詳報")]
    public int Detailedbattle_Report;
    [Header("改造必要試製甲板カタパルト")]
    public int Trialdeck_Catapult;
    [Header("改造必要新型航空兵装資材")]
    public int Newaviationarmament_Materials;
    [Header("改造必要新型砲熕兵装資材")]
    public int Newartilleryweapon_Materials;

    [Header("改造先")]
    public CharacterData Remodeling_Destination;

    [Header("画像")]
    public Sprite Imag;
}

public enum ShipType//艦種
{
    NoData,
    Destroyer,//駆逐艦
    Alightcruiser,//軽巡洋艦
    Heavycruiser,//重巡洋艦
    Battleship,//戦艦
    Lightaircraftcarrier,//軽空母
    aircraftcarrier,//空母
    Aircraftcarrieronthewater,//水上空母
    Aircraftbattleship,//航空戦艦
    Submarine//潜水艦
}

public enum Ship_Range//射程
{
    Nodata,
    ShortRange,//射程短
    InRange,//射程中
    LongRang,//射程長
    LongestRang//射程最長
}

public enum Ship_Speed//速力
{
    Nodata,
    SlowSpeed,//低速
    Midspeed,//中速
    HighSpeed//高速
}

