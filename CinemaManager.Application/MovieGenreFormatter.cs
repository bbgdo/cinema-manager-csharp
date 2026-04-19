using CinemaManager.Models;

namespace CinemaManager.Application;

public static class MovieGenreFormatter
{
    public static string Format(MovieGenre genre) => genre switch
    {
        MovieGenre.Action   => "Action",
        MovieGenre.Anime    => "Anime",
        MovieGenre.Cartoon  => "Cartoon",
        MovieGenre.Comedy   => "Comedy",
        MovieGenre.Fantasy  => "Fantasy",
        MovieGenre.Drama    => "Drama",
        MovieGenre.Horror   => "Horror",
        MovieGenre.SciFi    => "Sci-Fi",
        MovieGenre.Thriller => "Thriller",
        _                   => genre.ToString()
    };
}
