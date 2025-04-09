using OfficeOpenXml;
using System.Reflection;

namespace Utilities.Services
{
    public static class ExcelParser
    {
        public static List<T> ParseFromExcel<T>(Stream stream) where T : new()
        {
            stream.Position = 0;

            ExcelPackage.License.SetNonCommercialPersonal("Hlbrtn-CRM");

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault()
                            ?? throw new Exception("Excel file not found.");

            var items = new List<T>();
            var properties = typeof(T).GetProperties();

            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var item = CreateItemFromRow<T>(worksheet, row, properties);
                items.Add(item);
            }

            return items;
        }

        private static T CreateItemFromRow<T>(ExcelWorksheet worksheet, int row, PropertyInfo[] properties) where T : new()
        {
            var item = new T();

            for (int col = 1; col <= properties.Length; col++)
            {
                var property = properties[col - 1];
                if (!property.CanWrite) continue;

                var cellValue = worksheet.Cells[row, col].Text?.Trim();
                if (cellValue == null) continue;

                SetPropertyValue(item, property, cellValue);
            }

            return item;
        }

        private static void SetPropertyValue<T>(T item, PropertyInfo property, string cellValue)
        {
            try
            {
                object? convertedValue = property.PropertyType == typeof(Guid)
                    ? ParseGuid(cellValue)
                    : Convert.ChangeType(cellValue, property.PropertyType);

                property.SetValue(item, convertedValue);
            }
            catch
            {
                // Handle conversion error if needed
                property.SetValue(item, GetDefaultValue(property.PropertyType));
            }
        }

        private static Guid ParseGuid(string value) =>
            Guid.TryParse(value, out var guid) ? guid : Guid.Empty;

        private static object? GetDefaultValue(Type type) =>
            type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
