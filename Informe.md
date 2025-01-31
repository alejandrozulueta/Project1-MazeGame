# Informe del Proyecto MazeGame

## Ejecución del proyecto

Para ejecutar el proyecto MazeGame, sigue estos pasos:
1. Navega a la carpeta `bin` dentro del directorio del proyecto.
2. Ejecuta el archivo `MazeGame.exe` para iniciar el juego.

## Dependencias

1. Las únicas dependencias del proyecto son `Newtonsoft.Json` y `Spectre.Console`. Estas dependencias están especificadas en el archivo `.csproj`, por lo que se descargarán automáticamente al abrir el proyecto en el entorno de desarrollo.

## Instrucciones

### Opciones Iniciales
Al comenzar, el juego ofrecerá las siguientes opciones:
1. **Iniciar nueva partida**
2. **Cargar partida anterior**
3. **Salir del juego**

#### Cargar Partida
- Si se selecciona esta opción, se reanudará la partida anterior (si existe).

#### Iniciar Nueva Partida
- Se presentarán 3 opciones de dificultad:
  - **Fácil**: Sin trampas ni enemigos.
  - **Media**: Laberinto más grande con algunas trampas y enemigos.
  - **Difícil**: Laberinto aún más grande con más trampas y enemigos.

### Selección de Jugadores
- Elegir cuántos jugadores humanos desean jugar.
- Elegir cuántos jugadores controlados por la IA se desean agregar.

### Configuración de Jugadores
- Cada jugador humano seleccionará:
  - **Habilidad asignada**
  - **Número de acciones por turno**: Se seleccionan desplazándose a la izquierda o derecha una vez entrado en esta configuración.
  - **Rango de visión**: Se seleccionan desplazándose a la izquierda o derecha una vez entrado en esta configuración.

### Objetivo del Juego
- Llegar a la casilla marcada con una letra **M**.
- El primer jugador en llegar será el ganador.

### Controles
- **Desplazamiento**: Flechas del teclado o teclas **W, S, A, D**.
- **Activar habilidad**: Tecla **Espacio**.

### Habilidades Disponibles
- **Mostrar Mapa**: Muestra todas las paredes.
- **Intercambiar Posición**: Intercambia tu posición con el jugador seleccionado.
- **Mostrar Meta**: Muestra la meta.
- **Crear Pared Temporal**: Selecciona la dirección y coloca una pared temporal en el mapa.
- **Mostrar Trampas**: Muestra las trampas en el rango de visión, marcándolas con una **T**.
- **Quitar Turno**: Selecciona un jugador y quítale su próximo turno.
- **Invisibilidad**: Dejas de estar visible en la visión de otros jugadores/enemigos.

### Trampas
- **Teleport**: Te teletransporta a un lugar aleatorio.
- **Blinded**: Aplica el efecto de ceguera.
- **Disorted**: Aplica el efecto de distorsión.

### Efectos
- **Blinded**: Reduce el campo de visión.
- **Disorted**: Invierte los controles.

## Clases Implementadas

### Lógica

- `TrampsManager`: gestiona las trampas en el juego. Tiene métodos para inicializar trampas, verificar activaciones y restaurar los tiempos de reutilización de las trampas.

- `Randoms`: proporciona métodos estáticos para generar posiciones aleatorias en el laberinto, asegurándose de que las posiciones no sean muros, trampas, personajes, enemigos o la meta.

- `Maze`: representa el laberinto del juego. Tiene métodos para inicializar el laberinto, generar el laberinto aleatoriamente, agregar muros adyacentes y establecer la meta.

- `GeneralMethods`: proporciona métodos estáticos generales, como calcular la distancia entre dos posiciones.

- `GameState`: almacena el estado del juego, incluyendo el número total de turnos, el número de trampas, los datos de las entidades, los jugadores, las trampas, las dimensiones del laberinto, las celdas y la posición de escape.

- `GameManager`: gestiona el flujo del juego, incluyendo la inicialización del juego, la carga y guardado del estado del juego, y el procesamiento de los turnos.

- `EntityManager`: gestiona las entidades del juego, incluyendo la inicialización de jugadores y enemigos, y la asignación de posiciones y visiones iniciales.

- `ActionManager`: gestiona las acciones de las entidades, incluyendo el movimiento y la ejecución de habilidades, así como la actualización de la visión de los jugadores.

### Visual

- `VisionOutput`: muestra la visión del jugador en el laberinto. Utiliza un `StringBuilder` para construir la representación visual del estado actual del laberinto y la imprime en la consola.

- `StartScreen`: gestiona la pantalla de inicio del juego. Muestra un menú con opciones para iniciar el juego, cargar una partida guardada o salir del juego.

- `PlayerConstructor`: permite la creación y configuración de personajes. Proporciona opciones para asignar habilidades, ajustar el número de acciones y el rango de visión del personaje.

- `MenuManager`: gestiona los menús del juego. Proporciona menús para seleccionar habilidades, jugadores, IA, constructor de personajes y dificultad del juego.

- `GetInfo`: proporciona métodos estáticos para obtener valores y teclas de entrada del usuario.

- `GameStateVisualizer`: muestra información del estado del juego, incluyendo el jugador actual, efectos negativos, habilidad asignada, cooldown de la habilidad y movimientos restantes.

- `Events`: gestiona los eventos del juego, como la victoria de un jugador, la activación de una trampa y la muerte de un jugador.

- `DifficultySettings`: configura los ajustes de dificultad del juego, incluyendo las dimensiones del laberinto, el número de trampas y el número de enemigos.

### Inteligencia Artificial

- `PlayerIA`: representa la inteligencia artificial de un jugador. Utiliza `IAStrategies` para determinar el movimiento del jugador en el laberinto, explorando nuevas posiciones y evitando trampas.

- `EnemyIA`: representa la inteligencia artificial de un enemigo. Utiliza `IAStrategies` para perseguir a los jugadores en el laberinto, seleccionando objetivos y moviéndose hacia ellos.

- `IAStrategies`: proporciona estrategias de inteligencia artificial para encontrar caminos en el laberinto. Implementa métodos como `FindPath` para encontrar el camino hacia un objetivo y `Lee` para calcular las distancias en el laberinto.

### Enumeraciones

- `GameKey`: enumera las posibles teclas de acción en el juego, como moverse hacia arriba, abajo, derecha, izquierda, espacio y escape.

- `OptionsPlayerConstructor`: enumera las opciones disponibles para configurar un jugador, como establecer el rango de visión, las acciones y las habilidades.

- `GameDifficulty`: enumera los niveles de dificultad del juego, como fácil, medio y difícil.

### Interfaces

- `IMaze`: define la interfaz para el laberinto del juego, incluyendo propiedades para la longitud, el ancho, la posición de escape y las celdas del laberinto.

### Datos

- `Cell`: representa una celda en el laberinto, con propiedades para indicar si es un muro, la meta, un personaje, una trampa o un enemigo.

- `EntityData`: clase abstracta que representa los datos de una entidad en el juego, incluyendo nombre, número asignado, posición actual, acciones totales y acciones actuales.

- `DataPlayer`: hereda de `EntityData` y representa los datos de un jugador, incluyendo el rango de visión, la visión actual, los estados, la habilidad asignada, la visibilidad y el turno.

## Flujo del juego

El flujo de Enchanted Maze es el siguiente:
1. El jugador es recibido con una pantalla de inicio gestionada por `StartScreen`, donde puede elegir iniciar un nuevo juego, cargar una partida guardada o salir del juego.
    - Si el jugador elige "Iniciar Juego", se inicializa un nuevo juego con la configuración seleccionada.
    - Si el jugador elige "Cargar Partida", se carga el estado del juego desde un archivo JSON.
    - Si el jugador elige "Salir", el juego se cierra.

2. El `GameManager` inicializa todos los componentes necesarios para el juego:
    - Primero, se inicializa el `Maze`:
        El proceso de generación del laberinto incluye la inicialización de todas las celdas como muros, la selección de una celda inicial aleatoria y la adición de muros adyacentes a una lista. Luego, se seleccionan y eliminan muros de la lista de manera aleatoria, asegurando que solo se eliminen muros que no creen caminos múltiples. Finalmente, se establece la posición de la meta en una celda aleatoria que no sea un muro.
    - Luego, se inicializa el `TrampsManager`, que genera de forma aleatoria cada trampa dentro del laberinto.
    - A continuación, se inicializa el `EntityManager`:
        - Instancia los jugadores y enemigos, asignándoles posiciones iniciales y configurando sus visiones iniciales en el laberinto.
        - Para los jugadores, se utiliza el `PlayerConstructor` para configurar sus habilidades, acciones y rango de visión. Luego, se asignan posiciones iniciales aleatorias y se establece su visión inicial en el laberinto.
        - Para los enemigos, se asignan posiciones iniciales aleatorias y se configuran sus acciones totales y actuales basadas en el mínimo de acciones de los jugadores.

3. El método `ProcessTurns` del `GameManager` se encarga de gestionar los turnos del juego. Este método realiza las siguientes acciones:
    - Incrementa el contador de turnos totales (`TotalTurns`).
    - Guarda el estado actual del juego llamando al método `SaveGame`, que crea una instancia de `GameState` con los datos actuales del juego, para serializar el estado del juego en un archivo JSON.
    - Restaura los tiempos de reutilización de todas las trampas llamando al método `RestoreAllTrampsCooldown` del `TrampsManager`, quien se encarga de iterar sobre todas las trampas en `DataTramps` y llama al método `RestoreTrampCooldown`.

4. A continucacion el método `NextTurn` del `GameManager` gestiona el turno de cada entidad:
    - Si la entidad es un jugador (`DataPlayer`), se llama al método `MovePlayer`.
    - Si la entidad es un enemigo (`EnemyIA`), se llama al método `MoveEnemy`.

    1.  El método `MovePlayer` del `GameManager` realiza las siguientes acciones:
        - Restaura los efectos de las habilidades del jugador llamando al método `RestoreEffects` de la habilidad asignada.
        - Restaura los efectos de los estados del jugador llamando al método `RestoreEffect` de cada estado.
        - Muestra la información del estado del juego y la visión del jugador en el laberinto.
        - Permite al jugador realizar acciones basadas en la entrada del usuario o la IA, llamando al método `Options` del `ActionManager`.
        - Verifica si el jugador ha activado una trampa llamando al método `CheckTrampActivation` del `TrampsManager` y aplica el efecto de la trampa si es necesario llamando al método `TrampEffect`, para posteriormente incrementar su `Cooldown`. 
        - Verifica si ha matado a otro jugador y aplica el efecto de muerte si es necesario.
        - Aplica los efectos de los estados del jugador llamando al método `ApplyEffect` de cada estado.
        - Verifica si el jugador ha alcanzado la meta y ha ganado el juego.
        

    2.  El método `MoveEnemy` del `GameManager` realiza las siguientes acciones:
        - Permite al enemigo realizar acciones basadas en la IA, llamando al método `Options` del `ActionManager`.
        - Verifica si el enemigo ha matado a un jugador y aplica el efecto de muerte si es necesario.
