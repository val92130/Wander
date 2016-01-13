var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ServerPlayer = (function (_super) {
    __extends(ServerPlayer, _super);
    function ServerPlayer(state, game, pseudo, position) {
        _super.call(this, state, game, pseudo, position);
        this.newPosition = new Phaser.Point(this.position.x, this.position.y);
    }
    ServerPlayer.prototype.update = function () {
        _super.prototype.update.call(this);
        this.texture.body.x = Lerp(this.newPosition.x, this.texture.body.x, 1.35);
        this.texture.body.y = Lerp(this.newPosition.y, this.texture.body.y, 1.35);
        if (this.texture.position.x === this.newPosition.x && this.texture.position.y === this.newPosition.y) {
            if (this.direction != EDirection.Idle) {
                this.direction = EDirection.Idle;
                _super.prototype.updateAnimationFrame.call(this);
            }
        }
    };
    ServerPlayer.prototype.updateInfos = function (newPosition, newDirection) {
        var distX = Math.abs(this.texture.position.x - newPosition.x);
        var distY = Math.abs(this.texture.position.y - newPosition.y);
        if (distX >= 10 || distY >= 10) {
            this.texture.position.x = newPosition.x;
            this.texture.position.y = newPosition.y;
            this.direction = newDirection;
        }
        console.log("DistX : " + distX + " DistY : " + distY);
        this.newPosition = newPosition;
        if (this.direction != newDirection) {
            this.direction = newDirection;
            _super.prototype.updateAnimationFrame.call(this);
        }
    };
    return ServerPlayer;
})(Player);
//# sourceMappingURL=ServerPlayer.js.map