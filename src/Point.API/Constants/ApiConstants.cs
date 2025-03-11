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
                public const string Remarks = "remarks";
                public const string Tags = "tags";
            }

            public static List<string> QueryFields =
            [
                Fields.Description,
                Fields.Category,
                Fields.Cost,
                Fields.Remarks,
                Fields.Tags
            ];
        }
    }
}
