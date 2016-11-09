/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * #CREATIONDATE#     WP      Initial version
 * 
 * *****************************************************************************/

using UnityEngine;
using System.Collections;

namespace KMTool
{
    
    /// <summary>
    /// 用于Excel导出的设置
    /// </summary>
    public class XLS_JsonSetting 
    {
        //用于读取保存
        public const string EXCEL_PATH = "/Demo_Excel_Json/";
        public const string EXPORT_JSON_PATH = "/Resources/Json/";

        /// <summary>
        /// 根据文件名得到JSON名，比如 Monster 注意没有后缀
        /// </summary>
        /// <returns>The json name.</returns>
        /// <param name="fileName">File name.</param>
        static public string GetJsonName(string fileName)
        {
            string xlsName = fileName;
            xlsName = char.ToUpper(xlsName[0]) + xlsName.Substring(1);
            return "Static" + xlsName + ".json";
        }
    }
}