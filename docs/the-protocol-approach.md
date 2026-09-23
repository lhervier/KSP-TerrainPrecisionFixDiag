# The protocol: coming back to a craft you left

Part of [Terrain Precision Fix Diag 1](../README.md): how to take the reading on a craft that is
loaded into a scene already running, step by step. Nothing is loaded here — from the first line to
the last, it is one single flight. The columns it fills are in [The window](the-window.md), and what
it reads is in [The measurements: coming back to a craft you left](the-measurements-approach.md).

The other protocol takes the same reading on a craft handed back by a save:
[The protocol: loading the same save](the-protocol-loading.md).

A craft is parked on flat ground. A rover drives far enough for the game to unload it, then comes
back, and the window follows the parked craft throughout.

## The save

[`approach-kerbin.sfs`](../diag/approach-kerbin.sfs), a sandbox game of KSP 1.12.5. Copy it into the
folder of a sandbox game and load it from that game. It holds two craft, a few metres apart on the
flat grass west of the KSC:

- **the craft the readings are about**: a Mk1 command pod on an FL-T100 tank, landed at latitude
  −0.0593°, longitude −74.7269°. It is already the target of the other one, so the window follows it
  from the moment the scene opens, and you have nothing to set;
- **the rover you drive**: a crewed rover on four wheels, with batteries and solar panels to recharge
  them.

You can of course build your own two craft instead. The parked one must meet the same conditions as
[the capsule of the other protocol](the-protocol-loading.md): flat ground, no suspension, nothing that needs
a slope to move on its own.

## The distances that decide everything

They are the game's own, from the `VesselRanges` of `Physics.cfg`. A landed craft is **unloaded at
2500 m**, **loaded again at 2250 m**, **packed at 350 m**, and **handed over to physics at 200 m**.

Unloaded and loaded again are not the same distance on purpose, so that a craft sitting right at the
limit does not load and unload over and over. Coming back, the numbers reappear at 2250 m — not at
2500 m, where they went away.

## The protocol

Load the save and **do not change scene again** — no save, no load, no trip back to the space centre.
Everything below happens in one flight, and that is the whole point of these readings.

Then, per round trip, five records.

**1. Next to it.** Press *Record* without moving.

![The rover beside the parked craft, at the opening of the scene](../imgs/approach-protocol/00-load-the-save.png)

**2. A few hundred metres away.** Drive off, past 350 m, and press *Record*.

![The rover stopped 717 m from the craft](../imgs/approach-protocol/10-further-600m.png)

**3. Out of range.** Keep going past 2500 m, until the line reads `too far away to read`, and press
*Record* on the empty line.

![The rover 2.6 km away, the window reading "too far away to read"](../imgs/approach-protocol/20-further-2.5km.png)

**4. Turn round.** Stop beyond 3 km, turn, and drive back. Nothing to record here.

![The rover stopped 3.0 km away](../imgs/approach-protocol/30-stop-at-3km.png)

**5. Back in range.** Once the numbers come back, press *Record*.

![The rover back at 2.2 km, the numbers showing again](../imgs/approach-protocol/40-back-to-2250m.png)

**6. Back beside it.** Come within 200 m, give it three to five seconds to settle, and press *Record*.

![The rover back at 144 m from the craft](../imgs/approach-protocol/50-less-than-200m.png)

## What the five lines are worth

| line | where | what the game is doing to the parked craft | what the line is worth |
|---|---|---|---|
| 1 | beside it | loaded, physics running on it | where the previous round trip left it |
| 2 | ~600 m | packed again, still loaded | `0.000`, by construction: a packed craft is held where it is |
| 3 | past 2500 m | unloaded | nothing to read — it is the proof the round trip really happened |
| 4 | back under 2250 m | loaded again, still packed | the height the game hands back |
| 5 | under 200 m | handed over to physics | **the measurement** |

Lines 2 and 4 measure nothing: a packed craft cannot move, so both columns carry the same number.
They prove something instead — that the height the game holds the craft at did not drift while it was
away. Everything happens between line 4 and line 5.

Line 3 cannot be recorded from anywhere else: the window only has nothing to show while the craft is
out of range. An empty line in the middle of the table is what tells a reader that the two records
around it really are separated by a trip out of range, and not by two readings taken on the spot.

⚠️ **Wait before recording line 5.** The craft is still settling for a second or two after physics
takes it over, by a few hundredths of a millimetre. Waiting the same three to five seconds every time
costs nothing and keeps the lines comparable.

⚠️ **Do not save while the round trips are running.** Saving would write the craft's current height
into the save, and the next reading would be taken against that instead of the one you started from.

**Then do it again.** One round trip says nothing: the size of the reading is not the same twice, and
a round trip can come back within a millimetre. It is the series that is worth reading, not a line.
