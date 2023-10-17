using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MyUnityExtensionTools
{
	/// <summary>
	/// 资源管理器
	/// </summary>
	public class ResourceManager
	{
        //资源映射表的数据结构
        private static Dictionary<string, string> configMap;
 
        static ResourceManager()
        {
            configMap = new Dictionary<string, string>();

            //读取配置文件 string
            string configFile = GetConfigFile("ResourceMap.txt");
            //形成数据结构
            BuildMap(configFile);
        }

        private static string GetConfigFile(string fileName)
        {
            string configPath = Application.streamingAssetsPath + "/" + fileName;
    
            if (Application.platform != RuntimePlatform.Android)
            {
                configPath = "file://" + configPath;
            }

            UnityWebRequest request = UnityWebRequest.Get(configPath);
            request.SendWebRequest();
            while (true)
            {
                if (request.downloadHandler.isDone)//是否读取完数据
                {
                    return request.downloadHandler.text;
                }
            }
        }

        private static void BuildMap(string configFile)
        {
            //字符串读取器：提供仅向前 逐行读取方式。
            StringReader reader = new StringReader(configFile);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] keyValue = line.Split('=');
                configMap.Add(keyValue[0], keyValue[1]); 
            }

            //string line = reader.ReadLine();
            //while (line != null)
            //{
            //    string[] keyValue = line.Split('=');
            //    configMap.Add(keyValue[0], keyValue[1]);
            //    line = reader.ReadLine();
            //}
        }

        /// <summary>
        /// 加载资源(内部根据资源名称，查找对应的路径。)
        /// </summary>
        /// <typeparam name="T">需要加载的数据类型</typeparam>
        /// <param name="resourceName">资源名称</param>
        /// <returns></returns>
        public static T Load<T>(string resourceName) where T : Object
        {
            //从配置文件中获取对应的路径
            string path = configMap[resourceName];
            //通过Resource加载
            return Resources.Load<T>(path);
        }
	}
}
