using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DataManager.GameTable
{
    public class DataTableParser
    {
        public const string GAME_TABLE_NAME_SPACE = "namespace GameDataTables";
        public const string SYSTEM_GENERIC_USING = "using System.Collections.Generic;\r\n";
        public const string DATA_ROW_NAME_FORMAT = "\tpublic partial class {0}_ROW";
        public const string DATA_CLASS_FIELD_FORMAT = "\t\tpublic {0} {1} {{ get; private set; }}";
        public const string DATA_CLASS_NAME_FORMAT = "\tpublic partial class {0}";
        public const string DATAS_CLASS_FORMAT = "\t\tpublic Dictionary<int, {0}_ROW> Datas {{ get; private set; }}";
        public const string GAME_DATA_MANAGER_CLASS = "GameDataManager";

        public const string CONST_GAME_NAME = "DATA_CONST_GAME";
        public const string CONST_STRING_NAME = "DATA_CONST_STRING";

        public const string CONST_CLASS_FORMAT = "\tpublic class {0}_DATA";
        public const string CONST_FIELD_FORMAT = "\t\tpublic const {0} {1} = {2};";

        private readonly Dictionary<string, DataTable> _dataMap;
        private readonly Dictionary<string, DataTable> _constMap;
        public DataTableParser(Dictionary<string, DataTable> dataMap, Dictionary<string, DataTable> constMap)
        {
            _dataMap = dataMap;
            _constMap = constMap;
        }

        public bool Parser()
        {
            var outPath = Directory.GetCurrentDirectory() + DataUtils.TABLE_OUTPUT_PATH;
            if (Directory.Exists(outPath) == false)
            {
                Directory.CreateDirectory(outPath);
            }

            ParserGameDataManager(outPath);
            ParserDataTable(outPath);
            ParserConstTable(outPath);

            return true;
        }

        private bool ParserGameDataManager(string outPath)
        {
            var sb = new StringBuilder();

            sb.AppendLine(GAME_TABLE_NAME_SPACE);
            sb.AppendLine("{");

            sb.AppendLine(string.Format(DATA_CLASS_NAME_FORMAT, GAME_DATA_MANAGER_CLASS));
            sb.AppendLine("\t{");

            foreach (var d in _dataMap)
            {
                var name = MakeGameDataFieldName(d.Key);
                sb.AppendLine(string.Format(DATA_CLASS_FIELD_FORMAT, d.Key, name));
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            WriteFile(outPath + GAME_DATA_MANAGER_CLASS + ".cs", sb.ToString());
            return true;
        }

        private bool ParserDataTable(string outPath)
        {
            if (_dataMap == null || _dataMap.Any() == false)
            {
                return false;
            }

            var sb = new StringBuilder();
            foreach (var d in _dataMap)
            {
                sb.Clear();
                var className = d.Key;

                var fieldTypeRows = d.Value.Rows[0];
                var fieldNameRows = d.Value.Rows[1];

                int index = 0;

                sb.AppendLine(SYSTEM_GENERIC_USING);
                sb.AppendLine();
                sb.AppendLine(GAME_TABLE_NAME_SPACE);
                sb.AppendLine("{");

                sb.AppendLine(string.Format(DATA_ROW_NAME_FORMAT, className));
                sb.AppendLine("\t{");
                foreach (var f in fieldTypeRows.ItemArray)
                {
                    var typeStr = DataUtils.ConvertType(f.ToString());
                    var nameStr = fieldNameRows.ItemArray[index].ToString();
                    sb.AppendLine(string.Format(DATA_CLASS_FIELD_FORMAT, typeStr, nameStr));
                    ++index;
                }
                sb.AppendLine("\t}");

                sb.AppendLine();
                sb.AppendLine(string.Format(DATA_CLASS_NAME_FORMAT, className));
                sb.AppendLine("\t{");
                sb.AppendLine(string.Format(DATAS_CLASS_FORMAT, className));
                sb.AppendLine("\t}");
                sb.AppendLine("}");

                WriteFile(outPath + className + ".cs", sb.ToString());
                //var file = File.Create(outPath + fileName + ".cs");
                //var bytes = Encoding.ASCII.GetBytes(sb.ToString());
                //file.Write(bytes);
                //file.Close();
            }
            return true;
        }

        private bool ParserConstTable(string outPath)
        {
            if (_constMap == null || _constMap.Any() == false)
            {
                return false;
            }

            var sb = new StringBuilder();
            foreach (var d in _constMap)
            {
                sb.Clear();

                sb.AppendLine(GAME_TABLE_NAME_SPACE);
                sb.AppendLine("{");

                var className = d.Key.Replace(DataUtils.DATA_FILE_NAME, "");
                var type = (d.Key.Equals(CONST_GAME_NAME)) ? typeof(int) : typeof(string);

                sb.AppendLine(string.Format(CONST_CLASS_FORMAT, className));
                sb.AppendLine("\t{");

                for (int i = 1; i < d.Value.Rows.Count; i++)
                {
                    var row = d.Value.Rows[i];
                    var fieldName = row.ItemArray[1].ToString();

                    //var val = DataUtils.GetValue(row.ItemArray[2].ToString(), type).ToString();
                    var val = row.ItemArray[2].ToString();
                    var typeStr = DataUtils.ConvertType(Type.GetTypeCode(type).ToString());
                    if (typeStr.Equals("string") == true)
                    {
                        val = $"\"{val}\"";
                    }
                    sb.AppendLine(string.Format(CONST_FIELD_FORMAT, typeStr, fieldName, val));
                }

                sb.AppendLine("\t}");
                sb.AppendLine("}");

                WriteFile(outPath + d.Key + ".cs", sb.ToString());
            }
            return true;
        }

        private string MakeGameDataFieldName(string key)
        {
            if (string.IsNullOrEmpty(key) == true)
            {
                return string.Empty;
            }

            var temp = key.Replace(DataUtils.DATA_FILE_NAME, "");
            var list = temp.Split('_');
            if (list.Any() == false)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var l in list)
            {
                if (string.IsNullOrEmpty(l) == true)
                {
                    continue;
                }

                sb.Append(l.Substring(0, 1) + l.Substring(1).ToLower());
            }
            return sb.ToString();
        }

        private void WriteFile(string path, string contents)
        {
            var file = File.Create(path);
            var bytes = Encoding.UTF8.GetBytes(contents);
            file.Write(bytes);
            file.Close();
        }
    }
}
