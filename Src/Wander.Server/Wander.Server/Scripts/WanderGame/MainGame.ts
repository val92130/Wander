class WanderGame {
    
    game: Phaser.Game;
    map: Phaser.Tilemap;
    cursors : Phaser.CursorKeys;
    constructor() {
        
        this.game = new Phaser.Game(640, 480, Phaser.AUTO, 'main', {
            create: this.create, preload:
            this.preload, render: this.render, update:this.update
        }); 
    }

    preload() {
        this.game.load.tilemap("ItsTheMap", "Content/Game/Maps/newMap2.json", null, Phaser.Tilemap.TILED_JSON);
        this.game.load.image("Tiles", "Content/Game/Images/breadinator_tileset.png");
    }

    render() {
        
    }

    create() {
        this.map = this.game.add.tilemap("ItsTheMap");
        this.map.addTilesetImage("tileset", "Tiles");

        this.map.createLayer("Calque de Tile 1").resizeWorld();
        this.game.camera.setSize(600,200);
        
        this.cursors = this.game.input.keyboard.createCursorKeys();
    }

    update() {
        if (this.cursors.up.isDown) {
            this.game.camera.y -= 4;
        }
        else if (this.cursors.down.isDown) {
            this.game.camera.y += 4;
        }

        if (this.cursors.left.isDown) {
            this.game.camera.x -= 4;
        }
        else if (this.cursors.right.isDown) {
            this.game.camera.x += 4;
        }


        this.game.debug.cameraInfo(this.game.camera, 32, 32);
    }



}