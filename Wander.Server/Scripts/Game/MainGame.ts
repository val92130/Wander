

var game: Game;
var currentState: GameState;


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
    overlay: Phaser.Sprite;
    filter: Phaser.Filter;
    dayNightCycle: DayNightCycle;
    rainEmiter: Phaser.Particles.Arcade.Emitter;
    soundManager: SoundManager;

    preload() {
        this.game.canvas.id = "canvas";
        this.game.scale.scaleMode = Phaser.ScaleManager.RESIZE;
        this.stage.disableVisibilityChange = true;
        this.game.stage.disableVisibilityChange = true;
        this.game.load.image("player", "Content/Game/Images/player.png");
        this.game.load.tilemap("Map", "Content/Game/Maps/map2.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/tileset3.png");
        this.game.load.image("Overlay", "Content/Game/Images/filter.png");
        this.game.load.image('light', 'Content/Game/Images/light.png');
        this.game.load.image('blurred-circle', 'Content/Game/Images/blurred-circle.png');

        this.map = new Map(this,this.game, "Map", "Tiles", "tileset3", 1);
        this.game.load.spritesheet('rain', 'Content/Game/Images/rain.png', 17, 17);
        this.soundManager = new SoundManager(this.game, this);
        this.soundManager.preload();

        this.game.load.script('gray', 'https://cdn.rawgit.com/photonstorm/phaser/master/filters/Gray.js');

    }

    create() {
        hub.invoke("GetAllPlayers");
        this.soundManager.create();
        this.game.physics.startSystem(Phaser.Physics.ARCADE);

        this.map.create();

        this.dayNightCycle = new DayNightCycle(this.game);
        this.dayNightCycle.create();
        hub.invoke("Update");
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
        this.game.world.bringToTop(currentState.dayNightCycle.rainOverlay);


        this.rainEmiter = this.game.add.emitter(game.world.centerX, 0, 400);
        this.rainEmiter.width = this.game.world.width;
        this.rainEmiter.makeParticles('rain');

        this.rainEmiter.minParticleScale = 0.5;
        this.rainEmiter.maxParticleScale = 1.2;

        this.rainEmiter.setYSpeed(450, 650);
        this.rainEmiter.setXSpeed(-5, 5);

        this.rainEmiter.minRotation = 0;
        this.rainEmiter.maxRotation = 0;

        this.rainEmiter.start(false, 1600, 2, 0);

        hub.invoke("IsRaining").done(function (val) {
            currentState.setRain(val);
        })




    }

    pressAction() {

        var y = Math.round(this.map.currentPlayer.texture.y / (this.map.tilemap.tileHeight * this.map.scale));
        var x = Math.round(this.map.currentPlayer.texture.x / (this.map.tilemap.tileWidth * this.map.scale));

        var tile = this.map.tilemap.getTile(x, y, "houseLayer");

        var curPlayerBounds:any = this.map.currentPlayer.texture.getBounds();
        for (var i = 0; i < this.map.players.length; i++) {
            var pBounds:any = this.map.players[i].texture.getBounds();
            if (Phaser.Rectangle.intersects(curPlayerBounds, pBounds)) {
                console.log("intersect with player : " + this.map.players[i].pseudo);
                hub.invoke("CheckIfDrugDealer", this.map.players[i].pseudo).done(function(res) {
                    if (res) {
                        openModalDrugs(currentState.map.players[i].pseudo);
                        $.notify("This user is a drug dealer ! ", "warn");
                        // open modal to buy drug
                    }
                });
            }
            break;
        }

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
        this.soundManager.update();

        var camX = Math.floor(this.map.currentPlayer.position.x / this.game.camera.width);
        var camY = Math.floor(this.map.currentPlayer.position.y / this.game.camera.height);

        this.game.camera.x = Lerp(camX * this.game.camera.width, this.game.camera.x, 40);
        this.game.camera.y = Lerp(camY * this.game.camera.height, this.game.camera.y, 40);

        this.rainEmiter.position.x = camX * this.game.camera.width;
        this.rainEmiter.position.y = camY * this.game.camera.height;

        this.map.update();
        

        if (this.game.input.keyboard.isDown(Phaser.Keyboard.LEFT)) {
            this.map.currentPlayer.move(EDirection.Left);
            this.map.currentPlayer.updatePosition();
        }
        else if (this.game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)) {
            this.map.currentPlayer.move(EDirection.Right);
            this.map.currentPlayer.updatePosition();
        }

        if (this.game.input.keyboard.isDown(Phaser.Keyboard.UP)) {
            this.map.currentPlayer.move(EDirection.Up);
            this.map.currentPlayer.updatePosition();
        }
        else if (this.game.input.keyboard.isDown(Phaser.Keyboard.DOWN)) {
            this.map.currentPlayer.move(EDirection.Down);
            this.map.currentPlayer.updatePosition();
        }

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

setInterval(function () {
    if (isConnected) {
        hub.invoke("Update");
    }
}, 10000);


$(window).resize(function () {
    if (currentState != undefined) currentState.resizeGame();
});



hub.on("setRain", function(rain)
{
    if(currentState == undefined) return;
    currentState.setRain(rain);
});

hub.on("playerConnected", function (player) {
    if (currentState == undefined || currentState.map == undefined) return;
    currentState.map.addPlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y));
    $.notify("player connected : " + player.Pseudo, "warn");
});

hub.on("playerDisconnected", function (player) {
    if (currentState == undefined || currentState.map == undefined) return;
    currentState.map.removePlayer(player.Pseudo);
    $.notify("player disconnected : " + player.Pseudo, "error");
});

hub.on("playerMoved", function (player) {
    if (currentState != undefined && currentState.map != undefined) {
        currentState.map.updatePlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y), player.Direction);
    }
});

hub.on("updateTime", function (isDay) {
    if (currentState != undefined || currentState.map != undefined) {
        currentState.dayNightCycle.isDay = isDay;
    }
});

hub.on("MessageReceived", function (msg) {

    var u = currentState.map.getPlayer(msg.UserName);
    if (msg.UserName == currentState.map.currentPlayer.pseudo) u = currentState.map.currentPlayer;
    if (u != undefined) {
        u.setTextMessage(msg.Content);
    }
});


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

function openModalDrugs(pseudo) {
    if (pseudo == undefined) return;
    $("#buyDrugModal").modal();
    console.log($('#sellerPseudo').val());
    $("#sellerPseudo").attr("value", pseudo);
}

$("#buyDrugsForm").submit(function (e) {
    var pseudo = $('#sellerPseudo').val();
    if (pseudo == undefined) {
        e.preventDefault();
        $("#buyDrugModal").modal("hide");
        return;
    }
    hub.invoke("BuyDrug", pseudo).done(function(res) {
        if (res) {
            $.notify("Your are now on drugs", "warn");
            currentState.map.currentPlayer.putOnDrug();
        }
    });
    $("#buyDrugModal").modal("hide");
    e.preventDefault();
});

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