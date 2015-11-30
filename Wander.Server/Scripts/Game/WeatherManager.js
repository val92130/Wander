var WeatherManager = (function () {
    function WeatherManager(game, state) {
        this.game = game;
        this.state = state;
    }
    WeatherManager.prototype.create = function () {
        this.rainEmiter = this.game.add.emitter(game.world.centerX, 0, 400);
        this.rainEmiter.width = this.game.world.width;
        this.rainEmiter.makeParticles('rain');
        this.rainEmiter.minParticleScale = 0.5;
        this.rainEmiter.maxParticleScale = 1.2;
        this.rainEmiter.setYSpeed(450, 650);
        this.rainEmiter.setXSpeed(-5, 5);
        this.rainEmiter.minRotation = 0;
        this.rainEmiter.maxRotation = 0;
        this.rainEmiter.start(false, 1600, 2, 0);
    };
    WeatherManager.prototype.update = function () {
        var camX = Math.floor(this.state.map.currentPlayer.position.x / this.game.camera.width);
        var camY = Math.floor(this.state.map.currentPlayer.position.y / this.game.camera.height);
        this.rainEmiter.position.x = camX * this.game.camera.width;
        this.rainEmiter.position.y = camY * this.game.camera.height;
    };
    return WeatherManager;
})();
//# sourceMappingURL=WeatherManager.js.map