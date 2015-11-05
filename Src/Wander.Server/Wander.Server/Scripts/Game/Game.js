/// <reference path="../UserInteraction.ts"/>
/// <reference path="Player.ts"/>
/// <reference path="GameLauncher.ts"/>
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
        this.update = function () {
            for (var i = 0; i < _this.players.length; i++) {
                _this.players[i].update();
                _this.players[i].updateServer();
            }
            _this.currentPlayer.update();
            if (_this.game.input.keyboard.isDown(Phaser.Keyboard.LEFT)) {
                _this.currentPlayer.position.x -= 4;
                _this.currentPlayer.updatePosition();
            }
            else if (_this.game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)) {
                _this.currentPlayer.position.x += 4;
                _this.currentPlayer.updatePosition();
            }
            if (_this.game.input.keyboard.isDown(Phaser.Keyboard.UP)) {
                _this.currentPlayer.position.y -= 4;
                _this.currentPlayer.updatePosition();
            }
            else if (_this.game.input.keyboard.isDown(Phaser.Keyboard.DOWN)) {
                _this.currentPlayer.position.y += 4;
                _this.currentPlayer.updatePosition();
            }
        };
        this.render = function () {
        };
        hub.invoke("GetAllPlayers");
        this.game = game;
        this.players = new Array();
        this.currentPlayer = new Player(game, userPseudo, new Phaser.Point(10, 10));
    }
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
//# sourceMappingURL=Game.js.map