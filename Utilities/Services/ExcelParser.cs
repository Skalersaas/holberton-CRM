using OfficeOpenXml;

namespace Utilities.Services
{
    public static class ExcelParser
    {
        public static List<T> ParseFromExcel<T>(Stream stream) where T : new()
        {
            stream.Position = 0;

            ExcelPackage.License.SetNonCommercialPersonal("Hlbrtn-CRM");

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                throw new Exception("Excel file not found.");

            var items = new List<T>();
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var item = new T();
                var properties = typeof(T).GetProperties();

                for (int col = 1; col <= properties.Length; col++)
                {
                    var property = properties[col - 1];
                    var value = worksheet.Cells[row, col].Text?.Trim();

                    if (value != null && property.CanWrite)
                    {
                        if (property.PropertyType == typeof(Guid))
                        {
                            if (Guid.TryParse(value, out Guid guidValue))
                            {
                                property.SetValue(item, guidValue);
                            }
                            else
                            {
                                property.SetValue(item, Guid.Empty);
                            }
                        }
                        else
                        {
                            property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
                        }
                    }
                }

                items.Add(item);
            }

            return items;
        }
    }
}
