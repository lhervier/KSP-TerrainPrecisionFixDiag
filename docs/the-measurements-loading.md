# The measurements: loading the same save

Part of [Terrain Precision Fix Diag 1](../README.md): the readings taken with
[the loading protocol](the-protocol-loading.md), on four worlds. The other series are in
[The measurements: coming back to a craft you left](the-measurements-approach.md) and
[The measurements: switching to a craft far away](the-measurements-switching.md).

## The install

KSP 1.12.5 on Windows, with `GameData` holding Harmony, ModuleManager,
[KSP Community Fixes](https://github.com/KSPModdingLibs/KSPCommunityFixes) 1.41.1 and this mod, and
nothing else — what most players run, give or take their other mods.

## One capsule

That is the whole demonstration, and it fits in one screenshot. Here is the same save, on Kerbin,
loaded six times:

![Six loadings of the same save, on Kerbin](../imgs/measures/reload/1part/00-kerbin.png)

The bottom line is the loading in progress, not a seventh one: after the sixth *Record*, it keeps
showing that same sixth loading, still live.

The same value in **On rails**, six times over: KSP handed the capsule back in exactly the same
place, every single time. Never a zero in **Moved**: on all six, the ground turned out to be
somewhere else. Sometimes lower, and the capsule dropped onto it; sometimes higher, and it got pushed
back out.

The same lone capsule, the same loadings of one save, done again on the Mun, on Minmus and on
Gilly — the smallest place there is to stand on:

![Loadings of the same save, on the Mun](../imgs/measures/reload/1part/10-mune.png)

![Loadings of the same save, on Minmus](../imgs/measures/reload/1part/20-minmus.png)

![Loadings of the same save, on Gilly](../imgs/measures/reload/1part/30-gilly.png)

| loading | Kerbin — **Moved** (mm) | Mun — **Moved** (mm) | Minmus — **Moved** (mm) | Gilly — **Moved** (mm) |
|---|---|---|---|---|
| 1 | −24.047 | −5.056 | −2.921 | +2.427 |
| 2 | +42.074 | +15.368 | −1.736 | −0.131 |
| 3 | +51.180 | +2.385 | +0.333 | +1.664 |
| 4 | +7.070 | −2.290 | −1.525 | −0.846 |
| 5 | −33.097 | −5.287 | +0.069 | +0.499 |
| 6 | +101.430 | +5.555 | +3.785 | +0.595 |
| **lowest to highest** | **134.5 mm** | **20.7 mm** | **6.7 mm** | **3.3 mm** |

**On rails** gives the same digits on every line of the Kerbin, Mun and Minmus series. On Gilly, the
first loading reads three thousandths of a millimetre above the five others. So on every world KSP
handed the capsule back where the save says it was, and on every world it still came to rest at a
height that changed from one loading to the next.

## The same craft, with two parts

A craft made of a single part is a special case for KSP (see [the protocol](the-protocol-loading.md)).
So the whole campaign was run again with a two-part craft: the same capsule, sitting on a small flat
fuel tank.

![Two parts, on Kerbin](../imgs/measures/reload/2parts/00-kerbin.png)

![Two parts, on the Mun](../imgs/measures/reload/2parts/10-mune.png)

![Two parts, on Minmus](../imgs/measures/reload/2parts/20-minmus.png)

![Two parts, on Gilly](../imgs/measures/reload/2parts/30-gilly.png)

| loading | Kerbin — **Moved** (mm) | Mun — **Moved** (mm) | Minmus — **Moved** (mm) | Gilly — **Moved** (mm) |
|---|---|---|---|---|
| 1 | +76.134 | −5.641 | +0.392 | +2.007 |
| 2 | +77.214 | −4.935 | +3.263 | −0.073 |
| 3 | −25.810 | +6.147 | −1.084 | +0.499 |
| 4 | +56.322 | +2.259 | +0.800 | +1.008 |
| 5 | −1.187 | −2.629 | +1.627 | −0.150 |
| 6 | −47.450 | −1.216 | −1.514 | −0.468 |
| **lowest to highest** | **124.7 mm** | **11.8 mm** | **4.8 mm** | **2.5 mm** |

**On rails** again gives the same digits on every line, except on Kerbin, where the first loading
reads two thousandths of a millimetre below the five others. One part or two, the picture is the
same.
