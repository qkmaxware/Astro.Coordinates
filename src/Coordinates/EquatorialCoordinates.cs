using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordiante in equatorial coordinates
/// </summary>
public class EquatorialCoordinate {
    /// <summary>
    /// Angle of right ascension
    /// </summary>
    public Angle RightAscension {get; private set;}
    /// <summary>
    /// Angle of declination
    /// </summary>
    public Angle Declination {get; private set;}

    /// <summary>
    /// Create a given ecliptic coordinate with latitude and longitude
    /// </summary>
    /// <param name="ra">right ascension</param>
    /// <param name="dec">declination</param>
    /// <returns>coordinate</returns>
    public EquatorialCoordinate(Angle ra, Angle dec) {
        this.RightAscension  = ra;
        this.Declination = dec;
    }
}

}