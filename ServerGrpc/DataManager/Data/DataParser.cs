using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DataManager.Data
{
    public class DataParser
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public DataParser()
        {

        }

        public bool ParserDataFile(Dictionary<string, DataTable> dataMap)
        {
            var outPath = Directory.GetCurrentDirectory() + DataUtils.DATA_OUTPUT_PATH;
            foreach (var d in dataMap)
            {
                var name = d.Key;
                var data = d.Value;
                _builder.Clear();

                if (Directory.Exists(outPath) == false)
                {
                    Directory.CreateDirectory(outPath);
                }
                var outputFile = File.CreateText(outPath + name + ".txt");
                ParserData(name, data);
                outputFile.Write(_builder);
                outputFile.Close();
            }
            return true;
        }

        private void ParserData(string name, DataTable data)
        {
            var r_count = data.Rows.Count;
            var c_count = data.Columns.Count;
            for (int r = 1; r < r_count; ++r)
            {
                // 0 : type
                // 1 : name
                for (int c = 0; c < c_count; ++c)
                {
                    var val = data.Rows[r][c].ToString();
                    _builder.Append(val);
                    if (c != c_count - 1)
                    {
                        _builder.Append(',');
                    }
                }
                _builder.AppendLine();
            }
        }

        // test
        public T FileReadTest<T>() where T : class, new()
        {
            var outPath = Directory.GetCurrentDirectory() + DataUtils.DATA_OUTPUT_PATH;
            var dirInfo = new DirectoryInfo(outPath);
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
                        var v = DataUtils.GetValue(val, rowProp.PropertyType);
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
    }
}
