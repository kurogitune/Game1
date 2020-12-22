using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Character_Editor : EditorWindow
{
    static string AssetFileName = "/AssetObj/CharacterAsset";
    [MenuItem("EditorEx/CharaEditor %#X")]
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
            Debug.Log("CharacterAssetフォルダー作成");
            AssetDatabase.Refresh();//Unityのファイル表示を更新  
        }

        GetWindow<Character_Editor>("CharacterEditor"); // タイトル名を指定
    }

    CharacterData[] CharaData=new CharacterData[0];
    Vector2 v2 = Vector2.zero;
    Vector2 RigitBox = Vector2.zero;
    int SelctNo=-1;
    int ItemNo;
    Object ImagData;


    //変更データ
    string CharaName;
    int BookNo, Durable;//図鑑番号  耐久（体力）

    int Def_FirepowerP, Def_ArmorP, Def_ThunderstormP, Def_Evasion_rate, Def_Anti_aircraftP, Def_Anti_submarine, Def_SearchenemyP, Def_luck;
    //初期値　　火力　装甲　雷装　回避　対空　対潜　　索敵　　運
    int Max_FirepowerP, Max_ArmorP, Max_ThunderstormP, Max_Evasion_rate, Max_Anti_aircraftP, Max_Anti_submarine, SearchenemyP, Max_SearchenemyP, Max_luck;
    //最大値　　火力　装甲　雷装　回避　対空　対潜　　索敵　　運
   
    int Max_Fuelconsumption, Max_Consumedammunition;//最大消費燃料　最大消費弾薬
    int Remodeling_Level, Steel, Ammunition, Development_Materials, HighSpeed_Construction_material;
    //改造レベル　鋼材　弾薬　開発資材　高速建造材
    int Refurbished_Blueprint, Detailedbattle_Report, Trialdeck_Catapult, Newaviationarmament_Materials, Newartilleryweapon_Materials;
    //改装設計図　戦闘詳報　試製甲板カタパルト　新型航空兵装資材　新型砲熕兵装資材
    CharacterData Remodeling_Destination;//改造先

    Sprite Imag;//画像
    ShipType Type;//艦種
    Ship_Range Range;//射程
    Ship_Speed Speed;//速力
    WeaponData[] Initial_equipment = new WeaponData[5];//初期装備
    int[] Numberofmounted = new int[5];//艦載機搭載数

    private void OnGUI()
    {
        //以下武器データ取得処理
        DirectoryInfo dir = new DirectoryInfo("Assets" + AssetFileName);//ファイルパス
        FileInfo[] fild = dir.GetFiles("*.asset");//こいつでファイル名を取得
        if (CharaData.Length != fild.Length)
        {
            CharaData = new CharacterData[fild.Length];
            for (int i = 0; i < fild.Length; i++)
            {
                string path = dir + "/" + fild[i].Name;
                string WeaponName = fild[i].Name.Substring(0, fild[i].Name.Length - 6);

                CharaData[i] = (CharacterData)AssetDatabase.LoadAssetAtPath(path, typeof(CharacterData));
            }
        }
        //ここまで

        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))//左枠
            {
                if (CharaData.Length != 0)
                {
                    EditorGUILayout.LabelField("キャラ選択");
                    v2 = EditorGUILayout.BeginScrollView(v2, GUI.skin.box);
                    {
                        for (int i = 0; i < CharaData.Length; i++)
                        {
                            if (GUILayout.Button(CharaData[i].CharacterName))
                            {
                                if (i != ItemNo) ItemNo = i;
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    EditorGUILayout.LabelField("No Data");
                    BookNo = Durable = Def_FirepowerP = Max_FirepowerP = Def_ArmorP = Max_ArmorP = Def_ThunderstormP = Max_ThunderstormP
                   = Def_Evasion_rate = Max_Evasion_rate = Def_Anti_aircraftP = Max_Anti_aircraftP = Def_Anti_submarine = Max_Anti_submarine
                   = Def_SearchenemyP = Max_SearchenemyP = Def_luck = Max_luck = Max_Fuelconsumption = Max_Consumedammunition
                   = Remodeling_Level = Steel = Ammunition = Development_Materials = HighSpeed_Construction_material = Refurbished_Blueprint
                   = Detailedbattle_Report = Trialdeck_Catapult = Newaviationarmament_Materials = Newartilleryweapon_Materials = 0;
                    Type = ShipType.NoData;
                    Range = Ship_Range.Nodata;
                    Speed = Ship_Speed.Nodata;
                    ImagData = null;
                    Remodeling_Destination = null;
                    CharaName = "";
                }
            }
     
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))//データ指定処理
            {
                RigitBox = EditorGUILayout.BeginScrollView(RigitBox, GUI.skin.box);

                    EditorGUI.BeginDisabledGroup(CharaData.Length == 0);
                    CharaName = EditorGUILayout.TextField("キャラ名前", CharaName);
                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        Type = (ShipType)EditorGUILayout.EnumPopup("艦種", Type);
                        BookNo = EditorGUILayout.IntField("図鑑番号", BookNo);
                        Remodeling_Level = EditorGUILayout.IntField("次改造可能レベル", Remodeling_Level);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("改造必要素材");
                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        Steel = EditorGUILayout.IntField("鋼材", Steel);
                        Ammunition = EditorGUILayout.IntField("弾薬", Ammunition);
                        Development_Materials = EditorGUILayout.IntField("開発資材", Development_Materials);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        HighSpeed_Construction_material = EditorGUILayout.IntField("高速建造材", HighSpeed_Construction_material);
                        Refurbished_Blueprint = EditorGUILayout.IntField("改装設計図", Refurbished_Blueprint);
                        Newaviationarmament_Materials = EditorGUILayout.IntField("新型航空兵装資材", Newaviationarmament_Materials);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        Detailedbattle_Report = EditorGUILayout.IntField("戦闘詳報", Detailedbattle_Report);
                        Trialdeck_Catapult = EditorGUILayout.IntField("試製甲板カタパルト", Trialdeck_Catapult);
                        Newartilleryweapon_Materials = EditorGUILayout.IntField("新型砲熕兵装資材", Newartilleryweapon_Materials);
                    }
                    EditorGUILayout.Space();
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                    Remodeling_Destination = (CharacterData)EditorGUILayout.ObjectField("改造先", Remodeling_Destination, objType: typeof(CharacterData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                        ImagData = EditorGUILayout.ObjectField("キャラ画像", ImagData, objType: typeof(Sprite));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です

                        GUI.backgroundColor = Color.red;
                        if (ImagData != null)
                        {
                            Texture t = (Texture)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(ImagData), typeof(Texture));// 指定画像ファイルのパスを取得し画像を表示
                            GUILayout.Box(t, GUILayout.Width(150), GUILayout.Height(150));
                        }
                        else
                        {
                            GUILayout.Box("NotData", GUILayout.Width(150), GUILayout.Height(150));
                        }
                        GUI.backgroundColor = GUI.color;
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("耐久");
                        Durable = EditorGUILayout.IntField("", Durable);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("火力");
                        Def_FirepowerP = EditorGUILayout.IntField("初期値", Def_FirepowerP);
                        Max_FirepowerP = EditorGUILayout.IntField("最大値", Max_FirepowerP);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("装甲");
                        Def_ArmorP = EditorGUILayout.IntField("初期値", Def_ArmorP);
                        Max_ArmorP = EditorGUILayout.IntField("最大値", Max_ArmorP);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("雷装");
                        Def_ThunderstormP = EditorGUILayout.IntField("初期値", Def_ThunderstormP);
                        Max_ThunderstormP = EditorGUILayout.IntField("最大値", Max_ThunderstormP);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("回避");
                        Def_Evasion_rate = EditorGUILayout.IntField("初期値", Def_Evasion_rate);
                        Max_Evasion_rate = EditorGUILayout.IntField("最大値", Max_Evasion_rate);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("対空");
                        Def_Anti_aircraftP = EditorGUILayout.IntField("初期値", Def_Anti_aircraftP);
                        Max_Anti_aircraftP = EditorGUILayout.IntField("最大値", Max_Anti_aircraftP);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("対潜");
                        Def_Anti_submarine = EditorGUILayout.IntField("初期値", Def_Anti_submarine);
                        Max_Anti_submarine = EditorGUILayout.IntField("最大値", Max_Anti_submarine);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("速力");
                        Speed = (Ship_Speed)EditorGUILayout.EnumPopup("", Speed);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("索敵");
                        Def_SearchenemyP = EditorGUILayout.IntField("初期値", Def_SearchenemyP);
                        Max_SearchenemyP = EditorGUILayout.IntField("最大値", Max_SearchenemyP);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("射程");
                        Range = (Ship_Range)EditorGUILayout.EnumPopup("", Range);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("運");
                        Def_luck = EditorGUILayout.IntField("初期値", Def_luck);
                        Max_luck = EditorGUILayout.IntField("最大値", Max_luck);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("最大消費燃料");
                        Max_Fuelconsumption = EditorGUILayout.IntField("", Max_Fuelconsumption);
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("最大消費弾薬");
                        Max_Consumedammunition = EditorGUILayout.IntField("", Max_Consumedammunition);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("初期装備");

                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))//初期装備選択
                    {
                        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                        {
                            EditorGUILayout.LabelField("スロット　一");
                            Numberofmounted[0] = EditorGUILayout.IntField("搭載数", Numberofmounted[0]);
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                            Initial_equipment[0] = (WeaponData)EditorGUILayout.ObjectField("装備", Initial_equipment[0], objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                        }

                        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                        {
                            EditorGUILayout.LabelField("スロット　二");
                            Numberofmounted[1] = EditorGUILayout.IntField("搭載数", Numberofmounted[1]);
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                            Initial_equipment[1] = (WeaponData)EditorGUILayout.ObjectField("装備", Initial_equipment[1], objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                        }

                        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                        {
                            EditorGUILayout.LabelField("スロット　三");
                            Numberofmounted[2] = EditorGUILayout.IntField("搭載数", Numberofmounted[2]);
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                            Initial_equipment[2] = (WeaponData)EditorGUILayout.ObjectField("装備", Initial_equipment[2], objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                        }

                        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                        {
                            EditorGUILayout.LabelField("スロット　四");
                            Numberofmounted[3] = EditorGUILayout.IntField("搭載数", Numberofmounted[3]);
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                            Initial_equipment[3] = (WeaponData)EditorGUILayout.ObjectField("装備", Initial_equipment[3], objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                        }

                        using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                        {
                            EditorGUILayout.LabelField("スロット　五");
                            Numberofmounted[4] = EditorGUILayout.IntField("搭載数", Numberofmounted[4]);
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                            Initial_equipment[4] = (WeaponData)EditorGUILayout.ObjectField("装備", Initial_equipment[4], objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.Space();

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        bool DataWrong = true;//データ変更ありか
                        if (CharaData.Length != 0)
                        {
                            if (ItemNo != SelctNo)//選択データ変更
                            {
                                SelctNo = ItemNo;
                                DataRset();
                            }

                            if (CharaData[SelctNo].CharacterName != CharaName) DataWrong = false;

                            EditorGUI.BeginDisabledGroup(DataWrong);
                            if (GUILayout.Button("Reset"))
                            {
                                DataRset();
                            }

                            if (GUILayout.Button("Save"))
                            {
                                DataRset();
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                    }
                EditorGUILayout.EndScrollView();
            }
        }

        var e = Event.current;
        switch (e.type)
        {
            case EventType.ContextClick://右クリックメニュー
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("キャラデータ追加"), false, () => { File_Set(); });
                menu.ShowAsContext();
                break;
        }
    }

    void File_Set()//ファイル追加処理
    {
        string DefName = "NewCharaData.asset";//初期値の名前
        DefName = Path.GetFileNameWithoutExtension(AssetDatabase.GenerateUniqueAssetPath(Path.Combine("Assets" + AssetFileName, DefName)));//同じ名前のものがあるかを判定
        var pas = EditorUtility.SaveFilePanelInProject("キャラデータ追加", DefName, "asset", "", "Assets" + AssetFileName);
        if (!string.IsNullOrEmpty(pas))//保存処理
        {
            string[] name1 = pas.Split('/');
            string WeaponName = name1[name1.Length - 1].Substring(0, name1[name1.Length - 1].Length - 6);
            CharacterData Savedata = new CharacterData();
            Savedata.CharacterName = WeaponName;//ファイル名を代入
            Savedata.No = CharaData.Length + 1;

            AssetDatabase.CreateAsset(Savedata, pas);
            AssetDatabase.Refresh();
        }
    }

    void DataRset()
    {
        BookNo = CharaData[SelctNo].BookNo;
        CharaName = CharaData[SelctNo].CharacterName;
        Type = CharaData[SelctNo].Type;
        Durable = CharaData[SelctNo].Durable;
        Def_FirepowerP = CharaData[SelctNo].Def_FirepowerP;
        Max_FirepowerP = CharaData[SelctNo].Max_FirepowerP;
        Def_ArmorP = CharaData[SelctNo].Def_ArmorP;
        Max_ArmorP = CharaData[SelctNo].Max_ArmorP;
        Def_ThunderstormP = CharaData[SelctNo].Def_ThunderstormP;
        Max_ThunderstormP = CharaData[SelctNo].Max_ThunderstormP;
        Def_Evasion_rate = CharaData[SelctNo].Def_Evasion_rate;
        Max_Evasion_rate = CharaData[SelctNo].Max_Evasion_rate;
        Def_Anti_aircraftP = CharaData[SelctNo].Def_Anti_aircraftP;
        Max_Anti_aircraftP = CharaData[SelctNo].Max_Anti_aircraftP;
        Def_Anti_submarine = CharaData[SelctNo].Def_Anti_submarine;
        Max_Anti_submarine = CharaData[SelctNo].Max_Anti_submarine;
        Speed = CharaData[SelctNo].Speed;
        Def_SearchenemyP = CharaData[SelctNo].Def_SearchenemyP;
        Max_SearchenemyP = CharaData[SelctNo].Max_SearchenemyP;
        Range = CharaData[SelctNo].Range;
        Def_luck = CharaData[SelctNo].Def_luck;
        Max_luck = CharaData[SelctNo].Max_luck;
        Max_Fuelconsumption = CharaData[SelctNo].Max_Fuelconsumption;
        Max_Consumedammunition = CharaData[SelctNo].Max_Consumedammunition;
        Remodeling_Level = CharaData[SelctNo].Remodeling_Level;
        ImagData = CharaData[SelctNo].Imag;
    }
}
