using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using System;

public class SqlManager : MonoBehaviour
{
    SqlAccess sqlaccess;

    private void Start()
    {
        sqlaccess = new SqlAccess();
        OpenUserData();
        SelectPersonalData("select * from personal_data");
        CloseMysql();
    }

    public void OpenUserData()
    {
        Dictionary<string, string> mysqlInfo = GetMysqlInfo("/userdata.txt");
        sqlaccess.OpenMysql(mysqlInfo["server"], mysqlInfo["port"], mysqlInfo["user"], mysqlInfo["password"], mysqlInfo["database"]);
    }

    //读取streamingAssets文件夹下的mysql配置txt文件
    //txt文件按照主机，端口，用户名，密码，数据库名，从上到下从五行
    public Dictionary<string, string> GetMysqlInfo(string txtInfo)
    {
        Dictionary<string,string> mysqlInfo = new Dictionary<string, string>();
        using(FileStream fs=new FileStream(Application.streamingAssetsPath+txtInfo,FileMode.Open))
        {
            using(StreamReader sr=new StreamReader(fs))
            {
                mysqlInfo.Add("server",sr.ReadLine());
                mysqlInfo.Add("port",sr.ReadLine());
                mysqlInfo.Add("user",sr.ReadLine());
                mysqlInfo.Add("password",sr.ReadLine());
                mysqlInfo.Add("database",sr.ReadLine());
            }
        }
        return mysqlInfo;
    }

    public List<string[]> SelectPersonalData(string sql)
    {
        List<string[]> data = new List<string[]>();
        try
        {
            DataSet ds = sqlaccess.CommonMysqlQuery(sql);
            if(ds.Tables[0].Rows.Count>0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    string[] str = new string[] { "", "" };
                    str[0] = row["name"].ToString();
                    str[1] = row["password"].ToString();
                    data.Add(str);
                }
            }    
        }
        catch (Exception ex)
        {

            Debug.LogError(ex.Message);
        }
        foreach (var item in data)
        {
            Debug.Log(item[0]);
            Debug.Log(item[1]);
        }
        return data;
    }
    public void CloseMysql()
    {
        sqlaccess.CloseMysql();
    }
}
