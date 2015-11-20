class Map {
    tilemap: Phaser.Tilemap;
    game: Phaser.Game;
    backgroundLayer: Phaser.TilemapLayer;
    objectsLayer: Phaser.TilemapLayer;
    collisionLayer: Phaser.TilemapLayer;
    houseLayer: Phaser.TilemapLayer; 
    tilesetImage: string;
    tilesetName: string;
    scale: number;
    mapName:string;
    constructor(game: Phaser.Game, mapName: string, tilesetImage: string, tilesetName: string, scale: number) {
        this.game = game;
        this.mapName = mapName;
        this.tilesetImage = tilesetImage;
        this.tilesetName = tilesetName;
        this.scale = scale;
    }

    create() {
        this.tilemap = this.game.add.tilemap(this.mapName);

        this.tilemap.addTilesetImage(this.tilesetName, this.tilesetImage);

        this.backgroundLayer = this.tilemap.createLayer("backgroundLayer");       
        

        this.collisionLayer = this.tilemap.createLayer("collisionLayer");
        this.collisionLayer.alpha = 0;

        this.houseLayer = this.tilemap.createLayer("houseLayer");
        this.houseLayer.alpha = 0;              

        this.objectsLayer = this.tilemap.createLayer("objectsLayer");

        this.backgroundLayer.setScale(this.scale);
        this.objectsLayer.setScale(this.scale);
        this.collisionLayer.setScale(this.scale);
        this.houseLayer.setScale(this.scale);
        
        this.backgroundLayer.resizeWorld();

        //this.tilemap.setCollision(2123, true, this.collisionLayer);
        this.tilemap.setCollisionBetween(2000, 2500, true, this.collisionLayer);
    }

    update() {
        
    }
}