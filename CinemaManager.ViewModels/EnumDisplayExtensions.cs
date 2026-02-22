using CinemaManager.Models;

namespace CinemaManager.ViewModels;

public static class EnumDisplayExtensions {
    public static string ToDisplayName(this HallType hallType) => hallType switch
    {
        HallType.Standard2D => "2D",
        HallType.ThreeD     => "3D",
        HallType.Imax       => "IMAX",
        HallType.VipLounge        => "VIP",
        _                   => hallType.ToString()
    };

    public static string ToDisplayName(this MovieGenre genre) => genre switch
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