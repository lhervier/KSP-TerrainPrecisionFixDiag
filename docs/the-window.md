# The window

Part of [Terrain Precision Fix Diag 1](../README.md): the table the mod shows in flight, column by column.

In flight, a window shows a table with one line per loading, in millimetres. The **bottom line is the
loading in progress**: its numbers move as you watch, it carries `--` where the others carry a record
number, and the *Record* button at the end of it freezes it into the table. The table survives scene
changes, so the lines pile up as you reload.

![Mod's Window](../imgs/window.png)

| column | meaning |
|---|---|
| **On rails** | read the instant the scene opens, before physics has started — the position the game gives to the craft at startup |
| **Settled** | the distance right now, running live until you press the button — so, once the craft has come to rest |
| **Moved** | **Settled** minus **On rails** — how far the craft ended up from the position it was given, this loading. Negative means it went down |

**Moved** is the figure to look at, and it should be zero. The craft was at rest on the ground when
you saved it; handed back at that very height, it has no reason to move at all.

Its sign says which of the two outcomes of [the coin toss](../README.md#why-it-matters) you got — and
how much the figure is worth.

**Negative** — the craft came back above the surface it ends up resting on, and dropped onto it. That
is a clean measurement: the figure is the gap it fell through.

**Positive** — the craft came back below that surface, so it started off inside it and the physics
pushed it back out. Still a craft that did not stay where it was put, but the figure itself is
spoiled: it measures how hard it was shoved, not how deep it started.

Watch the live line as the craft settles and you see the demonstration play out: while the craft is
still on rails the two distances are equal and **Moved** reads `0.000`, and it is the first step of
physics that breaks the zero.

Every frozen line carries a *Delete* button, so a line recorded too early — before the craft had
finished settling — costs one click to throw away. *Clear table* throws away the lot. The table lives
in memory only, and empties itself when KSP is closed.

