class ClientPlayer extends Player
{
    updateTimeMs: number;
    isDrugged: boolean;
    drugStartTime: any;
    drugEndTime: any;
    drugFilter: Phaser.Filter;

    constructor(state: GameState, game: Phaser.Game, pseudo: string, position: Phaser.Point) {
        this.updateTimeMs = 55;
        this.isDrugged = false;

        super(state, game, pseudo, position);

        this.drugStartTime = new Date().getTime();
        this.drugEndTime = new Date().getTime();
        this.drugFilter = this.game.add.filter('Gray');

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
            if (this.drugStartTime - this.drugEndTime >= 5000) {
                this.drugEndTime = this.drugStartTime;
                this.isDrugged = false;
                this.game.world.filters.splice(this.game.world.filters.indexOf(this.drugFilter), 1);
                this.drugFilter.destroy();

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
        //this.game.world.filters = [this.drugFilter];
        //this.isDrugged = true;
        this.drugEndTime = new Date().getTime();
    }
}
