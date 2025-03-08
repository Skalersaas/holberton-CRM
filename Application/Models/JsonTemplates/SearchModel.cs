
namespace Domain.Models.JsonTemplates
{
    public class SearchModel
    {
        public int Size { get; set; }
        public int Page {  get; set; }
        public bool Valid() => Size > 0 && Page > 0; 
    }
}
