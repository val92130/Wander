class GameLauncher {

    mainGame: MainGame;
    game: Phaser.Game;

    constructor() {
        this.game = new Phaser.Game(800, 500, Phaser.AUTO, 'main', { preload: this.preload, create: this.create, update: this.update, render: this.render });

    }

    preload() {
        this.game.load.image('player', 'Content/Game/player.png');
        this.game.stage.disableVisibilityChange = true;
    }

    create = () => {
        this.mainGame = new MainGame(this.game);

    }

    update = () => {
        if (this.mainGame != undefined) {
            this.mainGame.update();
        }

    }

    render = () => {
        if (this.mainGame != undefined) {
            this.mainGame.render();
        }
    }

}