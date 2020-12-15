using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FilePas_Editor : EditorWindow//ファイルパス確認用エディタ
{
[MenuItem("EditorEx/FilePas確認")]
    static void Open()
    {
        GetWindow<FilePas_Editor>("ファイルパス確認");
    }


    Object Obj;
    string FilePas;
    public void OnGUI()
    {
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
        Obj = EditorGUILayout.ObjectField("確認したいファイル", Obj, objType: typeof(Object));
#pragma warning restore CS0618 // 型またはメンバーが古い形式です

        if (Obj != null)
        {
            FilePas = AssetDatabase.GetAssetPath(Obj);//こいつでファイルパスを取得
            Obj = null;
        }
        EditorGUILayout.LabelField("ファイルパス");
        EditorGUILayout.TextField(FilePas);
    }
}
