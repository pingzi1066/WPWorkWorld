using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ExcelEditor : EditorWindow {

    static private Excel mExcel;
    static private ExcelTable mTable;
    static private int selectSheetIndex;

    static private int selectExcelIndex;
    static private Dictionary<string,Excel> allExcel = new Dictionary<string, Excel>();

    //用于读取保存
    const string EXCEL_PATH = "/Test/";
    const string EXPORT_JSON_PATH = "/Resources/Json/";
    static private string Current_Excel_Name
    {
        get
        {
            return allExcel.ElementAt(selectExcelIndex).Key + ".xlsx";
        }
    }

    static private string Current_Json_Name
    {
        get 
        { 
            string xlsName = allExcel.ElementAt(selectExcelIndex).Key;
            xlsName = char.ToUpper(xlsName[0]) + xlsName.Substring(1);
            return "Static" + xlsName + ".json";
        }
    }

    //用于实例化
    private static bool isInit = false;

    [MenuItem("MyEditor/ShowXlsEditor")]
    static void ShowWindow()
    {
        Init();

//        ExcelEditor window = EditorWindow.GetWindowWithRect<ExcelEditor>();
        GetWindow<ExcelEditor>(true, "Excel Editor", true).Show();
//        window.Show();

//        string path = Application.dataPath + "/Test/Test3.xlsx";
//
//        Debug.Log(path);
//        Excel xls =  ExcelHelper.LoadExcel(path);

//        xls.ShowLog();

//        window.Show(xls);
    }

    static void Init()
    {
        if (isInit)
            return;
        isInit = true;

        //加载目录下的所有的xlsx文件
        string[] objsPath = Directory.GetFiles(Application.dataPath + EXCEL_PATH, "*", SearchOption.AllDirectories);
        //循环遍历每一个路径，单独加载
        foreach (string excelPath in objsPath)
        {
            if (excelPath.EndsWith(".xlsx"))
            {
                //                Debug.Log(excelPath);
                FileInfo file = new FileInfo(excelPath);
                string fileName = file.Name.Substring(0, file.Name.Length - 5);
                Excel xls =  ExcelHelper.LoadExcel(file);
                allExcel.Add(fileName, xls);
            }
        }
    }


    public void Show(Excel xls)
    {
//        mExcel = xls;
//        for (int i = 0; i < mExcel.Tables.Count; i++)
//        {
            //显示类型
//            mExcel.Tables[i].SetCellTypeColumn(1, ExcelTableCellType.Label);
//            mExcel.Tables[i].SetCellTypeColumn(3, ExcelTableCellType.Popup, new List<string>(){"1","2"});
//            mExcel.Tables[i].SetCellTypeRow(1, ExcelTableCellType.Label);
//            mExcel.Tables[i].SetCellTypeRow(2, ExcelTableCellType.Label);
//        }
    }

    void OnGUI()
    {
        if (allExcel.Count > 0)
        {
            EditorDrawHelper.DrawExcelTab(allExcel, ref selectExcelIndex);
            mExcel = allExcel.ElementAt(selectExcelIndex).Value;

            if (mExcel != null)
            {
                EditorDrawHelper.DrawTableTab(mExcel, ref selectSheetIndex);
                mTable = mExcel.Tables[selectSheetIndex];
                EditorDrawHelper.DrawTable(mTable);
                DrawButton();
            }
        }
        else
            Init();
    }

    public void DrawButton()
    {
        EditorGUILayout.BeginHorizontal();
        EditorDrawHelper.DrawButton("Add", delegate()
        {
            mTable.NumberOfRows++;
            Show(mExcel);
        });

        EditorDrawHelper.DrawButton("Save", delegate()
        {
            string path = Application.dataPath + EXCEL_PATH + Current_Excel_Name;
            ExcelHelper.SaveExcel(mExcel, path);
            EditorUtility.DisplayDialog("Save Success", path, "ok");
        });

        EditorDrawHelper.DrawButton("Export .json", delegate()
            {
                string path = Application.dataPath + EXPORT_JSON_PATH + Current_Json_Name;
                if(JsonHelper.ExportToJson(mExcel,path))
                {
                    EditorUtility.DisplayDialog("Export Success", path, "ok");
                }
                else
                {
                    EditorUtility.DisplayDialog("Export Faild", path, "ok");
                }
            });
        EditorGUILayout.EndHorizontal();
    }
}
