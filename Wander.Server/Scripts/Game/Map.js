var Map = (function () {
    function Map(state, game, mapName, tilesetImage, tilesetName, scale) {
        this.game = game;
        this.mapName = mapName;
        this.tilesetImage = tilesetImage;
        this.tilesetName = tilesetName;
        this.scale = scale;
        this.state = state;
    }
    Map.prototype.create = function () {
        this.tilemap = this.game.add.tilemap(this.mapName);
        this.tilemap.addTilesetImage(this.tilesetName, this.tilesetImage);
        this.backgroundLayer = this.tilemap.createLayer("backgroundLayer");
        this.lightsLayer = this.tilemap.createLayer("lightsLayer");
        this.lightsLayer.visible = false;
        this.collisionLayer = this.tilemap.createLayer("collisionLayer");
        this.collisionLayer.visible = false;
        this.houseLayer = this.tilemap.createLayer("houseLayer");
        this.houseLayer.visible = false;
        this.objectsLayer = this.tilemap.createLayer("objectsLayer");
        this.backgroundLayer.setScale(this.scale);
        this.objectsLayer.setScale(this.scale);
        this.collisionLayer.setScale(this.scale);
        this.houseLayer.setScale(this.scale);
        this.backgroundLayer.resizeWorld();
        this.tilemap.setCollisionBetween(2000, 2500, true, this.collisionLayer);
        this.players = new Array();
        this.currentPlayer = new Player(this.state, this.game, userPseudo, new Phaser.Point(10, 10));
        for (var i = 0; i < this.lightsLayer.layer.width; i++) {
            for (var j = 0; j < this.lightsLayer.layer.height; j++) {
                if (this.lightsLayer.map.getTile(i, j, "lightsLayer") != null) {
                    this.game.add.existing(new Light(this.state, (i * this.lightsLayer.map.tileWidth * this.scale) + this.lightsLayer.map.tileWidth / 2, (j * this.lightsLayer.map.tileHeight * this.scale) + this.lightsLayer.map.tileHeight / 2, 0.40));
                    console.log("found light");
                }
            }
        }
    };
    Map.prototype.update = function () {
        this.game.physics.arcade.collide(this.currentPlayer.texture, this.collisionLayer);
        for (var i = 0; i < this.players.length; i++) {
            this.players[i].update();
            this.players[i].updateServer();
        }
        this.currentPlayer.update();
    };
    Map.prototype.addPlayer = function (pseudo, position) {
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
            this.players.push(new Player(this.state, this.game, pseudo, position));
        }
        this.game.world.bringToTop(currentState.dayNightCycle.overlay);
        this.game.world.bringToTop(currentState.dayNightCycle.rainOverlay);
    };
    Map.prototype.removePlayer = function (pseudo) {
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
        this.game.world.bringToTop(currentState.dayNightCycle.rainOverlay);
    };
    Map.prototype.updatePlayer = function (pseudo, position, direction) {
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
    Map.prototype.getPlayer = function (pseudo) {
        if (this.players == undefined)
            return;
        for (var i = 0; i < this.players.length; i++) {
            if (this.players[i].pseudo == pseudo) {
                return this.players[i];
            }
        }
        return undefined;
    };
    return Map;
})();
//# sourceMappingURL=Map.js.map