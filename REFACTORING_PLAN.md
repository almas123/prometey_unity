# План рефакторинга проекта Prometey

## Цели рефакторинга
1. Применить принципы SOLID, DRY, KISS
2. Создать базовый класс SpawnController для переиспользования
3. Реорганизовать структуру папок
4. Убрать хардкод и использовать ScriptableObject конфиги
5. Удалить использование рефлексии

## Новая структура папок

```
Assets/Scripts/
├── Camera/                          # Камера
│   └── CameraFollow.cs
│
├── Character/                       # Персонажи
│   ├── Character.cs
│   ├── PlayerCharacter.cs
│   ├── EnemyCharacter.cs
│   ├── AiState.cs
│   │
│   ├── Data/
│   │   └── CharacterData.cs
│   │
│   └── Components/
│       ├── Attack/
│       │   ├── IAttackComponent.cs
│       │   ├── AttackComponent.cs
│       │   └── AttackConfig.cs      # [НОВЫЙ] ScriptableObject
│       │
│       ├── Health/
│       │   ├── IHealthComponent.cs
│       │   ├── HealthComponent.cs
│       │   └── HealthConfig.cs      # [НОВЫЙ] ScriptableObject
│       │
│       ├── Input/
│       │   ├── IInputComponent.cs
│       │   ├── PlayerInputComponent.cs
│       │   └── AiInputComponent.cs
│       │
│       └── Movement/
│           ├── IMovementComponent.cs
│           ├── MovementComponent.cs
│           └── CharacterData.cs
│
├── Core/                            # Общие утилиты
│   └── Utils/
│       └── GameObjectFinder.cs      # [НОВЫЙ]
│
├── Spawn/                           # Система спауна
│   ├── Core/
│   │   ├── ISpawner.cs             # [НОВЫЙ] Интерфейс
│   │   ├── SpawnController.cs      # [НОВЫЙ] Базовый класс
│   │   └── SpawnConfig.cs          # [НОВЫЙ] ScriptableObject
│   │
│   └── Character/
│       ├── CharacterSpawnController.cs
│       ├── CharacterSpawnConfig.cs  # [НОВЫЙ] ScriptableObject
│       └── DifficultyScaler.cs     # [НОВЫЙ] Управление сложностью
│
└── UI/
    ├── HealthBar.cs
    └── PlayerHealthBarUI.cs
```

## Порядок выполнения

### Этап 1: Создание папок
1. Создать Assets/Scripts/Camera/
2. Создать Assets/Scripts/Core/Utils/
3. Создать Assets/Scripts/Spawn/Core/
4. Создать Assets/Scripts/Spawn/Character/
5. Создать Assets/Scripts/Character/Data/

### Этап 2: Создание утилит (DRY)
6. Создать GameObjectFinder - централизованный поиск игрока

### Этап 3: Создание базовой системы спауна (SOLID)
7. Создать ISpawner интерфейс
8. Создать SpawnConfig ScriptableObject
9. Создать базовый SpawnController<T> класс

### Этап 4: Создание конфигов для Character
10. Создать AttackConfig ScriptableObject
11. Создать HealthConfig ScriptableObject
12. Обновить AttackComponent для использования конфига
13. Обновить HealthComponent для использования конфига

### Этап 5: Рефакторинг CharacterSpawnController
14. Добавить публичный метод SetTarget() в EnemyCharacter
15. Создать CharacterSpawnConfig ScriptableObject
16. Создать DifficultyScaler класс (опционально)
17. Рефакторить CharacterSpawnController наследуя от SpawnController

### Этап 6: Перемещение файлов
18. Переместить CameraFollow.cs в Camera/
19. Переместить CharacterSpawnController.cs в Spawn/Character/
20. Переместить CharacterData.cs в Character/Data/

### Этап 7: Обновление использований
21. Обновить CameraFollow для использования GameObjectFinder
22. Обновить PlayerHealthBarUI для использования GameObjectFinder
23. Обновить Character для инициализации компонентов с конфигами

### Этап 8: Финальная проверка
24. Проверить все ссылки в Unity сценах
25. Убедиться что все компилируется
26. Протестировать спаун врагов
27. Коммит изменений

## Ключевые улучшения

### SOLID принципы
- **S (Single Responsibility)**: Каждый класс отвечает за одну вещь
- **O (Open/Closed)**: Расширяем через наследование, конфигурируем через ScriptableObject
- **L (Liskov Substitution)**: Наследники корректно заменяют базовый класс
- **I (Interface Segregation)**: Минимальные интерфейсы
- **D (Dependency Inversion)**: Зависимость от абстракций

### DRY (Don't Repeat Yourself)
- Общая логика спауна в базовом классе
- Утилиты для поиска объектов
- Переиспользуемые конфигурации

### KISS (Keep It Simple, Stupid)
- Удалена рефлексия
- Четкая структура
- Простые и понятные классы
