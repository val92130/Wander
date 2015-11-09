var game: Game;
var currentGame: any;
var currentState;
class Game extends Phaser.Game {
    
    constructor() {
        // init game
        currentState = new GameState();
        super(1024, 768, Phaser.CANVAS, "main", currentState);
        
    }
}

class GameState extends Phaser.State {

    game: Phaser.Game;
    map: Phaser.Tilemap;
    cursors: Phaser.CursorKeys;
    players: Player[];
    currentPlayer: Player;

    preload() {
        this.stage.disableVisibilityChange = true;
        this.game.stage.disableVisibilityChange = true;
        this.game.load.image("player", "Content/Game/Images/player.png");
        this.game.load.tilemap("Map", "Content/Game/Maps/wander_map.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/wander_tileset.png");
    }

    create() {
        hub.invoke("GetAllPlayers");
        currentGame = this;
        this.map = this.game.add.tilemap("Map");
        this.map.addTilesetImage("wander_tileset", "Tiles");

        var bg = this.map.createLayer("backgroundLayer");
        bg.setScale(2, 2);
        bg.resizeWorld();


        var col = this.map.createLayer("collisionLayer");
        col.setScale(2, 2);

        this.cursors = this.game.input.keyboard.createCursorKeys();

        this.players = new Array<Player>();

        this.currentPlayer = new Player(this.game, userPseudo, new Phaser.Point(10, 10));

    }

    update() {

        var camX = Math.floor(this.currentPlayer.position.x / this.game.camera.width);
        var camY = Math.floor(this.currentPlayer.position.y / this.game.camera.height);

        this.game.camera.x = Lerp(camX * this.game.camera.width, this.game.camera.x, 20);
        this.game.camera.y = Lerp(camY * this.game.camera.height, this.game.camera.y, 20);

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

    addPlayer(pseudo: string, position: Phaser.Point) {

        var flag: Boolean = false;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                flag = true;
                break;
            }
        }

        if (!flag) {
            this.players.push(new Player(game, pseudo, position));
        }
    }

    removePlayer(pseudo: string) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].remove();
                this.players.splice(i, 1);
                break;
            }
        }
    }

    updatePlayer(pseudo: string, position: Phaser.Point) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].newPosition = position;
                break;
            }
        }
    }

    getPlayer(pseudo: string) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                return this.players[i];
            }
        }
        return undefined;
    }
}



hub.on("MessageReceived", function (msg) {
    
});

hub.on("playerConnected", function (player) {
    currentState.addPlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
    $.notify("player connected : " + player.Pseudo, "warn");
});

hub.on("playerDisconnected", function (player) {
    currentState.removePlayer(player.Pseudo);
    $.notify("player disconnected : " + player.Pseudo, "error");
});

hub.on("playerMoved", function (player) {
    currentState.updatePlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
});



function createGame() {
    game = new Game();
}

function deleteGame() {
    game.destroy();
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