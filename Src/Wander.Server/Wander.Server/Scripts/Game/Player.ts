﻿
class Player {

    position: Phaser.Point;
    pseudo: string;
    texture: Phaser.Sprite;
    newPosition: Phaser.Point;
    speed : number;
    style: any;

    text: any;
    game:Phaser.Game;
    startTime: any;
    endTime: any;

    constructor(game: Phaser.Game, pseudo: string, position: Phaser.Point) {

        this.game = game;
        this.speed = 2;
        this.texture = game.add.sprite(position.x, position.y, "player");
        this.texture.width = 20;
        this.texture.height = 30;
        this.pseudo = pseudo;
        this.position = position;
        this.newPosition = new Phaser.Point(this.position.x, this.position.y);
        this.style = { font: "16px Arial", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width, align: "center" };
        this.text = game.add.text(0, 0, pseudo, this.style);
        this.startTime = new Date().getTime();
        this.endTime = new Date().getTime();

    }

    update() {
        this.texture.x = this.position.x;
        this.texture.y = this.position.y;

        this.text.x = this.texture.x;
        this.text.y = this.texture.y - 20;

    }

    move(direction: EDirection) {
        
        switch (direction) {
            case EDirection.Left:
                this.position.x -= this.speed;
                break;
            case EDirection.Right:
                this.position.x += this.speed;
                break;
            case EDirection.Up:
                this.position.y -= this.speed;
                break;
            case EDirection.Down:
                this.position.y += this.speed;
                break;
        }

        //this.game.camera.x = this.position.x - (this.game.camera.width / 2) + this.texture.width / 2;
        //this.game.camera.y = this.position.y - (this.game.camera.height / 2) + this.texture.height/2;
        

        
    }

    updateServer() {
        this.position.x = Lerp(this.newPosition.x, this.position.x, 2);
        this.position.y = Lerp(this.newPosition.y, this.position.y, 2);
    }

    remove() {
        this.text.kill();
        this.texture.kill();
    }

    updatePosition() {
        this.startTime = new Date().getTime();
        var time = this.startTime - this.endTime;
        if (time >= 55) {
            this.endTime = this.startTime;
            hub.invoke("MoveTo", { X: this.position.x, Y: this.position.y });
        }
    }

}