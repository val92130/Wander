/// <reference path="../UserInteraction.ts"/>
/// <reference path="Player.ts"/>
/// <reference path="GameLauncher.ts"/>

var game: any;

class MainGame {

    players: Player[];
    game: Phaser.Game;
    currentPlayer: Player;

    constructor(game: Phaser.Game) {
        hub.invoke("GetAllPlayers");
        this.game = game;
        this.players = new Array<Player>();

        this.currentPlayer = new Player(game, userPseudo, new Phaser.Point(10, 10));
    }

    addPlayer = (pseudo: string, position: Phaser.Point) => {

        var flag: Boolean = false;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                flag = true;
                break;
            }
        }

        if (!flag) {
            this.players.push(new Player(this.game, pseudo, position));
        }
    }

    removePlayer = (pseudo: string) => {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].remove();
                this.players.splice(i, 1);
                break;
            }
        }
    }

    updatePlayer = (pseudo: string, position: Phaser.Point) => {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].newPosition = position;
                break;
            }
        }
    }

    update = () => {
        for (var i = 0; i < this.players.length; i++) {
            this.players[i].update();
            this.players[i].updateServer();
        }

        this.currentPlayer.update();

        if (this.game.input.keyboard.isDown(Phaser.Keyboard.LEFT)) {
            this.currentPlayer.position.x -= 4;
            this.currentPlayer.updatePosition();
        }
        else if (this.game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)) {
            this.currentPlayer.position.x += 4;
            this.currentPlayer.updatePosition();
        }

        if (this.game.input.keyboard.isDown(Phaser.Keyboard.UP)) {
            this.currentPlayer.position.y -= 4;
            this.currentPlayer.updatePosition();
        }
        else if (this.game.input.keyboard.isDown(Phaser.Keyboard.DOWN)) {
            this.currentPlayer.position.y += 4;
            this.currentPlayer.updatePosition();
        }
    }

    render = () => {

    }
}


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
