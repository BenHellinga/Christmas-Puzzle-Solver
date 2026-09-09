# Christmas Puzzle Solver

**This project is no longer under active development.**

## About

This is an old project originally written in 2023 being uploaded to GitHub now. It's a C# console application that brute-force solves "Ice Puzzle 9" by Yuu Asaka, a packing puzzle where a set of oddly-shaped, tabbed/notched pieces has to be arranged to fill a 9x6 tray.

I got the puzzle as a Christmas present and couldn't solve it by hand, so instead of giving up I encoded every piece into the program and wrote a solver to search for the arrangement for me.

## Goal

Model the puzzle's 9x6 tray and its 9 pieces (each with their own interlocking tab/notch pattern) exactly enough that a brute-force search could try every position, rotation, and mirrored orientation of every piece, and stop as soon as it found a combination that fills the board with no overlaps and no unmatched tabs/notches.

## Implementation

Each piece is hand-entered as a small grid of tile IDs and rotations, matching the physical piece. Tiles are one of a fixed set of shapes (a plain edge, a straight tab/notch, an interlocking edge, etc), and two adjacent tiles are only allowed to sit next to each other if their edges actually match up.

The solver is a recursive backtracker: for each piece in turn, it tries every (x, y) position on the board, in each of the piece's 4 rotations, mirrored and not (8 orientations total), places it if it doesn't collide with anything already on the board, and recurses on the next piece. If a placement leads nowhere, it's undone and the next option is tried. It stops the moment all 9 pieces fit.

### The overnight run

I was confident the search covered every possible arrangement, so I let it run overnight expecting it to come back with the solution. Instead, it ran through the entire search space and reported no solution at all.

Since I trusted the search logic, that result told me something about my model of the puzzle had to be wrong rather than the puzzle being unsolvable, and the most likely candidate was that one of the pieces isn't meant to sit flat on the grid, but placed at a diagonal, which this grid-based, axis-aligned model has no way of representing. With that in mind I went back to solving it by hand with that possibility in mind, and found the actual solution shortly after.

## Usage

Open the project in VS Code (or any editor with the C# dev kit) and run it, no input required.

The program prints its search progress to the console as it goes, and either prints the solved board layout or "No solution found" once the search space is exhausted.
