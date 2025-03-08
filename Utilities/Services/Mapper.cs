namespace Utilities.Services
{
    public class Mapper
    {
        public static TDestination FromDTO<TDestination, TSource>(TSource dto)
        {
            var destination = Activator.CreateInstance<TDestination>();
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDestination).GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties
                    .FirstOrDefault(p => p.Name == sourceProperty.Name && p.CanWrite);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    var value = sourceProperty.GetValue(dto);
                    destinationProperty.SetValue(destination, value);
                }
            }

            return destination;
        }

    }
}
