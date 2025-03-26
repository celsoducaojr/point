namespace Point.API.Constants
{
    public static class ApiConstants
    {
        public static class Fields
        {
            public const string Category = "category";
            public const string Cost = "cost";
            public const string Description = "description";
            public const string Remarks = "remarks";
            public const string Tags = "tags";
        }

        public static class Item
        {
            public static List<string> Fields =
            [
                ApiConstants.Fields.Description,
                ApiConstants.Fields.Category,
                ApiConstants.Fields.Tags
            ];
        }

        public static class ItemUnit
        {
            public static List<string> Fields =
            [
                ApiConstants.Fields.Category,
                ApiConstants.Fields.Cost,
                ApiConstants.Fields.Description,
                ApiConstants.Fields.Remarks,
                ApiConstants.Fields.Tags
            ];
        }
    }
}
