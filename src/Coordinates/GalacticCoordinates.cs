using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordiante in galactic coordinates
/// </summary>
public class GalacticCoordinate {
    /// <summary>
    /// Angle of longitude
    /// </summary>
    public Angle GalacticLongitude {get; private set;}
    /// <summary>
    /// Angle of latitude
    /// </summary>
    public Angle GalacticLatitude {get; private set;}

    /// <summary>
    /// Create a given galactic coordinate with latitude and longitude
    /// </summary>
    /// <param name="lat">latitude</param>
    /// <param name="lon">longitude</param>
    /// <returns>coordinate</returns>
    public GalacticCoordinate(Angle lat, Angle lon) {
        this.GalacticLatitude  = lat.Wrap(); // 0-360 wrap
        this.GalacticLongitude = lon.Wrap(); // 0-360 wrap
    }
}

}