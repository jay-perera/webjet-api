namespace WebAPITest.Utilities
{
    public static class TestAppData
    {

        // test data files
        public static string CinemaWorldAllMoviesFilePath = @"SampleData\CinemaWorldAllMovies.json";
        public static string FilmWorldAllMoviesFilePath = @"SampleData\FilmWorldAllMovies.json";
        public static string AllMoviesFilePath = @"SampleData\AllMovies.json";

        public static string CinemaMovieDetailFilePath = @"SampleData\CinemaWorldMovieDetail.json";
        public static string FilmMovieDetailFilePath = @"SampleData\FilmWorldMovieDetail.json";
        public static string CombineMoveieDetailFilePath = @"SampleData\CombineMovieDetail.json";

        // URL
        public static string CinemaBaseUrl = @"http://webjetapitest.azurewebsites.net";
        public static string FilmBaseUrl = @"http://webjetapitest.azurewebsites.net";   // app will handle if the base urls are different

        public static string CinemaGetMoviesUrl = @"/api/cinemaworld/movies";
        public static string FilmGetMoviesUrl = @"/api/filmworld/movies";

        public static string CinemaGetMovieDetailUrl = @"/api/cinemaworld/movie";
        public static string FilmGetMovieDetailUrl = @"/api/filmworld/movie";

        // Other
        public static string ApiKey = @"sjd1HfkjU83ksdsm3802k";
        public static string CinemaTestMovieId = @"cw0076759";
        public static string FilmTestMovieId = @"fw0076759";
        public static string TestMovieIdCommon = @"0076759";


    }
}
