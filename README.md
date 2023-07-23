# SebLague-Chess-Challenge-Entry

My entry for Sebastian Lague's *[Tiny Chess Bot Challenge](https://youtu.be/iScy18pVR58)*

**Disclaimer:** I know very little (basically nothing) about chess

## Concept

score moves based on a priority system (highest to lowest):
- checkmate
- attack enemy king
- take enemy piece
- move closer to enemy king
- move closer to enemy piece

## optional goals
- [ ] add a common opening
  - [ ] choose which opening
  - [ ] change depending on played color
  - [ ] detect and defend against common openings
- [ ] add defense to priority system
  - [ ] detect if about to get checked
  - [ ] prioritize clearing the check
  - [ ] don't move to where enemy pieces can attack
