import { Grid } from '/js/wasteful-game/grid.js';
import { Level } from '/js/wasteful-game/level.js';
import { ItemBuilder } from '/js/wasteful-game/level-building/item-builder.js';
import { EndTypes } from '/js/wasteful-game/metadata.js';
import { Player } from '/js/wasteful-game/entity/player.js';
import { MovableComponent } from '/js/wasteful-game/entity/components/movableComponent.js';
import { AttackableComponent } from '/js/wasteful-game/entity/components/attackableComponent.js';
import { EntityManager } from '/js/wasteful-game/entityManager.js';
import { ScreenDisplay } from '/js/wasteful-game/screen-display.js';

export class Wasteful {
  /**
   * @param {HTMLCanvasElement} canvas canvas to draw on
   * @param {object} hub server to send messages to
   */
  constructor(canvas, hub) {
    this._mouseDownHandle = this._onMouseDown.bind(this);
    this._keyDownHandle = this._onKeyDown.bind(this);
    this._canvas = canvas;
    this._hub = hub;
    this._isRunning = false;
    this._endType = '';
    this._lastMouseTarget = null;

    const url = new URL(window.location.href);
    if(url.searchParams.has('autostart')) {
      this._lastMouseTarget = canvas;
      this.startGame({ displayName: url.searchParams.get('name'), userId: url.searchParams.get('userid')});
    }
  }

  /**
   * @public
   * @param {{displayName: string, userId: string}} userInfo details of the player
   * @param {Array<{string, number}>} startingItems starting items for the player
   */
  startGame(userInfo, startingItems) {
    if (this._isRunning) {
      return;
    }

    this._isRunning = true;
    this._userInfo = userInfo;
    this._entityManager = new EntityManager();
    this._grid = new Grid(this._entityManager, this._canvas);

    let itemBuilder = new ItemBuilder(this);
    let items = [];
    if (startingItems !== undefined && startingItems.length > 0) {
      items = startingItems.map(item => itemBuilder.createItemByName(item.name, item.uses));
      items.forEach(item => this._entityManager.add(item));
    }

    this._player = new Player(this, items);

    this._level = new Level(this, this._player, itemBuilder);

    this._level.next();

    this._screenDisplay = new ScreenDisplay(this, this._canvas);
    this._screenDisplay.start(userInfo, this._player, this._entityManager);
    this._mouseDownHandle = this._onMouseDown.bind(this);

    document.addEventListener('mousedown', this._mouseDownHandle);
    document.addEventListener('keydown', this._keyDownHandle);
  }

  displaySurvivorRankings(survivorRankingData) {
    if (this._isRunning) {
      return;
    }
    this._isRunning = true;

    this._screenDisplay = new ScreenDisplay(this, this._canvas);
    this._screenDisplay.showSurvivorRankings(survivorRankingData);

    // TODO: This is terrible; get rid of it.
    let delay = ms => new Promise(r => setTimeout(r, ms));
    delay(30000).then(() => {
      this._isRunning = false;
      this._screenDisplay._clearCanvas(); // Remove this once it works correctly.
    });
  }

  /**
   * @public
   * @returns {Grid} the grid object
   */
  get grid() {
    return this._grid;
  }

  /**
   * @public
   * @returns {EntityManager} the entity manager
   */
  get entityManager() {
   return this._entityManager;
  }

  /**
   * @public
   * @returns {Player} the player
   */
  get player() {
    return this._player;
  }

  /**
   * @public
   * @returns {Level} current level number
   */
  get level() {
    return this._level;
  }

  /**
   * @public
   * @param {string} direction direction to move player
   * @param {number} moveNumber number of spaces to move
   */
  movePlayer(direction, moveNumber = 1) {
    if (moveNumber > 0) {
      this._player.getComponent(MovableComponent).move(direction);

      this._level.update();

      if (this._player.getComponent(AttackableComponent).isDead) {
        this._endType = EndTypes.died;
        this._endGame();
        return;
      }

      //TODO: Stop recursion if going to new level.
      const millisecondsToWait = 200;
      setTimeout(() => this.movePlayer(direction, moveNumber - 1), millisecondsToWait);
    }
  }

  escape(escapeType) {
    this._endType = EndTypes.escaped;
    this._escapeType = escapeType;
    this._endGame();
  }

  /**
   * @private
   */
  _endGame() {
    document.removeEventListener('mousedown', this._mouseDownHandle);
    document.removeEventListener('keydown', this._keyDownHandle);
    this._screenDisplay.stop(this._endType);

    // TODO: Organize data better, so it's not coming from separate objects.
    let heldItems = this.player.inventory.items.map(item => ({
        name: item.name,
        uses: item.remainingUses
    }));

    this._hub.invoke('GameEnd', this.player.points, this._userInfo.displayName, this._userInfo.userId, this._endType, this.level.levelNumber, heldItems, this.player.money, this._escapeType)
      .catch(err => console.error(err.toString()));

    this._isRunning = false;
    this._endType = '';
    this._escapeType = '';
  }

  /**
   * @private
   * @param {object} event event args
   */
  _onMouseDown(event) {
    this._lastMouseTarget = event.target;
  }

  /**
   * @private
   * @param {object} event event args
   */
  _onKeyDown(event) {
    if(this._lastMouseTarget !== this._canvas) {
      return;
    }
    switch (event.keyCode) {
      case 38: // up
      case 87: // w
        this.movePlayer('up');
        break;
      case 39: // right
      case 68: // d
        this.movePlayer('right');
        break;
      case 40: // down
      case 83: // s
        this.movePlayer('down');
        break;
      case 37: // left
      case 65: // a
        this.movePlayer('left');
        break;
      case 90: // z
        this._level.next();
      break;
      case 82: // r
        const timestamp = new Date().getTime();
        const url = new URL(window.location.href);
        if(url.searchParams.has('t')) {
          url.searchParams.set('t', timestamp.toString());
        } else {
          url.searchParams.append('t', timestamp.toString());
        }
        if(!url.searchParams.has('autostart')) {
          url.searchParams.append('autostart', 'true');
        }
        if(!url.searchParams.has('name')) {
          url.searchParams.append('name', this._userInfo.displayName);
        }
        if(!url.searchParams.has('userid')) {
          url.searchParams.append('userid', this._userInfo.userId);
        }
        window.location.href = url.toString();
        break;
    }
  }
}
