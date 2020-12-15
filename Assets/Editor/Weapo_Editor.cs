using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Object = UnityEngine.Object;//Objectの定義を指定

public class Weapo_Editor : EditorWindow
{

    static string AssetFileName = "/AssetObj/WeapoAsset";
    [MenuItem("EditorEx/WeapoEditor %#Z")]
    static void Open()
    {
        if (!Directory.Exists(Application.dataPath + "/AssetObj"))//フォルダー確認
        {
            Debug.Log("AssetObjフォルダー作成");
            Directory.CreateDirectory(Application.dataPath + "/AssetObj");//ない場合作成
            AssetDatabase.Refresh();//Unityのファイル表示を更新  
        }

        if (!Directory.Exists(Application.dataPath + AssetFileName))
        {
            Directory.CreateDirectory(Application.dataPath + AssetFileName);
            Debug.Log("WeapoAssetフォルダー作成");
            AssetDatabase.Refresh();//Unityのファイル表示を更新  
        }

        GetWindow<Weapo_Editor>("WeapoEditor"); // タイトル名を指定
    }


    //こいつらは処理用
    string WeapoName="";//武器名前
    int  ItemNo, SelctitemNo=-1;//武器番号　　選択アイテム番号　セレクト番号
    int WeapoNo, Rarity, FirepowerP, ThunderstormP, BombP, Anti_aircraftP, Anti_submarine, SearchenemyP, Hit_rate, Evasion_rate, ArmorP, Radius;
    //火力　雷装　爆装　対空　対潜　索敵　命中　回避　装甲　　半径
    //射程  1:短  2:中  3:長  4:超長    半径　航空隊が対応可能範囲 
    bool[] Equipable_ship_type = new bool[9];//装備可能艦種
    //0:駆逐  1:軽巡洋艦　2:重巡洋艦  3:戦艦  4:軽空母  5:空母  6:水上空母  7:航空戦艦  8:潜水艦
    int BookNo;//図鑑番号
    bool development;//開発可能か
    WeaponType type;
    Weapon_Rang Range;//射程
    Object ImagData;
    Sprite Imag;

    //左表示関連
    WeaponData[] Weapondata=new WeaponData[0];//データ
    string[] DataName = new string[0];//ファイル名
    Vector2 LehtBox = Vector2.zero;
    Vector2 RigitBox = Vector2.zero;
    private void OnGUI()
    {
        //以下武器データ取得処理
        DirectoryInfo dir = new DirectoryInfo("Assets" + AssetFileName);//ファイルパス
        FileInfo[] fild = dir.GetFiles("*.asset");//こいつでファイル名を取得
        if (Weapondata.Length != fild.Length)
        {
           Weapondata = new WeaponData[fild.Length];
           DataName = new string[fild.Length];
           for(int i=0;i<fild.Length ; i++)
           {
                string path = dir + "/" +fild[i].Name;
                string WeaponName = fild[i].Name.Substring(0, fild[i].Name.Length - 6);

                Weapondata[i] = (WeaponData) AssetDatabase.LoadAssetAtPath(path,typeof(WeaponData));
           }

            WeaponData[] data = new WeaponData[Weapondata.Length];

            for (int i = 0; i < Weapondata.Length; i++)//No順に入れかえ
            {
                data[Weapondata[i].No-1] = Weapondata[i];
                DataName[Weapondata[i].No - 1] = data[Weapondata[i].No - 1].name;
            }

            Weapondata = data;           
        }
        //ここまで

        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
        {
            //以下データ選択表示処理
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                if (Weapondata.Length != 0)
                {
                    EditorGUILayout.LabelField("武器選択");
                    EditorGUILayout.LabelField("現在の武器数 : " + Weapondata.Length);

                    LehtBox = EditorGUILayout.BeginScrollView(LehtBox, GUI.skin.box);//選択枠
                    {
                        for (int i = 0; i < Weapondata.Length; i++)
                        {
                            switch (Weapondata[i].Type)//選択ボタンの色処理 主砲:赤　副砲：黄色　魚雷：青　戦闘機系：緑
                            {
                                case WeaponType.Small_caliber_gun://小口径主砲
                                    GUI.backgroundColor = Color.red;
                                    break;

                                case WeaponType.Medium_caliber_gun://中口径主砲
                                    GUI.backgroundColor = Color.red;
                                    break;

                                case WeaponType.Large_caliber_gun://大口径主砲
                                    GUI.backgroundColor = Color.red;
                                    break;

                                case WeaponType.Secondary_armament://副砲
                                    GUI.backgroundColor = Color.yellow;
                                    break;

                                case WeaponType.Torpedo://魚雷
                                    GUI.backgroundColor = Color.blue;
                                    break;

                                case WeaponType.Ship_mounted_boat://艦載艇
                                    
                                    break;

                                case WeaponType.Carrier_based_fighter://艦上戦闘機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Water_fighter://水上戦闘機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Shipboard_bomber://艦上爆撃機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Water_bomber://水上爆撃機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Carrier_based_attack_aircraft://艦上攻撃機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Ship_reconnaissance_aircraft://艦上偵察機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Water_reconnaissance_aircraft://水上偵察機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Anti_submarine_patrol_aircraft://対潜哨戒機
                                    
                                    break;

                                case WeaponType.Electric_search://電探
                                   
                                    break;

                                case WeaponType.Organ://機関
                                    
                                    break;

                                case WeaponType.Enhanced_bullet://強化弾
                                    
                                    break;

                                case WeaponType.Anti_aircraft_machine_gun://対空機銃
                                  
                                    break;

                                case WeaponType.Anti_aircraft_device://高射装置
                                   
                                    break;

                                case WeaponType.Depth_charge://爆雷
                                    
                                    break;

                                case WeaponType.Sonar://ソナー
                                  
                                    break;

                                case WeaponType.Repai_personnel://修理要員
                                   
                                    break;

                                case WeaponType.Expansion_basil://増設バジル
                                   
                                    break;

                                case WeaponType.No_classification://分類無
                                    
                                    break;

                                case WeaponType.Land_attack_aircraft://陸上攻撃機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Land_reconnaissance_aircraft://陸上偵察機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Local_fighter://局地戦闘機
                                    GUI.backgroundColor = Color.green;
                                    break;

                                case WeaponType.Army_fighter://陸軍戦闘機
                                    GUI.backgroundColor = Color.green;
                                    break;
                            }

                            if (Weapondata[i].No == SelctitemNo+1) GUI.backgroundColor = Color.black;//選択しているものの色を変更

                            if (GUILayout.Button(DataName[i]))
                            {
                                if (i != ItemNo) ItemNo = i;
                            }
                            GUI.backgroundColor = GUI.color;
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    EditorGUILayout.LabelField("NoData");
                    WeapoName = "";
                    WeapoNo = Rarity = FirepowerP = ThunderstormP = BombP = Anti_aircraftP = Anti_submarine = SearchenemyP = Hit_rate = Evasion_rate = ArmorP = Radius = 0;
                    Range = Weapon_Rang.Nodata;
                    type = WeaponType.Null;
                    Equipable_ship_type = new bool[9];
                } 
             
            }
            //ここまで

            //以下データ変更処理
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))//ここから縦並び
            {
                RigitBox = EditorGUILayout.BeginScrollView(RigitBox, GUI.skin.box);//選択枠
                { 

                EditorGUI.BeginDisabledGroup(DataName.Length == 0);//こいつで囲んだボタンをおせなくする
               if(DataName.Length!=0) EditorGUILayout.LabelField("武器データ変更 : "+DataName[ItemNo]);
                else EditorGUILayout.LabelField("武器データ変更");

                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    WeapoNo = EditorGUILayout.IntField("武器番号", WeapoNo);
                    BookNo = EditorGUILayout.IntField("図鑑番号", BookNo);
                    Rarity = EditorGUILayout.IntField("レア度", Rarity);
                }
                   
                WeapoName = EditorGUILayout.TextField("武器の名前", WeapoName);

                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {                  
                    FirepowerP = EditorGUILayout.IntField("火力", FirepowerP);
                    ThunderstormP = EditorGUILayout.IntField("雷装", ThunderstormP);
                    BombP = EditorGUILayout.IntField("爆装", BombP);
                }
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {                  
                    Anti_aircraftP = EditorGUILayout.IntField("対空", Anti_aircraftP);
                    Anti_submarine = EditorGUILayout.IntField("対潜", Anti_submarine);
                    SearchenemyP = EditorGUILayout.IntField("索敵", SearchenemyP);
                }
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {        
                    Hit_rate = EditorGUILayout.IntField("命中", Hit_rate);
                    Evasion_rate = EditorGUILayout.IntField("回避", Evasion_rate);
                    ArmorP = EditorGUILayout.IntField("装甲", ArmorP);
                }
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {                 
                    Range = (Weapon_Rang)EditorGUILayout.EnumPopup("射程", Range);
                    Radius = EditorGUILayout.IntField("半径", Radius);
                }

                development = EditorGUILayout.Toggle("開発可能か", development);

                EditorGUILayout.Space();

                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    EditorGUILayout.LabelField("装備可能艦種");
                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        Equipable_ship_type[0] = EditorGUILayout.Toggle("駆逐艦", Equipable_ship_type[0]);
                        Equipable_ship_type[1] = EditorGUILayout.Toggle("軽巡洋艦", Equipable_ship_type[1]);
                        Equipable_ship_type[2] = EditorGUILayout.Toggle("重巡洋艦", Equipable_ship_type[2]);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        Equipable_ship_type[3] = EditorGUILayout.Toggle("戦艦", Equipable_ship_type[3]);
                        Equipable_ship_type[4] = EditorGUILayout.Toggle("軽空母", Equipable_ship_type[4]);
                        Equipable_ship_type[5] = EditorGUILayout.Toggle("空母", Equipable_ship_type[5]);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        Equipable_ship_type[6] = EditorGUILayout.Toggle("水上空母", Equipable_ship_type[6]);
                        Equipable_ship_type[7] = EditorGUILayout.Toggle("航空戦艦", Equipable_ship_type[7]);
                        Equipable_ship_type[8] = EditorGUILayout.Toggle("潜水艦", Equipable_ship_type[8]);
                    }
                }

                EditorGUILayout.Space();
                type = (WeaponType)EditorGUILayout.EnumPopup("武器タイプ", type);

                EditorGUILayout.Space();

                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                    ImagData = EditorGUILayout.ObjectField("武器画像", ImagData, objType: typeof(Sprite));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                    GUI.backgroundColor = Color.red;
                    Imag = (Sprite)ImagData;
                    if (ImagData != null)
                    {
                        Texture t = (Texture)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(ImagData), typeof(Texture));// 指定画像ファイルのパスを取得し画像を表示
                        GUILayout.Box(t, GUILayout.Width(200), GUILayout.Height(200));
                    }
                    else
                    {
                        GUILayout.Box("NotData", GUILayout.Width(200), GUILayout.Height(200));
                    }
                    GUI.backgroundColor = GUI.color;
                }

                EditorGUI.EndDisabledGroup();//ここまで

                    if (Weapondata.Length != 0)//代入先がある場合
                    {
                        if (ItemNo != SelctitemNo)//代入先が変更されたら
                        {
                            SelctitemNo = ItemNo;
                            //一時保存データ初期化
                            WeapoName = Weapondata[SelctitemNo].WeaponName;
                            WeapoNo = Weapondata[SelctitemNo].No;
                            BookNo = Weapondata[SelctitemNo].BookNo;
                            development = Weapondata[SelctitemNo].development;
                            Rarity = Weapondata[SelctitemNo].Rarity;
                            FirepowerP = Weapondata[SelctitemNo].FirepowerP;
                            ThunderstormP = Weapondata[SelctitemNo].ThunderstormP;
                            BombP = Weapondata[SelctitemNo].BombP;
                            Anti_aircraftP = Weapondata[SelctitemNo].Anti_aircraftP;
                            Anti_submarine = Weapondata[SelctitemNo].Anti_submarine;
                            SearchenemyP = Weapondata[SelctitemNo].SearchenemyP;
                            Hit_rate = Weapondata[SelctitemNo].Hit_rate;
                            Evasion_rate = Weapondata[SelctitemNo].Evasion_rate;
                            ArmorP = Weapondata[SelctitemNo].ArmorP;
                            Range = Weapondata[SelctitemNo].Range;
                            Radius = Weapondata[SelctitemNo].Radius;
                            type = Weapondata[SelctitemNo].Type;
                            Equipable_ship_type = Weapondata[SelctitemNo].Equipable_ship_type;
                            ImagData = Weapondata[SelctitemNo].Imag;
                        }

                        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                        {
                            bool Data_change = true;

                            if (Data_change & Weapondata[SelctitemNo].No != WeapoNo || Data_change & Weapondata[SelctitemNo].WeaponName != WeapoName || Data_change & Weapondata[SelctitemNo].Rarity != Rarity ||
                                Data_change & Weapondata[SelctitemNo].FirepowerP != FirepowerP || Data_change & Weapondata[SelctitemNo].ThunderstormP != ThunderstormP ||
                                Data_change & Weapondata[SelctitemNo].BombP != BombP || Data_change & Weapondata[SelctitemNo].Anti_aircraftP != Anti_aircraftP ||
                                Data_change & Weapondata[SelctitemNo].Anti_submarine != Anti_submarine || Data_change & Weapondata[SelctitemNo].SearchenemyP != SearchenemyP ||
                                Data_change & Weapondata[SelctitemNo].Hit_rate != Hit_rate || Data_change & Weapondata[SelctitemNo].Evasion_rate != Evasion_rate ||
                                Data_change & Weapondata[SelctitemNo].ArmorP != ArmorP || Data_change & Weapondata[SelctitemNo].Range != Range ||
                                Data_change & Weapondata[SelctitemNo].Radius != Radius || Data_change & Weapondata[SelctitemNo].Type != type ||
                                Data_change & Weapondata[SelctitemNo].development != development || Data_change & Weapondata[SelctitemNo].Equipable_ship_type != Equipable_ship_type ||
                                Data_change & Weapondata[SelctitemNo].Imag != Imag || Data_change & Weapondata[SelctitemNo].BookNo != BookNo) Data_change = false;//データが更新されたか

                            EditorGUI.BeginDisabledGroup(Data_change);//こいつで囲んだボタンをおせなくする
                            if (GUILayout.Button("Reset"))
                            {
                                //一時保存データ初期化
                                WeapoName = Weapondata[SelctitemNo].WeaponName;
                                WeapoNo = Weapondata[SelctitemNo].No;
                                BookNo = Weapondata[SelctitemNo].BookNo;
                                development = Weapondata[SelctitemNo].development;
                                Rarity = Weapondata[SelctitemNo].Rarity;
                                FirepowerP = Weapondata[SelctitemNo].FirepowerP;
                                ThunderstormP = Weapondata[SelctitemNo].ThunderstormP;
                                BombP = Weapondata[SelctitemNo].BombP;
                                Anti_aircraftP = Weapondata[SelctitemNo].Anti_aircraftP;
                                Anti_submarine = Weapondata[SelctitemNo].Anti_submarine;
                                SearchenemyP = Weapondata[SelctitemNo].SearchenemyP;
                                Hit_rate = Weapondata[SelctitemNo].Hit_rate;
                                Evasion_rate = Weapondata[SelctitemNo].Evasion_rate;
                                ArmorP = Weapondata[SelctitemNo].ArmorP;
                                Range = Weapondata[SelctitemNo].Range;
                                Radius = Weapondata[SelctitemNo].Radius;
                                type = Weapondata[SelctitemNo].Type;
                                Equipable_ship_type = Weapondata[SelctitemNo].Equipable_ship_type;
                                ImagData = Weapondata[SelctitemNo].Imag;
                            }

                            if (GUILayout.Button("Save"))
                            {
                                //データ保存
                                Weapondata[SelctitemNo].No = WeapoNo;
                                Weapondata[SelctitemNo].BookNo = BookNo;
                                Weapondata[SelctitemNo].development = development;
                                Weapondata[SelctitemNo].WeaponName = WeapoName;
                                Weapondata[SelctitemNo].Rarity = Rarity;
                                Weapondata[SelctitemNo].FirepowerP = FirepowerP;
                                Weapondata[SelctitemNo].ThunderstormP = ThunderstormP;
                                Weapondata[SelctitemNo].BombP = BombP;
                                Weapondata[SelctitemNo].Anti_aircraftP = Anti_aircraftP;
                                Weapondata[SelctitemNo].Anti_submarine = Anti_submarine;
                                Weapondata[SelctitemNo].SearchenemyP = SearchenemyP;
                                Weapondata[SelctitemNo].Hit_rate = Hit_rate;
                                Weapondata[SelctitemNo].Evasion_rate = Evasion_rate;
                                Weapondata[SelctitemNo].ArmorP = ArmorP;
                                Weapondata[SelctitemNo].Range = Range;
                                Weapondata[SelctitemNo].Radius = Radius;
                                Weapondata[SelctitemNo].Type = type;
                                Weapondata[SelctitemNo].Equipable_ship_type = Equipable_ship_type;
                                Weapondata[SelctitemNo].Imag = Imag;
                                EditorUtility.SetDirty(Weapondata[SelctitemNo]);//指定したScriptObject変更を記録
                                AssetDatabase.SaveAssets();//ScriptObjectをセーブする

                                //一時保存データ初期化
                                WeapoName = Weapondata[SelctitemNo].WeaponName;
                                WeapoNo = Weapondata[SelctitemNo].No;
                                BookNo = Weapondata[SelctitemNo].BookNo;
                                development = Weapondata[SelctitemNo].development;
                                Rarity = Weapondata[SelctitemNo].Rarity;
                                FirepowerP = Weapondata[SelctitemNo].FirepowerP;
                                ThunderstormP = Weapondata[SelctitemNo].ThunderstormP;
                                BombP = Weapondata[SelctitemNo].BombP;
                                Anti_aircraftP = Weapondata[SelctitemNo].Anti_aircraftP;
                                Anti_submarine = Weapondata[SelctitemNo].Anti_submarine;
                                SearchenemyP = Weapondata[SelctitemNo].SearchenemyP;
                                Hit_rate = Weapondata[SelctitemNo].Hit_rate;
                                Evasion_rate = Weapondata[SelctitemNo].Evasion_rate;
                                ArmorP = Weapondata[SelctitemNo].ArmorP;
                                Range = Weapondata[SelctitemNo].Range;
                                Radius = Weapondata[SelctitemNo].Radius;
                                type = Weapondata[SelctitemNo].Type;
                                Equipable_ship_type = Weapondata[SelctitemNo].Equipable_ship_type;
                                ImagData = Weapondata[SelctitemNo].Imag;
                            }
                            EditorGUI.EndDisabledGroup();//ここまで
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
            }//ここまで
        }

        var e = Event.current;
        switch (e.type)
        {
            case EventType.ContextClick://右クリックメニュー
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("武器を追加"), false, () => { File_Set(); });
                menu.ShowAsContext();
                break;
        }
    }

    void File_Set()//ファイル追加処理
    {
        string DefName = "NewWeponData.asset";//初期値の名前
        DefName = Path.GetFileNameWithoutExtension(AssetDatabase.GenerateUniqueAssetPath(Path.Combine("Assets"+ AssetFileName,DefName)));//同じ名前のものがあるかを判定
        var pas = EditorUtility.SaveFilePanelInProject("武器を追加",DefName,"asset","", "Assets" + AssetFileName);
        if (!string.IsNullOrEmpty(pas))//保存処理
        {
            string[] name1 = pas.Split('/');
            string WeaponName = name1[name1.Length - 1].Substring(0, name1[name1.Length - 1].Length - 6);
            WeaponData Savedata = new WeaponData();
            Savedata.WeaponName = WeaponName;//ファイル名を代入
            Savedata.No = Weapondata.Length+1;

            AssetDatabase.CreateAsset(Savedata, pas);
            AssetDatabase.Refresh();
        }
    }
}
