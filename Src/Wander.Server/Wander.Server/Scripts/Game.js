/// <reference path="UserInteraction.ts"/>
var game;
function createGame() {
    game = new GameLauncher();
}
function deleteGame() {
    game.game.destroy();
}
var GameLauncher = (function () {
    function GameLauncher() {
        var _this = this;
        this.create = function () {
            _this.mainGame = new MainGame(_this.game);
        };
        this.update = function () {
            if (_this.mainGame != undefined) {
                _this.mainGame.update();
            }
        };
        this.render = function () {
            if (_this.mainGame != undefined) {
                _this.mainGame.render();
            }
        };
        this.game = new Phaser.Game(800, 500, Phaser.AUTO, 'main', { preload: this.preload, create: this.create, update: this.update, render: this.render });
    }
    GameLauncher.prototype.preload = function () {
        this.game.load.image('player', 'Content/Game/player.png');
        this.game.stage.disableVisibilityChange = true;
    };
    return GameLauncher;
})();
var Player = (function () {
    function Player(game, pseudo, position) {
        var _this = this;
        this.update = function () {
            _this.texture.x = _this.position.x;
            _this.texture.y = _this.position.y;
            _this.text.x = _this.texture.x;
            _this.text.y = _this.texture.y - 20;
        };
        this.remove = function () {
            _this.text.kill();
            _this.texture.kill();
        };
        this.updatePosition = function () {
            _this.startTime = new Date().getTime();
            var time = _this.startTime - _this.endTime;
            if (time >= 15) {
                _this.endTime = _this.startTime;
                hub.invoke("MoveTo", { X: _this.position.x, Y: _this.position.y });
            }
        };
        this.texture = game.add.sprite(200, 200, "player");
        this.pseudo = pseudo;
        this.position = position;
        this.style = { font: "16px Arial", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width, align: "center" };
        this.text = game.add.text(0, 0, pseudo, this.style);
        this.startTime = new Date().getTime();
        this.endTime = new Date().getTime();
    }
    return Player;
})();
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
                    _this.players[i].position = position;
                    break;
                }
            }
        };
        this.update = function () {
            for (var i = 0; i < _this.players.length; i++) {
                _this.players[i].update();
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
//# sourceMappingURL=Game.js.map