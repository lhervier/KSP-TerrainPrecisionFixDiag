# This mod's demonstration

Part of [Terrain Precision Fix Diag 1](../README.md): what the mod reads at every loading, and why those two numbers are enough.

You cannot look at the ground and see this: the surface you walk on and the surface you see are one
and the same, so the picture shifts along with it. What you can see is what rests *on* the ground. So
the mod measures the distance from your craft to the centre of the body, in millimetres, and records
two values for every loading:

- **on rails**, the instant the scene opens, before physics has run — the position the save gives
  back;
- **settled**, once the craft has come to rest on the ground (every frame).

Both are the same measurement, taken between the origin of the root part of the craft — the very
point KSP writes to the save and hands back on loading — and the centre of the body, in double
precision from end to end:

```csharp
private static double DistanceToCentreMm(Vessel vessel)
{
    // Vector3d on both sides, deliberately: reading a few millimetres out of six hundred
    // kilometres leaves no room for anything short of double precision.
    Vector3d toCentre = (Vector3d)vessel.vesselTransform.position - vessel.mainBody.position;
    return toCentre.magnitude * 1000.0;
}
```

Reload the same save several times, then read the two columns against each other. The first one tells
you whether KSP puts the craft back where it was; the second one tells you where it actually came to
rest. As long as neither of them varies from one loading to the next, the round trip is exact and
nothing about the craft itself has changed. One of them does vary, though — spoiler: the second one.

