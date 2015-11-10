var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var game;
var currentState;
var Game = (function (_super) {
    __extends(Game, _super);
    function Game() {
        // init game
        currentState = new GameState();
        _super.call(this, 1024, 768, Phaser.CANVAS, "main", currentState);
    }
    return Game;
})(Phaser.Game);
var GameState = (function (_super) {
    __extends(GameState, _super);
    function GameState() {
        _super.apply(this, arguments);
    }
    GameState.prototype.preload = function () {
        this.stage.disableVisibilityChange = true;
        this.game.stage.disableVisibilityChange = true;
        this.game.load.image("player", "Content/Game/Images/player.png");
        this.game.load.tilemap("Map", "Content/Game/Maps/map.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/wander_tileset.png");
    };
    GameState.prototype.create = function () {
        hub.invoke("GetAllPlayers");
        this.game.physics.startSystem(Phaser.Physics.ARCADE);
        this.map = this.game.add.tilemap("Map");
        this.map.addTilesetImage("wander_tileset", "Tiles");
        this.bgLayer = this.map.createLayer("backgroundLayer");
        this.bgLayer.resizeWorld();
        this.colLayer = this.map.createLayer("collisionLayer");
        this.colLayer.alpha = 0;
        this.map.setCollisionBetween(1, 2500, true, this.colLayer);
        this.objLayer = this.map.createLayer("objectsLayer");
        this.cursors = this.game.input.keyboard.createCursorKeys();
        this.players = new Array();
        this.currentPlayer = new Player(this.game, userPseudo, new Phaser.Point(10, 10));
    };
    GameState.prototype.update = function () {
        this.game.physics.arcade.collide(this.currentPlayer.texture, this.colLayer);
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
    };
    GameState.prototype.addPlayer = function (pseudo, position) {
        var flag = false;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                flag = true;
                break;
            }
        }
        if (!flag) {
            this.players.push(new Player(game, pseudo, position));
        }
    };
    GameState.prototype.removePlayer = function (pseudo) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].remove();
                this.players.splice(i, 1);
                break;
            }
        }
    };
    GameState.prototype.updatePlayer = function (pseudo, position) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].newPosition = position;
                break;
            }
        }
    };
    GameState.prototype.getPlayer = function (pseudo) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                return this.players[i];
            }
        }
        return undefined;
    };
    return GameState;
})(Phaser.State);
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
//# sourceMappingURL=MainGame.js.map