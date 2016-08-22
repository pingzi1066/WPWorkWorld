using System.Reflection;
using System;

public class ModelBase<T> where T : StaticJson<T>, new()
{
    protected static T mDatas;
    protected static T datas
    {
        get
        {
            if (mDatas == null)
            {
                // static method use this: 
//                Type staticJsonType = typeof(StaticJson<>).MakeGenericType(typeof(T));
//
//                MethodInfo method = staticJsonType.GetMethod("GetInstance");
//
//                object obj = method.Invoke(null, null);
//
//				mDatas = obj as T;

				mDatas = StaticJson<T>.Instance();
            }
            return mDatas;
        }
    }

    public int templateID { get; protected set; }

    /// <summary>
    /// 名字 name
    /// </summary>
    public string name { get { return datas.GetStr(templateID, "name"); } }
    /// <summary>
    /// 描述 describe
    /// </summary>
    public string desc { get { return datas.GetStr(templateID, "describe"); } }
    /// <summary>
    /// 图标 icon 常用于UI
    /// </summary>
    public string icon { get { return datas.GetStr(templateID, "icon"); } }
    /// <summary>
    /// 资源名字 resname
    /// </summary>
    public string resname { get { return datas.GetStr(templateID, "resname"); } }
    /// <summary>
    /// 内容 content
    /// </summary>
    public string content { get { return datas.GetStr(templateID, "content"); } }

    public float this[string name]
    {
        get
        {
            return datas.GetFloat(templateID, name);
        }
    }

    public int GetInt(string name) { return datas.GetInt(templateID, name); }

    public float GetFloat(string name) { return datas.GetFloat(templateID, name); }

    public string GetStr(string name) { return datas.GetStr(templateID, name); }

    public virtual void SetTemplateID(int id)
    {
        templateID = id;
    }

    static private ModelBase<T> mInstance;
    static public ModelBase<T> GetData(int id)
    {
        if (mInstance == null)
        {
            mInstance = new ModelBase<T>();
            mInstance.templateID = id;
        }
        else
            mInstance.templateID = id;

        return mInstance;
    }
}