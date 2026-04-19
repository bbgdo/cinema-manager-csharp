using CinemaManager.Models;

namespace CinemaManager.Application;

public static class HallTypeFormatter
{
    public static string Format(HallType hallType) => hallType switch
    {
        HallType.Standard2D => "2D",
        HallType.ThreeD     => "3D",
        HallType.Imax       => "IMAX",
        HallType.VipLounge  => "VIP",
        _                   => hallType.ToString()
    };
}
