namespace holberton_CRM.Helpers
{
    public class EnvLoader
    {
        public static void LoadEnvFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The .env file was not found at: {filePath}");

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length != 2)
                    continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim().Trim('"');
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}
