using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Oracle.DataAccess.Client;
using System.IO;
using System.Text.RegularExpressions;

namespace EntityClassGeneratorForOracle
{
    public static class DataTableExtensions
    {
        /// <summary>DataTableの各RowをExpandoObjectに変換します。</summary>
        public static IEnumerable<dynamic> AsDynamic(this DataTable table)
        {
            return table.AsEnumerable().Select(x =>
            {
                IDictionary<string, object> dict = new ExpandoObject();
                foreach (DataColumn column in x.Table.Columns)
                {
                    var value = x[column];
                    if (value is System.DBNull) value = null;
                    dict.Add(column.ColumnName, value);
                }
                return (dynamic)dict;
            });
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var nameSpace = args[0];

            var match = new Regex("^(?<user>[a-zA-Z0-9]+)/(?<password>[a-zA-Z0-9]+)@(?<tnsName>[a-zA-Z0-9]+)").Match(args[1]);
            var user = match.Groups["user"].Value;
            var password = match.Groups["password"].Value;
            var tnsName = match.Groups["tnsName"].Value;
            
            var connectionString = String.Format(
                "User Id={0};Password={1};Data Source={2}",
                user,
                password,
                tnsName);

            using (var conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (OracleException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                var typeDictionary = conn.GetSchema("DataTypes").AsDynamic()
                    .ToDictionary(x =>
                        x.TypeName,
                        x => (Type)Type.GetType(x.DataType));

                Func<string, bool, decimal, decimal, string> getTypeName = (dataType, isNullable, precision, scale) =>
                {
                    var type = typeDictionary[dataType];
                    var typeName = type.Name;
                    if (precision > 0 && scale == 0)
                    {
                        // NUMBER型が無条件にDecimalになるので、小数桁数が0なら整数型として扱う
                        if (precision < 10)
                        {
                            typeName = "Int32";
                        } 
                        else
                        {
                            typeName = "Int64";
                        }
                    }
                    return (isNullable && type.IsValueType)
                        ? typeName + "?" // 値型かつnull許可の時
                        : typeName;
                };

                var columnGroups = conn.GetSchema("Columns").AsDynamic()
                    .Where(x => x.OWNER == user)
                    .GroupBy(x => x.TABLE_NAME) // 全てのカラムが平らに列挙されてくるのでテーブル名でグルーピング
                    .Select(g => new
                    {
                        ClassName = g.Key, // クラス名はテーブル名(= グルーピングのキー)
                        Properties = g
                            .OrderBy(x => x.ID) // どんな順序で来るか不明なので、カラム定義順にきちんと並び替え
                            .Select(x => new
                            {
                                Name = x.COLUMN_NAME,
                                Type = getTypeName(
                                    x.DATATYPE,
                                    x.NULLABLE == "Y",
                                    x.PRECISION ?? 0m,
                                    x.SCALE ?? 0m)
                            })
                            .ToArray()
                    });

                foreach (var item in columnGroups)
                {
                    var tt = new TableGeneratorTemplate(nameSpace, item.ClassName, item.Properties);
                    var text = tt.TransformText();
                    File.WriteAllText(item.ClassName + ".cs", text, Encoding.UTF8);
                }
            }
        }
    }

    public partial class TableGeneratorTemplate
    {
        public string NameSpace { get; private set; }
        public string ClassName { get; private set; }
        public IEnumerable<dynamic> Properties { get; set; }

        public TableGeneratorTemplate(string nameSpace, string className, IEnumerable<dynamic> properties)
        {
            this.NameSpace = nameSpace;
            this.ClassName = className;
            this.Properties = properties;
        }
    }
}
