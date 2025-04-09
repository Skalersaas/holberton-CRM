using Domain.Models.Entities;
using OfficeOpenXml;

namespace Utilities.Services
{
    public class ExcelParser
    {
        public static List<Student> ParseStudentsFromExcel(Stream stream)
        {
            stream.Position = 0;

            ExcelPackage.License.SetNonCommercialPersonal("Hlbrtn-CRM");

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                throw new Exception("Excel file not found");

            var students = new List<Student>();
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var student = new Student
                {
                    Name = worksheet.Cells[row, 1].Text?.Trim(),
                    Surname = worksheet.Cells[row, 2].Text?.Trim(),
                    Phone = worksheet.Cells[row, 3].Text?.Trim(),
                };

                students.Add(student);
            }

            return students;
        }
    }
}
