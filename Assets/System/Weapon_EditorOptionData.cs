using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Weapon_EditorOptionData : ScriptableObject//武器エディタオプションデータ
{
    [Header("武器の枠色")]
    public Color Select_Color=Color.black;//選択している
    public Color Null_Color=GUI.backgroundColor;//データ無
    public Color Small_caliber_gun_Color=Color.red;//小口径主砲
    public Color Medium_caliber_gun_Color = Color.red;//中口径主砲
    public Color Large_caliber_gun_Color = Color.red;//大口径主砲
    public Color Secondary_armament_Color = Color.yellow;//副砲
    public Color Torpedo_Color = Color.blue;//魚雷
    public Color Ship_mounted_boat_Color;//艦載艇
    public Color Carrier_based_fighter_Color = Color.green;//艦上戦闘機
    public Color Water_fighter_Color = Color.green + Color.yellow;//水上戦闘機
    public Color Shipboard_bomber_Color = Color.red + Color.red;//艦上爆撃機
    public Color Water_bomber_Color = Color.green;//水上爆撃機
    public Color Carrier_based_attack_aircraft_Color = Color.green + Color.blue;//艦上攻撃機
    public Color Ship_reconnaissance_aircraft_Color = Color.green;//艦上偵察機
    public Color Water_reconnaissance_aircraft_Color = Color.green;//水上偵察機
    public Color Anti_submarine_patrol_aircraft_Color;//対潜哨戒機
    public Color Electric_search_Color=Color.green + Color.red + Color.red;//電探
    public Color Organ_Color;//機関
    public Color Enhanced_bullet_Color;//強化弾
    public Color Anti_aircraft_machine_gun_Color;//対空機銃
    public Color Anti_aircraft_device_Color;//高射装置
    public Color Depth_charge_Color;//爆雷
    public Color Sonar_Color=Color.blue+Color.blue;//ソナー
    public Color Repai_personnel_Color;//修理要員
    public Color Expansion_basil_Color;//増設バジル
    public Color No_classification_Color = GUI.backgroundColor;//分類無
    public Color Land_attack_aircraft_Color = Color.green;//陸上攻撃機
    public Color Land_reconnaissance_aircraft_Color = Color.green;//陸上偵察機
    public Color Local_fighter_Color = Color.green;//局地戦闘機
    public Color Army_fighter_Color = Color.green;//陸軍戦闘機
}
