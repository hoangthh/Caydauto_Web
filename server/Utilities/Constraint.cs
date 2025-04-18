public static class Constraint
{
    public static class Image
    {
        public const string DefaultProductImageUrl =
            "https://www.aaronfaber.com/wp-content/uploads/2017/03/product-placeholder-wp.jpg";
        public const string DefaultUserImageUrl =
            "https://media.istockphoto.com/id/470100848/vector/male-profile-icon-white-on-the-blue-background.jpg?s=612x612&w=0&k=20&c=2Z3As7KdHqSKB6UDBpSIbMkwOgYQtbhSWrF1ZHX505E=";
    }

    public static class Port
    {
        public const string Redis = "6379";
    }

    public static class Url
    {
        public const string Server = "http://localhost:5118";
        public const string Client = "http://localhost:3000";
        public static readonly string[] Clients =
        {
            "http://localhost:5118",
            "https://localhost:3000",
            "https://localhost:3001",
        };
    }
}
