﻿var game: Game;
var currentState;
class Game extends Phaser.Game {
    
    constructor() {
        // init game
        currentState = new GameState();
        super(1024, 768, Phaser.WEBGL, "main", currentState);
        
    }
}

class GameState extends Phaser.State {

    game: Phaser.Game;
    map: Phaser.Tilemap;
    cursors: Phaser.CursorKeys;
    players: Player[];
    currentPlayer: Player;
    colLayer:Phaser.TilemapLayer;
    bgLayer:Phaser.TilemapLayer;
    objLayer: Phaser.TilemapLayer;
    overlay: Phaser.Sprite;
    filter: Phaser.Filter;
    dayNightCycle: DayNightCycle;
    preload() {
        this.stage.disableVisibilityChange = true;
        this.game.stage.disableVisibilityChange = true;
        this.game.load.image("player", "Content/Game/Images/player.png");
        this.game.load.tilemap("Map", "Content/Game/Maps/map7.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/tileset3.png");
        this.game.load.image("Overlay", "Content/Game/Images/filter.png");
        
    }

    create() {
        hub.invoke("GetAllPlayers");
        
        this.game.physics.startSystem(Phaser.Physics.ARCADE);

        this.map = this.game.add.tilemap("Map");

        this.map.addTilesetImage("tilset3", "Tiles");

        this.bgLayer = this.map.createLayer("Calque de Tile 1");
       
        this.bgLayer.resizeWorld();


        //this.colLayer = this.map.createLayer("collisionLayer");
        //this.colLayer.alpha = 0;
        //this.map.setCollisionBetween(1, 2500, true, this.colLayer);

        //this.objLayer = this.map.createLayer("objectsLayer");

        this.cursors = this.game.input.keyboard.createCursorKeys();

        this.players = new Array<Player>();

        this.currentPlayer = new Player(this.game, userPseudo, new Phaser.Point(10, 10));

        this.dayNightCycle = new DayNightCycle(this.game);
        this.dayNightCycle.create();
        hub.invoke("update");
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);

    }

    update() {
        this.dayNightCycle.update();

        //this.game.physics.arcade.collide(this.currentPlayer.texture, this.colLayer);

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
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
    }

    removePlayer(pseudo: string) {
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].remove();
                this.players.splice(i, 1);
                break;
            }
        }
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
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

hub.on("updateTime", function (isDay) {
    currentState.dayNightCycle.isDay = isDay;
});

hub.on("MessageReceived", function (msg) {
    
    var u = currentState.getPlayer(msg.UserName);
    if (msg.UserName == currentState.currentPlayer.pseudo) u = currentState.currentPlayer;
    if (u != undefined) {
        u.setTextMessage(msg.Content);
    }
});

setInterval(function () {
    if (isConnected) {
        hub.invoke("update");
    }

}, 10000);



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