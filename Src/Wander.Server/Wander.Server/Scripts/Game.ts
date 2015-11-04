/// <reference path="UserInteraction.ts"/>

var game: any;

function createGame() {
    game = new GameLauncher();
}

function deleteGame() {
    game.game.destroy();
}

class GameLauncher {
    
    mainGame: MainGame;
    game: Phaser.Game;

    constructor() {
        this.game = new Phaser.Game(800, 500, Phaser.AUTO, 'main', { preload: this.preload, create: this.create, update: this.update, render:this.render });

    }

    preload() {
        this.game.load.image('player', 'Content/Game/player.png');
        this.game.stage.disableVisibilityChange = true;
    }

    create = () => {
        this.mainGame = new MainGame(this.game);
        
    }

    update = () =>  {
        if (this.mainGame != undefined) {
            this.mainGame.update();
        }
        
    }

    render = () => {
        if (this.mainGame != undefined) {
            this.mainGame.render();
        }
    }

}

class Player {
    
    position: Phaser.Point;
    pseudo: string;
    texture: Phaser.Sprite;

    style :any ;

    text :any;

    startTime: any;
    endTime: any;

    constructor(game:Phaser.Game, pseudo:string, position:Phaser.Point) {
        this.texture = game.add.sprite(200, 200, "player");
        this.pseudo = pseudo;
        this.position = position;
        this.style = { font: "16px Arial", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width, align: "center" };
        this.text = game.add.text(0, 0, pseudo, this.style);
        this.startTime = new Date().getTime();

        this.endTime = new Date().getTime();
    }

    update = () => {
        this.texture.x = this.position.x;
        this.texture.y = this.position.y;

        this.text.x = this.texture.x;
        this.text.y = this.texture.y - 20;
    }

    remove = () => {
        this.text.kill();
        this.texture.kill();
        
    }

    updatePosition = () => {
        this.startTime = new Date().getTime();
        var time = this.startTime - this.endTime;
        if (time >= 15) {
            this.endTime = this.startTime;
            hub.invoke("MoveTo", { X: this.position.x, Y: this.position.y });
        }
        
       
    }

}

class MainGame {
    
    players: Player[];
    game:Phaser.Game;
    currentPlayer: Player;

    constructor(game: Phaser.Game) {
        hub.invoke("GetAllPlayers");
        this.game = game;
        this.players = new Array<Player>();

        this.currentPlayer = new Player(game, userPseudo, new Phaser.Point(10, 10));
    }

    addPlayer = (pseudo: string, position: Phaser.Point) => {

        var flag:Boolean = false;
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
                this.players[i].position = position;
                break;
            }
        }
    }

    update = () => {
        for (var i = 0; i < this.players.length; i++) {
            this.players[i].update();
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
