class Player {

    position: Phaser.Point;
    pseudo: string;
    texture: Phaser.Sprite;
    newPosition: Phaser.Point;
    speed : number;
    style: any;
    messageStyle: any;
    text: any;
    game:Phaser.Game;
    startTime: any;
    endTime: any;

    textMessageContent: string;
    textMessage: any;

    messageTime: any;
    messageTimeEnd: any;

    constructor(game: Phaser.Game, pseudo: string, position: Phaser.Point) {

        this.game = game;


        this.speed = 7;
        this.texture = game.add.sprite(position.x, position.y, "player");
        this.texture.width = 20;
        this.texture.height = 30;

        this.textMessageContent = "";
        this.messageStyle = { font: "18px Arial", fill: "#FFFFFF", wordWrap: true };
        this.textMessage = game.add.text(0, 0, this.textMessageContent, this.messageStyle);
        
        this.pseudo = pseudo;
        this.position = position;
        this.newPosition = new Phaser.Point(this.position.x, this.position.y);
        this.style = { font: "16px Arial", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width, align: "center" };
        this.text = game.add.text(0, 0, pseudo, this.style);
        this.startTime = new Date().getTime();
        this.endTime = new Date().getTime();
        this.messageTime = new Date().getTime();
        this.messageTimeEnd = new Date().getTime();

        this.game.physics.enable(this.texture);
        this.texture.body.collideWorldBounds = true;
        this.texture.body.maxVelocity = 20;

    }

    update() {
        
        this.texture.body.velocity.x = 0;
        this.texture.body.velocity.y = 0;

        this.position.x = this.texture.x;
        this.position.y = this.texture.y;


        this.text.x = this.texture.x;
        this.text.y = this.texture.y - 20;

        this.textMessage.x = this.texture.x;
        this.textMessage.y = this.texture.y - 45;
        
        if (this.textMessageContent != "") {
            this.messageTime = new Date().getTime();
            var nTime = this.messageTime - this.messageTimeEnd;
            if (nTime >= 5000) {
                this.messageTimeEnd = this.messageTime;
                this.textMessageContent = "";
                
            }
        }
        this.textMessage.text = this.textMessageContent;

    }

    public setTextMessage(text: string) {
        this.textMessageContent = text;
        this.messageTime = new Date().getTime();
        this.messageTimeEnd = this.messageTime;
    }

    move(direction: EDirection) {
        
        switch (direction) {
            case EDirection.Left:
                this.texture.body.velocity.x = -(this.speed * this.game.time.elapsedMS);
                break;
            case EDirection.Right:
                this.texture.body.velocity.x = this.speed * this.game.time.elapsedMS;
                break;
            case EDirection.Up:
                this.texture.body.velocity.y = -(this.speed * this.game.time.elapsedMS);
                break;
            case EDirection.Down:
                this.texture.body.velocity.y = this.speed * this.game.time.elapsedMS;
                break;
        }

        
    }

    updateServer() {
        this.texture.body.x = Lerp(this.newPosition.x, this.texture.body.x, 2);
        this.texture.body.y = Lerp(this.newPosition.y, this.texture.body.y, 2);
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