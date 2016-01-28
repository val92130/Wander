﻿

var game: Game;
var currentState: GameState;


function createGame() {
    $("#main-game").show();
    $("#main-container").hide();
    $(".overlay").fadeIn();
    game = new Game(-1);
    $(".overlay").fadeOut();
}

function deleteGame() {
    game.destroy();
    $("#main-container").fadeIn();
}

function changeMap(houseId: number) {
    hub.invoke("EnterHouse", houseId).done(function(success) {
        if (success) {                        
            game.destroy();
            game = new Game(houseId);
        }
    });
}

function resetGame() {
    game.destroy();
    game = new Game(-1);
}



class Game extends Phaser.Game {

    constructor(houseId:number) {
        // init game
        currentState = new GameState(houseId);
        super("100%", "100%", Phaser.AUTO, "main-game", currentState);
    }


}


class GameState extends Phaser.State {
    map: Map;
    game: Phaser.Game;
    overlay: Phaser.Sprite;
    filter: Phaser.Filter;
    dayNightCycle: DayNightCycle;
    soundManager: SoundManager;
    sprite: any;
    weatherManager: WeatherManager;
    houseId: number;
    isInHouse:boolean;

    constructor(houseId: number) {
        super();
        this.houseId = houseId;
    }

    preload() {
        this.game.canvas.id = "canvas";
        this.game.scale.scaleMode = Phaser.ScaleManager.RESIZE;
        this.stage.disableVisibilityChange = true;
        this.game.stage.disableVisibilityChange = true;
        this.game.load.image("player", "Content/Game/Images/player.png");
        this.game.load.image("player", "Content/Game/Images/player_girl.png");
        this.game.load.tilemap("Map", "Content/Game/Maps/map2.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.tilemap("MapHouse", "Content/Game/Maps/house.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/tileset3.png");
        this.game.load.image("tileHouse", "Content/Game/Images/tileHouse.png");
        this.game.load.image("Number_Tiles", "Content/Game/Images/number_spritesheet.png");
        this.game.load.image("Overlay", "Content/Game/Images/filter.png");
        this.game.load.image('blurred-circle', 'Content/Game/Images/blurred-circle.png');

        if (this.houseId !== -1) {
            this.map = new Map(this, this.game, "MapHouse", "Tiles", "tileset3", 1);
        } else {
            this.map = new Map(this, this.game, "Map", "Tiles", "tileset3", 1);
        }
        
        this.game.load.spritesheet('rain', 'Content/Game/Images/rain.png', 17, 17);
        this.soundManager = new SoundManager(this.game, this);
        this.soundManager.preload();
        this.weatherManager = new WeatherManager(this.game, this);
        this.dayNightCycle = new DayNightCycle(this.game);
        this.game.load.spritesheet('player-girl-anim', 'Content/Game/Images/girl_playerSheet.png', 64, 64);
        this.game.load.spritesheet('player-anim', 'Content/Game/Images/player-spritesheet.png', 64, 64);
        this.game.load.script('gray', 'https://cdn.rawgit.com/photonstorm/phaser/master/filters/Gray.js');

    }

    create() {
        
        this.soundManager.create();
        this.game.physics.startSystem(Phaser.Physics.ARCADE);

        this.map.create();
        this.weatherManager.create();

        this.dayNightCycle.create();
        
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
        this.game.world.bringToTop(currentState.dayNightCycle.rainOverlay);


        hub.invoke("IsRaining").done(function(val) {
            currentState.setRain(val);
        });

        hub.invoke("GetAllPlayersMap", this.houseId).done(function (players) {
            if (players == null)return;
            for (var i = 0; i < players.length; i++) {
                currentState.map.addPlayer(players[i].Pseudo, new Phaser.Point(players[i].Position.X, players[i].Position.Y), players[i].Sex);
            }
        });
        
        hub.invoke("Update");

    }

    pressAction() {

        var y = Math.round(this.map.currentPlayer.texture.y / (this.map.tilemap.tileHeight * this.map.scale));
        var x = Math.round(this.map.currentPlayer.texture.x / (this.map.tilemap.tileWidth * this.map.scale));


        var curPlayerBounds:any = this.map.currentPlayer.texture.getBounds();
        for (var i = 0; i < this.map.players.length; i++) {
            var pBounds:any = this.map.players[i].texture.getBounds();
            if (Phaser.Rectangle.intersects(curPlayerBounds, pBounds)) {
                console.log("intersect with player : " + this.map.players[i].pseudo);
                hub.invoke("CheckIfDrugDealer", this.map.players[i].pseudo).done(function(res) {
                    if (res) {
                        openModalDrugs(currentState.map.players[i].pseudo);
                        $.notify("This user is a drug dealer ! ", "warn");
                    }
                });
            }
            break;
        }

        var tile = this.map.tilemap.getTile(x, y, "houseLayer");

        if (tile != undefined) {
            var propId = tile.properties.propertyId;
            if (propId != undefined) {
                openModalProperty(propId);
                currentState.soundManager.playActionSound();
            } else {
                var isMairie = tile.properties.Mairie;
                var isExit = tile.properties.exit;
                if (isExit) {
                    openExitPropertyModal();                
                }
                if (isMairie != undefined) {
                    getAllJobs();
                    currentState.soundManager.playActionSound();
                }
            }
        }
    }

    setRain(val: boolean) {
        if (this.houseId !== -1) {
            this.weatherManager.rainEmiter.on = false;
            return;
        }
        this.weatherManager.rainEmiter.on = val;
    }

    isRaining() :boolean {
        return this.weatherManager.rainEmiter.on;
    }


    update() {
        this.weatherManager.update();
        this.dayNightCycle.update();
        this.soundManager.update();

        var camX = Math.floor(this.map.currentPlayer.position.x / this.game.camera.width);
        var camY = Math.floor(this.map.currentPlayer.position.y / this.game.camera.height);

        this.game.camera.x = Lerp(camX * this.game.camera.width, this.game.camera.x, 40);
        this.game.camera.y = Lerp(camY * this.game.camera.height, this.game.camera.y, 40);



        this.map.update();
        

        if (this.game.input.keyboard.isDown(Phaser.Keyboard.LEFT)) {
            this.map.currentPlayer.move(EDirection.Left);
        }
        else if (this.game.input.keyboard.isDown(Phaser.Keyboard.RIGHT)) {
            this.map.currentPlayer.move(EDirection.Right);
        }

        if (this.game.input.keyboard.isDown(Phaser.Keyboard.UP)) {
            this.map.currentPlayer.move(EDirection.Up);
        }
        else if (this.game.input.keyboard.isDown(Phaser.Keyboard.DOWN)) {
            this.map.currentPlayer.move(EDirection.Down);
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
    $.notify("player connected : " + player.Pseudo, "warn");
});

hub.on("playerDisconnected", function (player) {
    if (typeof(currentState) == "undefined" || typeof(currentState.map) == "undefined") return;
    currentState.map.removePlayer(player.Pseudo);
    $.notify("player disconnected : " + player.Pseudo, "error");
});

hub.on("playerMoved", function (player) {
    if (typeof (currentState) !== "undefined" && typeof (currentState.map) !== "undefined") {
        currentState.map.updatePlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y), player.Direction);
    }
});

hub.on("updateTime", function (isDay) {
    if (typeof (currentState) !== "undefined" && typeof (currentState.map) !== "undefined") {
        if (currentState.houseId !== -1) {
            currentState.dayNightCycle.isDay = true;
        }
        currentState.dayNightCycle.isDay = isDay;

    }
});

hub.on("MessageReceived", function (msg) {
    var u = msg.UserName == currentState.map.currentPlayer.pseudo ? currentState.map.currentPlayer: currentState.map.getPlayer(msg.UserName);
    if (u != undefined) {
        u.setTextMessage(msg.Content);
    }
    currentState.soundManager.playChatSound();
});

hub.on("playerEnterMap", function (player) {
    if (typeof (currentState) == "undefined" || typeof (currentState.map) === "undefined") return;
    currentState.map.addPlayer(player.Pseudo, new Phaser.Point(player.Position.X, player.Position.Y), player.Sex);
});

hub.on("playerExitMap", function (player) {
    if (typeof (currentState) == "undefined" || typeof (currentState.map) === "undefined") return;
    currentState.map.removePlayer(player.Pseudo);
});

hub.on("sendQuestionToClient", function (question) {
    if (currentState == undefined) return;
    console.log(question);
    $('#questionContent').text(question.Question);
    openQuestionModal(question);
});

hub.on("notifyNewPosition", function(newPos) {
    if (typeof (currentState) === "undefined" || typeof (currentState.map) === "undefined") return;
    currentState.map.currentPlayer.texture.x = newPos.X;
    currentState.map.currentPlayer.texture.y = newPos.Y;
});

function exitMap() {
    hub.invoke("ExitHouse").done(function(val){
        if (val) {

            resetGame();
        }
    });
}


function openModalProperty(id) {
    hub.invoke("GetOwnersCount", id).done(function (res) {
        if (res !== -1) {
            $("#nbrOwnersProperty").text(res);
            hub.invoke("GetPropertyInfo", id).done(function (model) {
                if (model != null && model != undefined) {
                    if (typeof (currentUser) == undefined || currentUser == null) return;
                    $("#propertyModalBody").text("");

                    $("#propertyModalBody").append("<tr class='" + (res >= model.Threshold ? 'danger' : 'success') + "'><td>" + model.PropertyName + "</td><td>" + model.PropertyDescription + "</td><td>" + model.Threshold + "</td> <td>" + model.Price + "</td><td><button type='button ' " + (res >= model.Threshold ? 'disabled' : '') + " onclick = 'BuyProperty(" + model.PropertyId + ")' class='btn btn-success' data-dismiss='modal'>Buy</button></tr>");
                    $("#hiddenEnterHouseId").val(model.PropertyId);
                    $("#propertyModal").modal();

                }
            });
        }
    });

}

function openExitPropertyModal() {
    $("#exitPropertyModal").modal();
}

function openQuestionModal(question) {
    if (question == null) return;
    $("#questionid").val(question.QuestionId);
    $("#questionModal").modal({
        backdrop: 'static',
        keyboard: false
    });

}

$("#enterHouseForm").submit(function (e) {
    var houseId = $("#hiddenEnterHouseId").val();
    
    e.preventDefault();
    if (typeof (houseId) != "undefined" && houseId !== null) {
        $("#propertyModal").modal('hide');
        changeMap(houseId);
        console.log("Changing map");
    }
});

$("#questionForm").submit(function (e) {
    var answer = $('input[name=radioAnswer]:checked', '#questionForm').val();
    var questId = $("#questionid").val();
    var answ = answer === "yes" ? true : false;
    hub.invoke("CheckAnswer", questId, answ).done(function(res) {
        $("#questionModal").modal('hide');
    });
    e.preventDefault();
});

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