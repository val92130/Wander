var GameLauncher = (function () {
    function GameLauncher() {
        this.game = new Phaser.Game(800, 500, Phaser.AUTO, 'main', { preload: this.preload, create: this.create, update: this.update, render: this.render });
    }
    GameLauncher.prototype.preload = function () {
        this.game.load.image('player', 'Content/Game/Images/player.png');
        this.game.load.tilemap('Map1', 'Content/Game/Maps/newMap.json', null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image('TilesetBG', 'Content/Game/Images/breadinator_tileset.png');
        this.game.stage.disableVisibilityChange = true;
        this.game.camera.position = new Phaser.Point(10, 200);
    };
    GameLauncher.prototype.create = function () {
        console.log(this);
        this.map = this.game.add.tilemap('Map1');
        this.map.addTilesetImage('tileset', 'TilesetBG');
        this.background = this.map.createLayer('backgroundLayer');
        this.background.resizeWorld();
        //Physics        
        this.game.physics.startSystem(Phaser.Physics.P2JS);
        this.mainGame = new MainGame(this.game);
    };
    GameLauncher.prototype.update = function () {
        if (this.mainGame != undefined) {
            this.mainGame.update();
        }
    };
    GameLauncher.prototype.render = function () {
        if (this.mainGame != undefined) {
            this.mainGame.render();
        }
    };
    return GameLauncher;
})();
