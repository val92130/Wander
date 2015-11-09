var game;
var currentGame;
var WanderGame = (function () {
    function WanderGame() {
        this.t = 10;
        this.game = new Phaser.Game(1024, 768, Phaser.AUTO, 'main', {
            create: this.create, preload: this.preload, render: this.render, update: this.update
        });
    }
    WanderGame.prototype.preload = function () {
        this.game.stage.disableVisibilityChange = true;
        this.game.load.image("player", "Content/Game/Images/player.png");
        this.game.load.tilemap("Map", "Content/Game/Maps/wander_map.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/wander_tileset.png");
    };
    WanderGame.prototype.render = function () {
    };
    WanderGame.prototype.create = function () {
        console.log(this.t);
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
        this.players = new Array();
        this.currentPlayer = new Player(this.game, userPseudo, new Phaser.Point(10, 10));
    };
    WanderGame.prototype.update = function () {
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
    WanderGame.prototype.addPlayer = function (game, pseudo, position) {
        var flag = false;
        for (var i = 0; i < game.players.length; i++) {
            if (game.players[i].pseudo == pseudo) {
                flag = true;
                break;
            }
        }
        if (!flag) {
            game.players.push(new Player(game, pseudo, position));
        }
    };
    WanderGame.prototype.removePlayer = function (game, pseudo) {
        for (var i = 0; i < game.players.length; i++) {
            if (game.players[i].pseudo == pseudo) {
                game.players[i].remove();
                game.players.splice(i, 1);
                break;
            }
        }
    };
    WanderGame.prototype.updatePlayer = function (game, pseudo, position) {
        for (var i = 0; i < game.players.length; i++) {
            if (game.players[i].pseudo == pseudo) {
                game.players[i].newPosition = position;
                break;
            }
        }
    };
    WanderGame.prototype.getPlayer = function (game, pseudo) {
        for (var i = 0; i < game.players.length; i++) {
            if (game.players[i].pseudo == pseudo) {
                return game.players[i];
            }
        }
        return undefined;
    };
    return WanderGame;
})();
hub.on("MessageReceived", function (msg) {
});
hub.on("playerConnected", function (player) {
    game.addPlayer(currentGame, player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
    $.notify("player connected : " + player.Pseudo, "warn");
});
hub.on("playerDisconnected", function (player) {
    game.removePlayer(currentGame, player.Pseudo);
    $.notify("player disconnected : " + player.Pseudo, "error");
});
hub.on("playerMoved", function (player) {
    game.updatePlayer(currentGame, player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
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
//# sourceMappingURL=MainGame.js.map