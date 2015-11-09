var Player = (function () {
    function Player(game, pseudo, position) {
        this.speed = 2;
        this.texture = game.add.sprite(position.x, position.y, "player");
        this.texture.width = 15;
        this.texture.height = 20;
        this.pseudo = pseudo;
        this.position = position;
        this.newPosition = new Phaser.Point(this.position.x, this.position.y);
        this.style = { font: "16px Arial", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width, align: "center" };
        this.text = game.add.text(0, 0, pseudo, this.style);
        this.startTime = new Date().getTime();
        this.endTime = new Date().getTime();
    }
    Player.prototype.update = function () {
        this.texture.x = this.position.x;
        this.texture.y = this.position.y;
        this.text.x = this.texture.x;
        this.text.y = this.texture.y - 20;
    };
    Player.prototype.move = function (direction) {
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
    };
    Player.prototype.updateServer = function () {
        this.position.x = Lerp(this.newPosition.x, this.position.x, 3);
        this.position.y = Lerp(this.newPosition.y, this.position.y, 3);
    };
    Player.prototype.remove = function () {
        this.text.kill();
        this.texture.kill();
    };
    Player.prototype.updatePosition = function () {
        this.startTime = new Date().getTime();
        var time = this.startTime - this.endTime;
        if (time >= 40) {
            this.endTime = this.startTime;
            hub.invoke("MoveTo", { X: this.position.x, Y: this.position.y });
        }
    };
    return Player;
})();
