using System;
using Qkmaxware.Measurement;

// Algorithm from:
// https://en.wikipedia.org/wiki/Astronomical_coordinate_systems#Converting_coordinates

namespace Qkmaxware.Astro.Coordinates.Transformations {

/// <summary>
/// Transformation for equatorial to horizontal
/// </summary>
public class EquatorialToHorizontal : ITransformation<EquatorialCoordinate, HorizontalCoordinates> {
    private Angle latitude;
    private Angle localSiderealTime;
    public EquatorialToHorizontal(Angle localSiderealTime, Angle latitude) {
        this.latitude = latitude;
        this.localSiderealTime = localSiderealTime;
    }

    public HorizontalCoordinates Transform(EquatorialCoordinate eq) {
        // A = azimuth
        // h = altitude
        // a = ra
        // d = dec
        // ϕo = lat
        // λo = lon (here is measured positively westward from the prime meridian; this is contrary to current IAU standards)
        // θL = local sidereal time
        // θG = greenwich sidereal time
        var d = (double)eq.Declination.TotalRadians();

        var Oo = (double)latitude.TotalRadians();

        var hR = (double)(localSiderealTime - eq.RightAscension).TotalRadians(); 
        var AR = -Math.Atan2(
            -Math.Sin(Oo) * Math.Cos(d) * Math.Cos(hR) + Math.Cos(Oo) * Math.Sin(d),
                                        Math.Cos(d) * Math.Sin(hR) 
        );

        return new HorizontalCoordinates(
            alt: Angle.Radians(hR),
            az: Angle.Radians(AR)
        );
    }

    public EquatorialCoordinate Transform(HorizontalCoordinates az) {
        var Oo = (double)latitude.TotalRadians();
        var A = (double)az.Azimuth.TotalRadians();

        var aR = (double)(localSiderealTime - az.Altitude).TotalRadians();
        var dR = Math.Asin(
            Math.Sin(Oo) * Math.Sin(aR) - Math.Cos(Oo) * Math.Cos(aR) * Math.Cos(A)
        );

        return new EquatorialCoordinate(
            ra: Angle.Radians(aR),
            dec: Angle.Radians(dR)
        );
    }
}

/// <summary>
/// Transformation for horizontal to equatorial
/// </summary>
public class HorizontalToEquatorial : ITransformation<HorizontalCoordinates, EquatorialCoordinate> {
    private EquatorialToHorizontal reverse;

    public HorizontalToEquatorial(Angle localSiderealTime, Angle latitude) {
        this.reverse = new EquatorialToHorizontal(
            localSiderealTime: localSiderealTime,
            latitude: latitude
        );
    }    

    public EquatorialCoordinate Transform(HorizontalCoordinates to) => reverse.Transform(to);
    public HorizontalCoordinates Transform(EquatorialCoordinate from) => reverse.Transform(from);
}

}