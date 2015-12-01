class ClientPlayer extends Player
{
    updateTimeMs: number;
    isDrugged: boolean;
    drugStartTime: any;
    drugEndTime: any;
    drugFilter: Phaser.Filter;
    drugTime:number;

    constructor(state: GameState, game: Phaser.Game, pseudo: string, position: Phaser.Point) {
        this.drugTime = 20000;
        this.updateTimeMs = 55;
        this.isDrugged = false;

        super(state, game, pseudo, position);

        this.drugStartTime = new Date().getTime();
        this.drugEndTime = new Date().getTime();
        this.drugFilter = this.game.add.filter('Gray');
        this.game.world.filters = [this.drugFilter];
        this.drugFilter.uniforms.gray.value = 0;
    }

    update() {
        var velX = this.texture.body.velocity.x;
        var velY = this.texture.body.velocity.y;

        if (velX === 0 && velY === 0) {
            if (this.direction != EDirection.Idle) {
                this.direction = EDirection.Idle;
                super.updateAnimationFrame();
            }           
        }

        this.texture.body.velocity.x = 0;
        this.texture.body.velocity.y = 0;

        this.drugStartTime = new Date().getTime();
        if (this.isDrugged) {
            if (this.drugStartTime - this.drugEndTime >= this.drugTime) {
                this.drugEndTime = this.drugStartTime;
                this.isDrugged = false;
                this.drugFilter.uniforms.gray.value = 0;

            }
            console.log("drugged");
        }

        var newDir;
        if (velX > 0) {
            if (velY > 0) {
                newDir = EDirection.DownRight;
            } else if (velY < 0) {
                newDir = EDirection.UpRight;
            } else {
                newDir = EDirection.Right;
            }
        }
        else if (velX < 0) {
            if (velY > 0) {
                newDir = EDirection.DownLeft;
            } else if (velY < 0) {
                newDir = EDirection.UpLeft;
            } else {
                newDir = EDirection.Left;
            }
        } else if (velY !== 0) {
            if (velY > 0) {
                newDir = EDirection.Down;
            } else {
                newDir = EDirection.Up;
            }
        } else if (velX === 0 && velY === 0) {
            newDir = EDirection.Idle;
        }

        if (newDir != this.direction && typeof (newDir) != undefined) {
            this.direction = newDir;
            super.updateAnimationFrame();
        }

        super.update();
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
        var velX = this.texture.body.velocity.x;
        var velY = this.texture.body.velocity.y;

        this.updatePositionToServer();
        
    }

    updatePositionToServer() {
        this.startTime = new Date().getTime();
        var time = this.startTime - this.endTime;
        if (time >= this.updateTimeMs) {
            this.endTime = this.startTime;
            if (this.position.x != null && this.position.y != null && this.direction != undefined && this.position.x != undefined && this.position.y != undefined && this.direction != undefined) {
                hub.invoke("UpdatePosition", { X: this.position.x, Y: this.position.y }, this.direction.toString());
            }
            
        }
    }

    putOnDrug() {
        this.drugFilter.uniforms.gray.value = 1;
        this.isDrugged = true;
        this.drugEndTime = new Date().getTime();
    }
}
