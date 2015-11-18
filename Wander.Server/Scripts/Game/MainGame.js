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
        _super.call(this, "100%", 768, Phaser.WEBGL, "main", currentState);
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
        this.game.load.tilemap("Map", "Content/Game/Maps/map2.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/tileset3.png");
        this.game.load.image("Overlay", "Content/Game/Images/filter.png");
        this.map = new Map(this.game, "Map", "Tiles", "tileset3", 1.5);
    };
    GameState.prototype.create = function () {
        hub.invoke("GetAllPlayers");
        this.game.physics.startSystem(Phaser.Physics.ARCADE);
        this.map.create();
        this.players = new Array();
        this.currentPlayer = new Player(this.game, userPseudo, new Phaser.Point(10, 10));
        this.dayNightCycle = new DayNightCycle(this.game);
        this.dayNightCycle.create();
        hub.invoke("update");
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
        this.game.input.keyboard.addKey(Phaser.Keyboard.E).onDown.add(this.pressAction, { _game: this });
    };
    GameState.prototype.pressAction = function () {
        var _this = this._game;
        var y = Math.round(_this.currentPlayer.texture.y / (_this.map.tilemap.tileHeight * _this.map.scale));
        var x = Math.round(_this.currentPlayer.texture.x / (_this.map.tilemap.tileWidth * _this.map.scale));
        var tile = _this.map.tilemap.getTile(x, y, "houseLayer");
        if (tile != undefined) {
            var propId = tile.properties.propertyId;
            if (propId != undefined) {
                console.log("propId : " + propId);
            }
        }
        console.log("x : " + x + " y : " + y);
    };
    GameState.prototype.update = function () {
        this.dayNightCycle.update();
        this.game.physics.arcade.collide(this.currentPlayer.texture, this.map.collisionLayer);
        var camX = Math.floor(this.currentPlayer.position.x / this.game.camera.width);
        var camY = Math.floor(this.currentPlayer.position.y / this.game.camera.height);
        this.game.camera.x = Lerp(camX * this.game.camera.width, this.game.camera.x, 40);
        this.game.camera.y = Lerp(camY * this.game.camera.height, this.game.camera.y, 40);
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
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
    };
    GameState.prototype.removePlayer = function (pseudo) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].remove();
                this.players.splice(i, 1);
                break;
            }
        }
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
    };
    GameState.prototype.updatePlayer = function (pseudo, position, direction) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].newPosition = position;
                this.players[i].direction = direction;
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
    GameState.prototype.resizeGame = function () {
        var width = $(window).width();
        this.game.width = width;
        this.game.stage.width = width;
        this.dayNightCycle.overlay.width = width;
        this.game.camera.width = width;
        if (this.game.renderType === Phaser.WEBGL) {
            this.game.renderer.resize(width, this.game.renderer.height);
        }
    };
    return GameState;
})(Phaser.State);
hub.on("MessageReceived", function (msg) {
});
hub.on("playerConnected", function (player) {
    if (currentState == undefined)
        return;
    currentState.addPlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
    $.notify("player connected : " + player.Pseudo, "warn");
});
hub.on("playerDisconnected", function (player) {
    if (currentState == undefined)
        return;
    currentState.removePlayer(player.Pseudo);
    $.notify("player disconnected : " + player.Pseudo, "error");
});
hub.on("playerMoved", function (player) {
    if (currentState != undefined) {
        console.log(player.Direction);
        currentState.updatePlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y), player.Direction);
    }
});
hub.on("updateTime", function (isDay) {
    if (currentState != undefined) {
        currentState.dayNightCycle.isDay = isDay;
    }
});
hub.on("MessageReceived", function (msg) {
    var u = currentState.getPlayer(msg.UserName);
    if (msg.UserName == currentState.currentPlayer.pseudo)
        u = currentState.currentPlayer;
    if (u != undefined) {
        u.setTextMessage(msg.Content);
    }
});
setInterval(function () {
    if (isConnected) {
        hub.invoke("update");
    }
}, 10000);
$(window).resize(function () { currentState.resizeGame(); });
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
