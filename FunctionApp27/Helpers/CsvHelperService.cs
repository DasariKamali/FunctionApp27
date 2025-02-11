using System.Data;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.TypeConversion;

namespace FunctionApp27.Helpers
{
    public class CsvHelperService
    {
        public void WriteToCsv(DataTable variations, DataTable commentaries, string filePath)
        {
            using var writer = new StreamWriter(filePath, false, Encoding.UTF8, bufferSize: 65536);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            var options = new TypeConverterOptions { Formats = ["dd/MM/yyyy HH:mm:ss"] };
            csvWriter.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);

            var commentaryDict = commentaries.AsEnumerable()
                .ToDictionary(row => row["CommentaryID"].ToString(), row => row["Comment"].ToString());

            foreach (DataColumn col in variations.Columns)
            {
                csvWriter.WriteField(col.ColumnName);
            }
            csvWriter.WriteField("Comment");
            csvWriter.NextRecord();
            foreach (DataRow row in variations.Rows)
            {
                foreach (DataColumn col in variations.Columns)
                {
                    csvWriter.WriteField(row[col]);
                }

                string commentId = row["VariationReasonsCommentary"]?.ToString();
                if (!string.IsNullOrEmpty(commentId) && commentaryDict.TryGetValue(commentId, out string comment))
                {
                    csvWriter.WriteField(comment.Length > 5000 ? comment[..5000] : comment);
                }
                else
                {
                    csvWriter.WriteField("");
                }
                csvWriter.NextRecord();
            }
        }
    }
}
