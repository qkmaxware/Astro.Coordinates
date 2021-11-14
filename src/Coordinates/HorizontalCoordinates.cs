using Qkmaxware.Measurement;

namespace Qkmaxware.Astro.Coordinates {

/// <summary>
/// Coordiante in alt-az coordinates
/// </summary>
public class HorizontalCoordinates {
    /// <summary>
    /// Angle of longitude
    /// </summary>
    public Angle Altitude {get; private set;}
    /// <summary>
    /// Angle of latitude
    /// </summary>
    public Angle Azimuth {get; private set;}

    /// <summary>
    /// Create a given horizontal coordinate with altitude and azimuthal angles
    /// </summary>
    /// <param name="alt">altitude</param>
    /// <param name="az">azimuth</param>
    /// <returns>coordinate</returns>
    public HorizontalCoordinates(Angle alt, Angle az) {
        this.Altitude = alt;
        this.Azimuth  = az;
    }
}

}