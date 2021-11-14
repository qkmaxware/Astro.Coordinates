using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordiante in ecliptic coordinates
/// </summary>
public class EclipticCoordinate {
    /// <summary>
    /// Angle of longitude
    /// </summary>
    public Angle EclipticLongitude {get; private set;}
    /// <summary>
    /// Angle of latitude
    /// </summary>
    public Angle EclipticLatitude {get; private set;}

    /// <summary>
    /// Create a given ecliptic coordinate with latitude and longitude
    /// </summary>
    /// <param name="lat">latitude</param>
    /// <param name="lon">longitude</param>
    /// <returns>coordinate</returns>
    public EclipticCoordinate(Angle lat, Angle lon) {
        this.EclipticLatitude  = lat.Wrap(); // 0-360 wrap
        this.EclipticLongitude = lon.Wrap(); // 0-360 wrap
    }
}

}