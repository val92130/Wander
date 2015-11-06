var game;
var MainGame = (function () {
    function MainGame(game) {
        var _this = this;
        this.addPlayer = function (pseudo, position) {
            var flag = false;
            for (var i = 0; i < _this.players.length; i++) {
                if (_this.players[i].pseudo == pseudo) {
                    flag = true;
                    break;
                }
            }
            if (!flag) {
                _this.players.push(new Player(_this.game, pseudo, position));
            }
        };
        this.removePlayer = function (pseudo) {
            for (var i = 0; i < _this.players.length; i++) {
                if (_this.players[i].pseudo == pseudo) {
                    _this.players[i].remove();
                    _this.players.splice(i, 1);
                    break;
                }
            }
        };
        this.updatePlayer = function (pseudo, position) {
            for (var i = 0; i < _this.players.length; i++) {
                if (_this.players[i].pseudo == pseudo) {
                    _this.players[i].newPosition = position;
                    break;
                }
            }
        };
        hub.invoke("GetAllPlayers");
        this.game = game;
        this.players = new Array();
        this.currentPlayer = new Player(game, userPseudo, new Phaser.Point(10, 10));
    }
    MainGame.prototype.update = function () {
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
    MainGame.prototype.render = function () {
        this.game.debug.cameraInfo(this.game.camera, 32, 32);
    };
    return MainGame;
})();
hub.on("playerConnected", function (player) {
    game.mainGame.addPlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
    $.notify("player connected : " + player.Pseudo, "warn");
});
hub.on("playerDisconnected", function (player) {
    game.mainGame.removePlayer(player.Pseudo);
    $.notify("player disconnected : " + player.Pseudo, "error");
});
hub.on("playerMoved", function (player) {
    game.mainGame.updatePlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
});
function createGame() {
    game = new GameLauncher();
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
