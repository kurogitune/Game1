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
    string[] DataName = new string[0];
    Vector2 v2 = Vector2.zero;
    int SelctNo;
    int ItemNo;
    Object ImagData;


    //変更データ
    string CharaName;
    int CharaNo, Durable;//図鑑番号  耐久（体力）

    int Def_FirepowerP, Def_ArmorP, Def_ThunderstormP, Def_Evasion_rate, Def_Anti_aircraftP, Def_Anti_submarine, Def_SearchenemyP, Def_luck;
    //初期値　　火力　装甲　雷装　回避　対空　対潜　　索敵　　運
    int Max_FirepowerP, Max_ArmorP, Max_ThunderstormP, Max_Evasion_rate, Max_Anti_aircraftP, Max_Anti_submarine, SearchenemyP, Max_SearchenemyP, Max_luck;
    //最大値　　火力　装甲　雷装　回避　対空　対潜　　索敵　　運
   
    int Max_Fuelconsumption, Max_Consumedammunition;//最大消費燃料　最大消費弾薬
    int Remodeling_level;//改造レベル
    Sprite Imag;//画像
    ShipType Type;//艦種
    Ship_Range Range;//射程
    Ship_Speed Speed;//速力
    WeaponData[] Initial_equipment = new WeaponData[5];//初期装備
    int[] Numberofmounted = new int[5];//艦載機搭載数
    bool DataWrong=true;
    private void OnGUI()
    {
        //以下武器データ取得処理
        DirectoryInfo dir = new DirectoryInfo("Assets" + AssetFileName);//ファイルパス
        FileInfo[] fild = dir.GetFiles("*.asset");//こいつでファイル名を取得
        if (CharaData.Length != fild.Length)
        {
            CharaData = new CharacterData[fild.Length];
            DataName = new string[fild.Length];
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
                            if (GUILayout.Button(DataName[i]))
                            {
                                if (i != SelctNo) SelctNo = i;
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                else
                {
                    EditorGUILayout.LabelField("No Data");
                }
            }

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))//データ指定処理
            {
                CharaName = EditorGUILayout.TextField("キャラ名前", CharaName);
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    Type = (ShipType)EditorGUILayout.EnumPopup("艦種", Type);
                    CharaNo = EditorGUILayout.IntField("図鑑番号", CharaNo);         
                }

                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                    ImagData = EditorGUILayout.ObjectField("キャラ画像", ImagData, objType: typeof(Sprite));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です

                    GUI.backgroundColor = Color.red;
                    if (ImagData != null)
                    {
                        Texture t =(Texture) AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(ImagData),typeof(Texture));// 指定画像ファイルのパスを取得し画像を表示
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
                    Durable = EditorGUILayout.IntField("",Durable);
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
                        Numberofmounted[0] = EditorGUILayout.IntField("搭載数",Numberofmounted[0]);
                        Object ob = null;
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                        Initial_equipment[0] =(WeaponData) EditorGUILayout.ObjectField("装備",ob ,objType:typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("スロット　二");
                        Numberofmounted[1] = EditorGUILayout.IntField("搭載数", Numberofmounted[1]);
                        Object ob = null;
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                        Initial_equipment[1] = (WeaponData)EditorGUILayout.ObjectField("装備", ob, objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("スロット　三");
                        Numberofmounted[2] = EditorGUILayout.IntField("搭載数", Numberofmounted[2]);
                        Object ob = null;
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                        Initial_equipment[2] = (WeaponData)EditorGUILayout.ObjectField("装備", ob, objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("スロット　四");
                        Numberofmounted[3] = EditorGUILayout.IntField("搭載数", Numberofmounted[3]);
                        Object ob = null;
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                        Initial_equipment[3] = (WeaponData)EditorGUILayout.ObjectField("装備", ob, objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                    }

                    using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField("スロット　五");
                        Numberofmounted[4] = EditorGUILayout.IntField("搭載数", Numberofmounted[4]);
                        Object ob = null;
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
                        Initial_equipment[4] = (WeaponData)EditorGUILayout.ObjectField("装備", ob, objType: typeof(WeaponData));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
                    }
                }

                EditorGUILayout.Space();

                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    EditorGUI.BeginDisabledGroup(DataWrong);
                    if (GUILayout.Button("Reset"))
                    {

                    }

                    if (GUILayout.Button("Save"))
                    {

                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
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
        string DefName = "NewCharaData.asset";//初期値の名前
        DefName = Path.GetFileNameWithoutExtension(AssetDatabase.GenerateUniqueAssetPath(Path.Combine("Assets" + AssetFileName, DefName)));//同じ名前のものがあるかを判定
        var pas = EditorUtility.SaveFilePanelInProject("キャラデータ追加", DefName, "asset", "", "Assets" + AssetFileName);
        if (!string.IsNullOrEmpty(pas))//保存処理
        {
            string[] name1 = pas.Split('/');
            string WeaponName = name1[name1.Length - 1].Substring(0, name1[name1.Length - 1].Length - 6);
            WeaponData Savedata = new WeaponData();
            Savedata.WeaponName = WeaponName;//ファイル名を代入
            Savedata.No = CharaData.Length + 1;

            AssetDatabase.CreateAsset(Savedata, pas);
            AssetDatabase.Refresh();
        }
    }
}
