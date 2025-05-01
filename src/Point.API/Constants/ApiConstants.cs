namespace Point.API.Constants
{
    public static class ApiConstants
    {
        public static class EntityFields
        {
            public const string Category = "category";
            public const string Cost = "cost";
            public const string Description = "description";
            public const string Remarks = "remarks";
            public const string Tags = "tags";
            public const string Units = "units";
        }

        public static List<string> ItemFields =
            [
                EntityFields.Description,
                EntityFields.Category,
                EntityFields.Tags,
                EntityFields.Units
            ];

        public static List<string> ItemUnitFields =
            [
                EntityFields.Category,
                EntityFields.Cost,
                EntityFields.Description,
                EntityFields.Remarks,
                EntityFields.Tags
            ];
    }
}
