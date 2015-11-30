class ServerPlayer extends Player {
    newPosition: Phaser.Point;

    constructor(state: GameState, game: Phaser.Game, pseudo: string, position: Phaser.Point) {
        super(state, game, pseudo, position);
        this.newPosition = new Phaser.Point(this.position.x, this.position.y);
    }

    update() {
        
        super.update();

        this.texture.body.x = Lerp(this.newPosition.x, this.texture.body.x, 1.35);
        this.texture.body.y = Lerp(this.newPosition.y, this.texture.body.y, 1.35);

        if (this.texture.position.x === this.newPosition.x && this.texture.position.y === this.newPosition.y) {
            if (this.direction != EDirection.Idle) {
                this.direction = EDirection.Idle;
                super.updateAnimationFrame();
            }
            
        }       
    }

    updateInfos(newPosition: Phaser.Point, newDirection:EDirection) {
        this.newPosition = newPosition;
        if (this.direction != newDirection) {
            this.direction = newDirection;
            super.updateAnimationFrame();
        }

    }
}