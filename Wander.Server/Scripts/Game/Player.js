var Player = (function () {
    function Player(state, game, pseudo, position) {
        this.isDrugged = false;
        this.game = game;
        this.state = state;
        this.direction = EDirection.Idle;
        this.speed = 5;
        this.texture = this.game.add.sprite(position.x, position.y, 'player-anim');
        this.texture.alpha = 0;
        this.texture.width = 15;
        this.texture.height = 25;
        this.texture.visible = true;
        this.animTexture = this.game.add.sprite(position.x, position.y, 'player-anim');
        this.animTexture.animations.add('walk-up', Phaser.ArrayUtils.numberArray(105, 112));
        this.animTexture.animations.add('walk-left', Phaser.ArrayUtils.numberArray(118, 125));
        this.animTexture.animations.add('walk-down', Phaser.ArrayUtils.numberArray(131, 137));
        this.animTexture.animations.add('walk-right', Phaser.ArrayUtils.numberArray(144, 151));
        this.animTexture.animations.add('idle', [130]);
        this.animTexture.animations.play('idle', 10, true);
        this.animTexture.scale.set(0.55, 0.55);
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
        this.footstepEndTime = new Date().getTime();
        this.footstepStartTime = new Date().getTime();
        this.drugFilter = this.game.add.filter('Gray');
    }
    Player.prototype.update = function () {
        this.footstepStartTime = new Date().getTime();
        this.animTexture.x = this.texture.x - this.texture.width / 2;
        this.animTexture.y = this.texture.y - this.texture.height / 2;
        var velX = this.texture.body.velocity.x;
        var velY = this.texture.body.velocity.y;
        if (currentState.map.currentPlayer === this) {
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
            else if (velY !== 0) {
                if (velY > 0) {
                    this.direction = EDirection.Down;
                }
                else {
                    this.direction = EDirection.Up;
                }
            }
            if (velX === 0 && velY === 0) {
                this.direction = EDirection.Idle;
            }
        }
        if (this.direction !== EDirection.Idle) {
            var elaps = this.footstepStartTime - this.footstepEndTime;
            if (elaps >= 600) {
                this.footstepEndTime = new Date().getTime();
                this.state.soundManager.playFootStep(this);
            }
        }
        this.texture.body.velocity.x = 0;
        this.texture.body.velocity.y = 0;
        this.position.x = this.texture.x;
        this.position.y = this.texture.y;
        this.text.x = this.texture.x;
        this.text.y = this.texture.y - 20;
        this.textMessage.x = this.texture.x;
        this.textMessage.y = this.texture.y - 45;
        if (this.textMessageContent !== "") {
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
            if (this.drugStartTime - this.drugEndTime >= 5000) {
                this.drugEndTime = this.drugStartTime;
                this.isDrugged = false;
                this.game.world.filters.splice(this.game.world.filters.indexOf(this.drugFilter), 1);
                this.drugFilter.destroy();
            }
            console.log("drugged");
        }
        switch (this.direction) {
            case EDirection.Down:
                this.animTexture.animations.play('walk-down', 10, true);
                break;
            case EDirection.Up:
                this.animTexture.animations.play('walk-up', 10, true);
                break;
            case EDirection.Left:
                this.animTexture.animations.play('walk-left', 10, true);
                break;
            case EDirection.Right:
                this.animTexture.animations.play('walk-right', 10, true);
                break;
            case EDirection.Idle:
                this.animTexture.animations.play('idle', 10, true);
                break;
            case EDirection.DownLeft:
                this.animTexture.animations.play('walk-down', 10, true);
                break;
            case EDirection.DownRight:
                this.animTexture.animations.play('walk-down', 10, true);
                break;
            case EDirection.UpLeft:
                this.animTexture.animations.play('walk-up', 10, true);
                break;
            case EDirection.UpRight:
                this.animTexture.animations.play('walk-up', 10, true);
                break;
            default:
                this.animTexture.animations.play('idle', 10, true);
                break;
        }
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
    Player.prototype.putOnDrug = function () {
        //this.game.world.filters = [this.drugFilter];
        //this.isDrugged = true;
        this.drugEndTime = new Date().getTime();
    };
    Player.prototype.updateServer = function () {
        this.texture.body.x = Lerp(this.newPosition.x, this.texture.body.x, 1.40);
        this.texture.body.y = Lerp(this.newPosition.y, this.texture.body.y, 1.40);
        if (this.texture.position.x === this.newPosition.x && this.texture.position.y === this.newPosition.y) {
            this.direction = EDirection.Idle;
        }
    };
    Player.prototype.remove = function () {
        this.textMessage.kill();
        this.text.kill();
        this.texture.kill();
        this.animTexture.kill();
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
