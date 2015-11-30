﻿class Player {

    position: Phaser.Point;
    pseudo: string;
    texture: Phaser.Sprite;
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

    footstepEndTime: any;
    footstepStartTime: any;

    animTexture: Phaser.Sprite;

    constructor(state:GameState, game: Phaser.Game, pseudo: string, position: Phaser.Point) {
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
        this.style = { font: "16px Arial", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width * 10, align: "center" };
        this.text = game.add.text(0, 0, pseudo, this.style);
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

    update() {

        this.footstepStartTime = new Date().getTime();

        this.animTexture.x = this.texture.x - this.texture.width / 2;
        this.animTexture.y = this.texture.y - this.texture.height / 2;

        this.position.x = this.texture.x;
        this.position.y = this.texture.y;

        this.text.x = this.texture.x;
        this.text.y = this.texture.y - 20;

        this.textMessage.x = this.texture.x;
        this.textMessage.y = this.texture.y - 45;

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

    }

    updateAnimationFrame() {
        console.log("changing frame");
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
    }

    public setTextMessage(text: string) {
        this.textMessageContent = text;
        this.messageTime = new Date().getTime();
        this.messageTimeEnd = this.messageTime;
    }




    remove() {
        this.textMessage.kill();
        this.text.kill();
        this.texture.kill();
        this.animTexture.kill();
    }

}