using DataManager.Data;
using DataManager.GameTable;
using GameDataTables;
using System;

namespace DataManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = (args.Length > 0) ? args[0] : string.Empty;
            var reader = new DataFileReader(path);

            reader.ReadMapExcelFile();
            reader.ParserZoneInfo(reader.MapInfo);
            return;

            reader.ReadExcelFiles();

            var parser = new DataParser();
            parser.ParserDataFile(reader.DataFileMap);

            var tableParser = new DataTableParser(reader.DataFileMap, reader.ConstFileMap);
            tableParser.Parser();
            // test
            var manager = parser.FileReadTest<GameDataManager>();
            int aa = 0;
        }
    }
}
