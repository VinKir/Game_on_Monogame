Systems - системы
{
	classes:
	GameScene - 
	{
		fields:
			gameObjects () - список игровых объектов на сцене;
			bullets () - список снарядов на сцене;
			gameButtons () - список кнопок на сцене;
			enemySpawnTimer () - ;
			maxEnemySpawnTreshold () - ;
			enemySpawnTreshold () - ;
			timeToBoss () - ;
			timeToBossCounter () - ;
			player () - ссылка на игрока;
			bullet () - префаб снаряда;
		methods:
			LoadContent() - ;
			backToMenuButton_Click(object sender, EventArgs e) - ;
			SpawnEnemies(GameTime gameTime) - ;
			SpawnBoss() - ;
			SpawnEnemy() - ;
			CreateBullet(GameObject parent, Vector2 direction) - ;
	},
	GarageScene -
	{
		fields:
			garageMenuGameObjects (List<GameObject>) - ;
			carBlocks (Block[,]) - ;
			carBlocksButtons (Button[,]) - ;
			currentBlock(Block) - ;
		methods:
			InitializeMachineArray() - ;
			LoadContent() - ;
			CreateButtons() - ;
			backToMenuButton_Click(object sender, EventArgs e) - ;
	},
	MainScene -
	{
		fields:
			mainMenuGameObjects (List<GameObject>) - ;
		methods:
			LoadContent() - ;
			QuitButton_Click(object sender, EventArgs e) - ;
			garageSceneOpenButton_Click(object sender, EventArgs e) - ;
			loadLevelButton_Click(object sender, EventArgs e) - ;
	},
}
Entities - сущности
{
	classes:
	GameObject - 
	{
		fields:
			components () - ;
			transform () - ;
			sprite () - ;
			isStatic () - ;
			LifeSpan () - ;
			isRemoved () - ;
			rectangle () - ;
			LifeSpan () - ;
		methods:
			AddComponent(IComponent component) - ;
			RemoveComponent(IComponent component) - ;
			GetComponent<T>() - ;
			Update(GameTime gameTime) - ;
			Draw(GameTime gameTime, SpriteBatch spriteBatch) - ;
			Clone() - ;
	},
	Bullet -
	{
		fields:
			team (Team) - ;
			damage (int) - ;
		methods:
			DoDamage(CarBlock carBlock) - ;
	},
	Button - кнопка
	{
		fields:
		methods:
	},
	Enemy - 
	{
		fields:
			bounty (int) - ;
			strangeMovingTimerTreshold (float) - ;
			moveDistance (float) - ;
			moveTreshold (float) - ;
			speed (float) - ;
			fireTimerTreshold (float) - ;
			fireDistance (float) - ;
		methods:
			FireMethod(GameTime gameTime) - вызывается из Update, 
							отвечает за логику стрельбы врага;
			MoveMethod(GameTime gameTime) - вызывается из Update,
							отвечает за логику движения врага;
	},
	Player - 
	{
		fields:
			car (List<List<GameObject>>) - ;
			mainBlock (GameObject) - ;
			mainBlockIndexX (int) - ;
			mainBlockIndexY (int) - ;
			carSpeed (float) - ;
			carRotationSpeed (float) - ;
			carScaleMultiplier (float) - ;
		methods:
			InitializeMachine() - ;
			Fire(GameTime gameTime) - ;
			Move() - ;
			MoveOtherCarBlocks() - ;
			ScanCar() - ;
			CalculateSpeedAndRotSpeed(int leftWheelsCount, int rightWheelsCount) - ;
	},
}
Components - могут быть навешаны на GameObject
{
	enums:
	Team - определяет, к какой команде принадлежит блок или пуля
	{
		Player - команда игрока,
		Enemy - команда врагов,
		Obstacle - препятствия,
	}
	interfaces:
	IComponent - все компоненты наследуются от этого интерфейса
	{
		boundEntity (GameObject) - ссылка на игровой объект, на который навешана эта компонента;
	}
	classes:
	CarBlock - компонента, если висит на  игровом объекте, тогда он является блоком.
	{
		HP (int) - здоровье блока, при изменении меняется цвет блока, 
		     чем больше разница с MaxHP, тем краснее блок;
		MaxHP (int) - максимальное здоровье блока;
		block (Block) - тип блока;
		team (Team) - к какой команде пирнадлежит блок;
	},
	Rigidbody - компонента, отвечающая за физику, пока что нигде не используется.
	{
	},
	Sprite - компонента, если висит на объекте, то этот объект отрисовывается и имеет текстуру.
	{
		texture (Texture2D) - текстура для отрисовки;
		layer (float) - на каком уровне будет отрисовываться текстура, 
				если слой выше, то текстура будет отрисовываться поверх других текстур;
		color (Color) - цвет текстуры;
	},
	Transform - компонента, отвечает за положение объекта в сцене.
	{
		scale (float) - размер объекта;
		Position (Vector2) - позиция объекта на сцене (координаты), 
				     рассчитывается от левого верхнего угла объекта;
		Origin (Vector2) - центр объекта;
		Parent (Transform) - ссылка на объект, от которого наследуется в иерархии объектов этот объект;
		Velocity (Vector2) - скорость передвижения;
		RotationVelocity (float) - скорость вращения;
		LinearVelocity (float) - линейная скорость;
		Rotation (float) - тоже отвечает куда повернут объект, нужно для отрисовки;
		Direction (Vector2) - куда повернут объект;
	},
}