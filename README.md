# Ground Height Probe

A measuring instrument for KSP 1.12, and nothing else. It moves no vessel, patches no stock code and
writes no file. It exists to show one fact:

> The same vessel, in the same save reloaded five times, does not end up twice at the same distance
> from the centre of the body.

## Install

Copy `GameData/GroundHeightProbeMod` into the `GameData` of KSP. No dependency — stock KSP is enough.

To build it yourself: set `KSPDIR` to your KSP install folder and run `build.bat`.

## The window

In flight, a window shows the distance from the centre of mass of the active vessel to the centre of
its body, live, in millimetres. One button records a line in a table that survives scene changes:

| column | meaning |
|---|---|
| **On rails** | read while the vessel is still packed, before the first physics step — the saved position, handed back untouched |
| **Settled** | read when you press the button, so once the vessel has come to rest |
| **Delta** | difference with the first recorded loading |

The two range lines at the bottom are the point. **On rails** must not move from one loading to the
next; **settled** does.

The table lives in memory only: it empties itself when KSP is closed, and the *Clear table* button
empties it on demand. Recording twice in the same flight scene updates that loading's line rather
than adding a second one.

## The protocol

1. Put a **capsule alone** — no anchor, no wheels, no landing legs — on **flat bare terrain a few km
   from the KSC**. `Alt+F12 → Cheats → Set Position` does it in stock.
   ⚠️ **Not on the launchpad and not on the runway.** Those are static colliders, not PQS terrain;
   they have their own behaviour and would mix two things in one measurement.
2. Let it settle, and **save once**.
3. **Reload that same save four or five times.** After each loading, let the capsule settle a couple
   of seconds, then press *Record this loading*.
4. **Never save again during the campaign** — otherwise each save writes a new position and you also
   measure your own drift.

Then screenshot the window.

### Notes

- no time warp before measuring: a packed vessel is no longer physically resting on anything;
- do not walk away: past 350 m the vessel repacks, past 2 250 m it unloads;
- do not touch the terrain detail setting during a campaign;
- no need to restart KSP between loadings, and no need to wait: the offset is settled as soon as the
  scene is loaded.

### Reading the results

If the **settled** value of one loading is *above* its own **on rails** value, the collision surface
was born above the capsule and PhysX pushed it out. That loading still counts as evidence that the
ground was not in the same place, but its magnitude is polluted — the clean quantitative readings are
the ones where the vessel fell onto the ground rather than being ejected from it.

## Why nobody notices this in normal play

The collision mesh and the rendered mesh are the same quad: the visuals move with it, and six
centimetres at eye height is invisible. Only the consequences show, and they get blamed on the
kraken.
