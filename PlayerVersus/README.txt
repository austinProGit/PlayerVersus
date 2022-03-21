Project Title: PlayerVersus
Author: Austin Lee
Class: CPSC 3175 OOD
Professor: Dr. Obando
Description: Final project for the course. Fight your way
through sixteen rooms with multiple types of monsters and 
challenges.

To win: Your goal is simple: find and kill the dragon named Smaug. To
do so, you will have to fight your way through numerous other 
monsters of differing types. Watch out for the zombies; they move
fast. 
To lose: If you are killed by a monster, you lose the game. Watch
your health dilligently!

Monsters:
	Vampire: Normal health, high attack, immobile.
	Troll:   High health, high attack, immobile. 
	Zombie:  Low health, medium attack, extremely mobile.
	Fairies: Low health, low attack, and immobile.
	Werewolf:Normal health, normal attack, immobile.
	Ork:     Normal health, normal attack, immobile.
	Ogre:    Normal health, normal attack, immobile.
	Hyde:    High health, high attack, mobile. Stay abreast of his location!
	Cyclops: High health, low attack, immobile.
	Vampire: Normal health, high attack, immobile.
	Wizard:  Immortal and immobile but cunning.
	Dragon:  High health, extremely high attack, immobile.

	Player Stats: Health, Inventory, Current Weight,
	Current Volume

	Items in Game:
	rifle
	rifleAmmo
	medKit
	key
	various containers

Player Commands:
	1.  help: displays game help info
	2.  quit: quits the game
	3.  close: closes a door
	4.  drop: drops an item
	5.  fire: fires a weapon (currently the only weapon is a rifle)
	6.  go: takes the player to a new room
	7.  inspects: inspects an item and displays relevant info
	8.  load: loads a weapon with the correct ammunition
	9.  open: opens a door if the door is unlocked
	10. open-container: opens a container
	11. remove: removes an item from a container
	12. say: makes the player say whatever is typed in the console
	13. inventory: displays current inventory and other important stats
	14. take: allows the player to take an item
	15. unlock: unlocks a door if the player has a key
	16. use: allows the player to use a medKit
	17. retrace: allows the user to retrace his steps

Design Patterns Used:
	1.	Command pattern: creates all commands
	2.	Singleton: creates a single gameworld that can be easily accessed and modified
	3.	Container: allows us to create containers of items that we drop in the game world
	4:	Observer: allows us to send and receive notifications that events have occurred.
		In PlayerVersus, used to track the location of the Hyde monster and use the timer
		to determine when the monsters will move.
	5.	Delegate: allows for the creation of two trap rooms.
	6.	Decorator: used to customize the rifle with attachments
	7.	Strategy: allows us to create locks on the doors to rooms and keys to open the locks

Base Functionality Checklist:
	Has numerous locations.
	Player can move through locations.
	Items are in rooms.
	sao;difnaposdfniao;sdfn

Challenge Tasks:
	Add characters to game
	Characters interact with player (monsters attack player)
	Locked doors with keys
	Add volume to items
	Create weapons

Ongoing Bugs/ Places to Improve:
	Because the itemContainers use a dictionary as a data structure (which uses a unique key/value pair to store data), 
	my containers are only able to hold one object of a given type (ex: one rifleAmmo in a chest). I believe that the 
	way to fix this would be to somehow differentiate the keys such that they are unique for each created item. For
	example, I could increment a counter and append it to each item as I add it into the dictionary. The second option,
	of course, would be to go back and completely redesign the itemContainers using a more fitting data structure.

	When a monster enters a room while the user is in the room, the user is not notified until the monster actually 
	attacks. It would be preferable for the user to hear about that immediately.