using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.IO;

namespace KAINamespace
{
    public class KAIBaseConfig<T> where T : KAIBaseConfigRecord, new()
    {
        public List<T> records = new List<T>();
        public Dictionary<object, T> dictRecords = new Dictionary<object, T>();

        public KAIBaseConfig()
        {
            records.Clear();
            dictRecords.Clear();
        }

        public virtual void LoadConfigFromTextAsset(string path)
        {
            TextAsset data = Resources.Load<TextAsset>(path);
            if (data == null)
            {
                Debug.LogWarning("Resource Not Found");
                return;
            }
            string[] lines = data.text.Split('\n');
            if (lines == null || lines.Length <= 0)
                return;
            //Ignore Line 0
            for (int i = 1; i < lines.Length; i++)
            {
                string[] col = lines[i].Split('\t');
                T configRecord = new T();
                FieldInfo[] fields = configRecord.GetType().GetFields();
                if (fields.Length < col.Length)
                {
                    Debug.LogError(string.Format("Field Length: {0} Col Length: {1}", fields.Length, col.Length));
                    break;
                }
                for (int j = 0; j < fields.Length; j++)
                {
                    try
                    {
                        fields[j].SetValue(configRecord, Convert.ChangeType(col[j], fields[j].FieldType));
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(string.Format("Failed to convert line: {0} col: {1} ", i, j));
                    }
                }
                records.Add(configRecord);
            }

            for (int i = 0; i < records.Count; i++)
            {
                records[i].GetDum();
            }
            Debug.Log("Load Config Thanh Cong: " + records.Count +" records") ;
        }

        public virtual void ReadFileFromPath(string path)
        {
            if (File.Exists(path) == false)
                return;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    records.Clear();
                    int i = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 0)
                        {
                            i++;
                            continue;
                        }
                        Debug.Log("Line: " + line + " i: " + i);
                        string[] col = line.Split('\t');
                        T configRecord = new T();
                        FieldInfo[] fields = configRecord.GetType().GetFields();
                        if (fields.Length < col.Length)
                        {
                            Debug.LogError(string.Format("Field Length: {0} Col Length: {1}", fields.Length, col.Length));
                            break;
                        }
                        for (int j = 0; j < fields.Length; j++)
                        {
                            try
                            {
                                fields[j].SetValue(configRecord, Convert.ChangeType(col[j], fields[j].FieldType));
                            }
                            catch (Exception ex)
                            {
                                Debug.LogError(string.Format("Failed to convert line: {0} col: {1} ", i, j));
                            }
                        }
                        records.Add(configRecord);
                        i++;
                    }

                    for (i = 0; i < records.Count; i++)
                    {
                        records[i].GetDum();
                    }
                    //Debug.Log("Load Config Thanh Cong: " + records.Count + " records");
                }
            }
            catch (Exception e)
            {
                // thong bao loi.
                Debug.Log("Khong the doc du lieu tu file da cho: ");
            }

        }

        public void RebuidConfig(string fieldName)
        {
            for (int i = 0; i < records.Count; i++)
            {
                FieldInfo fieldInfo = records[i].GetType().GetField(fieldName);
                if (fieldInfo != null)
                    dictRecords[fieldInfo.GetValue(records[i])] = records[i];
            }
        }

        public T GetConfig<U>(U param)
        {
            if (dictRecords.ContainsKey(param))
                return dictRecords[param];
            return null;
        }
    }

}