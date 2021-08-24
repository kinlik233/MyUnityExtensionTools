using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class SqlAccess
{
    public MySqlConnection mysqlconn;

    public void OpenMysql(string sever, string port, string user, string password, string database)
    {
        try
        {
            string connstr = string.Format("server={0};port={1};user={2};password={3};database={4};", sever, port, user, password, database);
            mysqlconn = new MySqlConnection(connstr);
            mysqlconn.Open();
            Debug.Log("数据库" + database + "连接成功");
        }
        catch (Exception ex)
        {
            Debug.Log("数据库" + database + "连接错误" + ex.ToString());
        }
    }
    public void CloseMysql()
    {
        if (mysqlconn != null)
        {
            Debug.Log("数据库"+mysqlconn.Database+"关闭");
            mysqlconn.Close();
            mysqlconn = null;
        }
    }
    
    //通用mysql语句,可执行增删改查，返回执行后的结果dataset
    public DataSet CommonMysqlQuery(string sqlString)
    {
        if (mysqlconn.State == ConnectionState.Open)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(sqlString, mysqlconn);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            return ds;
        }
        return null;
    }

    //通用mysql查询，返回查询结果MysqlDataReader对象
    public MySqlDataReader CommonMysqlSelect(string sqlString)
    {
        if(mysqlconn.State==ConnectionState.Open)
        {
            MySqlCommand cmd = new MySqlCommand(sqlString, mysqlconn);
            MySqlDataReader data = cmd.ExecuteReader();
            //while (data.Read()) { }在条件语句中读取
            return data;
        }
        return null;
    }
    
    //通用Mysql增删改
    public void CommonMysqlCUD(string sqlString)
    {
        if(mysqlconn.State==ConnectionState.Open)
        {
            MySqlCommand cmd = new MySqlCommand(sqlString,mysqlconn);
            cmd.ExecuteNonQuery();
        }
    }
}
