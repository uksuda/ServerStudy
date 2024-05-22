using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DataManager.Data
{
    public class DataFileReader
    {
        private readonly string _filePath;
        private readonly ExcelReaderConfiguration _confExcel;

        private readonly Dictionary<string, DataTable> _dataFileMap = new Dictionary<string, DataTable>();
        private readonly Dictionary<string, DataTable> _constFileMap = new Dictionary<string, DataTable>();
        private readonly Dictionary<string, DataTable> _mapInfoMap = new Dictionary<string, DataTable>();

        private readonly Dictionary<string, int> _zoneIndex = new Dictionary<string, int>();

        public DataFileReader(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _confExcel = new ExcelReaderConfiguration()
            {
                FallbackEncoding = Encoding.GetEncoding(1252),
            };

            _filePath = DataUtils.GetFilePath(path);

            RegistZoneIndex();
        }

        public Dictionary<string, DataTable> DataFileMap => _dataFileMap;
        public Dictionary<string, DataTable> ConstFileMap => _constFileMap;
        public Dictionary<string, DataTable> MapInfo => _mapInfoMap;

        public bool ReadExcelFiles()
        {
            var dataPath = new DirectoryInfo(_filePath);
            var dataFiles = dataPath.GetFiles(DataUtils.EXCEL_FILE).Where(x => !x.Name.StartsWith("~"));

            foreach (var f in dataFiles)
            {
                using (var s = f.OpenRead())
                {
                    using (var reader = ExcelReaderFactory.CreateReader(s, _confExcel))
                    {
                        var data = reader.AsDataSet().Tables[0];
                        var ff = Path.GetFileNameWithoutExtension(f.Name);
                        if (ff.StartsWith(DataUtils.CONST_FILE_NAME) == true)
                        {
                            _constFileMap.Add(ff, data);
                        }
                        else
                        {
                            _dataFileMap.Add(ff, data);
                        }
                    }
                }
            }
            return true;
        }

        public bool ReadMapExcelFile()
        {
            var mapFilePath = new DirectoryInfo(_filePath + DataUtils.MAP_FILE_PATH);
            var mapFile = mapFilePath.GetFiles(DataUtils.EXCEL_FILE).Where(x => !x.Name.StartsWith("~")).FirstOrDefault();
            if (mapFile == null)
            {
                return false;
            }

            using var s = mapFile.OpenRead();
            using var reader = ExcelReaderFactory.CreateReader(s, _confExcel);
            var dataSet = reader.AsDataSet();
            foreach (var d in dataSet.Tables)
            {
                var dt = d as DataTable;
                if (dt == null)
                {
                    continue;
                }
                var zoneName = dt.TableName;
                _mapInfoMap.Add(zoneName, dt);
            }
            return true;
        }

        public bool ParserZoneInfo(Dictionary<string, DataTable> zoneInfo)
        {
            if (zoneInfo == null || zoneInfo.Any() == false)
            {
                return false;
            }

            var outPath = _filePath + DataUtils.MAP_FILE_PATH;
            var builder = new StringBuilder();
            foreach (var z in zoneInfo)
            {
                builder.AppendLine(z.Key);
                var dataTable = z.Value;
                var maxY = dataTable.Rows.Count;
                var maxX = dataTable.Columns.Count;
                int index = GetZoneStartIndex(z.Key);
                for (int y = 0; y < maxY; ++y)
                {
                    //for (int y = 0; y < maxY; ++y)
                    for (int x = 0; x < maxX; ++x)
                    {
                        var val = dataTable.Rows[y][x];
                        int.TryParse(val.ToString(), out int value);
                        if (value == 0)
                        {
                            continue;
                        }

                        var movable = GetMovableDirection(dataTable.Rows, maxX, maxY, x, y);
                        var infoStr = $"index : {index}. x: {x} y: {maxY - y - 1} move list: {movable}";
                        builder.AppendLine(infoStr);
                        ++index;
                    }
                }
                builder.AppendLine();
            }

            var outputFile = File.CreateText(outPath + DataUtils.MAP_INFO_PARSER_FILE + ".txt");
            outputFile.Write(builder);
            outputFile.Close();
            return true;
        }

        private string GetMovableDirection(DataRowCollection row, int maxX, int maxY, int x, int y)
        {
            var str = new StringBuilder();
            object val = null;
            int value = 0;

            // 1 (west) -- x + 1
            if (maxX - 1 > x)
            {
                val = row[y][x + 1];
                int.TryParse(val.ToString(), out value);
                if (value > 0)
                {
                    str.Append("1;");
                }
            }

            // 2 (east) -- x - 1
            if (x > 0)
            {
                val = row[y][x - 1];
                int.TryParse(val.ToString(), out value);
                if (value > 0)
                {
                    str.Append("2;");
                }
            }

            // 3 (north) -- y - 1
            //if (maxY - 1 > y)
            if (y > 0)
            {
                val = row[y - 1][x];
                int.TryParse(val.ToString(), out value);
                if (value > 0)
                {
                    str.Append("3;");
                }
            }

            // 4 (south) -- y + 1
            //if (y > 0)
            if (maxY - 1 > y)
            {
                val = row[y + 1][x];
                int.TryParse(val.ToString(), out value);
                if (value > 0)
                {
                    str.Append("4;");
                }
            }
            return str.ToString().TrimEnd(';');
        }

        private void RegistZoneIndex()
        {
            // ref type.proto ZoneStartIndex
            _zoneIndex.Add("Tutorial", 1);
            _zoneIndex.Add("Normal", 1001);
        }

        private int GetZoneStartIndex(string zoneName)
        {
            if (_zoneIndex.TryGetValue(zoneName, out int index) == true)
            {
                return index;
            }
            throw new System.ArgumentOutOfRangeException("not registed zone");
        }
    }
}
