# Terrain Precision Fix - Diagnostic Mod 1

**⚠️ Work in progress.** This is an active investigation, not a finished mod. The figures, the code and the conclusions on this page can still change, and several questions are still open.

**How this was made.** Written with Claude, Anthropic's AI assistant, and reviewed line by line by a
human — me. I am saying so before anything else, because contributions made with an AI deserve a closer
look than others, and because some people would rather stop reading here. This mod measures and fixes
nothing, so what there is to check is the reading itself: the source is public, and the protocol it
comes with runs on a stock install, on your own craft, against the figures given here.

A measuring instrument for KSP 1.12, and the first of a small family of them. It lets you check, on
your own install, a claim about the ground your craft is parked on:

> **The ground KSP builds under you is never built at the same height twice.** Load the same save five
> times, and the surface your craft is standing on comes back a little higher or a little lower each
> time — a few centimetres apart on Kerbin, less on smaller worlds.

## Why it matters

Every time you load, it is a coin toss between two outcomes.

**The ground comes back lower than it was when you saved.** Your craft is now hovering a couple of
centimetres above it, so it drops those two centimetres. You never notice, and nothing breaks.

**The ground comes back higher than it was when you saved.** Your craft is now *inside* the ground —
and the physics engine will not leave two solid things overlapping. It pushes them apart, hard, in
the only direction available: up. Your craft gets launched.

![A craft jumping on its own the moment a save is reloaded](https://raw.githubusercontent.com/lhervier/KSP-TerrainPrecisionFix/master/imgs/Booing-scaled.gif)

*KSP 1.12 with [KSP Community Fixes](https://github.com/KSPModdingLibs/KSPCommunityFixes) as the only
mod installed. A pod on an empty fuel tank, parked in the grass at the KSC, saved, then reloaded from the
pause menu, several times if needed — nothing touched in between.*

That second case is the symptom everybody already knows. The lander that twitches, hops or flips the
moment the scene finishes loading. The base that sat perfectly flush yesterday and is buried up to
the hatches today. The big base that tears itself apart the very first time you load it, and never
again afterwards. A craft with many parts spread over a wide area gives the coin toss more chances
to land the wrong way up.

**Loading is not the only time the coin is tossed.** A landed craft you fly towards is loaded long
before you reach it, but held still at the position it was left at; its physics only starts once you
are within 200 m. The ground under it was not built when that position was recorded, so the same toss
happens there. From 200 m away you see much less of it — and it does just as much damage.

Both can be measured with this mod, and each has its own protocol: loading the same save over and
over, which is the easiest to repeat, and driving away from a parked craft and coming back, which
never loads anything at all.

### Disclaimer: it is not the only cause

The ground moving is one cause among several, and this page does not claim it is the only one. Plenty
of other things move a craft when a scene opens. Two well-known examples, among others:

- **suspensions.** Landing legs and wheels come back fully extended, because that is the only state
  KSP can restore them to. They then compress under the weight of the craft, and the craft moves
  while they do.
- **a craft bent to fit the ground.** While you play, physics twists the joints between parts so the
  craft settles onto the shape of the ground beneath it. That twisting is not saved. On loading, the
  craft comes back in its original, unbent shape — and if the ground is not flat, part of it really
  *is* underground, with no measurement error involved.

Both of those are avoidable, and that is exactly why [the first protocol below](docs/the-protocol-loading.md) uses a
single capsule with no legs and no wheels, on flat ground: it takes them out of the picture, along with anything else that
needs a suspension, several parts, or a slope to happen.

## This mod's demonstration

You cannot look at the ground and see this: the surface you walk on and the surface you see are one and
the same, so the picture shifts along with it. What you can see is what rests *on* the ground. So the
mod measures the distance from the root part of your craft to the centre of the body, in millimetres and
in double precision from end to end, and records it twice: **on rails**, while the game is still
holding the craft at the position it was given, before physics has run on it — at the opening of a
scene, the position the save gives back — and **settled**, once the craft has come to rest. Reload the same save several times, then read the two columns against each
other. As long as neither of them varies from one loading to the next, the round trip is exact and
nothing about the craft itself has changed. One of them does vary.

**→ Full chapter: [This mod's demonstration](docs/this-mods-demonstration.md)**

## The window

In flight, a window shows a table with one line per reading, in millimetres: **On rails**, **Settled**,
and **Moved**, the difference between the two. The bottom line is the reading in progress, its numbers
running live until the *Record* button at the end of it freezes it into the table. They are about the
craft you are flying, or about your target when you have set one on another craft — which is what lets
the second protocol below follow a parked craft while you drive away from it. **Moved** is the
figure to look at, and it should be zero: the craft was at rest on the ground when the game took it in
hand, and was handed back at that very height. Its sign tells you how much the figure is worth — a craft that fell
onto the surface gives a clean reading, one that was pushed back out of it does not.

**→ Full chapter: [The window](docs/the-window.md)**

## The protocol

Two protocols, one for each of the two moments the ground is built under a craft. Both fill the same
window, and both come with the craft and the save they were written for.

**Loading the same save.** A lone capsule — no anchor, no wheels, no landing legs — set down on bare
flat ground, away from the runway and the launchpad, which are structures rather than ground. Let it
settle, save once, then load that same save five or six times, pressing *Record* each time the live
line has stopped moving.

**→ Full chapter: [The protocol: loading the same save](docs/the-protocol-loading.md)**

**Coming back to a craft you left.** This one loads nothing at all. A craft stays parked on the same
kind of ground while you drive a rover away from it, past 2500 m, where the game unloads it — then
back to within 200 m, where its physics starts again. Five records per round trip, and as many round
trips as you like, without ever changing scene.

**→ Full chapter: [The protocol: coming back to a craft you left](docs/the-protocol-approach.md)**

## The measurements

Two series, one per protocol, and they were not taken in the same install. The first was taken with
nothing in `GameData` but this mod, then run a second time in an install with
[KSP Community Fixes](https://github.com/KSPModdingLibs/KSPCommunityFixes), which changes nothing to
it. The second was taken in that kind of install — Harmony, ModuleManager and KSP Community Fixes,
what most players run — with this mod added, and it has no bare-install twin.

In both, **On rails** — the height the game hands the craft back at — barely moves: the same digits
down to the last one from one loading to the next in the first series, six thousandths of a
millimetre at most across a round trip in the second. So the craft itself is put back where it was.
What it then comes to rest on is never quite where it was.

**Loading the same save** ([the protocol in full](docs/the-protocol-loading.md)). The same save
loaded six times on Kerbin, on the Mun, on Minmus and on Gilly, with a lone capsule, then the whole
campaign again with a two-part craft. The height the craft comes to rest at is never the same twice:
lowest to highest, 135.5 mm on Kerbin, 18.1 mm on the Mun, 3.9 mm on Minmus, 1.2 mm on Gilly.

**→ Full chapter: [The measurements: loading the same save](docs/the-measurements-loading.md)**

**Coming back to a craft you left** ([the protocol in full](docs/the-protocol-approach.md)). Six
round trips in a row on Kerbin, in a single flight, with nothing loaded at any point: the craft comes
to rest 7.5 to 19.2 mm from the height it was handed back at, upwards as often as downwards, and a
different amount every time.

**→ Full chapter: [The measurements: coming back to a craft you left](docs/the-measurements-approach.md)**


## What this instrument shows, and what it does not

Every table measures the **craft**: set down on the ground, it does not come back to rest where the save
left it, one loading to the next. That is where the instrument stops — it does not, on its own, name
what moved. A ground rebuilt a little higher or a little lower at every loading accounts for the
figures, but so would a perfectly steady ground with the craft set down beside it. A second instrument,
[Terrain Precision Fix Diag 2](https://github.com/lhervier/KSP-TerrainPrecisionFixDiag2), tells the two
apart: it measures the ground itself, with no craft in the picture at all.

**→ Full chapter: [What this instrument shows, and what it does not](docs/what-this-instrument-shows.md)**

## Get it

Either way you end up with the same `GameData/TerrainPrecisionFixDiagMod/` folder.

**Download it** — from the assets of the
[latest release](https://github.com/lhervier/KSP-TerrainPrecisionFixDiag/releases/latest).

**Or compile it** — clone this repository, set `KSPDIR` to your KSP install folder and run
`build.bat`. It needs the .NET SDK, takes a few seconds, reads the KSP assemblies straight from
your install, and puts the DLL in `GameData/TerrainPrecisionFixDiagMod/` inside the repository. It
does not install anything. Worth doing if you would rather not run a binary you have no source for
while reporting a measurement.

## Install

Drop `GameData/TerrainPrecisionFixDiagMod` into the `GameData` of KSP, so that you end up with
`GameData/TerrainPrecisionFixDiagMod/TerrainPrecisionFixDiagMod.dll`. It runs on a stock install.

## License

MIT
