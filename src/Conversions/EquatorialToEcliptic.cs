using System;
using Qkmaxware.Measurement;

// Algorithm from:
// https://en.wikipedia.org/wiki/Astronomical_coordinate_systems#Converting_coordinates

namespace Qkmaxware.Astro.Coordinates.Transformations {

/// <summary>
/// Transformation for equatorial to ecliptic using the same obliquity
/// </summary>
public class EquatorialToEcliptic : ITransformation<EquatorialCoordinate, EclipticCoordinate> {
    
    private double ε;

    public EquatorialToEcliptic(Angle obliquity) {
        this.ε = (double)obliquity.TotalRadians();
    }

    public EclipticCoordinate Transform(EquatorialCoordinate eq) {
        var ε = this.ε;
        var d = (double)eq.Declination.TotalRadians();
        var ra = (double)eq.RightAscension.TotalRadians();

        var latR = Math.Asin(
            Math.Sin(d) * Math.Cos(ε) - Math.Cos(d) * Math.Sin(ε) * Math.Sin(ra)
        );
        var lonR = Math.Atan2(
            Math.Sin(ra) * Math.Cos(ε) + Math.Tan(d) * Math.Sin(ε),
                                Math.Cos(ra)
        );

        return new EclipticCoordinate(
            lat: Angle.Radians(latR),
            lon : Angle.Radians(lonR)
        );
    }

    public EquatorialCoordinate Transform(EclipticCoordinate to) {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// Transformation for equatorial to ecliptic using the same obliquity, reverse of EquatorialToEcliptic
/// </summary>
public class EclipticToEquatorial : ITransformation<EclipticCoordinate, EquatorialCoordinate> {
    private EquatorialToEcliptic forward;

    public EclipticToEquatorial(Angle obliquity) {
        this.forward = new EquatorialToEcliptic(obliquity);
    }

    public EquatorialCoordinate Transform(EclipticCoordinate ec) => forward.Transform(ec);
    public EclipticCoordinate Transform(EquatorialCoordinate eq) => forward.Transform(eq);
}

}