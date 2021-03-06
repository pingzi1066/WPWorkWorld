using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;

public abstract class StaticJson<T> where T : StaticJson<T>, new()
{
    private volatile static T _instance = null;
    private static readonly object lockHelper = new object();

    public static T Instance()
    {
        if (_instance == null)
        {
            lock (lockHelper)
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.ReadData();
                    _instance.Init();
                }
            }
        }
        return _instance;
    }

    /// <summary>
    /// 在读完数据时调用
    /// </summary>
    protected void Init()
    {

    }

    //all datas array , [indexById,indexByKey]
    protected string[,] _DataArray { private set; get; }
    //templateID to index
    protected Dictionary<int, int> indexById = new Dictionary<int, int>();
    //key to index
    protected Dictionary<string, int> indexByKey = new Dictionary<string, int>();

    public int[] allID { get { return indexById.Keys.ToArray(); } }

    /// <summary>
    /// 数组总长度
    /// </summary>
    public int Count { get { return indexById.Count; } }

    //get row index for array by templateId
    protected int GetIdxByTemplateID(int templateID)
    {
        if (indexById.ContainsKey(templateID))
        {
            return indexById[templateID];
        }
        return -1;
    }

    //get index for array by key name
    protected virtual int GetIdxByKey(string keyName)
    {
        if (indexByKey.ContainsKey(keyName))
        {
            return indexByKey[keyName];
        }
        return -1;
    }

    public virtual string GetStr(int id, string title)
    {
        string value = GetValue(id, title);
        return value;
    }

    //转换get的类型为int返回
    public virtual int GetInt(int id, string title)
    {
        string value = GetValue(id, title);
        if (string.IsNullOrEmpty(value)) { return -1; }

        return int.Parse(value);
    }

    //转换get的类型为float返回
    public virtual float GetFloat(int id, string title)
    {
        string value = GetValue(id, title);
        if (string.IsNullOrEmpty(value)) { return -1; }

        return float.Parse(value);
    }

    protected virtual string GetValue(int id, string title)
    {
        int idx = GetIdxByTemplateID(id);
        if (idx == -1)
        {
            Debug.LogWarning(this.GetType().Name + "-------get error,has't id: " + id);
            return "";
        }
        int num = GetIdxByKey(title);
        if (num == -1)
        {
            Debug.LogWarning(this.GetType().Name + "-------get error,has't key: " + title);
            return "";
        }
        return _DataArray[idx, num];
    }

    protected virtual void ReadData()
    {
        string fileName = "Json/" + this.ToString();

        Object file = Resources.Load(fileName);

        if (file == null)
        {
            Debug.LogError(fileName + "   is null");
            return;
        }

        TextAsset textObj = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
        JSONNode data = JSON.Parse(textObj.text);

        JSONClass obj = data.AsObject;
        JSONClass sub = obj[0] as JSONClass;
        if (obj == null || sub == null)
        {
            Debug.LogError(" obj or sub is null");
            return;
        }
        int l = obj.Count;
        int h = sub.Count;

        _DataArray = new string[l, h + 1];
        indexByKey.Add("templateID", 0);

        for (int j = 0; j < sub.Count; j++)
        {
            indexByKey.Add(sub.AsObject.GetKey(j), j + 1);
        }

        for (int i = 0; i < obj.Count; i++)
        {
            if (obj.GetKey(i) == "-1")
            {
                continue;
            }
            _DataArray[i, 0] = obj.GetKey(i);

            JSONClass subObj = obj[i] as JSONClass;
            for (int j = 0; j < subObj.Count; j++)
            {
                _DataArray[i, j + 1] = subObj[j].Value;
            }
        }


        int num = _DataArray.GetLength(0);
        indexById.Clear();
        for (int i = 0; i < num; i++)
        {
            int id = System.Int32.Parse(_DataArray[i, 0]);
            indexById.Add(id, i);
        }
    }

    //打印
    public virtual string Print()
    {
        int row = _DataArray.GetLength(0);
        int column = _DataArray.GetLength(1);

        string key = "";
        foreach (string k in indexByKey.Keys)
        {
            key += k + " ";
        }
        //Debug.Log(key);

        string printData = "";
        for (int i = 0; i < row; i++)
        {
            printData += "\n";
            printData += i + " :";
            for (int j = 0; j < column; j++)
            {
                printData += (_DataArray[i, j] + " ");
            }
        }

        string text = key + printData;

        Debug.Log(text);
        return text;
    }
}