using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

public class ResourceHelper : Singleton<ResourceHelper>
{
    public static Dictionary<string, UnityEngine.Object> CachedResources = new Dictionary<string, UnityEngine.Object>();
    

    protected static IEnumerator LoadAsyncInternal(string path, Action<WWW> onLoad)
    {
        WWW www = new WWW(LocalToURLPath(path));
        yield return www;
        onLoad(www);
    }

    public static T LoadCached<T>(string filePath) where T : UnityEngine.Object
    {
        UnityEngine.Object obj = null;
        if (!CachedResources.TryGetValue(filePath, out obj))
        {
            obj = Resources.Load<T>(filePath);
            CachedResources.Add(filePath, obj);
        }
        
        return obj as T;
    }

    public static void LoadAsync(string filePath, Action<WWW> onLoad)
    {
        
       Instance.StartCoroutine(LoadAsyncInternal(filePath, onLoad));
    }

    public static void LoadFromStreamingAssetsAsync(string file, Action<WWW> onLoad)
    {
        LoadAsync(GetStreamingAssetsPath(file), onLoad);
    }

    public static void LoadFromPersistentDataAsync(string file, Action<WWW> onLoad)
    {
        LoadAsync(GetPersistentDataPath(file), onLoad);
    }

    public static string Read(string path)
    {
        return File.ReadAllText(path);
    }

    public static T Read<T>(string path)
    {
        return DecodeObject<T>(Read(path));
    }

    public static string ReadFromPersistentData(string file)
    {
        return Read(GetPersistentDataPath(file));
    }

    public static T ReadFromPersistentData<T>(string file)
    {
        return Read<T>(GetPersistentDataPath(file));
    }

    public static string ReadFromStreamingAssets(string file)
    {
        return Read(GetStreamingAssetsPath(file));
    }

    public static T ReadFromStreamingAssets<T>(string file)
    {
        return Read<T>(GetStreamingAssetsPath(file));
    }

    public static void ReadAsync(string path, Action<string> onRead)
    {
        LoadAsync(path, (WWW www) =>
        {
            onRead(www.text);
        });
    }

    public static void ReadAsync<T>(string path, Action<T> onRead)
    {
        ReadAsync(path, (string content) =>
        {
            onRead(DecodeObject<T>(content));
        });
    }

    public static void ReadFromPersistentDataAsync(string file, Action<string> onRead)
    {
        ReadAsync(GetPersistentDataPath(file), onRead);
    }

    public static void ReadFromPersistentDataAsync<T>(string file, Action<T> onRead)
    {
        ReadAsync<T>(GetPersistentDataPath(file), onRead);
    }

    public static void ReadFromStreamingAssetsAsync(string file, Action<string> onRead)
    {
        ReadAsync(GetStreamingAssetsPath(file), onRead);
    }

    public static void ReadFromStreamingAssetsAsync<T>(string file, Action<T> onRead)
    {
        ReadAsync<T>(GetStreamingAssetsPath(file), onRead);
    }

    public static void Save(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }

    public static void Save(string path, object contents)
    {
        Save(path, EncodeObject(contents));
    }

    public static void SaveToPersistentData(string file, string contents)
    {
        Save(GetPersistentDataPath(file), contents);
    }

    public static void SaveToPersistentData(string file, object contents)
    {
        Save(GetPersistentDataPath(file), contents);
    }
    
    public static void SaveToResourcesData(string file, string contents)
    {
        Save(GetResourcesDataPath(file), contents);
    }

    public static void SaveToResourcesData(string file, object contents)
    {
        Save(GetResourcesDataPath(file), contents);
    }

    public static void SaveToStreamingAssets(string file, string contents)
    {
        Save(GetStreamingAssetsPath(file), contents);
    }

    public static void SaveToStreamingAssets(string file, object contents)
    {
        Save(GetStreamingAssetsPath(file), contents);
    }

    public static bool Exists(string path)
    {
        return File.Exists(path);
    }

    public static bool ExistsInPersistentData(string file)
    {
        return Exists(GetPersistentDataPath(file));
    }

    public static bool ExistsInStreamingAssets(string file)
    {
        return Exists(GetStreamingAssetsPath(file));
    }

    public static void Delete(string path)
    {
        File.Delete(path);
    }

    public static void DeleteFromPersistentData(string file)
    {
        Delete(GetPersistentDataPath(file));
    }

    public static void DeleteFromStreamingAssets(string file)
    {
        Delete(GetStreamingAssetsPath(file));
    }

    public static string GetPersistentDataPath(string file)
    {
        return Path.Combine(Application.persistentDataPath, file);
    }

    public static string GetResourcesDataPath(string file)
    {
        return Path.Combine(Application.dataPath+ "/Resources", file);
    }

    public static string GetStreamingAssetsPath(string file)
    {
        return Path.Combine(Application.streamingAssetsPath, file);
    }

    public static string LocalToURLPath(string path)
    {
        if (!path.Contains("file://"))
            path = "file://" + path;
        return path;
    }

    public static T DecodeObject<T>(string encoded)
    {
        return JsonConvert.DeserializeObject<T>(encoded);
    }

    public static string EncodeObject(object data)
    {
        return JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
    }
    private static string hash = "123987@!abc";
    public static string Encrypt(string input){
        byte[]data = UTF8Encoding.UTF8.GetBytes(input);
		using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider()){
			byte[]key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
			using(TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider(){Key=key,Mode=CipherMode.ECB,Padding = PaddingMode.PKCS7}){
				ICryptoTransform tr = trip.CreateEncryptor();
				byte[]results = tr.TransformFinalBlock(data,0,data.Length);
				return Convert.ToBase64String(results,0,results.Length);
			}
		}
    }
}