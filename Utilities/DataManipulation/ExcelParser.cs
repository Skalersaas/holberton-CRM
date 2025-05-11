using OfficeOpenXml;
using System.Reflection;

namespace Utilities.DataManipulation
{
    /// <summary>
    /// Provides functionality to parse Excel files into strongly-typed objects.
    /// This class handles the conversion of Excel data into C# objects using reflection.
    /// </summary>
    public static class ExcelParser
    {
        /// <summary>
        /// Parses an Excel file stream into a list of strongly-typed objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create from the Excel data.</typeparam>
        /// <param name="stream">The stream containing the Excel file data.</param>
        /// <returns>A list of objects of type T populated with data from the Excel file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the stream is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no worksheet is found in the Excel file.</exception>
        /// <remarks>
        /// Expects headers in the first row matching property names, with data starting from the second row.
        /// </remarks>
        public static List<T> ParseFromExcel<T>(Stream stream) where T : new()
        {
            ArgumentNullException.ThrowIfNull(stream);

            stream.Position = 0;

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault()
                            ?? throw new InvalidOperationException("No worksheet found in Excel file.");

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToArray();

            var items = new List<T>(worksheet.Dimension.Rows - 1);

            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var item = CreateItemFromRow<T>(worksheet, row, properties);
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Creates a single object of type T from a row in the Excel worksheet.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="worksheet">The Excel worksheet containing the data.</param>
        /// <param name="row">The row number to read from.</param>
        /// <param name="properties">The properties of type T to populate.</param>
        /// <returns>A new object of type T with properties set from the Excel row data.</returns>
        private static T CreateItemFromRow<T>(ExcelWorksheet worksheet, int row, PropertyInfo[] properties) where T : new()
        {
            var item = new T();

            for (int col = 1; col <= properties.Length; col++)
            {
                var property = properties[col - 1];
                var cellValue = worksheet.Cells[row, col].Text?.Trim();

                if (!string.IsNullOrEmpty(cellValue))
                {
                    SetPropertyValue(item, property, cellValue);
                }
            }

            return item;
        }

        /// <summary>
        /// Sets the value of a property on an object, converting the string value to the appropriate type.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="item">The object whose property to set.</param>
        /// <param name="property">The property to set.</param>
        /// <param name="cellValue">The string value to convert and set.</param>
        /// <remarks>
        /// Sets the property to its default value if conversion fails.
        /// </remarks>
        private static void SetPropertyValue<T>(T item, PropertyInfo property, string cellValue)
        {
            try
            {
                object? convertedValue = ConvertToPropertyType(cellValue, property.PropertyType);
                property.SetValue(item, convertedValue);
            }
            catch
            {
                property.SetValue(item, GetDefaultValue(property.PropertyType));
            }
        }

        /// <summary>
        /// Converts a string value to the specified target type.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="targetType">The target type to convert to.</param>
        /// <returns>The converted value as an object.</returns>
        /// <remarks>
        /// Handles special conversions for Guid, Enum, and Nullable types.
        /// </remarks>
        private static object? ConvertToPropertyType(string value, Type targetType)
        {
            if (targetType == typeof(Guid))
                return Guid.TryParse(value, out var guid) ? guid : Guid.Empty;

            if (targetType.IsEnum && Enum.TryParse(targetType, value, true, out var enumValue))
                return enumValue;

            return Convert.ChangeType(value, Nullable.GetUnderlyingType(targetType) ?? targetType);
        }

        /// <summary>
        /// Gets the default value for a given type.
        /// </summary>
        /// <param name="type">The type to get the default value for.</param>
        /// <returns>The default value for the type (null for reference types, default for value types).</returns>
        private static object? GetDefaultValue(Type type) =>
            type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
