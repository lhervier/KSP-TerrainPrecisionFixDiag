# The measurements: coming back to a craft you left

Part of [Terrain Precision Fix Diag 1](../README.md): the readings taken with
[the approach protocol](the-protocol-approach.md) — a craft parked on flat ground, a rover driving
away until the game unloads it, then coming back. Nothing is loaded at any point: from the first line
to the last, it is one single flight. The other series, on a craft handed back by a save, is in
[The measurements: loading the same save](the-measurements-loading.md).

The save the protocol uses is [`diag/approach-kerbin.sfs`](../diag/approach-kerbin.sfs), and
[the protocol page](the-protocol-approach.md#the-save) says what it holds.

## The install

KSP 1.12.5 on Windows, with `GameData` holding Harmony, ModuleManager,
[KSP Community Fixes](https://github.com/KSPModdingLibs/KSPCommunityFixes) 1.41.1 and this mod, and
nothing else.

## The readings

Six round trips in a row, in one flight. The table was cleared between them, so each screenshot holds
one round trip and its first line is the last line of the one before. The bottom line of each is the
reading in progress, not a record.

| round trip | **Moved** | screenshot |
|---|---|---|
| 1 | **+12.570 mm** | [`run1.png`](../imgs/approach/run1.png) |
| 2 | **−17.549 mm** | [`run2.png`](../imgs/approach/run2.png) |
| 3 | **+10.969 mm** | [`run3.png`](../imgs/approach/run3.png) |
| 4 | **−7.749 mm** | [`run4.png`](../imgs/approach/run4.png) |
| 5 | **−7.484 mm** | [`run5.png`](../imgs/approach/run5.png) |
| 6 | **+19.170 mm** | [`run6.png`](../imgs/approach/run6.png) |

![The first round trip of the series](../imgs/approach/run1.png)

The round trip pictured under the protocol, taken in another flight of the same save, read
−17.572 mm.

The first line of the first screenshot is not a round trip: it is the scene opening, the craft handed
back by the save and settling 37.699 mm lower. That is the reading
[the other series](the-measurements-loading.md) is about, and it is left aside here.

## What these readings show

**The game gives the craft back where it took it.** Lines 2 and 4 of every round trip — before the
trip and after it — never differ by more than six thousandths of a millimetre. Whatever happened
while the rover was away, the height the game holds the craft at came back untouched.

**What it comes to rest on is somewhere else.** Once physics takes it over again, it settles between
7.5 and 19.2 mm from that height, upwards as often as downwards, and a different amount every time.

**Nothing was loaded.** No scene change, no save, no quickload: the whole table was filled in one
flight, by driving away and coming back.

The size is not the same from one round trip to the next, and it is not guaranteed either: we have
seen a round trip come back within a millimetre. That is why the protocol asks for a series.

As everywhere else with this instrument, what is measured is **the craft**, not the ground it rests
on — [what this instrument shows, and what it does not](what-this-instrument-shows.md) applies
to these readings word for word.

## The logs

[`diag/runs/approach-stock-6cycles.log`](../diag/runs/approach-stock-6cycles.log) — the `KSP.log` of the session
the six round trips were taken in.
