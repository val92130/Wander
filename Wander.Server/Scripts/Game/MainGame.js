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
        _super.call(this, "100%", "100%", Phaser.AUTO, "main-game", currentState);
    }
    return Game;
})(Phaser.Game);
var GameState = (function (_super) {
    __extends(GameState, _super);
    function GameState() {
        _super.apply(this, arguments);
    }
    GameState.prototype.preload = function () {
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
        hub.invoke("GetMoneyBags").done(function (bags) {
            for (var i = 0; i < bags.length; i++) {
                currentState.map.addMoneyBag(bags[i].Id, bags[i].Position, bags[i].Ammount);
            }
        });
        this.rainEmiter = this.game.add.emitter(game.world.centerX, 0, 400);
        this.rainEmiter.width = this.game.world.width;
        this.rainEmiter.makeParticles('rain');
        this.rainEmiter.minParticleScale = 0.1;
        this.rainEmiter.maxParticleScale = 0.5;
        this.rainEmiter.setYSpeed(300, 500);
        this.rainEmiter.setXSpeed(-5, 5);
        this.rainEmiter.minRotation = 0;
        this.rainEmiter.maxRotation = 0;
        this.rainEmiter.start(false, 1600, 2, 0);
        hub.invoke("IsRaining").done(function (val) {
            currentState.setRain(val);
        });
    };
    GameState.prototype.pressAction = function () {
        var y = Math.round(this.currentPlayer.texture.y / (this.map.tilemap.tileHeight * this.map.scale));
        var x = Math.round(this.currentPlayer.texture.x / (this.map.tilemap.tileWidth * this.map.scale));
        var tile = this.map.tilemap.getTile(x, y, "houseLayer");
        if (tile != undefined) {
            var propId = tile.properties.propertyId;
            if (propId != undefined) {
                openModalProperty(propId);
            }
            else {
                var isMairie = tile.properties.Mairie;
                if (isMairie != undefined) {
                    getAllJobs();
                }
            }
        }
    };
    GameState.prototype.setRain = function (val) {
        this.rainEmiter.on = val;
    };
    GameState.prototype.update = function () {
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
    };
    GameState.prototype.addPlayer = function (pseudo, position) {
        if (this.players == undefined)
            return;
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
        if (this.players == undefined)
            return;
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
        if (this.players == undefined)
            return;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                this.players[i].newPosition = position;
                this.players[i].direction = direction;
                break;
            }
        }
    };
    GameState.prototype.getPlayer = function (pseudo) {
        if (this.players == undefined)
            return;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                return this.players[i];
            }
        }
        return undefined;
    };
    GameState.prototype.resizeGame = function () {
        var height = $(".game").height();
        var width = $(".game").width();
        this.game.width = width;
        this.game.height = height;
        this.game.stage.width = width;
        this.game.stage.height = height;
        if (this.game.renderType === Phaser.WEBGL) {
            this.game.renderer.resize(width, height);
        }
    };
    return GameState;
})(Phaser.State);
hub.on("setRain", function (rain) {
    if (currentState == undefined)
        return;
    currentState.setRain(rain);
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
hub.on("addMoneyBag", function (moneyBag) {
    if (currentState != undefined) {
        currentState.map.addMoneyBag(moneyBag.Id, moneyBag.Position, moneyBag.Ammount);
    }
});
setInterval(function () {
    if (isConnected) {
        hub.invoke("update");
    }
}, 10000);
$(window).resize(function () {
    if (currentState != undefined)
        currentState.resizeGame();
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
            if (currentUser == "unedfined" || currentUser == null)
                return;
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
//# sourceMappingURL=MainGame.js.map