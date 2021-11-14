# Astro.Coordinates
Qkmaxware.Astro.Coordinates contains utility classes and methods to convert coordinates between different astronomical frames of reference.

## Example Usages
Convert from a saved J2000 coordinate to JNow AltAz
```cs
var raDec = new EquatorialCoordinate(
    ra: ...
    dec: ...
);
var from = new EquatorialGeocentric();
var to = new HorizontalObserver(
    Moment.Now,
    new GeographicCoordinate(
        lat: ...
        lon: ...
    )
);
var transformation  = from.TransformTo(to);
var altAz           = transformation.Transform(raDec);

Console.WriteLine(altAz.Altitude);
Console.WriteLine(altAz.Azimuth);
```