# The saves and the runs

Part of [Terrain Precision Fix Diag 1](../README.md): the saves and the logs of the readings taken
with [the approach protocol](../docs/the-protocol-approach.md) and
[the switching protocol](../docs/the-protocol-switching.md), and of loadings on Real Solar System. What those readings say is in
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

## On Real Solar System

Taken on [Real Solar System](https://github.com/KSP-RO/RealSolarSystem) 20.1.3.0 and what it requires
(Kopernicus, Modular Flight Integrator, KSPTextureLoader, the RSS textures), on an install of their own:
these saves only load there. The protocol is [the loading protocol](../docs/the-protocol-loading.md).

- [`reload-moon-rss.sfs`](reload-moon-rss.sfs) — a capsule on an empty FL-T100, landed on flat ground
  on the Moon.
- [`reload-moon-rss-resave.sfs`](reload-moon-rss-resave.sfs) — the same craft, saved again at a later
  load.
- [`reload-earth-rss-resave.sfs`](reload-earth-rss-resave.sfs) — the same kind of craft, on the grass
  about 1.4 km west of the KSC on Earth.

Copy a save into the folder of a sandbox game and load it from that game.

- [`runs/reload-moon-rss-stock.log`](runs/reload-moon-rss-stock.log) — six loads of
  `reload-moon-rss.sfs`, then one of a save taken after the sixth.
- [`runs/reload-moon-rss-vgpeoff-stock.log`](runs/reload-moon-rss-vgpeoff-stock.log) — the first load
  of `reload-moon-rss.sfs` with Real Solar System's `VesselGroundPositionEnhancer` turned off, by an
  empty assembly named `WorldStabilizer` in `GameData`.
- [`runs/reload-moon-rss-stock-14loads.log`](runs/reload-moon-rss-stock-14loads.log) — fourteen loads
  of `reload-moon-rss.sfs`, with Terrain Precision Fix Diag 2 open as well.
- [`runs/reload-earth-rss-stock.log`](runs/reload-earth-rss-stock.log) — six loads of
  `reload-earth-rss-resave.sfs`, with Terrain Precision Fix Diag 2 open as well.
