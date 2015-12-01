var Player = (function () {
    function Player(state, game, pseudo, position) {
        this.game = game;
        this.state = state;
        this.direction = EDirection.Idle;
        this.speed = 5;
        this.texture = this.game.add.sprite(position.x, position.y, 'player-anim');
        this.texture.alpha = 0;
        this.texture.width = 15;
        this.texture.height = 25;
        this.texture.visible = true;
        this.texture.autoCull = true;
        this.animTexture = this.game.add.sprite(position.x, position.y, 'player-anim');
        this.animTexture.animations.add('walk-up', Phaser.ArrayUtils.numberArray(105, 112));
        this.animTexture.animations.add('walk-left', Phaser.ArrayUtils.numberArray(118, 125));
        this.animTexture.animations.add('walk-down', Phaser.ArrayUtils.numberArray(131, 137));
        this.animTexture.animations.add('walk-right', Phaser.ArrayUtils.numberArray(144, 151));
        this.animTexture.animations.add('idle', [130]);
        this.animTexture.animations.play('idle', 10, true);
        this.animTexture.scale.set(0.55, 0.55);
        this.animTexture.autoCull = true;
        this.textMessageContent = "";
        this.messageStyle = { font: "18px Segoe UI", fill: "#FFFFFF", wordWrap: true, wordWrapWidth: this.texture.width * 10, align: "center" };
        this.textMessage = game.add.text(0, 0, this.textMessageContent, this.messageStyle);
        this.pseudo = pseudo;
        this.position = position;
        this.style = { font: "16px Segoe UI", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width * 10, align: "center" };
        this.pseudoText = game.add.text(0, 0, pseudo, this.style);
        this.startTime = new Date().getTime();
        this.endTime = new Date().getTime();
        this.messageTime = new Date().getTime();
        this.messageTimeEnd = new Date().getTime();
        this.game.physics.enable(this.texture);
        this.texture.body.collideWorldBounds = true;
        this.texture.body.maxVelocity = 20;
        this.footstepEndTime = new Date().getTime();
        this.footstepStartTime = new Date().getTime();
    }
    Player.prototype.update = function () {
        this.footstepStartTime = new Date().getTime();
        this.animTexture.x = this.texture.x - this.texture.width / 2;
        this.animTexture.y = this.texture.y - this.texture.height / 2;
        this.position.x = this.texture.x;
        this.position.y = this.texture.y;
        this.pseudoText.x = this.texture.x - (this.pseudoText.width / 2) + this.animTexture.width / 2;
        this.pseudoText.y = this.texture.y - 35;
        this.textMessage.x = this.texture.x - (this.textMessage.width / 2) + this.animTexture.width / 2;
        this.textMessage.y = this.texture.y - 55 - this.textMessage.height;
        if (this.direction !== EDirection.Idle) {
            var elaps = this.footstepStartTime - this.footstepEndTime;
            if (elaps >= 600) {
                this.footstepEndTime = new Date().getTime();
                this.state.soundManager.playFootStep(this);
            }
        }
        if (this.textMessageContent !== "") {
            this.messageTime = new Date().getTime();
            var nTime = this.messageTime - this.messageTimeEnd;
            if (nTime >= 5000) {
                this.messageTimeEnd = this.messageTime;
                this.textMessageContent = "";
            }
        }
        this.textMessage.text = this.textMessageContent;
    };
    Player.prototype.updateAnimationFrame = function () {
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
    Player.prototype.remove = function () {
        this.textMessage.kill();
        this.pseudoText.kill();
        this.texture.kill();
        this.animTexture.kill();
    };
    return Player;
})();
//# sourceMappingURL=Player.js.map