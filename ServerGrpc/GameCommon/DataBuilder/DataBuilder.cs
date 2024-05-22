using GameDataTables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameCommon.DataBuilder
{
    public class DataBuilder
    {
        private readonly string _path = string.Empty;
        private GameDataManager _dataManager;

        public DataBuilder(string path)
        {
            _path = path;
        }

        public GameDataManager GetData()
        {
            if (_dataManager == null)
            {
                throw new ArgumentException("build data first");
            }
            return _dataManager;
        }

        public void BuildData()
        {
            try
            {
                _dataManager = DataBuild<GameDataManager>();
            }
            catch (Exception e)
            {
                throw new Exception($"data builder err: {e} -- inner: {e.InnerException}");
            }
        }

        private T DataBuild<T>() where T : class, new()
        {
            var dataPath = _path;
            var dirInfo = new DirectoryInfo(dataPath);
            var fileMap = dirInfo.GetFiles("*.txt").ToDictionary(x => Path.GetFileNameWithoutExtension(x.Name), x => x);

            Type type = typeof(T);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var inst = new T();

            foreach (var prop in properties)
            {
                var dataTableInst = Activator.CreateInstance(prop.PropertyType);
                var dataTableType = dataTableInst.GetType();
                var dataTableProps = dataTableType.GetProperties();

                fileMap.TryGetValue(prop.PropertyType.Name, out var data);
                if (data == null)
                {
                    continue;
                }

                var lineList = new List<string>();
                using var sr = data.OpenText();
                while (sr.EndOfStream == false)
                {
                    var lines = sr.ReadLine();
                    lineList.Add(lines);
                }
                sr.Close();

                if (lineList.Any() == false || lineList.Count <= 1)
                {
                    continue;
                }

                var nameList = lineList.First();
                var mapProp = dataTableProps.First();
                var dataType = mapProp.PropertyType;
                var constructedMap = typeof(Dictionary<,>).MakeGenericType(dataType.GenericTypeArguments);
                var realMap = (IDictionary)Activator.CreateInstance(constructedMap);
                var keyType = dataType.GenericTypeArguments[0];
                var rowType = dataType.GenericTypeArguments[1];

                for (int i = 1; i < lineList.Count; ++i)
                {
                    var row = Activator.CreateInstance(rowType);
                    var rowList = lineList[i].Split(',');
                    int index = 0;
                    foreach (var rowProp in rowType.GetProperties())
                    {
                        var val = rowList.ElementAt(index);
                        var v = GetValue(val, rowProp.PropertyType);
                        rowProp.GetSetMethod(true).Invoke(row, new object[] { v });
                        ++index;
                    }
                    int keyVal = int.Parse(rowList.First());
                    realMap.Add(keyVal, row);
                }

                mapProp.GetSetMethod(true).Invoke(dataTableInst, new object[] { realMap });
                prop.GetSetMethod(true).Invoke(inst, new object[] { dataTableInst });
            }
            return inst;
        }

        public static object GetValue(string value, Type dataPropType)
        {
            if (dataPropType.IsEnum)
            {
                return Enum.Parse(dataPropType, value);
            }

            switch (Type.GetTypeCode(dataPropType))
            {
                case TypeCode.Boolean:
                    if (bool.TryParse(value, out bool vb))
                    {
                        return vb;
                    }

                    return null;
                case TypeCode.Byte:
                    if (byte.TryParse(value, out byte bytev))
                    {
                        return bytev;
                    }

                    return null;
                case TypeCode.SByte:
                    if (sbyte.TryParse(value, out sbyte vsb))
                    {
                        return vsb;
                    }

                    return null;
                case TypeCode.UInt16:
                    if (ushort.TryParse(value, out ushort vs))
                    {
                        return vs;
                    }

                    return null;
                case TypeCode.UInt32:
                    if (uint.TryParse(value, out uint vui))
                    {
                        return vui;
                    }

                    return null;
                case TypeCode.UInt64:
                    if (ulong.TryParse(value, out ulong vul))
                    {
                        return vul;
                    }

                    return null;
                case TypeCode.Int16:
                    if (short.TryParse(value, out short vst))
                    {
                        return vst;
                    }

                    return null;
                case TypeCode.Int32:
                    if (int.TryParse(value, out int vi))
                    {
                        return vi;
                    }

                    return null;
                case TypeCode.Int64:
                    if (long.TryParse(value, out long vlo))
                    {
                        return vlo;
                    }

                    return null;
                case TypeCode.Decimal:
                    if (decimal.TryParse(value, out decimal vdi))
                    {
                        return vdi;
                    }

                    return null;
                case TypeCode.Double:
                    if (double.TryParse(value, out double vd))
                    {
                        return vd;
                    }

                    return null;
                case TypeCode.Single:
                    if (float.TryParse(value, out float vf))
                    {
                        return vf;
                    }

                    return null;
                case TypeCode.DateTime:
                    if (DateTime.TryParse(value, out DateTime dt))
                    {
                        return dt;
                    }

                    return null;
                default:
                    return value;
            }
        }
    }
}
