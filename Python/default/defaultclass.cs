using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
public class defaultclass : defaultCsvDataParent
{

    private volatile static defaultclass _instance = null;
    private static readonly object lockHelper = new object();
    private defaultclass() { }
    private Dictionary<int, int> dict = new Dictionary<int, int>();
    public static defaultclass Instance()
    {
        if (_instance == null)
        {
            lock (lockHelper)
            {
                if (_instance == null)
                {
                    _instance = new defaultclass();
                }
            }
        }
        return _instance;
    }

    private void initDict()
    {
        int length = num();
        for (int i = 0; i < length; i++)
        {
            int id = System.Int32.Parse(_DataArray[i, 0]);
            dict.Add(id, i);
        }
    }
    public int getIdxByTemplateID(int templateID)
    {
        if (dict.ContainsKey(templateID))
        {
            return dict[templateID];
        }
        return -1;
    }
    //通过类型和行数获取内容 num 0 start
    public string getStrByIdx(int num, string typeName)
    {
        int typenum = getTypeNum(typeName);
        if (typenum == -1)
        {
            Debug.Log(typeName + "   " + num + "  error");
            return "-1";
        }
        return _DataArray[num, typenum];
    }

    public override string getStr(int id, string title)
    {
        int idx = getIdxByTemplateID(id);
        if (idx == -1)
        {
            Debug.Log("getTemplate  " + idx + "  error");
            return "-1";
        }
        int num = getTypeNum(title);
        return _DataArray[idx, num];
    }

    //转换get的类型为int返回
    public override int getInt(int id, string title)
    {
        int idx = getIdxByTemplateID(id);
        if (idx == -1)
        {
            Debug.Log("getTemplate  " + idx + "  error");
            return -1;
        }
        int num = getTypeNum(title);
        return int.Parse(_DataArray[idx, num]);
    }

    //转换get的类型为float返回
    public override float getFloat(int id, string title)
    {
        int idx = getIdxByTemplateID(id);
        if (idx == -1)
        {
            Debug.Log("getTemplate  " + idx + "  error");
            return -1f;
        }
        int num = getTypeNum(title);
        return float.Parse(_DataArray[idx, num]);
    }
    // "key1","key2","key3"
    private string[] _AllKey = { };
    // {"10","12","11"}
    private string[,] _DataArray = { { } };
    public Dictionary<int, string> data = new Dictionary<int, string>();

    public void readData()
    {
        string file = "Assets/Game/Json/" + this.ToString() + ".json";
        TextAsset textObj = Resources.LoadAssetAtPath(file, typeof(TextAsset)) as TextAsset;
        JSONNode data = JSON.Parse(textObj.text);

        JSONClass obj = data.AsObject;
        JSONClass sub = obj[0] as JSONClass;
        if (obj == null || sub == null)
        {
            return;
        }
        int l = obj.Count;
        int h = sub.Count;
        _DataArray = null;
        _DataArray = new string[l, h + 1];
        _AllKey = new string[h + 1];
        _AllKey[0] = "templateID";

        for (int j = 0; j < sub.Count; j++)
        {
            _AllKey[j + 1] = sub.AsObject.key(j);
        }


        for (int i = 0; i < obj.Count; i++)
        {
            _DataArray[i, 0] = obj.key(i);

            JSONClass subObj = obj[i] as JSONClass;
            for (int j = 0; j < subObj.Count; j++)
            {
                _DataArray[i, j + 1] = subObj[j].ToString();
            }
        }

        initDict();
        //		print ();
    }
    public void readData(string filePath)
    {
        string file = filePath;
        TextAsset textObj = Resources.LoadAssetAtPath(file, typeof(TextAsset)) as TextAsset;
        JSONNode data = JSON.Parse(textObj.text);

        JSONClass obj = data.AsObject;
        JSONClass sub = obj[0] as JSONClass;
        if (obj == null || sub == null)
        {
            return;
        }
        int l = obj.Count;
        int h = sub.Count;
        _DataArray = null;
        _DataArray = new string[l, h + 1];
        _AllKey = new string[h + 1];
        _AllKey[0] = "templateID";

        for (int j = 0; j < sub.Count; j++)
        {
            _AllKey[j + 1] = sub.AsObject.key(j);
        }


        for (int i = 0; i < obj.Count; i++)
        {
            _DataArray[i, 0] = obj.key(i);

            JSONClass subObj = obj[i] as JSONClass;
            for (int j = 0; j < subObj.Count; j++)
            {
                _DataArray[i, j + 1] = subObj[j].ToString();
            }
        }

        initDict();
        //		print ();
    }
    //打印
    public override void print()
    {

        int row = _DataArray.GetLength(0);
        int column = _DataArray.GetLength(1);

        string key = "";
        for (int i = 0; i < column; i++)
        {
            key += _AllKey[i] + " ";
        }
        Debug.Log(key);
        for (int i = 0; i < row; i++)
        {
            string printData = i + "row: ";
            for (int j = 0; j < column; j++)
            {
                printData += (_DataArray[i, j] + " ");
            }
            Debug.Log(printData);
        }

    }

    //获取所有的Key
    public override string[] getKeyArray()
    {
        return _AllKey;
    }

    //获取所有的Data
    public override string[,] getDataArray()
    {
        return _DataArray;
    }

    public override int num()
    {
        return _DataArray.GetLength(0);
    }

    public override int keynum()
    {
        return _AllKey.Length;
    }

    //通过type获取num标识 
    private int getTypeNum(string typeName)
    {
        for (int i = 0; i < keynum(); i++)
        {
            if (_AllKey[i] == typeName)
            {
                return i;
            }
        }
        return -1;
    }

    public bool hasKey(string key)
    {
        for (int i = 0; i < keynum(); i++)
        {
            if (_AllKey[i] == key)
            {
                return true;
            }
        }
        return false;
    }
}