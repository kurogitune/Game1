using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 [CreateAssetMenu(menuName = "WeapondataSet")]
public class WeaponData : ScriptableObject//武器データ用　変更する場合こいつを変更する
{
    [Header("武器番号")]
    public int No;
    [Header("武器の名前")]
    public string WeaponName;
    [Header("レア度")]
    public int Rarity;
    [Header("図鑑番号")]
    public int BookNo;
    [Header("火力")]
    public int FirepowerP;
    [Header("雷装")]
    public int ThunderstormP;
    [Header("爆装")]
    public int BombP;
    [Header("対空")]
    public int Anti_aircraftP;
    [Header("対潜")]
    public int Anti_submarine;
    [Header("索敵")]
    public int SearchenemyP;
    [Header("命中")]
    public int Hit_rate;
    [Header("回避")]
    public int Evasion_rate;
    [Header("装甲")]
    public int ArmorP;

    [Header("射程")]
    public Weapon_Rang Range;//0:無  1:短  2:中  3:長  4:超長
    [Header("半径")]
    public int Radius;


    [Header("改修したレベル")]
    public int Refurbishment_Count;//最大10
    [Header("装備開発できるか")]
    public bool development;

    [Header("装備可能艦種")]
    public bool[] Equipable_ship_type = new bool[9];
    //0:駆逐  1:軽巡洋艦　2:重巡洋艦  3:戦艦  4:軽空母  5:空母  6:水上空母  7:航空戦艦  8:潜水艦
    [Header("タイプ")]
    public WeaponType Type;


    [Header("表示画像")]
    public Sprite Imag;
}

public enum WeaponType//武器タイプ
{
    Null,
    Small_caliber_gun,//小口径主砲
    Medium_caliber_gun,//中口径主砲
    Large_caliber_gun,//大口径主砲
    Secondary_armament,//副砲
    Torpedo,//魚雷
    Ship_mounted_boat,//艦載艇
    Carrier_based_fighter,//艦上戦闘機
    Water_fighter,//水上戦闘機
    Shipboard_bomber,//艦上爆撃機
    Water_bomber,//水上爆撃機
    Carrier_based_attack_aircraft,//艦上攻撃機
    Ship_reconnaissance_aircraft,//艦上偵察機
    Water_reconnaissance_aircraft,//水上偵察機
    Anti_submarine_patrol_aircraft,//対潜哨戒機
    Electric_search,//電探
    Organ,//機関
    Enhanced_bullet,//強化弾
    Anti_aircraft_machine_gun,//対空機銃
    Anti_aircraft_device,//高射装置
    Depth_charge,//爆雷
    Sonar,//ソナー
    Repai_personnel,//修理要員
    Expansion_basil,//増設バジル
    No_classification,//分類無
    Land_attack_aircraft,//陸上攻撃機
    Land_reconnaissance_aircraft,//陸上偵察機
    Local_fighter,//局地戦闘機
    Army_fighter//陸軍戦闘機
}

public enum Weapon_Rang//武器射程
{
    Nodata,
    ShortRange,//射程短
    InRange,//射程中
    LongRang,//射程長
    LongestRang//射程最長
}


