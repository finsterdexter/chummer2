chummer2
========

Chummer 2: SR5 character generator

Nebular did all the work on this. He gets all the credit.

Here is a sitrep for the current state of this code (according to Nebular):

Unfortunately I don't have a very comprehensive list. I had made a number of changes based on the previews:
- Skills should now allow a maximum Rating of 12
- All Skills have been entered
- All Qualities have been entered
- All Spells and Traditions have been entered
- All Traditions have been entered
- All Mentor Spirits have been entered
- All Ammo has been entered
- All Weapons have been entered
- All Weapon Accessories have been entered
- All Ranges have been entered
- All Armors have been entered
- All Armor Accessories have been entered
- All Gear has been entered
- Cybereyes and Cyberears have been entered - might have covered a few more items in the Cyberware list
- Limits have been added as properties of the character and should calculate correctly
- Armor calculations for total Armor value, encumbrance, and Armor Capacity should calculate correctly

Here's the list of item's I had in my bug tracker:
- Need to implement support for Cyberdecks.
- Mentor Spirits allow a choice between Magician and Adept aspects. Need to get support for picking these implemented.
- ImprovementManager needs support for <adeptpower> which increases the Rating for an Adept Power.
- Copy Protection, Registration, and Optimization ProgramOptions should be removed if they are no longer part of SR5.

I haven't taken a good hard look through the SR5 book so there's a chance I might have missed things here-and-there though I believe I have most of the basics covered. The things I know are missing:
- Vehicle class does not have support for Seats yet
- Magical equipment like Foci and Formulae
- Vehicles and Drones
- Remaining Cyberware
- Bioware
- Cyberdecks
- Programs
- Anything Matrix related
