# Wander

Wander is an Open-Source online multiplayer game based on C# and Javascript.

 This game will alow users to wander freely in a 2D environment, to practice a virtual job, to buy a house and to tchat with other players.

----------

**Money system**
-------------
	Each player will have an income which will be delivered every hour as long as they're connected to the server.

	The earned money will allow you to :
	- Buy houses and appartments
	- Buy in-game utilities
	- Participate to events

**No real money will ever be involved in the game !**

----------

**Rules**
-------------
	- Don't insult other players
	- Don't attempt to hack or bypass the security of the game

----------

**Developers notes**
-------------
	Wander is an open-source projet, you can host your own game server and modify it as you wish.
	
	To do so :

	- Download the last version of the project on GitHub
	- Execute the create.sql file in an SqlServer database
	- Replace the default connection string in the SqlConnectionService class
	- OR Create an environment variable named "DB_CONNECTION_STRING" with your connection string in value
	- Build the solution
	
	And voila :) You're ready to go


**Team**
-------------
- **Valentin Chatelain** - Developer, project leader
- **Rami Morri** - Developer
