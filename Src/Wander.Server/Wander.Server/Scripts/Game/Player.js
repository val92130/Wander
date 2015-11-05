var Player = (function () {
    function Player(game, pseudo, position) {
        var _this = this;
        this.update = function () {
            _this.texture.x = _this.position.x;
            _this.texture.y = _this.position.y;
            _this.text.x = _this.texture.x;
            _this.text.y = _this.texture.y - 20;
        };
        this.updateServer = function () {
            _this.position.x = Lerp(_this.newPosition.x, _this.position.x, 3);
            _this.position.y = Lerp(_this.newPosition.y, _this.position.y, 3);
        };
        this.remove = function () {
            _this.text.kill();
            _this.texture.kill();
        };
        this.updatePosition = function () {
            _this.startTime = new Date().getTime();
            var time = _this.startTime - _this.endTime;
            if (time >= 50) {
                _this.endTime = _this.startTime;
                hub.invoke("MoveTo", { X: _this.position.x, Y: _this.position.y });
            }
        };
        this.texture = game.add.sprite(200, 200, "player");
        this.pseudo = pseudo;
        this.position = position;
        this.newPosition = new Phaser.Point(this.position.x, this.position.y);
        this.style = { font: "16px Arial", fill: "#ff0044", wordWrap: true, wordWrapWidth: this.texture.width, align: "center" };
        this.text = game.add.text(0, 0, pseudo, this.style);
        this.startTime = new Date().getTime();
        this.endTime = new Date().getTime();
    }
    return Player;
})();
//# sourceMappingURL=Player.js.map