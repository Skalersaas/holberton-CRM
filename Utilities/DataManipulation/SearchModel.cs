﻿namespace Utilities.DataManipulation
{
    public class SearchModel
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public string SortedField { get; set; } = string.Empty;
        public bool IsAscending { get; set; }
        public Dictionary<string, string> Filters { get; set; } = [];
        public bool PaginationValid() => Size > 0 && Page > 0;
    }
}
