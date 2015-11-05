var GameLauncher = (function () {
    function GameLauncher() {
        var _this = this;
        this.create = function () {
            _this.mainGame = new MainGame(_this.game);
        };
        this.update = function () {
            if (_this.mainGame != undefined) {
                _this.mainGame.update();
            }
        };
        this.render = function () {
            if (_this.mainGame != undefined) {
                _this.mainGame.render();
            }
        };
        this.game = new Phaser.Game(800, 500, Phaser.AUTO, 'main', { preload: this.preload, create: this.create, update: this.update, render: this.render });
    }
    GameLauncher.prototype.preload = function () {
        this.game.load.image('player', 'Content/Game/player.png');
        this.game.stage.disableVisibilityChange = true;
    };
    return GameLauncher;
})();
//# sourceMappingURL=GameLauncher.js.map