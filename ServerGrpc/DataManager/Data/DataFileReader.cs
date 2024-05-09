using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Data
{
    public class DataFileReader
    {
        private readonly string _filePath;
        private readonly ExcelReaderConfiguration _confExcel;

        private readonly Dictionary<string, DataTable> _dataFileMap = new Dictionary<string, DataTable>();
        private readonly Dictionary<string, DataTable> _constFileMap = new Dictionary<string, DataTable>();


        public DataFileReader(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _confExcel = new ExcelReaderConfiguration()
            {
                FallbackEncoding = Encoding.GetEncoding(1252),
            };

            _filePath = DataUtils.GetFilePath(path);
        }

        public Dictionary<string, DataTable> DataFileMap => _dataFileMap;
        public Dictionary<string, DataTable> ConstFileMap => _constFileMap;

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
    }
}
