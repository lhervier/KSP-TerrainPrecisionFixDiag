# The approach and switching runs

Part of [Terrain Precision Fix Diag 1](../README.md): the save and the logs of the readings taken
with [the approach protocol](../docs/the-protocol-approach.md) and
[the switching protocol](../docs/the-protocol-switching.md). What those readings say is in
[The measurements: coming back to a craft you left](../docs/the-measurements-approach.md) and
[The measurements: switching to a craft far away](../docs/the-measurements-switching.md).

- [`approach-kerbin.sfs`](approach-kerbin.sfs) — the save the approach protocol uses: a capsule
  landed on the flat grass west of the KSC, and a rover 26 m from it.
- [`switch-kerbin.sfs`](switch-kerbin.sfs) — the save the switching protocol uses: a capsule landed
  on the same grass, and a rover 1.97 km to the south of it.

Copy a save into the folder of a sandbox game and load it from that game.

- [`runs/switching-stock.log`](runs/switching-stock.log) — the `KSP.log` of the session the six
  switching rounds were taken in.
- [`runs/approach-stock-6cycles.log`](runs/approach-stock-6cycles.log) — the `KSP.log` of the session
  the six round trips were taken in, with an earlier version of the approach save: the same two
  craft, a few metres apart, 1.4 km further east.
- [`runs/approach-diag1-stock.log`](runs/approach-diag1-stock.log) — the `KSP.log` of an earlier
  session, kept for the record: its round trips stayed within range of the parked craft, so the game
  never unloaded it and there was nothing to read.
