using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public static class DataUtils
    {
        public enum ExcelDataType
        {
            None = 0,
            Bool,       // bool
            Int32,      // int
            Int64,      // long
            Array,      // array
            String,     // string
        }

        public const string EXCEL_FILE = "*.xlsx";
        public const string DEFAULT_PATH = "/../../../Data";
        public const string DATA_OUTPUT_PATH = "/Output/";
        public const string TABLE_OUTPUT_PATH = "/DataTable/";

        public const string DATA_FILE_NAME = "DATA_";
        public const string CONST_FILE_NAME = "DATA_CONST_";

        public const string MAP_FILE_PATH = "/Map";
        public const string MAP_INFO_PARSER_FILE = "/DATA_ZONE_INFO.txt";

        public static string GetFilePath(string path)
        {
            if (string.IsNullOrEmpty(path) == true)
            {
#if DEBUG
                return Directory.GetCurrentDirectory() + DEFAULT_PATH;
#else
                return Directory.GetCurrentDirectory();
#endif
            }
            return path;
        }

        public static string ConvertType(string typeStr)
        {
            var str = typeStr.ToLower();
            switch (str)
            {
                case "int":
                case "int32":
                    return "int";
                case "int64":
                case "long":
                    return "long";
                case "bool":
                    return "bool";
                case "int_array":
                case "int32_array":
                    return nameof(List<int>);
                case "int64_array":
                case "long_array":
                    return nameof(List<long>);
                default:
                    return "string";
            }
        }

        public static TypeCode ConvertTypeToCode(string typeStr)
        {
            var str = typeStr.ToLower();
            switch (str)
            {
                case "int":
                case "int32":
                case "int_array":
                case "int32_array":
                    return TypeCode.Int32;
                case "int64":
                case "long":
                case "int64_array":
                case "long_array":
                    return TypeCode.Int64;
                case "bool":
                    return TypeCode.Boolean;
                default:
                    return TypeCode.String;
            }
        }

        public static ExcelDataType ConvertTypeToExcelDataType(string type)
        {
            var typeStr = type.ToLower();
            switch (typeStr)
            {
                case "bool":
                    return ExcelDataType.Bool;
                case "byte":
                case "int":
                case "int32":
                case "uint":
                case "uint32":
                    return ExcelDataType.Int32;
                case "long":
                case "ulong":
                case "int64":
                case "uint64":
                    return ExcelDataType.Int64;
                case "string":
                    return ExcelDataType.String;
                case "int32_array":
                case "int64_array":
                    return ExcelDataType.Array;
                default:
                    return ExcelDataType.None;
            }
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
