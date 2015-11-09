var game: WanderGame;
var currentGame: any;

class WanderGame {
    
    game: Phaser.Game;
    map: Phaser.Tilemap;
    cursors: Phaser.CursorKeys;
    players: Player[];
    currentPlayer : Player;

    constructor() {
        
        this.game = new Phaser.Game(1024, 768, Phaser.AUTO, 'main', {
            create: this.create, preload:
            this.preload, render: this.render, update:this.update
        }); 
    }

    preload() {
        this.game.stage.disableVisibilityChange = true;
        this.game.load.image("player", "Content/Game/Images/player.png");
        this.game.load.tilemap("Map", "Content/Game/Maps/wander_map.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/wander_tileset.png");
        
    }

    render() {
        
    }

    create() {
        hub.invoke("GetAllPlayers");
        currentGame = this;
        this.map = this.game.add.tilemap("Map");
        this.map.addTilesetImage("wander_tileset", "Tiles");

        this.map.createLayer("backgroundLayer").resizeWorld();
        this.map.createLayer("collisionLayer");
        
        this.cursors = this.game.input.keyboard.createCursorKeys();

        this.players = new Array<Player>();

        this.currentPlayer = new Player(this.game, userPseudo, new Phaser.Point(10, 10));

    }

    update() {

        for (var i = 0; i < this.players.length; i++) {
            this.players[i].update();
            this.players[i].updateServer();
        }

        this.currentPlayer.update();

        if (this.game.input.keyboard.isDown(Phaser.Keyboard.LEFT)) {
            this.currentPlayer.move(EDirection.Left);
            this.currentPlayer.updatePosition();
        }
        else if (this.game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)) {
            this.currentPlayer.move(EDirection.Right);
            this.currentPlayer.updatePosition();
        }

        if (this.game.input.keyboard.isDown(Phaser.Keyboard.UP)) {
            this.currentPlayer.move(EDirection.Up);
            this.currentPlayer.updatePosition();
        }
        else if (this.game.input.keyboard.isDown(Phaser.Keyboard.DOWN)) {
            this.currentPlayer.move(EDirection.Down);
            this.currentPlayer.updatePosition();
        }

    }

    addPlayer(game:any, pseudo: string, position: Phaser.Point) {

        var flag: Boolean = false;
        for (var i = 0; i < game.players.length; i++) {
            if (game.players[i].pseudo == pseudo) {
                flag = true;
                break;
            }
        }

        if (!flag) {
            game.players.push(new Player(game, pseudo, position));
        }
    }

    removePlayer(game: any, pseudo: string) {
        for (var i = 0; i < game.players.length; i++) {
            if (game.players[i].pseudo == pseudo) {
                game.players[i].remove();
                game.players.splice(i, 1);
                break;
            }
        }
    }

    updatePlayer(game: any, pseudo: string, position: Phaser.Point) {
        for (var i = 0; i < game.players.length; i++) {
            if (game.players[i].pseudo == pseudo) {
                game.players[i].newPosition = position;
                break;
            }
        }
    }

    getPlayer(game: any, pseudo: string) {
        for (var i = 0; i < game.players.length; i++) {
            if (game.players[i].pseudo == pseudo) {
                return game.players[i];
            }
        }
        return undefined;
    }
}

hub.on("MessageReceived", function (msg) {
    
});

hub.on("playerConnected", function (player) {
    game.addPlayer(currentGame, player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
    $.notify("player connected : " + player.Pseudo, "warn");
});

hub.on("playerDisconnected", function (player) {
    game.removePlayer(currentGame,player.Pseudo);
    $.notify("player disconnected : " + player.Pseudo, "error");
});

hub.on("playerMoved", function (player) {
    game.updatePlayer(currentGame,player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
});


function createGame() {
    game = new WanderGame();
}

function deleteGame() {
    game.game.destroy();
}


function Lerp(goal, current, time) {
    var diff = goal - current;

    if (diff > time) {
        return current + time;
    }
    if (diff < -time) {
        return current - time;
    }
    return goal;
}