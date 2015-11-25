﻿

var game: Game;
var currentState;

class Game extends Phaser.Game {

    constructor() {
        // init game
        currentState = new GameState();
        super("100%", "100%", Phaser.AUTO, "main-game", currentState);
    }
}

class GameState extends Phaser.State {
    map: Map;
    game: Phaser.Game;
    players: Player[];
    currentPlayer: Player;
    overlay: Phaser.Sprite;
    filter: Phaser.Filter;
    dayNightCycle: DayNightCycle;
    rainEmiter: Phaser.Particles.Arcade.Emitter;


    preload() {

        this.game.canvas.id = "canvas";
        this.game.scale.scaleMode = Phaser.ScaleManager.RESIZE;
        this.stage.disableVisibilityChange = true;
        this.game.stage.disableVisibilityChange = true;
        this.game.load.image("player", "Content/Game/Images/player.png");
        this.game.load.tilemap("Map", "Content/Game/Maps/map2.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/tileset3.png");
        this.game.load.image("Overlay", "Content/Game/Images/filter.png");
        this.game.load.image("money-bag", "Content/Game/Images/money_bag.png");
        this.map = new Map(this.game, "Map", "Tiles", "tileset3", 1);
        this.game.load.spritesheet('rain', 'Content/Game/Images/rain.png', 17, 17);

        
    }

    create() {
        hub.invoke("GetAllPlayers");


        this.game.physics.startSystem(Phaser.Physics.ARCADE);

        this.map.create();

        this.players = new Array<Player>();

        this.currentPlayer = new Player(this.game, userPseudo, new Phaser.Point(10, 10));

        this.dayNightCycle = new DayNightCycle(this.game);
        this.dayNightCycle.create();
        hub.invoke("update");
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
        this.game.world.bringToTop(currentState.dayNightCycle.rainOverlay);

        hub.invoke("GetMoneyBags").done(function(bags) {
            for (var i = 0; i < bags.length; i++) {
                currentState.map.addMoneyBag(bags[i].Id, bags[i].Position, bags[i].Ammount);
            }
        });

        this.rainEmiter = this.game.add.emitter(game.world.centerX, 0, 400);
        this.rainEmiter.width = this.game.world.width;
        this.rainEmiter.makeParticles('rain');

        this.rainEmiter.minParticleScale = 0.5;
        this.rainEmiter.maxParticleScale = 1.2;

        this.rainEmiter.setYSpeed(300, 500);
        this.rainEmiter.setXSpeed(-5, 5);

        this.rainEmiter.minRotation = 0;
        this.rainEmiter.maxRotation = 0;

        this.rainEmiter.start(false, 1600, 2, 0);

        hub.invoke("IsRaining").done(function (val) {
            currentState.setRain(val);
        })
        
    }

    pressAction() {

        var y = Math.round(this.currentPlayer.texture.y / (this.map.tilemap.tileHeight * this.map.scale));
        var x = Math.round(this.currentPlayer.texture.x / (this.map.tilemap.tileWidth * this.map.scale));

        var tile = this.map.tilemap.getTile(x, y, "houseLayer");
        if (tile != undefined) {
            var propId = tile.properties.propertyId;
            if (propId != undefined) {
                openModalProperty(propId);
            } else {
                var isMairie = tile.properties.Mairie;
                if (isMairie != undefined) {
                    getAllJobs();
                }
            }
        }
    }

    setRain(val: boolean) {
        this.rainEmiter.on = val;
    }

    isRaining() :boolean {
        return this.rainEmiter.on;
    }


    update() {
        this.dayNightCycle.update();

        this.game.physics.arcade.collide(this.currentPlayer.texture, this.map.collisionLayer);

        var camX = Math.floor(this.currentPlayer.position.x / this.game.camera.width);
        var camY = Math.floor(this.currentPlayer.position.y / this.game.camera.height);

        this.game.camera.x = Lerp(camX * this.game.camera.width, this.game.camera.x, 40);
        this.game.camera.y = Lerp(camY * this.game.camera.height, this.game.camera.y, 40);

        this.rainEmiter.position.x = camX * this.game.camera.width;
        this.rainEmiter.position.y = camY * this.game.camera.height;

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
        if (this.players == undefined) return;
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
        this.game.world.bringToTop(currentState.dayNightCycle.rainOverlay);
    }

    removePlayer(pseudo: string) {
        if (this.players == undefined) return;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].remove();
                this.players.splice(i, 1);
                break;
            }
        }
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
        this.game.world.bringToTop(currentState.dayNightCycle.rainOverlay);
    }

    updatePlayer(pseudo: string, position: Phaser.Point, direction: EDirection) {
        if (this.players == undefined) return;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].newPosition = position;
                this.players[i].direction = direction;
                break;
            }
        }
    }

    getPlayer(pseudo: string) {
         if (this.players == undefined) return;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                return this.players[i];
            }
        }
        return undefined;
    }

    resizeGame() {
        var height = $(".game").height();
        var width = $(".game").width();

        this.game.width = width;
        this.game.height = height;
        this.game.stage.width = width;
        this.game.stage.height = height;

        if (this.game.renderType === Phaser.WEBGL) {
            this.game.renderer.resize(width, height);
        }
    }
}

hub.on("setRain", function(rain)
{
    if(currentState == undefined) return;
    currentState.setRain(rain);
});

hub.on("playerConnected", function (player) {
    if (currentState == undefined) return;
    currentState.addPlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
    $.notify("player connected : " + player.Pseudo, "warn");
});

hub.on("playerDisconnected", function (player) {
    if (currentState == undefined) return;
    currentState.removePlayer(player.Pseudo);
    $.notify("player disconnected : " + player.Pseudo, "error");
});

hub.on("playerMoved", function (player) {
    if (currentState != undefined) {
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
    if (msg.UserName == currentState.currentPlayer.pseudo) u = currentState.currentPlayer;
    if (u != undefined) {
        u.setTextMessage(msg.Content);
    }
});

hub.on("addMoneyBag", function(moneyBag) {
    if (currentState != undefined) {
        currentState.map.addMoneyBag(moneyBag.Id,moneyBag.Position, moneyBag.Ammount);
    }
});

setInterval(function () {
    if (isConnected) {
        hub.invoke("update");
    }
}, 10000);


$(window).resize(function () {
    if(currentState != undefined) currentState.resizeGame();
});


function createGame() {
    $("#main-game").show();
    $("#main-container").hide();
    $(".overlay").fadeIn();
    game = new Game();
    $(".overlay").fadeOut();
}

function deleteGame() {
    game.destroy();
    $("#main-container").fadeIn();
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

function openModalProperty(id) {
    hub.invoke("GetPropertyInfo", id).done(function (model) {
        if (model != null && model != undefined) {
            if (currentUser == "unedfined" || currentUser == null) return;
            $("#propertyModalBody").text("");


            $("#propertyModalBody").append("<tr class='success'><td>" + model.PropertyName + "</td><td>" + model.PropertyDescription + "</td><td>" + model.Threshold + "</td> <td>" + model.Price + "</td><td><button type='button' onclick = 'BuyProperty(" + model.PropertyId + ")' class='btn btn-success' data-dismiss='modal'>Buy</button></tr>");

            $("#propertyModal").modal();
        }      
    });
}


function BuyProperty(id) {
    hub.invoke("BuyProperty", id);
    $("#propertyModal").modal('hide');
}

$(document).keypress(function (event) { 
    if (isConnected) {
        if ($('input:focus').size() == 0) {
            if (event.which == 101) {
                if (currentState.game != undefined) {
                    currentState.pressAction();
                }

            }
        }
    }


});