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
    mapName: string;
    moneyBags:MoneyBag[];

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

        this.moneyBags = new Array<MoneyBag>();
    }

    addMoneyBag(id, position, ammount) {
        var flag = false;
        for (var i = 0; i < this.moneyBags.length; i++) {
            if (this.moneyBags[i].id === id) {
                flag = true;
                break;
            }
        }

        if (!flag) {
            var moneyBag = new MoneyBag(id, position,ammount, this.game);
            this.moneyBags.push(moneyBag);
        }
    }

    removeMoneyBag(id) {
        for (var i = 0; i < this.moneyBags.length; i++) {
            if (this.moneyBags[i].id === id) {
                this.moneyBags[i].remove();
                this.moneyBags.splice(i, 1);
                break;
            }
        }
    }

    update() {
        
    }
}


class MoneyBag {
    
    position: Phaser.Point;
    id: number;
    texture: Phaser.Sprite;
    game: Phaser.Game;
    ammount:number;

    constructor(id, position,ammount, game) {
        this.id = id;
        this.position = position;
        this.game = game;
        this.ammount = ammount;
        this.texture = this.game.add.sprite(position.X, position.Y, "money-bag");
        this.texture.width = 16;
        this.texture.height = 16;
        this.texture.position.x = position.X;
        this.texture.position.y = position.Y;
    }

    remove() {
        this.texture.kill();
    }
}