namespace Point.API.Constants
{
    public static class ApiConstants
    {
        public static class Items
        {
            public static class Fields
            {
                public const string Description = "description";
                public const string Category = "category";
                public const string Cost = "cost";
                public const string Tags = "tags";
                public const string Unit = "unit";
            }

            public static List<string> QueryFields =
            [
                Fields.Description,
                Fields.Category,
                Fields.Cost,
                Fields.Tags,
                Fields.Unit
            ];
        }
    }
}
