using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRestful : MonoBehaviour
{
    private static HttpRestful _instance;
    public static HttpRestful Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gb = new GameObject("HttpRestful");
                _instance = gb.AddComponent<HttpRestful>();
            }
            return _instance;
        }
    }
    /// <summary>
    /// get请求
    /// </summary>
    /// <param name="url">路径</param>
    /// <param name="name">数据名称，用于条件判断</param>
    /// <param name="actionResult">是否成功，返回的数据，数据名称</param>
    public void Get(string url,string name,Action<bool,string,string> actionResult=null)
    {
        StartCoroutine(_Get(url, name, actionResult));
    }
    private IEnumerator _Get(string url, string name, Action<bool, string, string> action)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            string resstr = "";
            if (request.isNetworkError || request.isHttpError)
            {
                resstr = request.error;
            }
            else
            {
                resstr = request.downloadHandler.text;
            }

            if (action != null)
            {
                action(request.isHttpError, resstr, name);
            }
        }
    }
    /// <summary>
    /// post请求json格式
    /// </summary>
    /// <param name="url">路径</param>
    /// <param name="data">提交的值</param>
    /// <param name="name">数据名称，用于条件判断</param>
    /// <param name="actionResult">是否成功，返回的数据，数据名称</param>
    public void Post(string url, string data, string name, Action<bool, string, string> actionResult = null)
    {
        StartCoroutine(_Post(url, data, name, actionResult));
    }
    private IEnumerator _Post(string url, string data, string name, Action<bool, string, string> action)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
            request.SetRequestHeader("content-type", "application/json;charset=utf-8");
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            string resstr = "";
            if (request.isNetworkError || request.isHttpError)
            {
                resstr = request.error;
            }
            else
            {
                resstr = request.downloadHandler.text; 
            }
            if (action != null)
            {
                action(request.isHttpError, resstr, name);
            }
        }
    }

    /// <summary>
    /// post请求soap xml格式
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <param name="name"></param>
    /// <param name="actionResult"></param>
    public void PostXml(string url, string data, string name, Action<bool, string, string> actionResult = null)
    {
        StartCoroutine(_PostXml(url, data, name, actionResult));
    }

    private IEnumerator _PostXml(string url, string data, string name, Action<bool, string, string> action)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {


            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
            request.SetRequestHeader("content-type", "application/soap+xml; charset=utf-8");
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            string resstr = "";
            if (request.isNetworkError || request.isHttpError)
            {
                resstr = request.error;
            }
            else
            {
                resstr = request.downloadHandler.text;
            }
            if (action != null)
            {
                action(request.isHttpError, resstr, name);
            }
        }
    }
}
