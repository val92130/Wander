var Player = (function () {
    function Player(game, pseudo, position) {
        this.game = game;
        this.direction = EDirection.Idle;
        this.speed = 7;
        this.texture = game.add.sprite(position.x, position.y, "player");
        this.texture.width = 15;
        this.texture.height = 25;
        this.textMessageContent = "";
        this.messageStyle = { font: "18px Arial", fill: "#FFFFFF", wordWrap: true, wordWrapWidth: this.texture.width * 3, align: "center" };
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
    Player.prototype.update = function () {
        var velX = this.texture.body.velocity.x;
        var velY = this.texture.body.velocity.y;
        if (velX > 0) {
            if (velY > 0) {
                this.direction = EDirection.DownRight;
            }
            else if (velY < 0) {
                this.direction = EDirection.UpRight;
            }
            else {
                this.direction = EDirection.Right;
            }
        }
        else if (velX < 0) {
            if (velY > 0) {
                this.direction = EDirection.DownLeft;
            }
            else if (velY < 0) {
                this.direction = EDirection.UpLeft;
            }
            else {
                this.direction = EDirection.Left;
            }
        }
        else if (velY != 0) {
            if (velY > 0) {
                this.direction = EDirection.Down;
            }
            else {
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
    };
    Player.prototype.setTextMessage = function (text) {
        this.textMessageContent = text;
        this.messageTime = new Date().getTime();
        this.messageTimeEnd = this.messageTime;
    };
    Player.prototype.move = function (direction) {
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
    };
    Player.prototype.updateServer = function () {
        this.texture.body.x = Lerp(this.newPosition.x, this.texture.body.x, 2);
        this.texture.body.y = Lerp(this.newPosition.y, this.texture.body.y, 2);
    };
    Player.prototype.remove = function () {
        this.text.kill();
        this.texture.kill();
    };
    Player.prototype.updatePosition = function () {
        this.startTime = new Date().getTime();
        var time = this.startTime - this.endTime;
        if (time >= 55) {
            this.endTime = this.startTime;
            hub.invoke("UpdatePosition", { X: this.position.x, Y: this.position.y }, this.direction.toString());
        }
    };
    return Player;
})();
//# sourceMappingURL=Player.js.map