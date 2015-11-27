class Player {

    position: Phaser.Point;
    pseudo: string;
    texture: Phaser.Sprite;
    newPosition: Phaser.Point;
    speed: number;
    style: any;
    messageStyle: any;
    text: any;
    game: Phaser.Game;
    startTime: any;
    endTime: any;

    textMessageContent: string;
    textMessage: any;

    messageTime: any;
    messageTimeEnd: any;
    direction: EDirection;
    state: GameState;

    isDrugged: boolean;
    drugStartTime: any;
    drugEndTime: any;
    drugFilter:Phaser.Filter;

    constructor(state:GameState, game: Phaser.Game, pseudo: string, position: Phaser.Point) {

        this.isDrugged = false;
        this.game = game;
        this.state = state;
        this.direction = EDirection.Idle;
        this.speed = 7;
        this.texture = game.add.sprite(position.x, position.y, "player");
        this.texture.width = 15;
        this.texture.height = 25;

        this.textMessageContent = "";
        this.messageStyle = { font: "18px Arial", fill: "#FFFFFF", wordWrap: true, wordWrapWidth: this.texture.width * 10, align: "center" };
        this.textMessage = game.add.text(0, 0, this.textMessageContent, this.messageStyle);

        this.pseudo = pseudo;
        this.position = position;
        this.newPosition = new Phaser.Point(this.position.x, this.position.y);
        this.style = { font: "16px Arial", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width * 10, align: "center" };
        this.text = game.add.text(0, 0, pseudo, this.style);
        this.startTime = new Date().getTime();
        this.endTime = new Date().getTime();
        this.messageTime = new Date().getTime();
        this.messageTimeEnd = new Date().getTime();

        this.game.physics.enable(this.texture);
        this.texture.body.collideWorldBounds = true;
        this.texture.body.maxVelocity = 20;

        this.drugStartTime = new Date().getTime();
        this.drugEndTime = new Date().getTime();

        this.drugFilter = this.game.add.filter('Gray');


        
    }

    update() {

        var velX = this.texture.body.velocity.x;
        var velY = this.texture.body.velocity.y;

        if (velX > 0) {
            if (velY > 0) {
                this.direction = EDirection.DownRight;
            } else if (velY < 0) {
                this.direction = EDirection.UpRight;
            } else {
                this.direction = EDirection.Right;
            }
        }
        else if (velX < 0) {
            if (velY > 0) {
                this.direction = EDirection.DownLeft;
            } else if (velY < 0) {
                this.direction = EDirection.UpLeft;
            } else {
                this.direction = EDirection.Left;
            }
        } else if (velY != 0) {
            if (velY > 0) {
                this.direction = EDirection.Down;
            } else {
                this.direction = EDirection.Up;
            }
        }

        if (velX == 0 && velY == 0) {
            this.direction = EDirection.Idle;
        }


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

        this.drugStartTime = new Date().getTime();
        if (this.isDrugged) {
            if (this.drugStartTime -this.drugEndTime  >= 5000) {
                this.drugEndTime = this.drugStartTime;
                this.isDrugged = false;
                this.game.world.filters.splice(this.game.world.filters.indexOf(this.drugFilter), 1);
                this.drugFilter.destroy();

            }
            console.log("drugged");
            
        }

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

    putOnDrug() {
        this.game.world.filters = [this.drugFilter];
        this.isDrugged = true;
        this.drugEndTime = new Date().getTime();
    }

    updateServer() {
        this.texture.body.x = Lerp(this.newPosition.x, this.texture.body.x, 2);
        this.texture.body.y = Lerp(this.newPosition.y, this.texture.body.y, 2);
    }

    remove() {
        this.textMessage.kill();
        this.text.kill();
        this.texture.kill();
    }

    updatePosition() {
        this.startTime = new Date().getTime();
        var time = this.startTime - this.endTime;
        if (time >= 55) {
            this.endTime = this.startTime;
            hub.invoke("UpdatePosition", { X: this.position.x, Y: this.position.y }, this.direction.toString());
        }
    }

}