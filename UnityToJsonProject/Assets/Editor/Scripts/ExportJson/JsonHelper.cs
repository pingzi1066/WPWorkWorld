/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * #CREATIONDATE#     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using UnityEditor;

/// <summary>
/// Json导出工具
/// </summary>
public class JsonHelper 
{

    #region export .json file

    public static bool ExportToJson(Excel xls,string path)
    {
        //
        JSONClass jc = new JSONClass();

        if (xls.Tables.Count > 0)
        {
            //写入头与判断头的格式
            ExcelTable table = xls.Tables[0];
            if (table == null)
            {
                Debug.Log("table is null!!!");
                return false;
            }

            //字段名字 加入
            List<string> headNames = new List<string>();

            for (int Column = 2; Column <= table.NumberOfColumns; Column++)
            {
                ExcelTableCell cell = table.GetCell(1, Column);
                if (cell == null)
                {
                    Debug.LogError("name cell is null!!! Column : " + Column + " table name " + table.TableName + "table sum col " + table.NumberOfColumns);
                    return false;
                }
                string name = cell.Value;
                headNames.Add(name);
            }

            List<System.Type> propTypes = new List<System.Type>();

//            string log = "";
            //字段属性判断 ，从第2列开始，从第3行开始,
            for (int column = 2; column <= table.NumberOfColumns; column++)
            {
                System.Type tp = typeof(System.Int32);
                for (int row = 3; row <= table.NumberOfRows; row++)
                {
                    System.Type temTp = GetType(table.GetCell(row, column).Value);
                    //字符串直接过
                    if (temTp.ToString() == typeof(string).ToString())
                    {
                        tp = temTp;
                        break;
                    }

                    //整形 遇到 浮点
                    if (tp.ToString() == typeof(int).ToString() && temTp.ToString() == typeof(float).ToString())
                    {
                        tp = temTp;
                    }
                }
//                log += tp.ToString() + " \n";
                propTypes.Add(tp);
            }
//            Debug.Log(log);

            //------------------字段名与类型加入完毕，开始写入值

            for (int i = 0; i < xls.Tables.Count; i++)
            {
                table = xls.Tables[i];

                if (table != null && table.NumberOfRows > 0)
                {
                    JSONClass jcBySheet = SheetToJson(table, headNames, propTypes);
                    if (jcBySheet != null)
                    {
//                        Debug.Log(jcBySheet.ToString());
                        for (int j = 0; j < jcBySheet.Count; j++)
                        {
                            jc.Add(jcBySheet.GetKey(j), jcBySheet[j]);
                        }
                    }
                }
            }
        }

        if (jc != null)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs,  System.Text.Encoding.UTF8);
            string text = jc.ToString();
            bw.Write(text);
            fs.Close();
            bw.Close();

            AssetDatabase.Refresh();
            return true;
        }
        return false;
    }

    public static JSONClass SheetToJson(ExcelTable table,List<string> headNames,List<System.Type> types)
    {
        if (table.NumberOfColumns != headNames.Count + 1)
        {
            Debug.Log("Length error!!! table.NumberOfColumns " + table.NumberOfColumns + "  headNames.Count  " +headNames.Count );
            return null;
        }

        //这个表所有的标识符对应字段
        JSONClass jn = new JSONClass();

        for (int row = 3; row <= table.NumberOfRows; row++)
        {
            //一个标识符对应的所有属性字段
            JSONClass rowData = new JSONClass();
            for (int column = 2; column <= table.NumberOfColumns; column++)
            {
                string value = table.GetCell(row, column).Value;
                System.Type tp = types[column - 2];

                JSONData cellValue = null;

                if (tp.ToString() == typeof(string).ToString())
                {
                    cellValue = new JSONData(value);
//                    Debug.Log("is string");
                }
                else if (tp.ToString() == typeof(float).ToString())
                {
                    cellValue = new JSONData(float.Parse(value));
//                    Debug.Log("is float");
                }
                else
                {
                    cellValue = new JSONData(int.Parse(value));
//                    Debug.Log("is int" + cellValue.ToString());
                }

                //id : {属性：值}
                rowData.Add(headNames[column - 2], cellValue);
            }

            jn.Add(table.GetCell(row, 1).Value, rowData);
        }

        return jn;
    }

    /// <summary>
    /// 取类型
    /// </summary>
    /// <param name="s">S.</param>
    public static System.Type GetType(string s)
    {
        //System.Single
        float f = 0;
        bool result = float.TryParse(s, out f);
        if (s.Contains("."))
        {
            if (result)
                return typeof(float);
        }

        // System.Int32
        int i = 0;
        result = int.TryParse(s, out i);
        if (result)
            return typeof(int);

        //System.String          
        return typeof(string);
    }

    #endregion
}